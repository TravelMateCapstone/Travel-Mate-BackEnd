using AutoMapper;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Request;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentDto payment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var domain = "https://travelmatefe.netlify.app/";

            var paymentLinkRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: payment.Amount,
                //ten chuyen di
                description: payment.TourName,
                 //items: [new("Mì tôm hảo hảo ly", 1, 2000)],
                 items: null,
                returnUrl: domain + "contract/ongoing",
                cancelUrl: domain + "contract/cancel"
            );
            var response = await _payOS.createPaymentLink(paymentLinkRequest);

            //update payment status of traveler if success
            return Redirect(response.checkoutUrl);
        }

    }
}
