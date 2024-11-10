using BusinessObjects.Configuration;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessObjects.Utils.Request
{
    public class TokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly ApplicationDBContext _context;

        public TokenService(UserManager<ApplicationUser> userManager, AppSettings appSettings, ApplicationDBContext context)
        {
            _userManager = userManager;
            _appSettings = appSettings;
            _context = context;
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            // Use the retrieved secrets
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Get user roles from UserManager
            var roles = await _userManager.GetRolesAsync(user);


            // Lấy thông tin Profile từ UserId
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id);

            // Create claims
            var claims = new List<Claim>
            {
                //new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("FullName", user.FullName ?? string.Empty) , // Thêm FullName vào claim
                 new Claim("ImageUser", profile.ImageUser ?? string.Empty)
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create token
            var token = new JwtSecurityToken(
                issuer: _appSettings.JwtSettings.Issuer,
                audience: _appSettings.JwtSettings.Audience,
                claims: claims,
                //expires: DateTime.Now.AddMinutes(Convert.ToDouble(_appSettings.JwtSettings.DurationInMinutes)),  //bỏ để không cần reload token
                //expires: DateTime.Now.AddMinutes(Convert.ToDouble(1440)),
                expires: DateTime.Now.AddDays(7), // Đặt thời hạn 7 ngày
                signingCredentials: creds
            );

            //return new JwtSecurityTokenHandler().WriteToken(token);
            return $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}";
        }
    }
}
