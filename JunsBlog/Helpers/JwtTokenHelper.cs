using JunsBlog.Entities;
using JunsBlog.Interfaces;
using JunsBlog.Interfaces.Services;
using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Authentication;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JunsBlog.Helpers
{
    public class JwtTokenHelper : IJwtTokenHelper
    {
        private readonly IJwtSettings jwtSettings;
        private readonly IDatabaseService databaseService;

        public JwtTokenHelper(IJwtSettings jwtSettings, IDatabaseService databaseService)
        {
            this.jwtSettings = jwtSettings;
            this.databaseService = databaseService;
        }

        public string GenerateJwtToken(User user)
        {
            if (user == null || String.IsNullOrWhiteSpace(user.Id) || String.IsNullOrWhiteSpace(user.Role) || String.IsNullOrWhiteSpace(user.Name)
                || jwtSettings == null || String.IsNullOrWhiteSpace(jwtSettings.TokenSecret)|| String.IsNullOrWhiteSpace(jwtSettings.Issuer)) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.TokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Audience = jwtSettings.Issuer,
                Issuer = jwtSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Claim ValidateToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.TokenSecret))
            };

            SecurityToken validatedToken;

            var claims = tokenHandler.ValidateToken(accessToken, tokenValidationParams, out validatedToken);

            if (claims.Identity.IsAuthenticated)
            {
                return claims.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            }

            return null;
        }

    }
}
