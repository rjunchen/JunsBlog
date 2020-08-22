
using JunsBlog.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace JunsBlog.Test.Mockups
{
    public class JwtSettingsFake : IJwtSettings
    {
        public string TokenSecret { get; set; }
        public string Issuer { get; set; }

        public JwtSettingsFake()
        {
            TokenSecret = "0C454365-CF6F-4F83-BF9A-9B3AB2841BE8";
            Issuer = "JunsBlogFake";
        }
    }
}
