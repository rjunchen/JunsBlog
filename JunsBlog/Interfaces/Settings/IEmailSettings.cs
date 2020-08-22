using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Interfaces.Settings
{
    public interface IEmailSettings
    {
        string Host { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string From { get; set; }
        string Sender { get; set; }
    }
}
