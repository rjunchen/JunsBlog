using System;
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
using Microsoft.Net.Http.Headers;

namespace JunsBlog.Controllers.v1
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


        /// <summary>
        /// Get article basic info
        /// </summary>
        /// <param name="articleId">Article Id</param>
        /// <response code="200">Retrieved article successfully</response>
        /// <response code="400">Invalid article Id or article doesn't exist</response>
        /// <response code="500">Oops! Can't get article right now</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("get")]
        public async Task<IActionResult> GetArticleBasicInfo(string articleId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(articleId))
                    return BadRequest(new { message = "Invalid articleId" });

                var article = await databaseService.GetArticleBasicInfoAsync(articleId);

                if (article == null) return BadRequest(new { message = "Article does not exist" });

                return Ok(article);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Save article
        /// </summary>
        /// <param name="aricleModel">Article info</param>
        /// <response code="200">Saved article successfully</response>
        /// <response code="400">Invalid article</response>
        /// <response code="500">Oops! Can't save article right now</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = Role.User)]
        [HttpPost("save")]
        public async Task<IActionResult> SaveArticle(ArticleBasicInfo aricleModel)
        {
            try
            {
                if (aricleModel == null || String.IsNullOrWhiteSpace(aricleModel.Title) 
                    || String.IsNullOrWhiteSpace(aricleModel.Abstract) || String.IsNullOrWhiteSpace(aricleModel.Content))
                    return BadRequest(new { message = "Invalid article" });

                Article article;

                // If the article model doesn't have an id then create a new article otherwise update the article
                if (String.IsNullOrWhiteSpace(aricleModel.Id))
                {
                    article = new Article(aricleModel, currentUserId);
                }
                else
                {
                    article = await databaseService.GetArticleAsync(aricleModel.Id);
                    article.Abstract = aricleModel.Abstract;
                    article.Content = aricleModel.Content;
                    article.CoverImage = aricleModel.CoverImage;
                    article.IsPrivate = aricleModel.IsPrivate;
                    article.Title = aricleModel.Title;
                    article.Categories = aricleModel.Categories;
                }

                Utilities.MassageArticleImages(article);

                await databaseService.SaveArticleAsync(article);
    
                return Ok(new ArticleId(article.Id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get article details
        /// </summary>
        /// <param name="articleId">Article Id</param>
        /// <response code="200">Retrieved article details successfully</response>
        /// <response code="400">Invalid article Id</response>
        /// <response code="500">Oops! Can't get article details right now</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("details/get")]
        public async Task<IActionResult> GetArticleDetails(string articleId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(articleId))
                    return BadRequest(new { message = "Invalid articleId" });

                var articleDetails = await databaseService.GetArticleDetailsAsync(articleId, currentUserId);

                return Ok(articleDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Search articles
        /// </summary>
        /// <param name="options">Article search options</param>
        /// <response code="200">Retrieved articles successfully</response>
        /// <response code="400">ProfileId is not specified in the search options</response>
        /// <response code="500">Oops! Can't search articles right now</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
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


        /// <summary>
        /// Rank article
        /// </summary>
        /// <param name="model">Article ranking info</param>
        /// <response code="200">Ranked article successfully</response>
        /// <response code="400">Invalid article ranking info</response>
        /// <response code="500">Oops! Can't rank article right now</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
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
                        if (ranking.DidIDislike) ranking.DidILike = false;
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


        /// <summary>
        /// Get article ranking details
        /// </summary>
        /// <param name="articleId">Article Id</param>
        /// <response code="200">Retrieved article ranking details successfully</response>
        /// <response code="400">Article Id is missing</response>
        /// <response code="500">Oops! Can't get article ranking details right now</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("rank")]
        public async Task<IActionResult> GetArticleRanking(string articleId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(articleId))
                    return BadRequest(new { message = "Article ID is missing" });

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
