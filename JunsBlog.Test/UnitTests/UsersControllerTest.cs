﻿using JunsBlog.Controllers;
using JunsBlog.Helpers;
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
        private readonly UsersController usersController; 

        public UsersControllerTest()
        {
            var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = logFactory.CreateLogger<UsersController>();
            var httpContextAccessFake = new HttpContextAccessorFake();
            var databaseService = new DatabaseServiceFake();
            var jwtTokenHelper = new JwtTokenHelper(new JwtSettingsFake());
            var notificationServiceFake = new NotificationServiceFake();

            usersController = new UsersController(httpContextAccessFake, databaseService, jwtTokenHelper, notificationServiceFake, logger);
        }

        [Fact]
        public async void Register_New_User_Should_Return_Status_OK()
        {
            var model = new RegisterRequest()
            {
                Email = "Test@gmail.com",
                Name = "Tester",
                Password = "123456"
            };

           var result = await usersController.Register(model);
           Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async void Register_Duplicate_User_Should_Return_BadRequest()
        {
            var testUserRegRequest = new RegisterRequest() { Email = "Test@gmail.com", Name = "Tester", Password = "123456" };

            var okResult = await usersController.Register(testUserRegRequest) as ObjectResult;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var badResult = await usersController.Register(testUserRegRequest) as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, badResult.StatusCode);
        }

        public static IEnumerable<Object[]> GetBadRegisterRequest()
        {
            yield return new object[] { new RegisterRequest() { Email = String.Empty, Name = "Tester", Password = "123456" } };
            yield return new object[] { new RegisterRequest() { Email = "Test@gmail.com", Name = String.Empty, Password = "123456" }};
            yield return new object[] { new RegisterRequest() { Email = "Test@gmail.com", Name = "Tester", Password = String.Empty } };
            yield return new object[] { new RegisterRequest() { Email = String.Empty, Name = String.Empty, Password = String.Empty } };
        }

        [Theory]
        [MemberData(nameof(GetBadRegisterRequest))]
        public async void Register_With_Empty_Data_Should_Return_BadRequest(RegisterRequest request)
        {
            var result = await usersController.Register(request) as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }


        [Fact]
        public async void Authenicate_Should_Return_OK()
        {
            var request = new AuthenticateRequest() { Email = "Test@gmail.com", Password = "123456" };
            var testUserRegRequest = new RegisterRequest() { Email = "Test@gmail.com", Name = "Tester", Password = "123456" };

            await usersController.Register(testUserRegRequest);

            var result = await usersController.Authenticate(request) as ObjectResult;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }


        [Fact]
        public async void Authenicate_With_Bad_Password_Should_Return_BadRequest()
        {
            var request = new AuthenticateRequest() { Email = "Test@gmail.com", Password = "1234567" };
            var testUserRegRequest = new RegisterRequest() { Email = "Test@gmail.com", Name = "Tester", Password = "123456" };

            await usersController.Register(testUserRegRequest);

            var result = await usersController.Authenticate(request) as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async void Authenicate_With_None_Existing_User_Should_Return_BadRequst()
        {
            var request = new AuthenticateRequest() { Email = "Test@gmail.com", Password = "123456" };

            var result = await usersController.Authenticate(request) as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }


        public static IEnumerable<Object[]> GetBadAuthenticationRequest()
        {
            yield return new object[] { new AuthenticateRequest() { Email = String.Empty, Password = "123456" } };
            yield return new object[] { new AuthenticateRequest() { Email = "Test@gmail.com", Password = String.Empty } };
            yield return new object[] { new AuthenticateRequest() { Email = String.Empty, Password = String.Empty } };
        }

        [Theory]
        [MemberData(nameof(GetBadAuthenticationRequest))]
        public async void Authenticate_With_Empty_Data_Should_Return_BadRequest(AuthenticateRequest request)
        {
            var result = await usersController.Authenticate(request) as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async void Reset_Password_Should_Return_OK()
        {
            var model = new RegisterRequest()
            {
                Email = "Test@gmail.com",
                Name = "Tester",
                Password = "123456"
            };

            await usersController.Register(model);
            var result = await usersController.ResetPassword(model.Email) as StatusCodeResult;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void Reset_Password_For_Non_Existing_User_Should_Return_BadRquest()
        {
            var nonExistingEmail = "nonExisting@gmail.com";
            var result = await usersController.ResetPassword(nonExistingEmail) as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        [Fact]
        public async void Reset_Password_With_Null_Email_Should_Return_BadRquest()
        {
            string nullEmail = null;
            var result = await usersController.ResetPassword(nullEmail) as StatusCodeResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

        public static IEnumerable<Object[]> GetBadPasswordResetRequest()
        {
            yield return new object[] { new PasswordResetRequest() { Email = String.Empty, Password = "123456", ResetToken = "FakeResetToken" } };
            yield return new object[] { new PasswordResetRequest() { Email = "Test@gmail.com", Password = String.Empty, ResetToken = "FakeResetToken" } };
            yield return new object[] { new PasswordResetRequest() { Email = String.Empty, Password = String.Empty, ResetToken = String.Empty } };
        }

        [Theory]
        [MemberData(nameof(GetBadPasswordResetRequest))]
        public async void Reset_Password_With_Imcomplete_Request_Info_Should_Return_BadRquest(PasswordResetRequest request)
        {
            var result = await usersController.ResetPassword(request) as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

    }
}
