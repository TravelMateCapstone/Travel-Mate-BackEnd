using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.EnumClass;
using BusinessObjects.Utils.Response;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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

            if (tourParticipant.PaymentStatus == PaymentStatus.Success)
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

            await _tourRepository.UpdateOrderCode(scheduleId, tourId, travelerId, response.orderCode);

            return Redirect(response.checkoutUrl);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> payOSTransferHandler(WebhookType body)
        {
            try
            {
                WebhookData data = _payOS.verifyPaymentWebhookData(body);

                var getTourInfo = await _tourParticipantRepository.GetParticipantWithOrderCode(data.orderCode);

                var tourSchedule = getTourInfo.Schedules
                      .FirstOrDefault(schedule => schedule.Participants
            .Any(participant => participant.OrderCode == data.orderCode));

                var matchingSchedule = tourSchedule.Participants
             .FirstOrDefault(participant => participant.OrderCode == data.orderCode);

                if (getTourInfo == null)
                {
                    return NotFound();
                }

                var transaction = new TravelerTransaction
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    ScheduleId = tourSchedule.ScheduleId,
                    TourId = getTourInfo.TourId,
                    TourName = getTourInfo.TourName,
                    StartDate = tourSchedule.StartDate,
                    EndDate = tourSchedule.EndDate,
                    ParticipantId = matchingSchedule.ParticipantId,
                    TransactionTime = GetTimeZone.GetVNTimeZoneNow(),
                    PaymentStatus = PaymentStatus.Success,
                    TotalAmount = getTourInfo.Price,
                };

                var tourTransaction = new TourTransaction
                {
                    Id = transaction.Id,
                    TourId = transaction.TourId,
                    TourName = transaction.TourName,
                    ScheduleId = transaction.ScheduleId,
                    StartDate = transaction.StartDate,
                    EndDate = transaction.EndDate,
                    ParticipantId = transaction.ParticipantId,
                    TransactionStatus = PaymentStatus.Pending,
                    TransactionTime = transaction.TransactionTime,
                    Amount = transaction.TotalAmount,
                    LocalId = getTourInfo.Creator.Id,
                    LocalName = getTourInfo.Creator.Fullname,
                    TravelerId = matchingSchedule.ParticipantId,
                    TravelerName = matchingSchedule.FullName,
                    LastAmount = data.amount,
                };

                if (data.description == "Ma giao dich thu nghiem" || data.description == "VQRIO123")
                {
                    return Ok(new Response(0, "Ok", null));
                }

                if (body.success)
                {
                    matchingSchedule.PaymentStatus = PaymentStatus.Success;
                    matchingSchedule.TransactionTime = GetTimeZone.GetVNTimeZoneNow();
                    matchingSchedule.TotalAmount = data.amount;

                    await _tourParticipantRepository.UpdatePaymentStatus(getTourInfo, (int)transaction.ParticipantId);
                    await _tourParticipantRepository.AddTransactionAsync(transaction);
                    await _contractService.UpdateStatusToCompleted((int)transaction.ParticipantId, getTourInfo.Creator.Id, getTourInfo.TourId, tourSchedule.ScheduleId);
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
