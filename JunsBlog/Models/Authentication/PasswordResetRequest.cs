namespace JunsBlog.Models.Authentication
{
    public class PasswordResetRequest
    {
        public string Password { get; set; }
        public string ResetToken { get; set; }
        public string Email { get; set; }
    }
}
