using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JunsBlog.Entities;
using JunsBlog.Helpers;
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

        public ArticlesController(IHttpContextAccessor httpContextAccessor, IDatabaseService databaseService, ILogger<ArticlesController> logger)
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

                var newArticle = new Article(model, currentUserId); 

                Utilities.MassageArticleImages(newArticle);

     
               var insertedArticle = await databaseService.SaveArticleAsync(newArticle);

                return Ok(insertedArticle);
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

                var article = await databaseService.GetArticleAsync(articleId);

                if (article == null) return BadRequest(new { message = "Article does not exist" });

                return Ok(article);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("update")]
        public async Task<IActionResult> UpdateArticle(ArticleRequest article)
        {
            try
            {
                if (article == null || String.IsNullOrWhiteSpace(article.Id) || String.IsNullOrWhiteSpace(article.Title) 
                    || String.IsNullOrWhiteSpace(article.Abstract) || String.IsNullOrWhiteSpace(article.Content))
                    return BadRequest(new { message = "Invalid article" });

                var existingArticle = await databaseService.GetArticleAsync(article.Id);

                existingArticle.UpdateContents(article);

                var updatedArticle = await databaseService.SaveArticleAsync(existingArticle);

                return Ok(updatedArticle);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("details/get")]
        public async Task<IActionResult> GetArticleDetails(string articleId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(articleId))
                    return BadRequest(new { message = "Invalid articleId" });

                var articleDetails = await databaseService.GetArticleDetailsAsync(articleId);

                if(articleDetails == null) return BadRequest(new { message = "Article does not exist" });

                return Ok(articleDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Search")]
        public async Task<IActionResult> SearchArticles(ArticleSearchPagingOption options)
        {
            try
            {
                if((options.Filter == ArticleFilterEnum.MyArticles || options.Filter == ArticleFilterEnum.MyFavorites 
                    || options.Filter == ArticleFilterEnum.MyLikes) && String.IsNullOrEmpty(options.ProfilerId))
                    return BadRequest(new { message = "ProfilerId is not specified in the search options" });

                var searchResult = await databaseService.SearchArticlesAsyc(options);

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

                var ranking = await databaseService.GetArticleRankingAsync(model.ArticleId, currentUserId);

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

                var rankings = await databaseService.GetArticleRankingsAsync(model.ArticleId);

                var rankingDetails = new ArticleRankingDetails(model.ArticleId, currentUserId, rankings);

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

                var rankings = await databaseService.GetArticleRankingsAsync(articleId);

                var articleRankingDetails = new ArticleRankingDetails(articleId, currentUserId, rankings);

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
