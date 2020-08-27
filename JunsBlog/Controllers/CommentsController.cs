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

                var commentDetails = await databaseService.GetCommentDetialsAsync(insertedComment.Id, currentUserId);

                return Ok(commentDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchComments(int page = 1, int pageSize = 10, string searchKey = null, CommentSearchOnEnum searchOn = CommentSearchOnEnum.CommentText, SortByEnum sortBy = SortByEnum.UpdatedOn, SortOrderEnum sortOrder = SortOrderEnum.Descending)
        {
            try
            {
                var searchResult = await databaseService.SearchCommentsAsync(page, pageSize, searchKey, searchOn, sortBy, sortOrder, currentUserId);

                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
