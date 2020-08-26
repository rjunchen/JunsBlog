﻿using DnsClient.Internal;
using JunsBlog.Entities;
using JunsBlog.Helpers;
using JunsBlog.Interfaces;
using JunsBlog.Interfaces.Services;
using JunsBlog.Models.Authentication;
using JunsBlog.Models.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JunsBlog.Controllers
{
    [Route("api")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatabaseService databaseService;
        private readonly INotificationService notificationService;
        private readonly IJwtTokenHelper jwtTokenHelper;
        private readonly string userId;
        private readonly Microsoft.Extensions.Logging.ILogger logger;

        public UsersController(IHttpContextAccessor httpContextAccessor, IDatabaseService databaseService, IJwtTokenHelper jwtTokenHelper, INotificationService notificationService, ILogger<UsersController> logger)
        {
            this.databaseService = databaseService;
            this.notificationService = notificationService;
            this.jwtTokenHelper = jwtTokenHelper;
            userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            this.logger = logger;
        }
       
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(model.Email) || String.IsNullOrWhiteSpace(model.Password))
                    return BadRequest(new { message = "Incomplete user authentication information" });

                var user = await databaseService.FindUserAsync(x=> x.Email.ToLower() == model.Email.ToLower());

                if (user == null) return BadRequest(new { message = "User doesn't exist" });

                if (user.Type != AccountType.Local) return BadRequest(new { message = $"The email have been registered under your {user.Type} account" });

                if (!Utilities.ValidatePassword(model.Password, user.Password)) return BadRequest(new { message = "Incorrect password" });

                var userToken = await databaseService.FindUserTokenAsync(x => x.UserId == user.Id);

                var response = new AuthenticateResponse(user, jwtTokenHelper.GenerateJwtToken(user), userToken.RefreshToken);

                return Ok(response);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }      
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            try
            {
                if(String.IsNullOrWhiteSpace(model.Name) || String.IsNullOrWhiteSpace(model.Email) || String.IsNullOrWhiteSpace(model.Password))
                    return BadRequest(new { message = "Incomplete user registration information" });

                var existingUser = await databaseService.FindUserAsync(x=> x.Email.ToLower() == model.Email.ToLower());

                if(existingUser != null) return BadRequest(new { message = "Email has been already registered" });

                var insertedUser = await databaseService.SaveUserAsync(new User(model));
                if (insertedUser == null) return BadRequest(new { message = "Failed to register user" });

                var insertedUserToken = await databaseService.SaveUserTokenAsync(new UserToken(insertedUser.Id));

                var response = new AuthenticateResponse(insertedUser, jwtTokenHelper.GenerateJwtToken(insertedUser), insertedUserToken.RefreshToken);

                notificationService.SendNotification(Notification.GenerateWelcomeNotification(insertedUser));

                return Ok(response);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }        
        }


        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword(PasswordResetRequest model)
        {
            try
            {
                if (model == null || String.IsNullOrWhiteSpace(model.ResetToken) || String.IsNullOrWhiteSpace(model.Email) || 
                    String.IsNullOrWhiteSpace(model.Password))
                    return BadRequest(new { message = "Incomplete password reset information" });

                var user = await databaseService.FindUserAsync(x=> x.Email.ToLower() ==  model.Email.ToLower());

                if (user == null) return StatusCode(StatusCodes.Status400BadRequest, "User not found");

                var userToken = await databaseService.FindUserTokenAsync(x=> x.UserId == user.Id);

                if(model.ResetToken != userToken.ResetToken || userToken.ResetExpiry < DateTime.UtcNow) 
                    return StatusCode(StatusCodes.Status400BadRequest, "Invalid or expired reset token");

                user.Password = Utilities.HashPassword(model.Password);

                await databaseService.SaveUserAsync(user);

                return Ok();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }  
        }

        [HttpGet("reset")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(email)) return StatusCode(StatusCodes.Status400BadRequest);

                var user = await databaseService.FindUserAsync(x=> x.Email.ToLower() ==  email.ToLower());

                if (user == null) return StatusCode(StatusCodes.Status400BadRequest, "User not found");

                var userToken = await databaseService.FindUserTokenAsync(x=> x.UserId == user.Id);

                userToken.CreateResetToken();

                var updatedUserToken = await databaseService.SaveUserTokenAsync(userToken);

                // Send the password reset email
                notificationService.SendNotification(Notification.GeneratePasswordResetNotification(user, updatedUserToken));
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
