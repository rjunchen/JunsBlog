using JunsBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Authentication
{
    public class AuthenticateResponse
    {
        public User User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string accessToken, string refreshToken)
        {
            this.User = user;
            this.RefreshToken = refreshToken;
            this.AccessToken = accessToken;
        }
    }
}
