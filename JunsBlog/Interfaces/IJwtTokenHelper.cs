using JunsBlog.Entities;
using JunsBlog.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JunsBlog.Interfaces
{
    public interface IJwtTokenHelper
    {
        string GenerateJwtToken(User user);
        Claim ValidateToken(string accessToken);
    }
}
