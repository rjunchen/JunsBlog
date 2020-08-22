using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Interfaces.Settings
{
    public interface IJwtSettings
    {
        string TokenSecret { get; set; }
        string Issuer { get; set; }
    }
}
