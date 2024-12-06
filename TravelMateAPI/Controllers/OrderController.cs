using AutoMapper;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json.Linq;
using Repositories.Interface;
using System.Security.Cryptography;
using System.Text;

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

        private static readonly string ChecksumKey = "1a54716c8f0efb2744fb28b6e38b25da7f67a925d98bc1c18bd8faaecadd7675";

        private static readonly string Transaction = "{'orderCode':123,'amount':3000,'description':'VQRIO123','accountNumber':'12345678','reference':'TF230204212323','transactionDateTime':'2023-02-04 18:25:00','currency':'VND','paymentLinkId':'124c33293c43417ab7879e14c8d9eb18','code':'00','desc':'Thành công','counterAccountBankId':'','counterAccountBankName':'','counterAccountName':'','counterAccountNumber':'','virtualAccountName':'','virtualAccountNumber':''}";
        private static readonly string TransactionSignature = "412e915d2871504ed31be63c8f62a149a4410d34c4c42affc9006ef9917eaa03";


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
            if (!IsValidSignature())
            {
                return Unauthorized("Invalid signature");
            }

            // Xử lý dữ liệu từ webhook
            if (data != null)
            {
                if (data.success)
                {
                    // lấy participant có order code gửi từ data => update
                    await _tourRepository.UpdatePaymentStatus(data.data.orderCode, data.data.amount);
                }
                else
                {
                    // Xử lý lỗi
                    return BadRequest("Cập nhật không thành công");
                }
            }

            return Ok("Cập nhật trạng thái thanh toán thành công");
        }

        [HttpPost("confirm-webhook")]
        public async void ConfirmWebhook()
        {
            await _payOS.confirmWebhook("https://travelmateapp.azurewebsites.net/api/order/webhook");
        }

        public static bool IsValidSignature()
        {
            try
            {
                // Parse JSON string to JObject
                var jsonObject = JObject.Parse(Transaction);

                // Sort keys and build transaction string
                var sortedKeys = jsonObject.Properties()
                                            .Select(p => p.Name)
                                            .OrderBy(key => key, StringComparer.Ordinal)
                                            .ToList();

                var transactionStr = new StringBuilder();
                foreach (var key in sortedKeys)
                {
                    var value = jsonObject[key]?.ToString() ?? string.Empty;
                    transactionStr.Append($"{key}={value}&");
                }

                // Remove the trailing '&' character
                if (transactionStr.Length > 0)
                {
                    transactionStr.Length--; // Remove last '&'
                }

                // Compute HMAC-SHA256
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ChecksumKey)))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(transactionStr.ToString()));
                    var computedSignature = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

                    // Compare with the provided signature
                    return computedSignature == TransactionSignature;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

    }
}
