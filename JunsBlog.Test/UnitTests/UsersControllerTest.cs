using FluentAssertions;
using JunsBlog.Controllers;
using JunsBlog.Entities;
using JunsBlog.Helpers;
using JunsBlog.Interfaces;
using JunsBlog.Models.Authentication;
using JunsBlog.Test.Mockups;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Xunit;

namespace JunsBlog.Test.UnitTests
{
    public class UsersControllerTest
    {
        private const int TWO_SECONDS_IN_MILLIONSECONDS = 2000;
        private readonly RegisterRequest validRegisterRquest = new RegisterRequest() { Email = "Test@gmail.com", Name = "Tester", Password = "123456" };
        public static IEnumerable<Object[]> GetBadRegisterRequest()
        {
            yield return new object[] { new RegisterRequest() { Email = String.Empty, Name = "Tester", Password = "123456" } };
            yield return new object[] { new RegisterRequest() { Email = "Test@gmail.com", Name = String.Empty, Password = "123456" } };
            yield return new object[] { new RegisterRequest() { Email = "Test@gmail.com", Name = "Tester", Password = String.Empty } };
            yield return new object[] { new RegisterRequest() { Email = String.Empty, Name = String.Empty, Password = String.Empty } };
        }

        public static IEnumerable<Object[]> GetBadPasswordResetRequest()
        {
            yield return new object[] { new PasswordResetRequest() { Email = String.Empty, Password = "123456", ResetToken = "FakeResetToken" } };
            yield return new object[] { new PasswordResetRequest() { Email = "Test@gmail.com", Password = String.Empty, ResetToken = "FakeResetToken" } };
            yield return new object[] { new PasswordResetRequest() { Email = String.Empty, Password = String.Empty, ResetToken = String.Empty } };
        }

        private readonly UsersController usersController;
        private readonly IJwtTokenHelper jwtTokenHelper;
        public UsersControllerTest()
        {
            var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = logFactory.CreateLogger<UsersController>();
            var httpContextAccessFake = new HttpContextAccessorFake();
            var databaseService = new DatabaseServiceFake();
            var notificationServiceFake = new NotificationServiceFake();
            jwtTokenHelper = new JwtTokenHelper(new JwtSettingsFake());

            usersController = new UsersController(httpContextAccessFake, databaseService, jwtTokenHelper, notificationServiceFake, logger);
        }

        [Fact]
        public async void Register_ValidUserInfo_Success()
        {       
            var result = await usersController.Register(validRegisterRquest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var authResponse = okResult.Value.Should().BeAssignableTo<AuthenticateResponse>().Subject;

            authResponse.AccessToken.Should().NotBeNullOrEmpty();
            Assert.NotNull(jwtTokenHelper.ValidateToken(authResponse.AccessToken));
            authResponse.RefreshToken.Should().NotBeNullOrEmpty();
            authResponse.User.Should().NotBeNull();
            authResponse.User.Name.Should().Be(validRegisterRquest.Name);
            authResponse.User.Email.Should().Be(validRegisterRquest.Email);
            Assert.True(Utilities.ValidatePassword(validRegisterRquest.Password, authResponse.User.Password));

            authResponse.User.Role.Should().Be(Entities.Role.User);
            authResponse.User.Type.Should().Be(AccountType.Local);
            authResponse.User.UpdatedOn.Should().BeCloseTo(DateTime.UtcNow, TWO_SECONDS_IN_MILLIONSECONDS);
            authResponse.User.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TWO_SECONDS_IN_MILLIONSECONDS);
        }


        [Theory]
        [MemberData(nameof(GetBadRegisterRequest))]
        public async void Register_InvalidUserInfo_Failure(RegisterRequest request)
        {
            var result = await usersController.Register(request);
            var badResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }


        [Fact]
        public async void Register_DuplicateUser_Failure()
        {
            var result = await usersController.Register(validRegisterRquest) ;
            result.Should().BeOfType<OkObjectResult>();
            result = await usersController.Register(validRegisterRquest);
            var badResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }


        [Fact]
        public async void Authenicate_ValidUser_Success()
        {
            await usersController.Register(validRegisterRquest);
            var authRequest = new AuthenticateRequest()
            {
                Email = validRegisterRquest.Email,
                Password = validRegisterRquest.Password
            };
            var result = await usersController.Authenticate(authRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var authResponse = okResult.Value.Should().BeAssignableTo<AuthenticateResponse>().Subject;

            authResponse.AccessToken.Should().NotBeNullOrEmpty();
            Assert.NotNull(jwtTokenHelper.ValidateToken(authResponse.AccessToken));
            authResponse.RefreshToken.Should().NotBeNullOrEmpty();
            authResponse.User.Should().NotBeNull();
            authResponse.User.Name.Should().Be(validRegisterRquest.Name);
            authResponse.User.Email.Should().Be(validRegisterRquest.Email);
            Assert.True(Utilities.ValidatePassword(validRegisterRquest.Password, authResponse.User.Password));

            authResponse.User.Role.Should().Be(Entities.Role.User);
            authResponse.User.Type.Should().Be(AccountType.Local);
        }


        [Fact]
        public async void Authenicate_InavlidPassword_Failure()
        {
            var result = await usersController.Register(validRegisterRquest);
            result.Should().BeOfType<OkObjectResult>();
            var authRequest = new AuthenticateRequest()
            {
                Email = validRegisterRquest.Email,
                Password = validRegisterRquest.Password + "extra characters"
            };

            result = await usersController.Authenticate(authRequest);
            var badResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async void Authenicate_NoneExistingUser_Failure()
        {
            var request = new AuthenticateRequest() { Email = "Test@gmail.com", Password = "123456" };

            var result = await usersController.Authenticate(request) as ObjectResult;
            var badResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }


        [Fact]
        public async void Reset_ResetTokenRequest_ValidRequest_Success()
        {
            await usersController.Register(validRegisterRquest);
            var result = await usersController.VerifyResetEmail(validRegisterRquest.Email);
            var okResult = result.Should().BeOfType<OkResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async void Reset_ResetTokenRequestNonExistingUser_Failure(string email)
        {
            var result = await usersController.VerifyResetEmail(email);
            var badResult = result.Should().BeOfType<StatusCodeResult>().Subject;
            badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Theory]
        [MemberData(nameof(GetBadPasswordResetRequest))]
        public async void Reset_IncompleteUserInfo_Failure(PasswordResetRequest request)
        {
            var result = await usersController.Register(validRegisterRquest);
            result.Should().BeOfType<OkObjectResult>();
            result = await usersController.ResetPassword(request);
            var badResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

    }
}
