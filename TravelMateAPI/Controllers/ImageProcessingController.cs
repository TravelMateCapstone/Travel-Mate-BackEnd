using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Tesseract;
using System.Drawing;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageProcessingController : ControllerBase
    {
        [HttpPost("extract-cccd")]
        public IActionResult ExtractCCCD(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No image file provided.");

            try
            {
                // Lưu ảnh tạm thời
                var tempPath = Path.GetTempFileName();
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                // Tesseract OCR xử lý hình ảnh
                var tessDataPath = Path.Combine(Directory.GetCurrentDirectory(), "tessdata");
                var ocrEngine = new TesseractEngine(tessDataPath, "vie", EngineMode.Default);

                using (var img = Pix.LoadFromFile(tempPath))
                {
                    using (var page = ocrEngine.Process(img))
                    {
                        var extractedText = page.GetText();

                        // Xử lý chuỗi văn bản để lấy thông tin CCCD
                        var cccdInfo = ParseCCCDInfo(extractedText);

                        return Ok(cccdInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private object ParseCCCDInfo(string extractedText)
        {
            // Xử lý thông tin văn bản để trích xuất các trường cụ thể
            var cccdData = new Dictionary<string, string>();

            // Lấy số CCCD
            var cccdMatch = System.Text.RegularExpressions.Regex.Match(extractedText, @"\b\d{12}\b");
            if (cccdMatch.Success)
                cccdData["CCCD Number"] = cccdMatch.Value;

            // Lấy Họ và Tên
            var nameLine = extractedText.Split('\n').FirstOrDefault(line => line.Contains("Họ và tên"));
            if (!string.IsNullOrEmpty(nameLine))
                cccdData["Full Name"] = nameLine.Replace("Họ và tên", "").Trim();

            // Lấy Ngày sinh
            var dobMatch = System.Text.RegularExpressions.Regex.Match(extractedText, @"\b\d{2}/\d{2}/\d{4}\b");
            if (dobMatch.Success)
                cccdData["Date of Birth"] = dobMatch.Value;

            // Lấy Giới tính
            if (extractedText.Contains("Nam"))
                cccdData["Gender"] = "Nam";
            else if (extractedText.Contains("Nữ"))
                cccdData["Gender"] = "Nữ";

            // Lấy Địa chỉ
            var addressLine = extractedText.Split('\n').FirstOrDefault(line => line.Contains("Nơi thường trú"));
            if (!string.IsNullOrEmpty(addressLine))
                cccdData["Address"] = addressLine.Replace("Nơi thường trú", "").Trim();

            return cccdData;
        }




        [HttpPost("extract-cccd-back")]
        public IActionResult ExtractCCCDBack(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No image file provided.");

            try
            {
                // Lưu ảnh tạm thời
                var tempPath = Path.GetTempFileName();
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                // Tesseract OCR xử lý hình ảnh
                var tessDataPath = Path.Combine(Directory.GetCurrentDirectory(), "tessdata");
                var ocrEngine = new TesseractEngine(tessDataPath, "vie", EngineMode.Default);

                using (var img = Pix.LoadFromFile(tempPath))
                {
                    using (var page = ocrEngine.Process(img))
                    {
                        var extractedText = page.GetText();

                        // Xử lý chuỗi văn bản để lấy thông tin từ mặt sau
                        var cccdBackInfo = ParseCCCDBackInfo(extractedText);

                        return Ok(cccdBackInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // Cập nhật phương thức ParseCCCDBackInfo
        private object ParseCCCDBackInfo(string extractedText)
        {
            var cccdData = new Dictionary<string, string>();

            // Lấy đặc điểm nhận dạng
            var personalIdentification = ExtractLine(extractedText, "Đặc điểm nhận dạng");
            if (!string.IsNullOrEmpty(personalIdentification))
                cccdData["Personal Identification"] = personalIdentification;

            // Lấy ngày cấp
            var issuedDateMatch = Regex.Match(extractedText, @"\b\d{2}/\d{2}/\d{4}\b");
            if (issuedDateMatch.Success)
                cccdData["Issued Date"] = issuedDateMatch.Value;

            // Lấy nguyên dòng MRZ
            var fullMRZ = ExtractFullMRZ(extractedText);
            if (!string.IsNullOrEmpty(fullMRZ))
                cccdData["MRZ"] = fullMRZ;

            return cccdData;
        }
        //private object ParseCCCDBackInfo(string extractedText)
        //{
        //    // Chuẩn bị dictionary để lưu thông tin
        //    var cccdData = new Dictionary<string, string>();

        //    // Lấy đặc điểm nhận dạng
        //    var personalIdentification = ExtractLine(extractedText, "Đặc điểm nhận dạng");
        //    if (!string.IsNullOrEmpty(personalIdentification))
        //        cccdData["Personal Identification"] = personalIdentification;

        //    // Lấy ngày cấp
        //    var issuedDateMatch = Regex.Match(extractedText, @"\b\d{2}/\d{2}/\d{4}\b");
        //    if (issuedDateMatch.Success)
        //        cccdData["Issued Date"] = issuedDateMatch.Value;

        //    // Xử lý dòng MRZ (Machine Readable Zone)
        //    var mrzData = ExtractMRZ(extractedText);
        //    if (mrzData != null)
        //    {
        //        foreach (var key in mrzData.Keys)
        //        {
        //            cccdData[key] = mrzData[key];
        //        }
        //    }

        //    return cccdData;
        //}

        private string ExtractLine(string text, string keyword)
        {
            var lines = text.Split('\n');
            return lines.FirstOrDefault(line => line.Contains(keyword))?.Replace(keyword, "").Trim();
        }

        //private Dictionary<string, string> ExtractMRZ(string text)
        //{
        //    var mrzData = new Dictionary<string, string>();

        //    // Tìm dòng MRZ
        //    var mrzLines = text.Split('\n')
        //                       .Where(line => line.Contains("<<") || Regex.IsMatch(line, @"^[A-Z0-9<]+$"))
        //                       .ToList();

        //    if (mrzLines.Count >= 2)
        //    {
        //        // Giả sử MRZ nằm ở 2 dòng cuối
        //        var line1 = mrzLines[mrzLines.Count - 2].Replace("\n", "").Replace("\r", "").Trim();
        //        var line2 = mrzLines[mrzLines.Count - 1].Replace("\n", "").Replace("\r", "").Trim();

        //        // Phân tích MRZ
        //        if (line1.Length >= 44 && line2.Length >= 44)
        //        {
        //            mrzData["Document Type"] = line1.Substring(0, 2); // Loại tài liệu
        //            mrzData["Country Code"] = line1.Substring(2, 3); // Mã quốc gia
        //            mrzData["ID Number"] = line1.Substring(5, 9); // Số CCCD
        //            mrzData["Date of Birth"] = FormatMRZDate(line2.Substring(0, 6)); // Ngày sinh
        //            mrzData["Gender"] = line2.Substring(7, 1) == "M" ? "Nam" : "Nữ"; // Giới tính
        //            mrzData["Expiry Date"] = FormatMRZDate(line2.Substring(8, 6)); // Ngày hết hạn
        //        }
        //    }

        //    return mrzData;
        //}

        private string ExtractFullMRZ(string text)
        {
            // Tìm các dòng chứa ký tự MRZ (<< hoặc chỉ ký tự in hoa và số)
            var mrzLines = text.Split('\n')
                               .Where(line => line.Contains("<<") || Regex.IsMatch(line, @"^[A-Z0-9<]+$"))
                               .Select(line => line.Trim())
                               .ToList();

            // Gộp các dòng MRZ lại thành một chuỗi duy nhất
            return string.Join("\n", mrzLines);
        }

        private string FormatMRZDate(string mrzDate)
        {
            // MRZ Date format: YYMMDD
            if (mrzDate.Length == 6)
            {
                var year = int.Parse(mrzDate.Substring(0, 2));
                var month = mrzDate.Substring(2, 2);
                var day = mrzDate.Substring(4, 2);

                // Giả sử năm > 2000
                year += year < 50 ? 2000 : 1900;

                return $"{day}/{month}/{year}";
            }

            return null;
        }
    }
}
