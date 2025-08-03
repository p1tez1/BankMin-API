using BankMin_API.Infrastructure.Entity;
using BankMin_API.Services.IServices;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankMin_API.Services
{
    public class JWTGenerator: IJWTGenerator
    {
        private readonly IRoleRepo _roleRepo;
        private readonly IConfiguration _configuration;
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;

        public JWTGenerator(IConfiguration configuration, IRoleRepo roleRepo)
        {
            _configuration = configuration;
            _key = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
            _issuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
            _audience = _configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
            _roleRepo = roleRepo;
        }

        public async Task<string> GenerateToken(User user)
        {
            var role = await _roleRepo.GetRoleByUserIdAsync(user.Id);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
