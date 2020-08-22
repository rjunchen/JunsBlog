using JunsBlog.Interfaces.Settings;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Settings
{
    public class JwtSettings : IJwtSettings
    {
        public string TokenSecret { get; set; }
        public string Issuer { get; set; }
    }
}
