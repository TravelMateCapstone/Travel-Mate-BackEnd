using BussinessObjects.Entities;
using BussinessObjects.Utils.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IConfiguration _configuration;

        //public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        //{
        //    _userManager = userManager;
        //    _configuration = configuration;
        //}

        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginDTO model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Username);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var authClaims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

        //        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        //        var token = new JwtSecurityToken(
        //            issuer: _configuration["Jwt:Issuer"],
        //            audience: _configuration["Jwt:Audience"],
        //            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireTime"])),
        //            claims: authClaims,
        //            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //        );

        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token),
        //            expiration = token.ValidTo
        //        });
        //    }
        //    return Unauthorized();
        //}

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        //{
        //    // Kiểm tra mật khẩu xác nhận có khớp không
        //    if (model.Password != model.ConfirmPassword)
        //    {
        //        return BadRequest("Mật khẩu và xác nhận mật khẩu không khớp.");
        //    }

        //    var userExists = await _userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Người dùng đã tồn tại!" });

        //    ApplicationUser user = new ApplicationUser()
        //    {
        //        Email = model.Email,
        //        SecurityStamp = Guid.NewGuid().ToString(),
        //        UserName = model.Username
        //    };

        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi khi tạo người dùng!" });
        //    }

        //    return Ok(new { message = "Người dùng đã đăng ký thành công!" });
        //}

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
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

            // Sau khi đăng ký thành công, có thể đăng nhập tự động và trả về token
            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }
}
