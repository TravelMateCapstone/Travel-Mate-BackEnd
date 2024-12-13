using BusinessObjects.Utils.Response;
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
        private readonly IContractService _contractService;

        public OrderController(PayOS payOS, ITourRepository tourRepository, IContractService contractService)
        {
            _payOS = payOS;
            _tourRepository = tourRepository;
            _contractService = contractService;
        }

        [HttpGet]
        public async Task<IActionResult> Create([FromQuery] string tourName, [FromQuery] string tourId, [FromQuery] int travelerId, [FromQuery] int Amount)
        {

            var domain = "https://travelmatefe.netlify.app/";

            var paymentLinkRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: Amount,
                description: tourName,
                 items: null,
                returnUrl: domain + "contract/ongoing",
                cancelUrl: domain + "contract/payment-failed"
            );
            var response = await _payOS.createPaymentLink(paymentLinkRequest);

            await _tourRepository.UpdateOrderCode(tourId, travelerId, response.orderCode);

            var DidUserPay = await _tourRepository.DidParticipantPay(response.orderCode);
            if (DidUserPay)
            {
                return BadRequest("You already paid for this tour");
            }

            //update payment status of traveler if success
            return Redirect(response.checkoutUrl);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> payOSTransferHandler(WebhookType body)
        {
            try
            {
                WebhookData data = _payOS.verifyPaymentWebhookData(body);

                var getTourInfo = await _tourRepository.GetParticipantWithOrderCode(data.orderCode);
                if (getTourInfo == null)
                {
                    return NotFound(new Response(-1, "Tour information not found", null));
                }
                var travelerId = getTourInfo.Creator.Id;
                var localId = 0;
                var tourId = getTourInfo.TourId;

                foreach (var item in getTourInfo.Participants)
                {
                    if (item.OrderCode == data.orderCode)
                    {
                        localId = item.ParticipantId;
                        break;
                    }
                }

                if (data.description == "Ma giao dich thu nghiem" || data.description == "VQRIO123")
                {
                    return Ok(new Response(0, "Ok", null));
                }

                if (body.success)
                {
                    await _tourRepository.UpdatePaymentStatus(data.orderCode, data.amount);
                    await _contractService.UpdateStatusToCompleted(travelerId, localId, tourId);
                }

                return Ok(new Response(0, "Ok", null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Ok(new Response(-1, "fail", null));
            }
        }

    }
}
