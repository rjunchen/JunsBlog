using JunsBlog.Interfaces.Services;
using JunsBlog.Models.Notifications;
using System;

namespace JunsBlog.Test.Mockups
{
    public class NotificationServiceFake : INotificationService
    {
        public void SendNotification(Notification notification)
        {
           // Do Nothing for now
        }
    }
}
