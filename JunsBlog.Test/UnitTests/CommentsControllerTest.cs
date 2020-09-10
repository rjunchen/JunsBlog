using FluentAssertions;
using JunsBlog.Controllers;
using JunsBlog.Entities;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Authentication;
using JunsBlog.Models.Comments;
using JunsBlog.Test.Mockups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace JunsBlog.Test.UnitTests
{
    public class CommentsControllerTest
    {
        private const int TWO_SECONDS_IN_MILLIONSECONDS = 2000;
        private const string SAMPLE_ARTICLE_ID = "5f46b0c21c7bff3eeff4cd74";
        private readonly CommentRequest validCommentRequest = new CommentRequest() { ArticleId = SAMPLE_ARTICLE_ID, CommentText = "commentText", ParentId = SAMPLE_ARTICLE_ID };


        private readonly static CommentRequest invalidCommentRequestOne = new CommentRequest() {  CommentText = "commentText", ParentId = SAMPLE_ARTICLE_ID };
        private readonly static CommentRequest invalidCommentRequestTwo = new CommentRequest() { ArticleId = SAMPLE_ARTICLE_ID,  ParentId = SAMPLE_ARTICLE_ID };
        private readonly static CommentRequest invalidCommentRequestThree = new CommentRequest() { ArticleId = SAMPLE_ARTICLE_ID, CommentText = "commentText" };
        private readonly static CommentRequest invalidCommentRequestFour = new CommentRequest() { };

        public static IEnumerable<Object[]> GetBadCommentPostRequest()
        {
            yield return new object[] { invalidCommentRequestOne };
            yield return new object[] { invalidCommentRequestTwo };
            yield return new object[] { invalidCommentRequestThree };
            yield return new object[] { invalidCommentRequestFour };
        }


        private readonly CommentsController commentsController;
        private readonly string currentUserId;
        public CommentsControllerTest()
        {
            var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = logFactory.CreateLogger<CommentsController>();
            var notificationServiceFake = new NotificationServiceFake();
            var databaseService = new DatabaseServiceFake();

            var requestModel = new RegisterRequest() { Email = "Test@gmail.com", Name = "Tester", Password = "123456" };
            var user = new User(requestModel)
            {
                Id = ObjectId.GenerateNewId().ToString()
            };

            var currentUser = databaseService.SaveUserAsync(user).Result;

            var articleRequest = new ArticleBasicInfo() { Title = "title", Abstract = "abstract", Content = "content" };
            var article = new Article(articleRequest, currentUserId);
            article.Id = SAMPLE_ARTICLE_ID;
            var sampleArticle = databaseService.SaveArticleAsync(article).Result;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, currentUser.Name),
                new Claim(ClaimTypes.NameIdentifier, currentUser.Id)
            };

            var httpContextAccessFake = new HttpContextAccessorFake(claims);
            currentUserId = httpContextAccessFake.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            commentsController = new CommentsController(httpContextAccessFake, databaseService, logger);
        }

        [Fact]
        public async void Post_ValidComment_Success()
        {
            var result = await commentsController.PostComment(validCommentRequest);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDetailsResponse = okResult.Value.Should().BeAssignableTo<CommentDetails>().Subject;
            commentDetailsResponse.Should().NotBeNull();
            commentDetailsResponse.ArticleId.Should().Be(validCommentRequest.ArticleId);
            commentDetailsResponse.CommentText.Should().Be(validCommentRequest.CommentText);
            commentDetailsResponse.ParentId.Should().Be(validCommentRequest.ParentId);
            commentDetailsResponse.ParentId.Should().Be(commentDetailsResponse.ArticleId);
            commentDetailsResponse.User.Id.Should().Be(currentUserId);
            commentDetailsResponse.UpdatedOn.Should().BeCloseTo(DateTime.UtcNow, TWO_SECONDS_IN_MILLIONSECONDS);
            commentDetailsResponse.ChildrenCommentsCount.Should().Be(0);
            commentDetailsResponse.Ranking.Should().NotBeNull();
           // commentDetailsResponse.Ranking.CommentId.Should().Be(commentDetailsResponse.Id);
            commentDetailsResponse.Ranking.DidILike.Should().Be(false);
            commentDetailsResponse.Ranking.DidIDislike.Should().Be(false);
            commentDetailsResponse.Ranking.DidIFavor.Should().Be(false);
            commentDetailsResponse.Ranking.DislikesCount.Should().Be(0);
            commentDetailsResponse.Ranking.LikesCount.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(GetBadCommentPostRequest))]
        public async void Create_InvalidArticleInput_Failure(CommentRequest request)
        {
            var result = await commentsController.PostComment(request);
            result.Should().BeOfType<BadRequestObjectResult>();
        }


        [Fact]
        public async void Rank_ValidRankingLike_Success()
        {
            var result = await commentsController.PostComment(validCommentRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDetailsResponse = okResult.Value.Should().BeAssignableTo<CommentDetails>().Subject;

            var rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(true);
            commentRankingResponse.DidIDislike.Should().Be(false);
            commentRankingResponse.DidIFavor.Should().Be(false);
            commentRankingResponse.DislikesCount.Should().Be(0);
            commentRankingResponse.LikesCount.Should().Be(1);
        }

        [Fact]
        public async void Rank_ValidRankingDislike_Success()
        {
            var result = await commentsController.PostComment(validCommentRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDetailsResponse = okResult.Value.Should().BeAssignableTo<CommentDetails>().Subject;

            var rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Dislike };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(false);
            commentRankingResponse.DidIDislike.Should().Be(true);
            commentRankingResponse.DidIFavor.Should().Be(false);
            commentRankingResponse.DislikesCount.Should().Be(1);
            commentRankingResponse.LikesCount.Should().Be(0);
        }

        [Fact]
        public async void Rank_ValidRankingFavor_Success()
        {
            var result = await commentsController.PostComment(validCommentRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDetailsResponse = okResult.Value.Should().BeAssignableTo<CommentDetails>().Subject;

            var rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Favor };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(false);
            commentRankingResponse.DidIDislike.Should().Be(false);
            commentRankingResponse.DidIFavor.Should().Be(true);
            commentRankingResponse.DislikesCount.Should().Be(0);
            commentRankingResponse.LikesCount.Should().Be(0);
        }

        [Fact]
        public async void Rank_ValidRankingLikeThenDislike_Success()
        {
            var result = await commentsController.PostComment(validCommentRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDetailsResponse = okResult.Value.Should().BeAssignableTo<CommentDetails>().Subject;

            var rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(true);
            commentRankingResponse.DidIDislike.Should().Be(false);
            commentRankingResponse.DidIFavor.Should().Be(false);
            commentRankingResponse.DislikesCount.Should().Be(0);
            commentRankingResponse.LikesCount.Should().Be(1);

            rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Dislike };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(false);
            commentRankingResponse.DidIDislike.Should().Be(true);
            commentRankingResponse.DidIFavor.Should().Be(false);
            commentRankingResponse.DislikesCount.Should().Be(1);
            commentRankingResponse.LikesCount.Should().Be(0);
        }

        [Fact]
        public async void Rank_ValidRankingDisLikeThenLike_Success()
        {
            var result = await commentsController.PostComment(validCommentRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDetailsResponse = okResult.Value.Should().BeAssignableTo<CommentDetails>().Subject;

            var rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Dislike };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(false);
            commentRankingResponse.DidIDislike.Should().Be(true);
            commentRankingResponse.DidIFavor.Should().Be(false);
            commentRankingResponse.DislikesCount.Should().Be(1);
            commentRankingResponse.LikesCount.Should().Be(0);

            rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(true);
            commentRankingResponse.DidIDislike.Should().Be(false);
            commentRankingResponse.DidIFavor.Should().Be(false);
            commentRankingResponse.DislikesCount.Should().Be(0);
            commentRankingResponse.LikesCount.Should().Be(1);
        }

        [Fact]
        public async void Rank_ValidRankingDisLikeThenUndoDislike_Success()
        {
            var result = await commentsController.PostComment(validCommentRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDetailsResponse = okResult.Value.Should().BeAssignableTo<CommentDetails>().Subject;

            var rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Dislike };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(false);
            commentRankingResponse.DidIDislike.Should().Be(true);
            commentRankingResponse.DidIFavor.Should().Be(false);
            commentRankingResponse.DislikesCount.Should().Be(1);
            commentRankingResponse.LikesCount.Should().Be(0);

            rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Dislike };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(false);
            commentRankingResponse.DidIDislike.Should().Be(false);
            commentRankingResponse.DidIFavor.Should().Be(false);
            commentRankingResponse.DislikesCount.Should().Be(0);
            commentRankingResponse.LikesCount.Should().Be(0);
        }

        [Fact]
        public async void Rank_ValidRankingLikeThenUndolike_Success()
        {
            var result = await commentsController.PostComment(validCommentRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDetailsResponse = okResult.Value.Should().BeAssignableTo<CommentDetails>().Subject;

            var rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(true);
            commentRankingResponse.DidIDislike.Should().Be(false);
            commentRankingResponse.DidIFavor.Should().Be(false);
            commentRankingResponse.DislikesCount.Should().Be(0);
            commentRankingResponse.LikesCount.Should().Be(1);

            rankRequest = new CommentRankingRequest() { CommentId = commentDetailsResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await commentsController.RankComments(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            commentRankingResponse = okResult.Value.Should().BeAssignableTo<CommentRankingDetails>().Subject;
            commentRankingResponse.DidILike.Should().Be(false);
            commentRankingResponse.DidIDislike.Should().Be(false);
            commentRankingResponse.DidIFavor.Should().Be(false);
            commentRankingResponse.DislikesCount.Should().Be(0);
            commentRankingResponse.LikesCount.Should().Be(0);
        }
    }
}
