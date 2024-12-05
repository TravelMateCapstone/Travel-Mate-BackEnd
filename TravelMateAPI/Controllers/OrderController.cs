using AutoMapper;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Repositories.Interface;

namespace TravelMateAPI.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly PayOS _payOS;
        private readonly ITourRepository _tourRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(PayOS payOS, UserManager<ApplicationUser> userManager, IMapper mapper, ITourRepository tourRepository)
        {
            _payOS = payOS;
            _tourRepository = tourRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create([FromQuery] string tourName, [FromQuery] string tourId, [FromQuery] int travelerId, [FromQuery] int Amount)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var domain = "https://travelmatefe.netlify.app/";

            var paymentLinkRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: Amount,
                //ten chuyen di
                description: tourName,
                 //items: [new("Mì tôm hảo hảo ly", 1, 2000)],
                 items: null,
                returnUrl: domain + "contract/ongoing",
                cancelUrl: domain + "contract/payment-failed"
            );
            var response = await _payOS.createPaymentLink(paymentLinkRequest);

            //add order code vao participant
            await _tourRepository.UpdateOrderCode(tourId, travelerId, response.orderCode);

            //update payment status of traveler if success
            return Redirect(response.checkoutUrl);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ReceivePaymentNotification([FromBody] WebhookType data)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Xử lý dữ liệu từ webhook
            if (data != null)
            {
                if (data.success)
                {
                    // lấy participant có order code gửi từ data => update
                    await _tourRepository.UpdatePaymentStatus(data.data.orderCode);
                }
                else
                {
                    // Xử lý lỗi
                    return BadRequest("Cập nhật không thành công");
                }
            }

            return Ok("Cập nhật trạng thái thanh toán thành công");
        }

    }
}
