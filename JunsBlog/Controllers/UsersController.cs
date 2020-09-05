using DnsClient.Internal;
using JunsBlog.Entities;
using JunsBlog.Helpers;
using JunsBlog.Interfaces;
using JunsBlog.Interfaces.Services;
using JunsBlog.Models.Authentication;
using JunsBlog.Models.Notifications;
using JunsBlog.Models.Profile;
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

        public UsersController(IHttpContextAccessor httpContextAccessor, IDatabaseService databaseService,
            IJwtTokenHelper jwtTokenHelper, INotificationService notificationService, ILogger<UsersController> logger)
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

                var user = await databaseService.GetUserByEmailAsync(model.Email);

                if (user == null) return BadRequest(new { message = "User doesn't exist" });

                if (user.Type != AccountType.Local) return BadRequest(new { message = $"The email have been registered under your {user.Type} account" });

                if (!Utilities.ValidatePassword(model.Password, user.Password)) return BadRequest(new { message = "Incorrect password" });

                var response = new AuthenticateResponse(user, jwtTokenHelper.GenerateJwtToken(user));

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

                var existingUser = await databaseService.GetUserByEmailAsync(model.Email);

                if(existingUser != null) return BadRequest(new { message = "Email has been already registered" });

                var insertedUser = await databaseService.SaveUserAsync(new User(model));
                if (insertedUser == null) return BadRequest(new { message = "Failed to register user" });

                var response = new AuthenticateResponse(insertedUser, jwtTokenHelper.GenerateJwtToken(insertedUser));

                notificationService.SendNotification(Notification.GenerateWelcomeNotification(insertedUser));

                return Ok(response);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }        
        }


        [HttpPost("reset/password")]
        public async Task<IActionResult> ResetPassword(PasswordResetRequest model)
        {
            try
            {
                if (model == null || String.IsNullOrWhiteSpace(model.ResetToken) || String.IsNullOrWhiteSpace(model.Email) || 
                    String.IsNullOrWhiteSpace(model.Password))
                    return BadRequest(new { message = "Incomplete password reset information" });

                var user = await databaseService.GetUserByEmailAsync(model.Email);

                if (user == null) return StatusCode(StatusCodes.Status400BadRequest, "User not found");

                if(model.ResetToken != user.ResetToken.Token || user.ResetToken.Expiry < DateTime.UtcNow) 
                    return StatusCode(StatusCodes.Status400BadRequest, "Invalid or expired reset token");

                user.UpdatePassword(model.Password);

                await databaseService.SaveUserAsync(user);

                return Ok();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }  
        }

        [HttpGet("reset/verifyEmail")]
        public async Task<IActionResult> VerifyResetEmail(string email)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(email)) return StatusCode(StatusCodes.Status400BadRequest);

                var user = await databaseService.GetUserByEmailAsync(email);

                if (user == null) return StatusCode(StatusCodes.Status400BadRequest, "Invalid or expired reset token");

                user.RenewResetToken();

                var updatedUserToken = await databaseService.SaveUserAsync(user);

                // Send the password reset email
                notificationService.SendNotification(Notification.GeneratePasswordResetNotification(user));
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("reset/verifyToken")]
        public async Task<IActionResult> VerifyResetToken(string email, string token)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(token)) return StatusCode(StatusCodes.Status400BadRequest);

                var user = await databaseService.GetUserByEmailAsync(email);

                if (user == null) return StatusCode(StatusCodes.Status400BadRequest, "User not found");

                if(user.ResetToken.Token == token && user.ResetToken.Expiry > DateTime.UtcNow)
                    return Ok();
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Invalid reset token");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(userId)) return StatusCode(StatusCodes.Status400BadRequest);

                var profileDetails = await databaseService.GetProfileDetailsAsync(userId);

                if (profileDetails == null) return StatusCode(StatusCodes.Status400BadRequest, "User not found");

                return Ok(profileDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("profile/update")]
        public async Task<IActionResult> UpdateProfile(ProfileUpdateRequest model)
        {
            try
            {
                if (model == null || String.IsNullOrWhiteSpace(model.Id) || String.IsNullOrWhiteSpace(model.Name) || String.IsNullOrWhiteSpace(model.Email))
                    return BadRequest(new { message = "Incomplete user registration information" });

                if (model.Id != userId) return BadRequest(new { message = "Invalid user update request" });

                var existingUser = await databaseService.GetUserAsync(model.Id);

                if (existingUser == null) return BadRequest(new { message = "User doesn't exit" });

                existingUser.UpdateUserInfo(model);

                var insertedUser = await databaseService.SaveUserAsync(existingUser);

                return Ok(insertedUser);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
