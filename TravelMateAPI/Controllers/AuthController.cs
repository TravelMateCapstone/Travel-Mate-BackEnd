﻿using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BusinessObjects;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelMateAPI.Services;
using TravelMateAPI.Services.Email;
using Google.Apis.Auth;

namespace TravelMateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IMailServiceSystem _mailService;
        private readonly ApplicationDBContext _context;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TokenService tokenService, IMailServiceSystem mailService, ApplicationDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mailService = mailService;
            _context = context;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            // Kiểm tra xem username có tồn tại không
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                return BadRequest("Tên đăng nhập không tồn tại.");
            }

            // Kiểm tra email đã xác nhận chưa
            if (!user.EmailConfirmed)
            {
                return BadRequest("Vui lòng xác nhận email trước khi đăng nhập.");
            }

            // Kiểm tra mật khẩu
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return BadRequest("Mật khẩu không chính xác.");
            }

            // Nếu đăng nhập thành công, tạo token và trả về
            var token = await _tokenService.GenerateToken(user);
            var image = $"https://travelmateapp.azurewebsites.net/api/Profile/current-user/image";
            var link = token + " " + image;

            // Thay thế {userId} bằng giá trị thực tế từ user.Id
            var avatarUrl = $"https://travelmateapp.azurewebsites.net/api/Profile/current-user/image";

            //return Ok(new { Token = token, AvataUrl = avatarUrl });
            return Ok(new { Token = token });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            // Kiểm tra xem username hoặc email đã tồn tại chưa
            if (await _userManager.FindByNameAsync(registerDto.Username) != null)
            {
                return BadRequest("Tên đăng nhập đã tồn tại");
            }

            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return BadRequest("Email đăng nhập đã tồn tại");
            }
            //  Kiểm tra mật khẩu xác nhận có khớp không
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return BadRequest("Mật khẩu và xác nhận mật khẩu không khớp.");
            }
            // Kiểm tra nếu dữ liệu không hợp lệ
            if (!ModelState.IsValid)
            {
                // Trả về lỗi xác thực với chi tiết lỗi trong ModelState
                return BadRequest(ModelState);
            }
            // Tạo đối tượng ApplicationUser nhưng chưa lưu vào hệ thống
            var user = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                EmailConfirmed = true,
                RegistrationTime = GetTimeZone.GetVNTimeZoneNow() // Lưu thời gian đăng ký
            };

            // Tạo người dùng nhưng chưa kích hoạt
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            // Thêm người dùng vào vai trò "User"
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            // Tạo bản ghi Profile mặc định
            var defaultProfile = new Profile
            {
                UserId = user.Id, 
                FirstName = "",
                LastName = "",
                Address = "",
                Phone = "",
                Gender = "",
                City = "",
                Description = "",
                HostingAvailability = "",
                WhyUseTravelMate = "",
                MusicMoviesBooks = "",
                WhatToShare = "",
                ImageUser= "https://img.freepik.com/premium-vector/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-vector-illustration_561158-3467.jpg"
            };
            _context.Profiles.Add(defaultProfile);

            // Tạo bản ghi UserHome mặc định
            var defaultUserHome = new UserHome
            {
                UserId = user.Id,
                MaxGuests = 0,
                GuestPreferences = "",
                AllowedSmoking = "",
                RoomDescription = "",
                RoomType = "",
                RoomMateInfo = "",
                Amenities = "",
                Transportation = "",
                OverallDescription = ""
            };
            _context.UserHomes.Add(defaultUserHome);

            // Tạo bản ghi UserContact mặc định
            var defaultUserContact = new UserContact
            {
                UserId = user.Id,
                Name = "",
                Phone = "",
                Email = "",
                NoteContact =""

            };
            _context.UserContacts.Add(defaultUserContact);

            //Tạo thông tin mặc định cho UserBank

            var defaultUserBank = new UserBank
            {
                UserId = user.Id,
                BankName = "",
                BankNumber = "",
                OwnerName = ""

            };
            _context.UserBanks.Add(defaultUserBank);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            //var roleExist = await _roleManager.RoleExistsAsync("User");
            //if (!roleExist)
            //{
            //    await _roleManager.CreateAsync(new IdentityRole("User"));
            //}

            // Tạo token xác thực email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Tạo liên kết xác nhận email
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = token }, Request.Scheme);

            // Gửi email xác nhận
            MailContent content = new MailContent
            {
                To = user.Email,
                Subject = "Xác nhận tài khoản - Travel Mate",
                //Body = $"<p>Xin chào, vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết bên dưới:</p><a href='{confirmationLink}'>Xác nhận email</a>"
                Body = $"<p>Xin chào, chào mừng bạn đến với Travel Mate ♥</a>"
            };

            await _mailService.SendMail(content);

            return Ok("Đăng ký thành công. Vui lòng kiểm tra email để xác nhận tài khoản.");
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            try
            {
                // 1. Xác thực Google ID token
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                if (payload is null || string.IsNullOrEmpty(payload.Email))
                {
                    return Unauthorized("Invalid Google token.");
                }

                // 2. Kiểm tra tài khoản trong hệ thống
                var user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    // Nếu chưa có tài khoản, tiến hành đăng ký
                    user = new ApplicationUser
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        FullName = payload.Name,
                        EmailConfirmed = true, // Email đã được xác nhận qua Google
                        RegistrationTime = GetTimeZone.GetVNTimeZoneNow()
                    };

                    var createUserResult = await _userManager.CreateAsync(user);
                    if (!createUserResult.Succeeded)
                    {
                        return BadRequest(new { message = "User registration failed", errors = createUserResult.Errors });
                    }

                    // Gán quyền "User" cho tài khoản
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!addToRoleResult.Succeeded)
                    {
                        return BadRequest(new { message = "Failed to assign role", errors = addToRoleResult.Errors });
                    }

                    // Tạo các bản ghi mặc định (Profile, UserHome)
                    var defaultProfile = new Profile
                    {
                        UserId = user.Id,
                        FirstName = "",
                        LastName = "",
                        Address = "",
                        Phone = "",
                        Gender = "",
                        City = "",
                        Description = "",
                        HostingAvailability = "",
                        WhyUseTravelMate = "",
                        MusicMoviesBooks = "",
                        WhatToShare = "",
                        ImageUser = "https://img.freepik.com/premium-vector/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-vector-illustration_561158-3467.jpg"
                    };
                    _context.Profiles.Add(defaultProfile);

                    var defaultUserHome = new UserHome
                    {
                        UserId = user.Id,
                        MaxGuests = 0,
                        GuestPreferences = "",
                        AllowedSmoking = "",
                        RoomDescription = "",
                        RoomType = "",
                        RoomMateInfo = "",
                        Amenities = "",
                        Transportation = "",
                        OverallDescription = ""
                    };
                    _context.UserHomes.Add(defaultUserHome);


                    // Tạo bản ghi UserContact mặc định
                    var defaultUserContact = new UserContact
                    {
                        UserId = user.Id,
                        Name = "",
                        Phone = "",
                        Email = "",
                        NoteContact = ""

                    };
                    _context.UserContacts.Add(defaultUserContact);


                    //Tạo thông tin mặc định cho UserBank

                    var defaultUserBank = new UserBank
                    {
                        UserId = user.Id,
                        BankName = "",
                        BankNumber = "",
                        OwnerName = ""

                    };
                    _context.UserBanks.Add(defaultUserBank);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    // Gửi email chào mừng
                    var content = new MailContent
                    {
                        To = user.Email,
                        Subject = "Chào mừng đến với Travel Mate!",
                        Body = $"<p>Xin chào {user.FullName}, chào mừng bạn đến với Travel Mate ♥</p>"
                    };
                    await _mailService.SendMail(content);
                }

                // 3. Tạo token cho người dùng
                var token = await _tokenService.GenerateToken(user);

                // 4. Trả về token
                return Ok(new
                {
                    message = user.EmailConfirmed ? "Login successful" : "Registration successful",
                    token = token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
            }
        }


        ////Link đăng nhập bằng google :  URL/api/auth/externallogin?provider=Google
        //[HttpGet("external-login")]
        //public IActionResult ExternalLogin(string provider)
        //{
        //    var redirectUrl = Url.Action("ExternalLoginCallback", "Auth");
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return Challenge(properties, provider);
        //}

        //[HttpGet("external-login-callback")]
        //public async Task<IActionResult> ExternalLoginCallback()
        //{
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return BadRequest("Lỗi khi đăng nhập bằng tài khoản Google.");
        //    }

        //    // Tìm kiếm người dùng trong hệ thống
        //    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //    var user = await _userManager.FindByEmailAsync(email);

        //    if (user == null)
        //    {
        //        // Nếu người dùng chưa tồn tại, tạo mới và thêm vào vai trò "User"
        //        user = new ApplicationUser
        //        {
        //            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
        //            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
        //            FullName = info.Principal.FindFirstValue(ClaimTypes.Name)
        //        };

        //        var result = await _userManager.CreateAsync(user);
        //        if (!result.Succeeded) return BadRequest("Không thể tạo người dùng.");

        //        // Thêm người dùng vào vai trò "User"
        //        await _userManager.AddToRoleAsync(user, "User");

        //        // Thêm thông tin xác thực bên ngoài
        //        await _userManager.AddLoginAsync(user, info);
        //    }

        //    // Đăng nhập người dùng
        //    await _signInManager.SignInAsync(user, isPersistent: false);

        //    // Tạo và trả về JWT token
        //    var token = _tokenService.GenerateToken(user);

        //    return Ok(new { Token = token });
        //}

        //[HttpPost("google-login")]
        //public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        //{
        //    // 1. Xác thực ID token với Google để lấy thông tin người dùng
        //    //var payload = await VerifyGoogleTokenAsync(idToken);
        //    // Xác thực token và lấy payload
        //    var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
        //    if (payload is null)
        //    {
        //        return Unauthorized("Invalid Google token.");
        //    }

        //    // 2. Kiểm tra xem người dùng đã có trong hệ thống chưa
        //    var user = await _userManager.FindByEmailAsync(payload.Email);
        //    if (user == null)
        //    {
        //        // Nếu chưa có, tạo người dùng mới
        //        user = new ApplicationUser
        //        {
        //            UserName = payload.Email,
        //            Email = payload.Email,
        //            FullName = payload.Name, // Lưu tên người dùng nếu cần
        //            EmailConfirmed = true,
        //            RegistrationTime = GetTimeZone.GetVNTimeZoneNow()
        //        };
        //        //var result = await _userManager.CreateAsync(user);
        //        //if (!result.Succeeded)
        //        //{
        //        //    return BadRequest(result.Errors);
        //        //}
        //        // Tạo người dùng nhưng chưa kích hoạt
        //        var result = await _userManager.CreateAsync(user);
        //        if (!result.Succeeded) return BadRequest(result.Errors);

        //        // Thêm người dùng vào vai trò "User"
        //        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        //        if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

        //        // Tạo bản ghi Profile mặc định
        //        var defaultProfile = new Profile
        //        {
        //            UserId = user.Id,
        //            FirstName = "",
        //            LastName = "",
        //            Address = "",
        //            Phone = "",
        //            Gender = "",
        //            City = "",
        //            Description = "",
        //            HostingAvailability = "",
        //            WhyUseTravelMate = "",
        //            MusicMoviesBooks = "",
        //            WhatToShare = "",
        //            ImageUser = "https://img.freepik.com/premium-vector/default-avatar-profile-icon-social-media-user-image-gray-avatar-icon-blank-profile-silhouette-vector-illustration_561158-3467.jpg"
        //        };
        //        _context.Profiles.Add(defaultProfile);

        //        // Tạo bản ghi UserHome mặc định
        //        var defaultUserHome = new UserHome
        //        {
        //            UserId = user.Id,
        //            MaxGuests = 0,
        //            GuestPreferences = "",
        //            AllowedSmoking = "",
        //            RoomDescription = "",
        //            RoomType = "",
        //            RoomMateInfo = "",
        //            Amenities = "",
        //            Transportation = "",
        //            OverallDescription = ""
        //        };
        //        _context.UserHomes.Add(defaultUserHome);

        //        // Lưu thay đổi vào cơ sở dữ liệu
        //        await _context.SaveChangesAsync();

        //        // Tạo token xác thực email
        //        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        //        // Tạo liên kết xác nhận email
        //        var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = token }, Request.Scheme);

        //        // Gửi email xác nhận
        //        MailContent content = new MailContent
        //        {
        //            To = user.Email,
        //            Subject = "Xác nhận tài khoản - Travel Mate",
        //            //Body = $"<p>Xin chào, vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết bên dưới:</p><a href='{confirmationLink}'>Xác nhận email</a>"
        //            Body = $"<p>Xin chào, chào mừng bạn đến với Travel Mate ♥</a>"
        //        };

        //        await _mailService.SendMail(content);

        //    }

        //    // 3. Đăng nhập người dùng
        //    await _signInManager.SignInAsync(user, isPersistent: false);

        //    return Ok(new { message = "Login successful" });
        //}



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDto)
        {
            // Tìm người dùng theo email
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Không thông báo cụ thể để tránh bị lộ thông tin
                return BadRequest("Nếu email tồn tại, chúng tôi đã gửi liên kết đặt lại mật khẩu.");
            }

            // Tạo token đặt lại mật khẩu
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Tạo liên kết để người dùng đặt lại mật khẩu
            var resetLink = Url.Action("ResetPassword", "Auth", new { token, email = user.Email }, Request.Scheme);

            // Gửi email với liên kết đặt lại mật khẩu
            MailContent content = new MailContent
            {
                To = user.Email,
                Subject = "Đặt lại mật khẩu",
                Body = $"<p>Vui lòng nhấp vào liên kết bên dưới để đặt lại mật khẩu của bạn:</p><a href='{resetLink}'>Đặt lại mật khẩu</a>"
            };

            await _mailService.SendMail(content);

            return Ok("Đã gửi liên kết đặt lại mật khẩu. Vui lòng kiểm tra email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            // Tìm người dùng theo email
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return BadRequest("Email không hợp lệ.");
            }
            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
            {
                return BadRequest("Mật khẩu và xác nhận mật khẩu không khớp.");
            }
            // Đặt lại mật khẩu
            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (!resetPassResult.Succeeded)
            {
                return BadRequest(resetPassResult.Errors);
            }

            return Ok("Mật khẩu đã được đặt lại thành công.");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            // Tìm người dùng theo userId
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Người dùng không tồn tại.");
            }

            // Xác nhận email với token
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                // Email đã xác nhận, kích hoạt tài khoản
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user); // Cập nhật thông tin người dùng

                return Ok("Email đã được xác nhận thành công. Tài khoản của bạn đã được kích hoạt.");
            }

            return BadRequest("Xác nhận email thất bại.");
        }

        [HttpPost("send-mail")]
        public async Task<IActionResult> SendMail([FromBody] MailContent mailContent)
        {

            if (string.IsNullOrEmpty(mailContent.To) || string.IsNullOrEmpty(mailContent.Subject) || string.IsNullOrEmpty(mailContent.Body))
            {
                return BadRequest("Email, subject, and body are required.");
            }

            await _mailService.SendMail(mailContent);
            return Ok("Email has been sent.");

        }

        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            // Lấy UserId từ Claims
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            // Kiểm tra nếu userIdString là null
            if (string.IsNullOrEmpty(userIdString))
            {
                return NotFound("UserId not found in token.");
            }

            // Chuyển UserId từ string sang int
            if (int.TryParse(userIdString, out int userId))
            {
                return Ok(userId); // Trả về userId dưới dạng int
            }
            else
            {
                return BadRequest($"Invalid UserId format.Value: {userIdString}");
            }
        }
        [HttpGet("Get-fullname-Image")]
        public IActionResult GetFullName()
        {
            // Lấy giá trị của claim "FullName" từ token
            var fullName = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
            var image =User.Claims.FirstOrDefault(c => c.Type == "ImageUser")?.Value;
            if (string.IsNullOrEmpty(fullName))
            {
                return NotFound("FullName không tồn tại trong token.");
            }
            return Ok(new { FullName = fullName, ImageUser= image });
        }
        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            // Lấy tất cả các claims
            var claims = User.Claims.Select(c => new
            {
                c.Type,
                c.Value
            }).ToList();

            // Tìm userId từ claims
            var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            // Trả về kết quả
            if (userIdClaim != null)
            {
                // Chuyển đổi userId từ string sang int nếu cần
                if (int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Ok(new
                    {
                        UserId = userId
                    });
                }
                else
                {
                    return BadRequest($"Invalid UserId format. Value: {userIdClaim.Value}");
                }
            }
            else
            {
                return NotFound("UserId not found in claims.");
            }
        }


        //ADMIN
        [HttpPost("login-admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginDTO loginDto)
        {
            // Kiểm tra xem username có tồn tại không
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                return BadRequest("Tên đăng nhập không tồn tại.");
            }

            // Kiểm tra mật khẩu
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return BadRequest("Mật khẩu không chính xác.");
            }

            // Nếu đăng nhập thành công, tạo token và trả về
            var token = _tokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }


        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDTO registerDto)
        {
            // Kiểm tra xem username hoặc email đã tồn tại chưa
            if (await _userManager.FindByNameAsync(registerDto.Username) != null)
            {
                return BadRequest("Tên đăng nhập đã tồn tại");
            }

            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return BadRequest("Email đăng nhập đã tồn tại");
            }
            //  Kiểm tra mật khẩu xác nhận có khớp không
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return BadRequest("Mật khẩu và xác nhận mật khẩu không khớp.");
            }
            // Tạo đối tượng ApplicationUser nhưng chưa lưu vào hệ thống
            var user = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                EmailConfirmed = false,
                RegistrationTime = GetTimeZone.GetVNTimeZoneNow() // Lưu thời gian đăng ký
            };

            // Tạo người dùng nhưng chưa kích hoạt
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            // Thêm người dùng vào vai trò "User"
            var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            // Tạo token xác thực email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Tạo liên kết xác nhận email
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = token }, Request.Scheme);

            // Gửi email xác nhận
            MailContent content = new MailContent
            {
                To = user.Email,
                Subject = "Xác nhận tài khoản ADMIN - Travel Mate",
                Body = $"<p>Xin chào ADMIN, vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết bên dưới:</p><a href='{confirmationLink}'>Xác nhận email</a>"
            };

            await _mailService.SendMail(content);

            return Ok("Đăng ký ADMIN thành công. Vui lòng kiểm tra email để xác nhận tài khoản.");
        }
        [HttpPost("log-out")]
        public async Task<IActionResult> Logout()
        {
            // Hủy đăng nhập người dùng
            await _signInManager.SignOutAsync();

            // Xóa thông tin người dùng khỏi claims
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // Xóa tất cả các claims
                foreach (var claim in claimsIdentity.Claims.ToList())
                {
                    claimsIdentity.RemoveClaim(claim);
                }
            }

            // Trả về phản hồi thành công
            return Ok(new { message = "Logout successful" });
        }

        //[HttpGet]
        //public IActionResult GetKeys()
        //{
        //    var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
        //    var googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");

        //    return Ok(new
        //    {
        //        GoogleClientId = googleClientId,
        //        GoogleClientSecret = googleClientSecret
        //    });
        //}

        [HttpGet]
        public async Task<IActionResult> GetKeysAsync()
        {
            var keyVaultUrl = new Uri("https://travelmatekeyvault.vault.azure.net/");
            var client = new SecretClient(vaultUri: keyVaultUrl, credential: new DefaultAzureCredential());
            KeyVaultSecret jwtSecretKey = (await client.GetSecretAsync("JwtSecretKey"));
            KeyVaultSecret jwtIssuer = (await client.GetSecretAsync("JwtIssuer"));
            KeyVaultSecret jwtAudience = (await client.GetSecretAsync("JwtAudience"));
            KeyVaultSecret jwtDurationInMinutes = (await client.GetSecretAsync("JwtDurationInMinutes"));
            var googleClientId = (await client.GetSecretAsync("GoogleClientID")).Value.Value;
            var googleClientSecret = (await client.GetSecretAsync("GoogleClientSecret")).Value.Value;

            return Ok(new
            {
                GoogleClientId = googleClientId,
                GoogleClientSecret = googleClientSecret,
                JwtSecretKey = jwtSecretKey,
                JwtIssuer = jwtIssuer
            });
        }
    }
}
