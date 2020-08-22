using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Authentication.Google;

namespace JunsBlog.Models.Settings
{
    public class AuthenticationSettings : IAuthenticationSettings
    {
        public GoogleClientInfo Google { get; set; }
    }
}
