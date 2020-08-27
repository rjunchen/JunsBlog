using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Enums;
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
        [HttpPost("create")]
        public async Task<IActionResult> CreateArticle(ArticleRequest model)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(model.Title) || String.IsNullOrWhiteSpace(model.Content) || String.IsNullOrWhiteSpace(model.Abstract))
                    return BadRequest(new { message = "Incomplete article information" });

                var artile = await databaseService.SaveArticleAsync(new Article(model, currentUserId));

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

                var articleDetails = await databaseService.GetArticleDetailsAsync(articleId);

                return Ok(articleDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchArticles(int page = 1, int pageSize = 10, string searchKey = null, SortByEnum sortBy = SortByEnum.UpdatedOn, SortOrderEnum sortOrder = SortOrderEnum.Descending)
        {
            try
            {
                var searchResult = await databaseService.SearchArticlesAsyc(page, pageSize, searchKey, sortBy, sortOrder);

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = Role.User)]
        [HttpPost("rank")]
        public async Task<IActionResult> RankArticle(ArticleRankingRequest model)
        {
            try
            {
                if (model == null || String.IsNullOrWhiteSpace(model.ArticleId))
                    return BadRequest(new { message = "Incomplete ranking information" });

                var ranking = await databaseService.FindArticleRankingAsync(x => x.ArticleId == model.ArticleId && x.UserId == currentUserId);

                if (ranking == null) ranking = new ArticleRanking(model.ArticleId, currentUserId);

                switch (model.Rank)
                {
                    case RankEnum.Like:
                        ranking.DidILike = !ranking.DidILike;
                        if (ranking.DidILike) ranking.DidIDislike = false;
                        break;
                    case RankEnum.Dislike:
                        ranking.DidIDislike = !ranking.DidIDislike;
                        if(ranking.DidIDislike) ranking.DidILike = false;
                        break;
                    case RankEnum.Favor:
                        ranking.DidIFavor = !ranking.DidIFavor;
                        break;
                }
                await databaseService.SaveArticleRankingAsync(ranking);

                var rankingDetails = await databaseService.GetArticleRankingDetailsAsync(model.ArticleId, currentUserId);

                return Ok(rankingDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("rank")]
        public async Task<IActionResult> GetArticleRanking(string articleId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(articleId))
                    return BadRequest(new { message = "Incomplete ranking information" });

                var articleRankingDetails = await databaseService.GetArticleRankingDetailsAsync(articleId, currentUserId);

                return Ok(articleRankingDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
