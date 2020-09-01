﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Authentication
{
    public class UserInfoUpdateRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
    }
}
