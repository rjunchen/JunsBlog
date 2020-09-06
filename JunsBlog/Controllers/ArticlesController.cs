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
using MongoDB.Bson;

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

                return Ok(article.Id);
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

                var articleDetails = await databaseService.GetArticleDetailsAsync(articleId, currentUserId);

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

                //var searchResult = await databaseService.SearchArticlesAsyc(options);

                return Ok();
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

                var articleRanking = await databaseService.GetArticleRankingAsync(model.ArticleId);

                if (articleRanking == null) articleRanking = new ArticleRanking(model.ArticleId);

                switch (model.Rank)
                {
                    case RankEnum.Like:

                        if (articleRanking.Likes.Contains(currentUserId))
                        {
                            articleRanking.Likes.Remove(currentUserId);
                        }
                        else
                        {
                            // If like the article then also need to remove the dislike from the article
                            articleRanking.Likes.Add(currentUserId);
                            articleRanking.Dislikes.Remove(currentUserId);
                        }
                        break;
                    case RankEnum.Dislike:

                        if (articleRanking.Dislikes.Contains(currentUserId))
                        {
                            articleRanking.Dislikes.Remove(currentUserId);
                        }
                        else
                        {
                            articleRanking.Dislikes.Add(currentUserId);
                            articleRanking.Likes.Remove(currentUserId);
                        }
                        break;
                    case RankEnum.Favor:
                        if (articleRanking.Favors.Contains(currentUserId))
                        {
                            articleRanking.Favors.Remove(currentUserId);
                        }
                        else
                        {
                            articleRanking.Favors.Add(currentUserId);
                        }
                        break;
                }

                await databaseService.SaveArticleRankingAsync(articleRanking);
                var rankingDetails = new ArticleRankingDetails(articleRanking, currentUserId);

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
                    return BadRequest(new { message = "Article ID is missing" });

                var articleRanking = await databaseService.GetArticleRankingAsync(articleId);

                var rankingDetails = new ArticleRankingDetails(articleRanking, currentUserId);

                return Ok(rankingDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
