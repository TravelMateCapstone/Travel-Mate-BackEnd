using BusinessObjects.Entities;
using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserContactController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public UserContactController(ApplicationDBContext context)
        {
            _context = context;
        }

        // Phương thức để lấy UserId từ JWT token
        private int GetUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdString, out var userId) ? userId : -1;
        }

        // GET: api/UserContact
        [HttpGet]
        public async Task<IActionResult> GetAllUserContacts()
        {
            var userContacts = await _context.UserContacts.ToListAsync();
            return Ok(userContacts);
        }

        // GET: api/UserContact/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserContactById(int id)
        {
            var userContact = await _context.UserContacts.FindAsync(id);
            if (userContact == null)
            {
                return NotFound(new { Message = "UserContact not found." });
            }
            return Ok(userContact);
        }
        // GET: api/UserContact/GetByUserId/{userId}
        [HttpGet("GetByUserId/{userId}")]
        public async Task<IActionResult> GetContactsByUserId(int userId)
        {
            // Kiểm tra xem UserId có hợp lệ hay không
            if (userId <= 0)
            {
                return BadRequest(new { Message = "Invalid UserId." });
            }

            // Truy vấn danh sách UserContact theo UserId
            var userContacts = await _context.UserContacts
                .Where(uc => uc.UserId == userId)
                .ToListAsync();

            // Kiểm tra nếu không có dữ liệu
            if (userContacts == null || !userContacts.Any())
            {
                return NotFound(new { Message = "No contacts found for this UserId." });
            }

            return Ok(userContacts);
        }

        // POST: api/UserContact
        [HttpPost]
        public async Task<IActionResult> CreateUserContact([FromBody] UserContact userContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserContacts.Add(userContact);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserContactById), new { id = userContact.UserContactId }, userContact);
        }

        // POST: api/UserContact/AddForCurrentUser
        [HttpPost("AddContact-CurrentUser")]
        public async Task<IActionResult> AddUserContactForCurrentUser([FromBody] UserContact userContact)
        {
            // Lấy UserId từ thông tin xác thực của người dùng hiện tại
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy UserId từ claim

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Gán UserId cho UserContact
            userContact.UserId = int.Parse(userId);

            // Thêm UserContact vào cơ sở dữ liệu
            _context.UserContacts.Add(userContact);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserContactById), new { id = userContact.UserContactId }, userContact);
        }

        // PUT: api/UserContact/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserContact(int id, [FromBody] UserContact updatedUserContact)
        {
            if (id != updatedUserContact.UserContactId)
            {
                return BadRequest(new { Message = "UserContact ID mismatch." });
            }

            var existingUserContact = await _context.UserContacts.FindAsync(id);
            if (existingUserContact == null)
            {
                return NotFound(new { Message = "UserContact not found." });
            }

            // Update fields
            existingUserContact.Name = updatedUserContact.Name;
            existingUserContact.Phone = updatedUserContact.Phone;
            existingUserContact.Email = updatedUserContact.Email;
            existingUserContact.NoteContact = updatedUserContact.NoteContact;

            await _context.SaveChangesAsync();

            return Ok(existingUserContact);
        }
        // PUT: api/UserContact/UpdateCurrentUserContact/{id}
        [HttpPut("Update-Contact-CurrentUser")]
        public async Task<IActionResult> UpdateCurrentUserContact( [FromBody] UserContact updatedContact)
        {
            // Lấy UserId của người dùng hiện tại từ token JWT
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Kiểm tra dữ liệu đầu vào
            if (updatedContact == null )
            {
                return BadRequest(new { Message = "Invalid data." });
            }

            // Tìm kiếm UserContact theo ID và UserId
            var existingContact = await _context.UserContacts
                .FirstOrDefaultAsync(uc => uc.UserId == currentUserId);

            // Kiểm tra nếu không tìm thấy
            if (existingContact == null)
            {
                return NotFound(new { Message = "Contact not found or you are not authorized to update it." });
            }

            // Cập nhật thông tin
            existingContact.Name = updatedContact.Name ?? existingContact.Name;
            existingContact.Phone = updatedContact.Phone ?? existingContact.Phone;
            existingContact.Email = updatedContact.Email ?? existingContact.Email;
            existingContact.NoteContact = updatedContact.NoteContact ?? existingContact.NoteContact;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Contact updated successfully.", Contact = existingContact });
        }

        // DELETE: api/UserContact/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserContact(int id)
        {
            var userContact = await _context.UserContacts.FindAsync(id);
            if (userContact == null)
            {
                return NotFound(new { Message = "UserContact not found." });
            }

            _context.UserContacts.Remove(userContact);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
