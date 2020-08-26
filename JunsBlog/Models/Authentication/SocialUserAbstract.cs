using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Authentication
{
    public abstract class SocialUserAbstract
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
    }
}
