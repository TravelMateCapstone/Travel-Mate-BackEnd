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

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var domain = "http://127.0.0.1:5500";

            var paymentLinkRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: 2000,
                //ten chuyen di
                description: "Thanh toan don hang",
                 //items: [new("Mì tôm hảo hảo ly", 1, 2000)],
                 items: null,
                returnUrl: domain + "/success.html",
                cancelUrl: domain + "/cancel.html"
            );
            var response = await _payOS.createPaymentLink(paymentLinkRequest);

            return Redirect(response.checkoutUrl);
        }

    }
}
