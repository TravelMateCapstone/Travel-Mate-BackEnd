using BussinessObjects.Entities;
using BussinessObjects.Utils.Reponse;
using BussinessObjects.Utils.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();

            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
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

            // Sau khi đăng ký thành công, có thể đăng nhập tự động và trả về token
            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }
}
