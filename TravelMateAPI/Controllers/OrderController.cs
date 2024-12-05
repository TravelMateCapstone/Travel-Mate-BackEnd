using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;

namespace TravelMateAPI.Controllers
{

    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly PayOS _payOS;
        public OrderController(PayOS payOS)
        {
            _payOS = payOS;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var domain = "http://127.0.0.1:5500";

            var paymentLinkRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: 2000,
                description: "Thanh toan don hang",
                items: [new("Mì tôm hảo hảo ly", 1, 2000)],
                returnUrl: domain + "/success.html",
                cancelUrl: domain + "/cancel.html"
            );
            var response = await _payOS.createPaymentLink(paymentLinkRequest);

            Response.Headers.Append("Location", response.checkoutUrl);
            return new StatusCodeResult(303);
        }

    }
}
