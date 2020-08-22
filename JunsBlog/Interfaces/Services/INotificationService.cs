using JunsBlog.Entities;
using JunsBlog.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Interfaces.Services
{
    public interface INotificationService
    {
        void SendNotification(Notification notification);
    }
}
