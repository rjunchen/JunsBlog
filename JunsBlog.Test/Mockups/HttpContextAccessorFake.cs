using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace JunsBlog.Test.Mockups
{
    public class HttpContextAccessorFake : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }

        public HttpContextAccessorFake()
        {
            HttpContext = new DefaultHttpContext();
        }

        public HttpContextAccessorFake(Claim[] claims)
        {
            HttpContext = new DefaultHttpContext();
            var identity = new ClaimsIdentity(claims, "Fake Claim");
            var principal = new ClaimsPrincipal(identity);
            HttpContext.User = new System.Security.Claims.ClaimsPrincipal(principal);     
        }
        
    }
}
