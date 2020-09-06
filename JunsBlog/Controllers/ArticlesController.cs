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
                    article = Article.CreateArticle(aricleModel, currentUserId);
                }
                else
                {
                    article = await databaseService.GetArticleAsync(aricleModel.Id);
                    article.UpdateContents(aricleModel);
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

                var articleWithUser = await databaseService.GetArticleWithUserInfoAsync(articleId);

                //TODO: increase article view

                if(articleWithUser == null) return BadRequest(new { message = "Article does not exist" });

                var articleDetails = ArticleDetails.GenerateArticleDetails(articleWithUser, currentUserId);

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

                var article = await databaseService.GetArticleAsync(model.ArticleId);

                switch (model.Rank)
                {
                    case RankEnum.Like:

                        if (article.Likes.Contains(currentUserId))
                        {
                            article.Likes.Remove(currentUserId);
                        }
                        else
                        {
                            // If like the article then also need to remove the dislike from the article
                            article.Likes.Add(currentUserId);
                            article.Dislikes.Remove(currentUserId);
                        }
                        break;
                    case RankEnum.Dislike:

                        if (article.Dislikes.Contains(currentUserId))
                        {
                            article.Dislikes.Remove(currentUserId);
                        }
                        else
                        {
                            article.Dislikes.Add(currentUserId);
                            article.Likes.Remove(currentUserId);
                        }
                        break;
                    case RankEnum.Favor:
                        if (article.Favors.Contains(currentUserId))
                        {
                            article.Favors.Remove(currentUserId);
                        }
                        else
                        {
                            article.Favors.Add(currentUserId);
                        }
                        break;
                }

                await databaseService.SaveArticleAsync(article);
                var rankingDetails = ArticleRankingDetails.GenerateArticleRankingDetails(article.Likes, article.Dislikes, article.Favors, currentUserId);

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
