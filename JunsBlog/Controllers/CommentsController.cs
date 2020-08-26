using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Comments;
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

        public CommentsController(IHttpContextAccessor httpContextAccessor, IDatabaseService databaseService, ILogger<OAuthsController> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
            this.currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }


        [Authorize(Roles = Role.User)]
        [HttpPost("reply")]
        public async Task<IActionResult> PostComment(CommentRequest model)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(model.TargetId) || String.IsNullOrWhiteSpace(model.CommentText))
                    return BadRequest(new { message = "Incomplete comment information" });

                var insertedComment = await databaseService.SaveCommentAsync(new Comment(model, currentUserId));

                var commentDetails = new CommentDetails()
                {
                    Commenter = await databaseService.FindUserAsync(x => x.Id == insertedComment.CommenterId),
                    Content = insertedComment.CommentText,
                    CreatedOn = insertedComment.CreatedOn,
                    Ranking = await GetCommentRanking(insertedComment.Id),
                    Id = insertedComment.Id
                };

                return Ok(commentDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        private async Task<CommentRankingResponse> GetCommentRanking(string commentId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(commentId))
                    return null;

                var rankings = await databaseService.FindCommentRankingsAsync(x => x.CommentId == commentId);

                var rankingResponse = new CommentRankingResponse() { CommentId = commentId };

                foreach (var item in rankings)
                {
                    if (item.DidIDislike) rankingResponse.Dislikes++;
                    if (item.DidILike) rankingResponse.Likes++;
                    rankingResponse.DidIFavor = item.DidIFavor;

                    if (item.UserId == currentUserId)
                    {
                        rankingResponse.DidIDislike = item.DidIDislike;
                        rankingResponse.DidILike = item.DidILike;
                        rankingResponse.DidIFavor = item.DidIFavor;
                    }
                }

                return rankingResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return null;
            }
        }

        [HttpGet("read")]
        public async Task<IActionResult> GetComments(string targetId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(targetId))
                    return BadRequest(new { message = "Missing targetId" });

                var comments = await databaseService.GetCommentsAsync(targetId);

                var commentsReponse = new List<CommentDetails>();

                foreach (var item in comments)
                {
                    var commentDetails = new CommentDetails()
                    {
                        Commenter = await databaseService.FindUserAsync(x => x.Id == item.CommenterId),
                        Content = item.CommentText,
                        CreatedOn = item.CreatedOn,
                        Ranking = await GetCommentRanking(item.Id),
                        Id = item.Id,
                        CommentsCount = databaseService.GetCommentsAsync(item.Id).Result.Count
                     };
                    commentsReponse.Add(commentDetails);
                }

                return Ok(commentsReponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
