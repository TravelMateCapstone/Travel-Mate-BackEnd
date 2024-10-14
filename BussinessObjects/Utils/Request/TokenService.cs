using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BussinessObjects.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BussinessObjects.Utils.Request
{
    public class TokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            // Retrieve secrets from Azure Key Vault
            var keyVaultUrl = new Uri("https://travelmatekeyvault.vault.azure.net/");
            var client = new SecretClient(vaultUri: keyVaultUrl, credential: new DefaultAzureCredential());

            KeyVaultSecret jwtSecretKey = (await client.GetSecretAsync("JwtSecretKey"));
            KeyVaultSecret jwtIssuer = (await client.GetSecretAsync("JwtIssuer"));
            KeyVaultSecret jwtAudience = (await client.GetSecretAsync("JwtAudience"));
            KeyVaultSecret jwtDurationInMinutes = (await client.GetSecretAsync("JwtDurationInMinutes"));

            // Use the retrieved secrets
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey.Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Get user roles from UserManager
            var roles = await _userManager.GetRolesAsync(user);

            // Create claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create token
            var token = new JwtSecurityToken(
                issuer: jwtIssuer.Value,
                audience: jwtAudience.Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtDurationInMinutes.Value)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
