using JunsBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Interfaces
{
    public interface IJwtTokenHelper
    {
        string GenerateJwtToken(User user);
    }
}
