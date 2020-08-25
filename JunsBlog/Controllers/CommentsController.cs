using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Models.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JunsBlog.Controllers
{
    [Route("api/ranking")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IDatabaseService databaseService;
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        private readonly string currentUserId;

        public CommentsController(IHttpContextAccessor httpContextAccessor, IDatabaseService databaseService, ILogger<OAuthsController> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
            this.currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

   
    }
}
