using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
        
    }
}
