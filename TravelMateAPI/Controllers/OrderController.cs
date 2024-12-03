using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;

namespace TravelMateAPI.Controllers
{

    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            // Keep your PayOS key protected by including it by an env variable
            string clientId = "54da4e30-6872-4b49-8674-b57eaf6e29f1";
            string apiKey = "2a397826-45a8-4c86-85d0-1c516136d717";
            string checksumKey = "470e7decb659c7dcff231bf33a8dd0255b4525a81c11f86436d426e94b5c2d7b";

            var payOS = new PayOS(clientId, apiKey, checksumKey);

            var domain = "http://localhost:5500/";

            var paymentLinkRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: 2000,
                description: "Thanh toan don hang",
                items: [new("Mì tôm hảo hảo ly", 1, 2000)],
                returnUrl: domain + "/success.html",
                cancelUrl: domain + "/cancel.html"
            );
            var response = await payOS.createPaymentLink(paymentLinkRequest);

            return Ok(new { checkoutUrl = response.checkoutUrl });
        }


    }
}
