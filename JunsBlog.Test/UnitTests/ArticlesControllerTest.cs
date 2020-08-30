﻿using FluentAssertions;
using JunsBlog.Controllers;
using JunsBlog.Entities;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Authentication;
using JunsBlog.Test.Mockups;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace JunsBlog.Test.UnitTests
{
    public class ArticlesControllerTest
    {
        private const int TWO_SECONDS_IN_MILLIONSECONDS = 2000;

        private static readonly ArticleRequest validArticleRequest = new ArticleRequest()
        {
            Abstract = "abstract",
            Categories = new string[] { "Food", "Games" },
            Title = "title",
            Content = "content",
        };

        private readonly static ArticleRequest invalidArticleRequestOne = new ArticleRequest()
        {
            Content = "content",
            Categories = new string[] { "Food", "Games" },
            Title = "title"
        };
        private readonly static ArticleRequest invalidArticleRequestTwo = new ArticleRequest()
        {
            Abstract = "abstract",
            Categories = new string[] { "Food", "Games" },
            Title = "title"
        };
        private readonly static ArticleRequest invalidArticleRequestThree = new ArticleRequest()
        {
            Abstract = "abstract",
            Categories = new string[] { "Food", "Games" },
            Content = "content"
        };

        private readonly static ArticleRequest invalidArticleRequestFour = new ArticleRequest() { };


        public static IEnumerable<Object[]> GetBadArticleCreationRequest()
        {
            yield return new object[] { invalidArticleRequestOne };
            yield return new object[] { invalidArticleRequestTwo };
            yield return new object[] { invalidArticleRequestThree };
            yield return new object[] { invalidArticleRequestFour };
        }


        private readonly ArticlesController ariclesController;
        private readonly string currentUserId;
        public ArticlesControllerTest()
        {
            var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = logFactory.CreateLogger<ArticlesController>();
            var notificationServiceFake = new NotificationServiceFake();
            var databaseService = new DatabaseServiceFake();

            var requestModel = new RegisterRequest() { Email = "Test@gmail.com", Name = "Tester", Password = "123456" };
            var user = new User(requestModel)
            {
                Id = ObjectId.GenerateNewId().ToString()
            };

            var currentUser = databaseService.SaveUserAsync(user).Result;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, currentUser.Name),
                new Claim(ClaimTypes.NameIdentifier, currentUser.Id)
            };

            var httpContextAccessFake = new HttpContextAccessorFake(claims);
            currentUserId = httpContextAccessFake.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;



            ariclesController = new ArticlesController(httpContextAccessFake, databaseService, logger);
        }


        [Fact]
        public async void Create_ValidArticleInput_Success()
        {
            var result = await ariclesController.CreateArticle(validArticleRequest);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Theory]
        [MemberData(nameof(GetBadArticleCreationRequest))]
        public async void Create_InvalidArticleInput_Failure(ArticleRequest request)
        {
            var result = await ariclesController.CreateArticle(request);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void Get_ValidArticleId_Success()
        {
            var result = await ariclesController.CreateArticle(validArticleRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleResponse = okResult.Value.Should().BeAssignableTo<Article>().Subject;

            result = await ariclesController.GetArticle(articleResponse.Id);
            result.Should().BeOfType<OkObjectResult>();
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleDetailsResponse = okResult.Value.Should().BeAssignableTo<ArticleDetails>().Subject;
            articleDetailsResponse.Should().NotBeNull();
            articleDetailsResponse.Title.Should().Be(validArticleRequest.Title);
            articleDetailsResponse.Abstract.Should().Be(validArticleRequest.Abstract);
            articleDetailsResponse.Content.Should().Be(validArticleRequest.Content);
            articleDetailsResponse.Author.Id.Should().Be(currentUserId);
            articleDetailsResponse.IsPrivate.Should().Be(false);
            articleDetailsResponse.IsApproved.Should().Be(false);
            //articleDetailsResponse.Categories.Should().Be(validArticleRequest.Categories.Count);
            articleDetailsResponse.Content.Should().Be(validArticleRequest.Content);
            articleDetailsResponse.UpdatedOn.Should().BeCloseTo(DateTime.UtcNow, TWO_SECONDS_IN_MILLIONSECONDS);
            articleDetailsResponse.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TWO_SECONDS_IN_MILLIONSECONDS);
        }

        [Fact]
        public async void Get_ValidArticleWithRanking_Success()
        {
            var result = await ariclesController.CreateArticle(validArticleRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleResponse = okResult.Value.Should().BeAssignableTo<Article>().Subject;

            var rankRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await ariclesController.RankArticle(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(true);
            articleRankingResponse.DidIDislike.Should().Be(false);
            articleRankingResponse.DidIFavor.Should().Be(false);
            articleRankingResponse.DislikesCount.Should().Be(0);
            articleRankingResponse.LikesCount.Should().Be(1);

            result = await ariclesController.GetArticle(articleResponse.Id);
            result.Should().BeOfType<OkObjectResult>();
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleDetailsResponse = okResult.Value.Should().BeAssignableTo<ArticleDetails>().Subject;
            articleDetailsResponse.Should().NotBeNull();
            articleDetailsResponse.Title.Should().Be(validArticleRequest.Title);
            articleDetailsResponse.Abstract.Should().Be(validArticleRequest.Abstract);
            articleDetailsResponse.Content.Should().Be(validArticleRequest.Content);
            articleDetailsResponse.Author.Id.Should().Be(currentUserId);
            articleDetailsResponse.IsPrivate.Should().Be(false);
            articleDetailsResponse.IsApproved.Should().Be(false);
            //articleDetailsResponse.Categories.Should().Be(validArticleRequest.Categories.Count);
            articleDetailsResponse.Content.Should().Be(validArticleRequest.Content);
            articleDetailsResponse.UpdatedOn.Should().BeCloseTo(DateTime.UtcNow, TWO_SECONDS_IN_MILLIONSECONDS);
            articleDetailsResponse.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TWO_SECONDS_IN_MILLIONSECONDS);
        }

        [Fact]
        public async void Get_InValidArticleId_Failure()
        {
            var result = await ariclesController.GetArticle(ObjectId.GenerateNewId().ToString());
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void Rank_ValidRankingLike_Success()
        {
            var result = await ariclesController.CreateArticle(validArticleRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleResponse = okResult.Value.Should().BeAssignableTo<Article>().Subject;

            var rankRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await ariclesController.RankArticle(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(true);
            articleRankingResponse.DidIDislike.Should().Be(false);
            articleRankingResponse.DidIFavor.Should().Be(false);
            articleRankingResponse.DislikesCount.Should().Be(0);
            articleRankingResponse.LikesCount.Should().Be(1);
        }

        [Fact]
        public async void Rank_ValidRankingDislike_Success()
        {
            var result = await ariclesController.CreateArticle(validArticleRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleResponse = okResult.Value.Should().BeAssignableTo<Article>().Subject;

            var rankRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Dislike };
            result = await ariclesController.RankArticle(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(false);
            articleRankingResponse.DidIDislike.Should().Be(true);
            articleRankingResponse.DidIFavor.Should().Be(false);
            articleRankingResponse.DislikesCount.Should().Be(1);
            articleRankingResponse.LikesCount.Should().Be(0);
        }

        [Fact]
        public async void Rank_ValidRankingFavor_Success()
        {
            var result = await ariclesController.CreateArticle(validArticleRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleResponse = okResult.Value.Should().BeAssignableTo<Article>().Subject;

            var rankRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Favor };
            result = await ariclesController.RankArticle(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(false);
            articleRankingResponse.DidIDislike.Should().Be(false);
            articleRankingResponse.DidIFavor.Should().Be(true);
            articleRankingResponse.DislikesCount.Should().Be(0);
            articleRankingResponse.LikesCount.Should().Be(0);
        }

        [Fact]
        public async void Rank_ValidRankingLikeThenDislike_Success()
        {
            var result = await ariclesController.CreateArticle(validArticleRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleResponse = okResult.Value.Should().BeAssignableTo<Article>().Subject;

            // Then likes the article
            var rankRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await ariclesController.RankArticle(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(true);
            articleRankingResponse.DidIDislike.Should().Be(false);
            articleRankingResponse.DidIFavor.Should().Be(false);
            articleRankingResponse.DislikesCount.Should().Be(0);
            articleRankingResponse.LikesCount.Should().Be(1);

            // then dislike the article, this will also change didIlike to false
            var rankDislikeRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Dislike };
            result = await ariclesController.RankArticle(rankDislikeRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(false);
            articleRankingResponse.DidIDislike.Should().Be(true);
            articleRankingResponse.DidIFavor.Should().Be(false);
            articleRankingResponse.DislikesCount.Should().Be(1);
            articleRankingResponse.LikesCount.Should().Be(0);
        }

        [Fact]
        public async void Rank_ValidRankingLikeThenUnDolike_Success()
        {
            var result = await ariclesController.CreateArticle(validArticleRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleResponse = okResult.Value.Should().BeAssignableTo<Article>().Subject;

            var rankRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await ariclesController.RankArticle(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(true);
            articleRankingResponse.DidIDislike.Should().Be(false);
            articleRankingResponse.DidIFavor.Should().Be(false);
            articleRankingResponse.DislikesCount.Should().Be(0);
            articleRankingResponse.LikesCount.Should().Be(1);

            // undo the like
            var rankDislikeRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await ariclesController.RankArticle(rankDislikeRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(false);
            articleRankingResponse.DidIDislike.Should().Be(false);
            articleRankingResponse.DidIFavor.Should().Be(false);
            articleRankingResponse.DislikesCount.Should().Be(0);
            articleRankingResponse.LikesCount.Should().Be(0);
        }

        [Fact]
        public async void Rank_ValidRankingFavorThenUndoFavor_Success()
        {
            var result = await ariclesController.CreateArticle(validArticleRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleResponse = okResult.Value.Should().BeAssignableTo<Article>().Subject;

      
            var rankRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Favor };
            result = await ariclesController.RankArticle(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(false);
            articleRankingResponse.DidIDislike.Should().Be(false);
            articleRankingResponse.DidIFavor.Should().Be(true);
            articleRankingResponse.DislikesCount.Should().Be(0);
            articleRankingResponse.LikesCount.Should().Be(0);


            // Undo the favor
            var rankDislikeRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Favor };
            result = await ariclesController.RankArticle(rankDislikeRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(false);
            articleRankingResponse.DidIDislike.Should().Be(false);
            articleRankingResponse.DidIFavor.Should().Be(false);
            articleRankingResponse.DislikesCount.Should().Be(0);
            articleRankingResponse.LikesCount.Should().Be(0);
        }

        [Fact]
        public async void GetRanking_ValidArticleId_Success()
        {
            // Create article
            var result = await ariclesController.CreateArticle(validArticleRequest);
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleResponse = okResult.Value.Should().BeAssignableTo<Article>().Subject;

            // Rank article
            var rankRequest = new ArticleRankingRequest() { ArticleId = articleResponse.Id, Rank = Models.Enums.RankEnum.Like };
            result = await ariclesController.RankArticle(rankRequest);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;

            // Get article ranking
            result = await ariclesController.GetArticleRanking(articleResponse.Id);
            okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(true);
            articleRankingResponse.DidIDislike.Should().Be(false);
            articleRankingResponse.DidIFavor.Should().Be(false);
            articleRankingResponse.DislikesCount.Should().Be(0);
            articleRankingResponse.LikesCount.Should().Be(1);
        }

        [Fact]
        public async void GetRanking_InvalidArticleId_Success()
        {
            // return the default ranking when the article doesn't exist
            var result = await ariclesController.GetArticleRanking(ObjectId.GenerateNewId().ToString());
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var articleRankingResponse = okResult.Value.Should().BeAssignableTo<ArticleRankingDetails>().Subject;
            articleRankingResponse.DidILike.Should().Be(false);
            articleRankingResponse.DidIDislike.Should().Be(false);
            articleRankingResponse.DidIFavor.Should().Be(false);
            articleRankingResponse.DislikesCount.Should().Be(0);
            articleRankingResponse.LikesCount.Should().Be(0);
        }
    }
}