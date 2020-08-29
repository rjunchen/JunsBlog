using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Authentication.Google;
using System;
using System.Collections.Generic;
using System.Text;

namespace JunsBlog.Test.Mockups
{
    public class AuthenticationSettingFake : IAuthenticationSettings
    {
        public GoogleClientInfo Google { get; set; }
        public AuthenticationSettingFake()
        {
            Google = new GoogleClientInfo()
            {
                ClientId = "GoogleClientId",
                ClientSecret = "GoogleClientSecret",
                RedirectUri = "https://localhost:44330/api/auth/google/signin",
                TokenRequestUri = "https://accounts.google.com/o/oauth2/token"
            };
        }
    }
}
