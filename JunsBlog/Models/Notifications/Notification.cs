using JunsBlog.Entities;

namespace JunsBlog.Models.Notifications
{
    public class Notification
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }

        public static Notification GenerateWelcomeNotification(User user)
        {
            return new Notification()
            {
                ReceiverName = user.Name,
                ReceiverEmail = user.Email,
                Subject = "Welcome to Juns Blog",
                Message = "Welcome to Juns Blog"
            };
        }

        public static Notification GeneratePasswordResetNotification(User user)
        {
            return new Notification()
            {
                ReceiverName = user.Name,
                ReceiverEmail = user.Email,
                Subject = "Password Reset for Juns Blog",
                Message = $"Please use the following reset token to complete to password reset process.  Reset Token: {user.ResetToken.Token}"
            };
        }
    }
}
