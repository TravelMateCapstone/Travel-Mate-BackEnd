using BusinessObjects;
using BusinessObjects.Entities;
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
        private readonly ITourParticipantRepository _tourParticipantRepository;
        private readonly ITourRepository _tourRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IContractService _contractService;

        public OrderController(PayOS payOS, ITourRepository tourRepository, ITourParticipantRepository tourParticipantRepository, IContractService contractService, ITransactionRepository transactionRepository)
        {
            _payOS = payOS;
            _tourParticipantRepository = tourParticipantRepository;
            _contractService = contractService;
            _transactionRepository = transactionRepository;
            _tourRepository = tourRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Create([FromQuery] string scheduleId, [FromQuery] string tourId, [FromQuery] int travelerId, [FromQuery] int Amount)
        {
            var domain = "https://travelmatefe.netlify.app/";
            var getTour = await _tourParticipantRepository.GetTourScheduleById(scheduleId, tourId);
            var tourSchedule = getTour.Schedules.FirstOrDefault(t => t.ScheduleId == scheduleId);

            var tourParticipant = tourSchedule.Participants.FirstOrDefault(t => t.ParticipantId == travelerId);

            if (tourParticipant.PaymentStatus == true)
                return BadRequest("You already paid for this tour");

            var registeredTime = tourParticipant.RegisteredAt;

            var registeredTimeUnix = new DateTimeOffset(registeredTime).ToUnixTimeSeconds();
            //var expiredTime = registeredTimeUnix - (7 * 3600) + (60 * 3);
            var expiredTime = registeredTimeUnix;
            var paymentLinkRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.Now.ToString("ffffff")),
                amount: Amount,
                description: "Thanh toan tour",
                items: null,
                returnUrl: domain + "contract/ongoing",
                cancelUrl: domain + "contract/payment-failed"
            //expiredAt: expiredTime
            );
            var response = await _payOS.createPaymentLink(paymentLinkRequest);

            tourParticipant.OrderCode = response.orderCode;

            await _tourRepository.UpdateTour(getTour.TourId, getTour);

            return Redirect(response.checkoutUrl);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> payOSTransferHandler(WebhookType body)
        {
            try
            {
                WebhookData data = _payOS.verifyPaymentWebhookData(body);

                var getTourInfo = await _tourParticipantRepository.GetParticipantWithOrderCode(data.orderCode);

                if (getTourInfo == null)
                {
                    return NotFound(new Response(-1, "Tour information not found", null));
                }

                var localId = getTourInfo.Creator.Id;
                var travelerId = 0;
                var tourId = getTourInfo.TourId;

                var transaction = new TourTransaction
                {
                    TourId = getTourInfo.TourId,
                    TourName = getTourInfo.TourName,
                    localId = getTourInfo.Creator.Id,
                    LocalName = getTourInfo.Creator.Fullname,
                    TravelerId = null,
                    TravelerName = null,
                    TransactionTime = GetTimeZone.GetVNTimeZoneNow(),
                    Price = getTourInfo.Price,
                };

                var matchingSchedule = getTourInfo.Schedules
                 .SelectMany(schedule => schedule.Participants)
                 .FirstOrDefault(participant => participant.OrderCode == data.orderCode);

                if (matchingSchedule != null)
                {
                    travelerId = matchingSchedule.ParticipantId;
                    transaction.TravelerId = travelerId;
                    transaction.TravelerName = matchingSchedule.FullName;
                }

                if (data.description == "Ma giao dich thu nghiem" || data.description == "VQRIO123")
                {
                    return Ok(new Response(0, "Ok", null));
                }

                if (body.success)
                {
                    matchingSchedule.PaymentStatus = true;
                    matchingSchedule.TotalAmount = data.amount;

                    await _tourParticipantRepository.UpdatePaymentStatus(getTourInfo, travelerId);
                    await _transactionRepository.AddTransactionAsync(transaction);
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
