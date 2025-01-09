using BusinessObjects.Entities;
using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBankController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserBankController(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Phương thức để lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }

        // GET: api/UserBank
        [HttpGet]
        public async Task<IActionResult> GetAllUserBanks()
        {
            var userBanks = await _context.UserBanks.ToListAsync();
            return Ok(userBanks);
        }

        // GET: api/UserBank/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserBankById(int id)
        {
            var userBank = await _context.UserBanks.FindAsync(id);
            if (userBank == null)
            {
                return NotFound(new { Message = "UserBank not found." });
            }
            return Ok(userBank);
        }
        // GET: api/UserBank/{id}
        [HttpGet("current-user")]
        public async Task<IActionResult> GetUserBankCurrent()
        {
            // Lấy UserId từ JWT
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            var userBank = await _context.UserBanks.FindAsync(userId);
            if (userBank == null)
            {
                return NotFound(new { Message = "UserBank not found." });
            }
            return Ok(userBank);
        }

        // POST: api/UserBank
        [HttpPost]
        public async Task<IActionResult> CreateUserBank([FromBody] UserBank userBank)
        {
            if (userBank == null)
            {
                return BadRequest(new { Message = "Invalid data." });
            }

            // Check if UserId already exists
            var existingUserBank = await _context.UserBanks.FindAsync(userBank.UserId);
            if (existingUserBank != null)
            {
                return Conflict(new { Message = "UserBank with this UserId already exists." });
            }

            _context.UserBanks.Add(userBank);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserBankById), new { id = userBank.UserId }, userBank);
        }

        // PUT: api/UserBank/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserBank(int id, [FromBody] UserBank updatedUserBank)
        {
            if (id != updatedUserBank.UserId)
            {
                return BadRequest(new { Message = "UserId mismatch." });
            }

            var userBank = await _context.UserBanks.FindAsync(id);
            if (userBank == null)
            {
                return NotFound(new { Message = "UserBank not found." });
            }

            // Update fields
            userBank.BankName = updatedUserBank.BankName ?? userBank.BankName;
            userBank.BankNumber = updatedUserBank.BankNumber ?? userBank.BankNumber;
            userBank.OwnerName = updatedUserBank.OwnerName ?? userBank.OwnerName;

            _context.UserBanks.Update(userBank);
            await _context.SaveChangesAsync();

            return Ok(userBank);
        }

        // DELETE: api/UserBank/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserBank(int id)
        {
            var userBank = await _context.UserBanks.FindAsync(id);
            if (userBank == null)
            {
                return NotFound(new { Message = "UserBank not found." });
            }

            _context.UserBanks.Remove(userBank);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "UserBank deleted successfully." });
        }

        // POST: api/UserBank/current-user
        [HttpPost("current-user")]
        public async Task<IActionResult> CreateUserBankForCurrentUser([FromBody] UserBank userBank)
        {
            // Lấy UserId từ JWT
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Kiểm tra nếu UserBank cho UserId đã tồn tại
            var existingUserBank = await _context.UserBanks.FindAsync(userId);
            if (existingUserBank != null)
            {
                return Conflict(new { Message = "UserBank already exists for the current user." });
            }

            // Tạo UserBank cho người dùng hiện tại
            userBank.UserId = userId;
            _context.UserBanks.Add(userBank);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserBankById), new { id = userBank.UserId }, userBank);
        }

        // PUT: api/UserBank/current-user
        [HttpPut("current-user")]
        public async Task<IActionResult> UpdateUserBankForCurrentUser([FromBody] UserBank updatedUserBank)
        {
            // Lấy UserId từ JWT
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy UserBank của người dùng hiện tại
            var userBank = await _context.UserBanks.FindAsync(userId);
            if (userBank == null)
            {
                return NotFound(new { Message = "UserBank not found for the current user." });
            }

            // Cập nhật thông tin UserBank
            userBank.BankName = updatedUserBank.BankName ?? userBank.BankName;
            userBank.BankNumber = updatedUserBank.BankNumber ?? userBank.BankNumber;
            userBank.OwnerName = updatedUserBank.OwnerName ?? userBank.OwnerName;

            _context.UserBanks.Update(userBank);
            await _context.SaveChangesAsync();

            return Ok(userBank);
        }
        // PUT: api/UserBank/current-user
        [HttpPut("current-user-valid")]
        public async Task<IActionResult> UpdateUserBankForCurrentUserValid([FromBody] UserBank updatedUserBank)
        {
            // Lấy UserId từ JWT
            var userId = GetUserId();
            if (userId == -1)
            {
                return Unauthorized("Invalid token or user not found.");
            }

            // Lấy thông tin người dùng từ UserManager
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            // Lấy FullName và chuyển thành không dấu
            var fullNameWithoutDiacritics = RemoveDiacritics(user.FullName ?? string.Empty);

            // Kiểm tra OwnerName
            if (!string.Equals(fullNameWithoutDiacritics, RemoveDiacritics(updatedUserBank.OwnerName ?? string.Empty), StringComparison.OrdinalIgnoreCase))
            {
                return Conflict(new { Message = "Tài khoản không chính chủ." });
            }

            // Lấy UserBank của người dùng hiện tại
            var userBank = await _context.UserBanks.FindAsync(userId);
            if (userBank == null)
            {
                return NotFound(new { Message = "Tài khoản ngân hàng không chính chủ." });
            }

            // Kiểm tra nếu bankName và bankNumber đã tồn tại ở bản ghi khác
            var isDuplicate = await _context.UserBanks.AnyAsync(ub =>
                ub.BankName == updatedUserBank.BankName &&
                ub.BankNumber == updatedUserBank.BankNumber &&
                ub.UserId != userId);
            if (isDuplicate)
            {
                return Conflict(new { Message = "Đã tồn tại số tài khoản này trong hệ thống." });
            }

            // Cập nhật thông tin UserBank
            userBank.BankName = updatedUserBank.BankName ?? userBank.BankName;
            userBank.BankNumber = updatedUserBank.BankNumber ?? userBank.BankNumber;
            userBank.OwnerName = updatedUserBank.OwnerName ?? userBank.OwnerName;

            _context.UserBanks.Update(userBank);
            await _context.SaveChangesAsync();

            return Ok(userBank);
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var normalizedText = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
