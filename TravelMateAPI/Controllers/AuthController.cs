using BussinessObjects.Entities;
using BussinessObjects.Utils.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelMateAPI.Services.Email;

namespace TravelMateAPI.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IMailServiceSystem _mailService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TokenService tokenService, IMailServiceSystem mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mailService = mailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();

            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            // Kiểm tra xem username hoặc email đã tồn tại chưa
            if (await _userManager.FindByNameAsync(registerDto.Username) != null)
            {
                return BadRequest("Username is already taken.");
            }

            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return BadRequest("Email is already registered.");
            }
            //  Kiểm tra mật khẩu xác nhận có khớp không
            //if (registerDto.Password != registerDto.ConfirmPassword)
            //{
            //    return BadRequest("Mật khẩu và xác nhận mật khẩu không khớp.");
            //}

            // Tạo một đối tượng CustomUser mới
            var user = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.FullName // Bạn có thể thêm thuộc tính khác
            };

            // Tạo người dùng với password
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            //gửi mail 
            MailContent content = new MailContent
            {
                To = user.Email,
                Subject = "Travel Mate",
                Body = "<p><strong>Xin chào ❤ Hãy trải nhiệm ở trên Travel Mate </strong></p> <a href='https://travelmateapp.azurewebsites.net/'>Bấm vào đây</a>"
            };

            await _mailService.SendMail(content);

            // Sau khi đăng ký thành công, có thể đăng nhập tự động và trả về token
            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }
        [HttpPost("sendmail")]
        public async Task<IActionResult> SendMail([FromBody] MailContent mailContent)
        {

            if (string.IsNullOrEmpty(mailContent.To) || string.IsNullOrEmpty(mailContent.Subject) || string.IsNullOrEmpty(mailContent.Body))
            {
                return BadRequest("Email, subject, and body are required.");
            }

            await _mailService.SendMail(mailContent);
            return Ok("Email has been sent.");

        }
    }
}
