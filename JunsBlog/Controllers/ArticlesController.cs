﻿using System;
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
    [Route("api/article")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IDatabaseService databaseService;
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        private readonly string currentUserId;

        public ArticlesController(IHttpContextAccessor httpContextAccessor, IDatabaseService databaseService, ILogger<OAuthsController> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
            this.currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [Authorize(Roles = Role.User)]
        [HttpPost("post")]
        public async Task<IActionResult> Post(ArticleRequest model)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(model.Title) || String.IsNullOrWhiteSpace(model.Content) || String.IsNullOrWhiteSpace(model.Abstract))
                    return BadRequest(new { message = "Incomplete article information" });

                var newArticle = new Article()
                {
                    Abstract = model.Abstract,
                    AuthorId = currentUserId,
                    Content = model.Content,
                    CoverImage = model.CoverImage,
                    CreationDate = DateTime.UtcNow,
                    IsPrivate = model.IsPrivate,
                    Title = model.Title,
                    Categories = model.Categories
                };

                var artile = await databaseService.SaveArticleAsync(newArticle);

                return Ok(artile);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetArticle(string articleId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(articleId))
                    return BadRequest(new { message = "Invalid articleId" });

                var artile = await databaseService.FindArticAsync(x=> x.Id.Equals(articleId));

                return Ok(artile);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
