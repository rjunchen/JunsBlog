using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Authentication
{
    public class ResetToken
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }

        public ResetToken(int expiresInMinutes = 10)
        {
            Token = Guid.NewGuid().ToString();
            Expiry = DateTime.UtcNow.AddMinutes(expiresInMinutes);
        }
    }
}
