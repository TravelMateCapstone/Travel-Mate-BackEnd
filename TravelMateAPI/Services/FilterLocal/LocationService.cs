using BusinessObjects;
using Google;
using Google.Cloud.Language.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Google.Api;
using MailKit;
using BusinessObjects.Entities;
using Newtonsoft.Json;

namespace TravelMateAPI.Services.FilterLocal
{
    public class LocationService
    {
        //private readonly ApplicationDBContext _context;
        //private readonly IHttpClientFactory _httpClientFactory;

        //public LocationService(ApplicationDBContext context, IHttpClientFactory httpClientFactory)
        //{
        //    _context = context;
        //    _httpClientFactory = httpClientFactory;
        //}

        //private readonly ApplicationDBContext _dbContext;
        //private readonly HttpClient _httpClient;

        //public LocationService(ApplicationDBContext dbContext, HttpClient httpClient)
        //{
        //    _dbContext = dbContext;
        //    _httpClient = httpClient;
        //}

        //public async Task<string> GetSuggestedLocation(string userInput)
        //{
        //    var requestBody = new
        //    {
        //        prompt = $"Dựa trên thông tin '{userInput}', hãy tìm địa điểm phù hợp tại Việt Nam.",
        //        max_tokens = 50
        //    };

        //    var response = await _httpClient.PostAsync(
        //        "https://api.openai.com/v1/engines/text-davinci-003/completions",
        //        new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        //    );

        //    response.EnsureSuccessStatusCode();

        //    var content = await response.Content.ReadAsStringAsync();
        //    var result = JsonSerializer.Deserialize<JsonElement>(content);
        //    return result.GetProperty("choices")[0].GetProperty("text").GetString().Trim();
        //}


        //public async Task<Location> GetLocationByKeywordAsync(string keyword)
        //{
        //    if (string.IsNullOrEmpty(keyword))
        //        return null;

        //    // Sử dụng AI để phân tích từ khóa
        //    var suggestedLocation = await GetSuggestedLocation(keyword);

        //    // Tìm địa điểm trong database
        //    var location = _dbContext.Locations
        //        .FirstOrDefault(l => EF.Functions.Like(l.LocationName, $"%{suggestedLocation}%"));

        //    return location;
        //}

        //public async Task<(int? LocationId, string LocationName)> SearchLocationAsync(string keyword)
        //{
        //    if (string.IsNullOrEmpty(keyword))
        //        return (null, null);

        //    // Gọi OpenAI API để phân tích từ khóa
        //    var requestBody = new
        //    {
        //        prompt = $"Dựa trên thông tin '{keyword}', hãy tìm địa điểm phù hợp tại Việt Nam.",
        //        max_tokens = 50
        //    };

        //    var response = await _httpClient.PostAsync(
        //        "https://api.openai.com/v1/engines/text-davinci-003/completions",
        //        new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        //    );

        //    response.EnsureSuccessStatusCode();

        //    var content = await response.Content.ReadAsStringAsync();
        //    var result = JsonSerializer.Deserialize<JsonElement>(content);
        //    var suggestedLocation = result.GetProperty("choices")[0].GetProperty("text").GetString().Trim();

        //    // Tìm kiếm trong cơ sở dữ liệu
        //    var location = _dbContext.Locations
        //        .FirstOrDefault(l => EF.Functions.Like(l.LocationName, $"%{suggestedLocation}%"));

        //    return location != null
        //        ? (location.LocationId, location.LocationName)
        //        : (null, null);
        //}

        //public async Task<(int? LocationId, string LocationName)> SearchLocationAsync(string keyword)
        //{
        //    if (string.IsNullOrEmpty(keyword))
        //        return (null, null);

        //    // Tạo HttpClient từ IHttpClientFactory
        //    var httpClient = _httpClientFactory.CreateClient();

        //    // Gọi OpenAI API để phân tích từ khóa
        //    var requestBody = new
        //    {
        //        prompt = $"Dựa trên thông tin '{keyword}', hãy tìm địa điểm phù hợp tại Việt Nam.",
        //        max_tokens = 50
        //    };

        //    var response = await httpClient.PostAsync(
        //        "https://api.openai.com/v1/engines/text-davinci-003/completions",
        //        new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        //    );

        //    response.EnsureSuccessStatusCode();

        //    var content = await response.Content.ReadAsStringAsync();
        //    var result = JsonSerializer.Deserialize<JsonElement>(content);
        //    var suggestedLocation = result.GetProperty("choices")[0].GetProperty("text").GetString().Trim();

        //    // Tìm kiếm trong cơ sở dữ liệu
        //    var location = _context.Locations
        //        .FirstOrDefault(l => EF.Functions.Like(l.LocationName, $"%{suggestedLocation}%"));

        //    return location != null
        //        ? (location.LocationId, location.LocationName)
        //        : (null, null);
        //}

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDBContext _context;
        private const string HuggingFaceApiKey = "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"; // Thay bằng API Key của bạn
        private const string HuggingFaceApiUrl = "https://api-inference.huggingface.co/models/cutycat2000x/MeowGPT-3.5"; // Ví dụ với GPT-2 của Hugging Face

        public LocationService(IHttpClientFactory httpClientFactory, ApplicationDBContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        public async Task<(int? LocationId, string LocationName)> SearchLocationAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return (null, null);

            var httpClient = _httpClientFactory.CreateClient();

            // Đặt API key vào header
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {HuggingFaceApiKey}");

            // Tạo payload để gửi đến Hugging Face API
            var requestBody = new
            {
                inputs = $"Dựa trên từ khóa '{keyword}', hãy trả về tên tỉnh thành nơi có địa điểm này.",
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            // Gửi yêu cầu đến Hugging Face API
            var response = await httpClient.PostAsync(HuggingFaceApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"API call failed: {response.StatusCode}");
            }

            // Đọc kết quả từ API
            var result = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<dynamic>(result);

            // Kết quả dự đoán từ AI (mô hình GPT-2 trả về văn bản tạo ra)
            var suggestedLocation = responseJson[0].generated_text.ToString().Trim();

            // Tìm địa điểm trong cơ sở dữ liệu dựa trên kết quả AI
            //var location = await _context.Locations
            //    .Where(l => l.LocationName.Contains(suggestedLocation))
            //    .FirstOrDefaultAsync();

            // Tìm kiếm trong cơ sở dữ liệu
            var location = _context.Locations
                .FirstOrDefault(l => EF.Functions.Like(l.LocationName, $"%{suggestedLocation}%"));

            // Nếu tìm thấy địa điểm, trả về ID và tên địa điểm
            return location != null ? (location.LocationId, location.LocationName) : (null, "Không tìm thấy địa điểm phù hợp.");
        }
    }
}
