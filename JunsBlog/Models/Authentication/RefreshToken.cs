using JunsBlog.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Authentication
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }

        public RefreshToken(int expiresInDays = 14)
        {
            Token = Utilities.GenerateToken();
            Expiry = DateTime.UtcNow.AddDays(expiresInDays);
        }
    }
}
