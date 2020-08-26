using JunsBlog.Entities;
using JunsBlog.Helpers;
using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Authentication;
using JunsBlog.Models.Settings;
using JunsBlog.Test.Mockups;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Xunit;

namespace JunsBlog.Test.UnitTests
{
    public class JwtTokenHelperTest
    {
        private readonly User sampleUser;
        public JwtTokenHelperTest()
        {
            var regRequest = new RegisterRequest()
            {
                Email = "Tester@gmail.com",
                Name = "Tester",
                Password = "123456",
            };
            sampleUser = new User(regRequest);        
        }

        public static IEnumerable<Object[]> GetBadUsers()
        {
            yield return new object[] { new User(new RegisterRequest() { Email = "Tester@gmail.com", Name = String.Empty, Password = "123456" }) };
            yield return new object[] { null };
        }

        [Theory]
        [MemberData(nameof(GetBadUsers))]
        public void GenerateJwtToken_Should_Return_Null_With_Missing_Or_Empty_User_Info(User user)
        {
            JwtTokenHelper helper = new JwtTokenHelper(new JwtSettingsFake(), new DatabaseServiceFake());

            var token = helper.GenerateJwtToken(user);

            Assert.Null(token);
        }

        public static IEnumerable<Object[]> GetBadJwtSettings()
        {
            yield return new object[] { new JwtSettings() { Issuer=String.Empty, TokenSecret= "8FB83E91-6A85-423B-A950-8DF62942E343" } };
            yield return new object[] { new JwtSettings() { Issuer = "TesterIssuer.com", TokenSecret = String.Empty } };
        }

        [Theory]
        [MemberData(nameof(GetBadJwtSettings))]
        public void GenerateJwtToken_Should_Return_Null_With_Missing_Or_Empty_JwtSettings_Info(IJwtSettings settings)
        {
            JwtTokenHelper helper = new JwtTokenHelper(settings, new DatabaseServiceFake());
            var token = helper.GenerateJwtToken(sampleUser);

            Assert.Null(token);
        }

        [Fact]
        public void GenerateJwtToken_Should_Return_Not_Null_Token()
        {
            JwtTokenHelper helper = new JwtTokenHelper(new JwtSettingsFake(), new DatabaseServiceFake());
            var token = helper.GenerateJwtToken(sampleUser);

            Assert.NotNull(token);
        }

        [Fact]
        public void GenerateJwtToken_Should_Return_Token_With_Correct_Info()
        {
            var jwtSettings = new JwtSettingsFake();
            var helper = new JwtTokenHelper(jwtSettings, new DatabaseServiceFake());
            var token = helper.GenerateJwtToken(sampleUser);

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

            var claims = tokenHandler.ValidateToken(token, tokenValidationParams, out validatedToken);

            Assert.True(claims.Identity.IsAuthenticated);
        }
    }
}
