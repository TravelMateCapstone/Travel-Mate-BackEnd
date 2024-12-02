using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json.Linq;

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
            var payOS = new PayOS(
                            clientId: "54da4e30-6872-4b49-8674-b57eaf6e29f1",
                            apiKey: "2a397826-45a8-4c86-85d0-1c516136d717",
                            checksumKey: "470e7decb659c7dcff231bf33a8dd0255b4525a81c11f86436d426e94b5c2d7b"
                            );

            var domain = "http://localhost:5041";

            var paymentLinkRequest = new PaymentData(
                                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: 2000,
                description: "Thanh toan don hang",
                items: [new("Mì tôm hảo hảo ly", 1, 2000)],
                returnUrl: domain,
                cancelUrl: domain
                );

            var response = await payOS.createPaymentLink(paymentLinkRequest);

            Response.Headers.Append("Location", response.checkoutUrl);
            return new StatusCodeResult(300);
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrderById(long orderId)
        {
            var response = new JObject();
            try
            {
                var order = _payOS.getPaymentLinkInformation(orderId);
                response["data"] = JObject.FromObject(order);
                response["error"] = 0;
                response["message"] = "ok";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response["error"] = -1;
                response["message"] = ex.Message;
                response["data"] = null;
                return StatusCode(500, response);
            }
        }

        [HttpPut("{orderId}")]
        public IActionResult CancelOrder(int orderId)
        {
            var response = new JObject();
            try
            {
                var order = _payOS.cancelPaymentLink(orderId, null);
                response["data"] = JObject.FromObject(order);
                response["error"] = 0;
                response["message"] = "ok";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response["error"] = -1;
                response["message"] = ex.Message;
                response["data"] = null;
                return StatusCode(500, response);
            }
        }


    }
}
