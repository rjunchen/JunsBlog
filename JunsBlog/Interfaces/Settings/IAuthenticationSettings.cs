using JunsBlog.Models.Authentication.Google;

namespace JunsBlog.Interfaces.Settings
{
    public interface IAuthenticationSettings
    {
        GoogleClientInfo Google { get; set; }
    }
}
