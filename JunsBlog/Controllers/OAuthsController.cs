using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JunsBlog.Entities;
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

        public OAuthsController(IAuthenticationSettings authSettings, IDatabaseService databaseService, ILogger<OAuthsController> logger)
        {
            this.authSettings = authSettings;
            this.databaseService = databaseService;
            this.logger = logger;
        }


        private async Task<GoogleUserInfo> GetGoogleUserInfoAsync(string accessToken)
        {
            try
            {
                var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + accessToken;
                using (HttpClient client = new HttpClient())
                {
                    var output = await client.GetAsync(urlProfile);

                    if (!output.IsSuccessStatusCode) return null;

                    string outputData = await output.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<GoogleUserInfo>(outputData);
                }

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
                    {"redirect_uri", authSettings.Google.RedirectUri },
                    {"grant_type", "authorization_code" }
                };

                using (var httpClient = new HttpClient())
                {
                    var respond = await httpClient.PostAsync(authSettings.Google.TokenRequestUri, new FormUrlEncodedContent(queryParams));

                    if (!respond.IsSuccessStatusCode) return null;

                    var respondString = await respond.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<GoogleTokenResponse>(respondString);
                }
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
                    CreationDate = DateTime.UtcNow,
                    Role = Role.User,
                    Type = AccountType.Google,
                };

                var authResponse = await databaseService.SaveUserAsync(newUser);

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
