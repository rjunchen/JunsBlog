using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JunsBlog.Entities;
using JunsBlog.Helpers;
using JunsBlog.Interfaces;
using JunsBlog.Interfaces.Services;
using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Authentication;
using JunsBlog.Models.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JunsBlog.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class OAuthsController : ControllerBase
    {
        private readonly IAuthenticationSettings authSettings;
        private readonly IDatabaseService databaseService;
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        private readonly IJwtTokenHelper jwtTokenHelper;
        private readonly IHttpContextAccessor httpContextAccessor;
        public OAuthsController(IHttpContextAccessor httpContextAccessor, IAuthenticationSettings authSettings, IDatabaseService databaseService, IJwtTokenHelper jwtTokenHelper, ILogger<OAuthsController> logger)
        {
            this.authSettings = authSettings;
            this.databaseService = databaseService;
            this.jwtTokenHelper = jwtTokenHelper;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;  
        }


        private async Task<GoogleUserInfo> GetGoogleUserInfoAsync(string accessToken)
        {
            try
            {
                var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + accessToken;
                using HttpClient client = new HttpClient();
                var output = await client.GetAsync(urlProfile);

                if (!output.IsSuccessStatusCode) return null;

                string outputData = await output.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<GoogleUserInfo>(outputData);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return null;
        }

        private async Task<GoogleTokenResponse> GetGoogleTokenAsync(string authorizeCode)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(authorizeCode)) return null;

                var queryParams = new Dictionary<string, string>()
                {
                    {"code", authorizeCode },
                    {"client_id", authSettings.Google.ClientId },
                    {"client_secret", authSettings.Google.ClientSecret },
                    {"redirect_uri", authSettings.Google.RedirectUri},
                    {"grant_type", "authorization_code" }
                };

                using var httpClient = new HttpClient();
                var respond = await httpClient.PostAsync(authSettings.Google.TokenRequestUri, new FormUrlEncodedContent(queryParams));

                if (!respond.IsSuccessStatusCode) return null;

                var respondString = await respond.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<GoogleTokenResponse>(respondString);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
            return null;
        }

        [HttpGet("google/signin")]
        public async Task<IActionResult> GoogleSingin(string code)
        {
            try
            {    
                var tokenResponse = await GetGoogleTokenAsync(code);

                if(tokenResponse == null) return StatusCode(StatusCodes.Status400BadRequest);

                var userInfo = await GetGoogleUserInfoAsync(tokenResponse.access_token);

                User newUser = new User()
                {
                    Name = userInfo.name,
                    Email = userInfo.email,
                    CreatedOn = DateTime.UtcNow,
                    Role = Role.User,
                    Type = AccountType.Google,
                    Image = userInfo.picture
                };

                var googleUser = await databaseService.FindUserAsync( x=> x.Email.ToLower() == userInfo.email.ToLower());

                UserToken userToken;

                if(googleUser == null)
                {
                    googleUser = await databaseService.SaveUserAsync(newUser);

                    userToken = new UserToken()
                    {
                        RefreshToken = Utilities.GenerateToken(),
                        RefreshExpiry = DateTime.UtcNow.AddDays(14),
                        UserId = googleUser.Id
                    };

                    userToken = await databaseService.SaveUserTokenAsync(userToken);
                }
                else
                {
                    userToken = await databaseService.FindUserTokenAsync(x => x.UserId == googleUser.Id);
                }

                var response = new AuthenticateResponse(googleUser, jwtTokenHelper.GenerateJwtToken(googleUser), userToken.RefreshToken);

                var baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}";

                return Redirect($"{baseUrl}/social?accessToken={response.AccessToken}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("google/url")]
        public IActionResult GoogleUrl()
        {
            try
            {
                var queryParams = new Dictionary<string, string>()
                {
                    {"scope", "https://www.googleapis.com/auth/userinfo.profile" },
                    {"access_type", "offline" },
                    {"client_id", authSettings.Google.ClientId },
                    {"include_granted_scopes", "true" },
                    {"redirect_uri", authSettings.Google.RedirectUri},
                    {"response_type", "code" },
                    {"state", "JunsBlog" }
                };

                var googleAuth = "https://accounts.google.com/o/oauth2/v2/auth";

                var AuthUrl = QueryHelpers.AddQueryString(googleAuth, queryParams);
                return Ok(AuthUrl);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("info")]
        public async Task<IActionResult> GetAuthenticationInfo(AuthInfoRequest request)
        {
            try
            {
                var claim = jwtTokenHelper.ValidateToken(request.AccessToken);

                var user = await databaseService.FindUserAsync(x=>x.Id == claim.Value);

                var userToken = await databaseService.FindUserTokenAsync(x => x.UserId == user.Id);

                var response = new AuthenticateResponse(user, jwtTokenHelper.GenerateJwtToken(user), userToken.RefreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


    }
}
