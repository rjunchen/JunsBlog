using JunsBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Authentication.Google
{
    public class GoogleUserInfo : SocialUserAbstract
    {
        public GoogleUserInfo(GoogleUserMaping userMapping)
        {
            Name = userMapping.name;
            Email = userMapping.email;
            Role = Entities.Role.User;
            Type = AccountType.Google;
            Image = userMapping.picture;
        }
    }
}