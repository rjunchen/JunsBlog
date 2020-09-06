using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Comments;
using JunsBlog.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JunsBlog.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IDatabaseService databaseService;
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        private readonly string currentUserId;

        public CommentsController(IHttpContextAccessor httpContextAccessor, IDatabaseService databaseService, ILogger<CommentsController> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
            this.currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }


        //[Authorize(Roles = Role.User)]
        //[HttpPost("reply")]
        //public async Task<IActionResult> PostComment(CommentRequest model)
        //{
        //    try
        //    {
        //        if (String.IsNullOrWhiteSpace(model.ArticleId) || String.IsNullOrWhiteSpace(model.CommentText) || String.IsNullOrWhiteSpace(model.ParentId))
        //            return BadRequest(new { message = "Incomplete comment information" });

        //        var insertedComment = await databaseService.SaveCommentAsync(new Comment(model, currentUserId));

        //        var commentDetails = await databaseService.GetCommentDetialsAsync(insertedComment.Id, currentUserId);

        //        return Ok(commentDetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //[HttpPost("search")]
        //public async Task<IActionResult> SearchComments(CommentSearchPagingOption options)
        //{
        //    try
        //    {
        //        var searchResult = await databaseService.SearchCommentsAsync(options, currentUserId);

        //        return Ok(searchResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}


        //[Authorize(Roles = Role.User)]
        //[HttpPost("rank")]
        //public async Task<IActionResult> RankComments(CommentRankingRequest model)
        //{
        //    try
        //    {
        //        if (model == null || String.IsNullOrWhiteSpace(model.CommentId))
        //            return BadRequest(new { message = "Incomplete ranking information" });

        //        var ranking = await databaseService.GetCommentRankingAsync(model.CommentId, currentUserId);

        //        if (ranking == null) ranking = new CommentRanking(model.CommentId, currentUserId);

        //        switch (model.Rank)
        //        {
        //            case RankEnum.Like:
        //                ranking.DidILike = !ranking.DidILike;
        //                if (ranking.DidILike) ranking.DidIDislike = false;
        //                break;
        //            case RankEnum.Dislike:
        //                ranking.DidIDislike = !ranking.DidIDislike;
        //                if (ranking.DidIDislike) ranking.DidILike = false;
        //                break;
        //            case RankEnum.Favor:
        //                ranking.DidIFavor = !ranking.DidIFavor;
        //                break;
        //        }
        //        await databaseService.SaveCommentRankingAsync(ranking);

        //        var rankingDetails = await GetCommentRankingDetails(model.CommentId, currentUserId);

        //        return Ok(rankingDetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //private async Task<CommentRankingDetails> GetCommentRankingDetails(string commentId, string userId)
        //{
        //    var rankings = await databaseService.GetCommentRankingsAsync(commentId);

        //    var rankingResponse = new CommentRankingDetails() { CommentId = commentId };

        //    foreach (var item in rankings)
        //    {
        //        if (item.DidIDislike) rankingResponse.DislikesCount++;
        //        if (item.DidILike) rankingResponse.LikesCount++;
        //        rankingResponse.DidIFavor = item.DidIFavor;

        //        if (item.UserId == userId)
        //        {
        //            rankingResponse.DidIDislike = item.DidIDislike;
        //            rankingResponse.DidILike = item.DidILike;
        //            rankingResponse.DidIFavor = item.DidIFavor;
        //        }
        //    }
        //    return rankingResponse;
        //}
    }
}
