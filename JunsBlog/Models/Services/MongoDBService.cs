using JunsBlog.Entities;
using JunsBlog.Interfaces.Services;
using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Articles;
using JunsBlog.Models.Comments;
using JunsBlog.Models.Enums;
using JunsBlog.Models.Profile;
using Microsoft.VisualBasic.CompilerServices;
using MimeKit.Encodings;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JunsBlog.Models.Services
{
    public class MongoDBService : IDatabaseService
    {
        private readonly IMongoCollection<User> users;
        private readonly IMongoCollection<Article> articles;
        private readonly IMongoCollection<ArticleRanking> articleRankings;
        private readonly IMongoCollection<CommentRanking> commentRankings;
        private readonly IMongoCollection<Comment> comments;

        public MongoDBService(IJunsBlogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<User>(settings.UsersCollectionName);
            articles = database.GetCollection<Article>(settings.ArticleCollectionName);
            articleRankings = database.GetCollection<ArticleRanking>(settings.RankingCollectionName);
            comments = database.GetCollection<Comment>(settings.CommentCollectionName);
            commentRankings = database.GetCollection<CommentRanking>(settings.CommentRankingCollectionName);
        }

        #region Users
        public async Task<User> GetUserAsync(string userId)
        {
            var user = await users.Find<User>(x => x.Id == userId).SingleOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await users.Find<User>(x => x.Email.ToLower() == email.ToLower()).SingleOrDefaultAsync();
            return user;
        }

        public async Task<User> SaveUserAsync(User user)
        {
            user.UpdatedOn = DateTime.UtcNow;
            await users.ReplaceOneAsync(s => s.Id == user.Id, user, new ReplaceOptions { IsUpsert = true });
            return user;
        }
        #endregion


        #region Articles
        public async Task<Article> SaveArticleAsync(Article article)
        {
            article.UpdatedOn = DateTime.UtcNow;
            await articles.ReplaceOneAsync(s => s.Id == article.Id, article, new ReplaceOptions { IsUpsert = true });
            return article;
        }
        public async Task<Article> GetArticleAsync(string articleId)
        {
            return await articles.Find(s => s.Id == articleId).SingleOrDefaultAsync();
        }

        public async Task<ArticleBasicInfo> GetArticleBasicInfoAsync(string articleId)
        {
            return await articles.Find(s => s.Id == articleId).Project(x => new ArticleBasicInfo()
            {
                Id = x.Id,
                Title = x.Title,
                Abstract = x.Abstract,
                Categories = x.Categories,
                Content = x.Content,
                IsPrivate = x.IsPrivate
            }).SingleOrDefaultAsync();
        }


        public async Task<ArticleDetails> GetArticleDetailsAsync(string articleId, string currentUserId)
        {
            var query = articles.AsQueryable().Where(x => x.Id == articleId).Join(users.AsQueryable(), x => x.AuthorId, y => y.Id, (x, y) => new { article = x, author = y })
                        .Select(a => new ArticleDetails
                        {
                            Id = a.article.Id,
                            Title = a.article.Title,
                            UpdatedOn = a.article.UpdatedOn,
                            CreatedOn = a.article.CreatedOn,
                            IsApproved = a.article.IsApproved,
                            IsPrivate = a.article.IsPrivate,
                            Views = a.article.Views,
                            Content = a.article.Content,
                            Categories = a.article.Categories,
                            Author = a.author
                        }).GroupJoin(articleRankings.AsQueryable(), x => x.Id, y => y.Id, (x, y) => new { article = x, rankings = y })
                        .Select(a => new ArticleDetails
                        {
                            Id = a.article.Id,
                            Title = a.article.Title,
                            UpdatedOn = a.article.UpdatedOn,
                            CreatedOn = a.article.CreatedOn,
                            IsApproved = a.article.IsApproved,
                            IsPrivate = a.article.IsPrivate,
                            Views = a.article.Views,
                            Content = a.article.Content,
                            Categories = a.article.Categories,
                            Author = a.article.Author
                        }).GroupJoin(comments.AsQueryable(), x => x.Id, y => y.ArticleId, (x, y) => new { article = x, comments = y })
                        .Select(a => new ArticleDetails
                        {
                            Id = a.article.Id,
                            Title = a.article.Title,
                            UpdatedOn = a.article.UpdatedOn,
                            CreatedOn = a.article.CreatedOn,
                            IsApproved = a.article.IsApproved,
                            IsPrivate = a.article.IsPrivate,
                            Views = a.article.Views,
                            Content = a.article.Content,
                            Categories = a.article.Categories,
                            Author = a.article.Author,
                            CommentsCount = a.comments.Count()
                        });

            var articleDetails = await query.FirstOrDefaultAsync();
            return articleDetails;
        }

        private class ArticleWithRankings : ArticleDetails
        {
            public IEnumerable<ArticleRanking> Rankings { get; set; }
        }

        private IMongoQueryable<ArticleWithRankings> GenerateArticleDetailsQuery()
        {
            var query = articles.AsQueryable().GroupJoin(comments.AsQueryable(), x => x.Id, y => y.ArticleId, (x, y) => new { article = x, comments = y })
                            .Select(a => new
                            {
                                Id = a.article.Id,
                                Title = a.article.Title,
                                Abstract = a.article.Abstract,
                                UpdatedOn = a.article.UpdatedOn,
                                CreatedOn = a.article.CreatedOn,
                                IsApproved = a.article.IsApproved,
                                IsPrivate = a.article.IsPrivate,
                                Views = a.article.Views,
                                Content = a.article.Content,
                                AuthorId = a.article.AuthorId,
                                Categories = a.article.Categories,
                                GalleryImages = a.article.GalleryImages,
                                commentsCount = a.comments.Count()
                            }).Join(users.AsQueryable(), x => x.AuthorId, y => y.Id, (x, y) => new ArticleDetails
                            {
                                Id = x.Id,
                                Title = x.Title,
                                Abstract = x.Abstract,
                                UpdatedOn = x.UpdatedOn,
                                CreatedOn = x.CreatedOn,
                                IsApproved = x.IsApproved,
                                IsPrivate = x.IsPrivate,
                                Views = x.Views,
                                Content = x.Content,
                                Categories = x.Categories,
                                GalleryImages = x.GalleryImages,
                                Author = y,
                                CommentsCount = x.commentsCount
                            }).GroupJoin(articleRankings.AsQueryable(), x => x.Id, y => y.ArticleId, (x, y) => new ArticleWithRankings
                            {
                                Id = x.Id,
                                Title = x.Title,
                                Abstract = x.Abstract,
                                UpdatedOn = x.UpdatedOn,
                                CreatedOn = x.CreatedOn,
                                IsApproved = x.IsApproved,
                                IsPrivate = x.IsPrivate,
                                Views = x.Views,
                                Content = x.Content,
                                Categories = x.Categories,
                                GalleryImages = x.GalleryImages,
                                Author = x.Author,
                                CommentsCount = x.CommentsCount,
                                Rankings = y
                            });
            return query;
        }

        public async Task<ArticleSearchPagingResult> SearchArticlesAsyc(ArticleSearchPagingOption options)
        {
            var query = GenerateArticleDetailsQuery();

            if (!string.IsNullOrEmpty(options.SearchKey)) query = query.Where(x => x.Content.ToLower().Contains(options.SearchKey.ToLower()));

            switch (options.SortBy)
            {
                case SortByEnum.UpdatedOn:
                    if (options.SortOrder == SortOrderEnum.Ascending)
                        query = query.OrderBy(x => x.UpdatedOn);
                    else
                        query = query.OrderByDescending(x => x.UpdatedOn);
                    break;
                case SortByEnum.Views:
                    if (options.SortOrder == SortOrderEnum.Ascending)
                        query = query.OrderBy(x => x.Views);
                    else
                        query = query.OrderByDescending(x => x.Views);
                    break;
            }

            switch (options.Filter)
            {
                case ArticleFilterEnum.MyArticles:
                    query = query.Where(x => x.Author.Id == options.ProfilerId);
                    break;
                case ArticleFilterEnum.MyLikes:
                    query = query.Where(x => x.Rankings.Any(x => x.DidILike == true && x.UserId == options.ProfilerId));
                    break;
                case ArticleFilterEnum.MyFavorites:
                    query = query.Where(x => x.Rankings.Any(x => x.DidIFavor == true && x.UserId == options.ProfilerId));
                    break;
            }

            var docsCount = await query.CountAsync();

            var documents = await query.Skip((options.CurrentPage - 1) * options.PageSize).Take(options.PageSize).ToListAsync();

            var articleDetailsList = new List<ArticleDetails>();

            foreach (var item in documents)
            {
                item.Content = null; // Don't return the content
                var articleDetails = new ArticleDetails()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Abstract = item.Abstract,
                    UpdatedOn = item.UpdatedOn,
                    CreatedOn = item.CreatedOn,
                    IsApproved = item.IsApproved,
                    IsPrivate = item.IsPrivate,
                    Views = item.Views,
                    Content = item.Content,
                    Categories = item.Categories,
                    GalleryImages = item.GalleryImages,
                    Author = item.Author,
                    CommentsCount = item.CommentsCount,
                    Ranking = new ArticleRankingDetails(item.Id, options.ProfilerId, item.Rankings)
                };
                articleDetailsList.Add(articleDetails);
            }

            return new ArticleSearchPagingResult(articleDetailsList, docsCount, options);
        }

        public async Task<ProfileDetails> GetProfileDetailsAsync(string currentUserId)
        {
            var query = users.AsQueryable().Where(x => x.Id == currentUserId).GroupJoin(articles.AsQueryable(), x => x.Id, y => y.AuthorId, (x, y) => new { UserId = x.Id, Articles = y, })
           .Select(a => new
           {
               Id = a.UserId,
               ArticlesCount = a.Articles.Count()
           }).GroupJoin(articleRankings.AsQueryable(), x => x.Id, y => y.UserId, (x, y) => new
           {
               Id = x.Id,
               ArticlesCount = x.ArticlesCount,
               Rankings = y
           }).Select(a => new
           {
               Id = a.Id,
               ArticlesCount = a.ArticlesCount,
               FavorsCount = a.Rankings.Count(x => x.DidIFavor == true),
               LikesCount = a.Rankings.Count(x => x.DidILike == true),
           }).GroupJoin(users, x => x.Id, y => y.Id, (x, y) => new
           {
               Id = x.Id,
               ArticlesCount = x.ArticlesCount,
               FavorsCount = x.FavorsCount,
               LikesCount = x.LikesCount,
               Users = y
           }).Select(a => new ProfileDetails
           {
               LikesCount = a.LikesCount,
               ArticlesCount = a.ArticlesCount,
               FavorsCount = a.FavorsCount,
               User = a.Users.First()
           });
            return await query.FirstOrDefaultAsync();
        }

        #endregion


        #region ArtileRankings
        public async Task<ArticleRanking> GetArticleRankingAsync(string articleId, string userId)
        {
            return await articleRankings.Find<ArticleRanking>(x => x.ArticleId == articleId && x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<ArticleRanking> SaveArticleRankingAsync(ArticleRanking ranking)
        {
            ranking.UpdatedOn = DateTime.UtcNow;
            await articleRankings.ReplaceOneAsync(s => s.Id == ranking.Id, ranking, new ReplaceOptions { IsUpsert = true });
            return ranking;
        }

        public async Task<List<ArticleRanking>> GetArticleRankingsAsync(string articleId)
        {
            return await articleRankings.Find<ArticleRanking>(x => x.ArticleId == articleId).ToListAsync();
        }
        #endregion


        #region Comments
        public async Task<List<Comment>> GetCommentsAsync(string articleId)
        {
            return await comments.Find<Comment>(x => x.ArticleId == articleId).ToListAsync();
        }

        public async Task<Comment> SaveCommentAsync(Comment comment)
        {
            comment.UpdatedOn = DateTime.UtcNow;

            await comments.ReplaceOneAsync(s => s.Id == comment.Id, comment, new ReplaceOptions { IsUpsert = true });

            return comment;
        }

        private IMongoQueryable<CommentDetails> GenerateCommentsDetailsQuery()
        {
            var query = comments.AsQueryable().GroupJoin(comments.AsQueryable(), x => x.Id, y => y.ParentId, (x, y) => new { comment = x, childrenComments = y })
                            .Select(a => new
                            {
                                Id = a.comment.Id,
                                CommentText = a.comment.CommentText,
                                UpdatedOn = a.comment.UpdatedOn,
                                ArticleId = a.comment.ArticleId,
                                ChildrenCommentsCount = a.childrenComments.Count(),
                                ParentId = a.comment.ParentId,
                                UserId = a.comment.UserId,

                            }).Join(users.AsQueryable(), x => x.UserId, y => y.Id, (x, y) => new CommentDetails
                            {
                                Id = x.Id,
                                CommentText = x.CommentText,
                                UpdatedOn = x.UpdatedOn,
                                ArticleId = x.ArticleId,
                                ChildrenCommentsCount = x.ChildrenCommentsCount,
                                ParentId = x.ParentId,
                                User = y
                            });
            return query;
        }

        public async Task<CommentSearchPagingResult> SearchCommentsAsync(CommentSearchPagingOption options, string currentUserId)
        {
            var query = GenerateCommentsDetailsQuery();

            if (!string.IsNullOrEmpty(options.SearchKey))
            {
                switch (options.SearchOn)
                {
                    case CommentSearchOnEnum.ArticleId:
                        query = query.Where(x => x.ArticleId == options.SearchKey.Trim());
                        break;
                    case CommentSearchOnEnum.ParentId:
                        query = query.Where(x => x.ParentId == options.SearchKey.Trim());
                        break;
                    case CommentSearchOnEnum.CommentText:
                        query = query.Where(x => x.CommentText.Contains(options.SearchKey.Trim()));
                        break;
                    default:
                        break;
                }
            }

            switch (options.SortBy)
            {
                case SortByEnum.UpdatedOn:
                    if (options.SortOrder == SortOrderEnum.Ascending)
                        query = query.OrderBy(x => x.UpdatedOn);
                    else
                        query = query.OrderByDescending(x => x.UpdatedOn);
                    break;
            }

            var docsCount = await query.CountAsync();

            var documents = await query.Skip((options.CurrentPage - 1) * options.PageSize).Take(options.PageSize).ToListAsync();

            foreach (var item in documents)
            {
                item.Ranking = await GetCommentRankingDetailsAsync(item.Id, currentUserId);
            }

            return new CommentSearchPagingResult(documents, docsCount, options);
        }


        #endregion


        #region CommentRanking
        public async Task<CommentRanking> GetCommentRankingAsync(string commentId, string userId)
        {
            return await commentRankings.Find<CommentRanking>(x => x.CommentId == commentId && x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<CommentRanking>> GetCommentRankingsAsync(string commentId)
        {
            return await commentRankings.Find<CommentRanking>(x => x.CommentId == commentId).ToListAsync();
        }

        public async Task<CommentRanking> SaveCommentRankingAsync(CommentRanking ranking)
        {
            ranking.UpdatedOn = DateTime.UtcNow;
            await commentRankings.ReplaceOneAsync(s => s.Id == ranking.Id, ranking, new ReplaceOptions { IsUpsert = true });
            return ranking;
        }

        public async Task<CommentRankingDetails> GetCommentRankingDetailsAsync(string commentId, string userId)
        {
            var rankings = await commentRankings.Find(x => x.CommentId == commentId).ToListAsync();

            var rankingResponse = new CommentRankingDetails();

            foreach (var item in rankings)
            {
                if (item.DidIDislike) rankingResponse.DislikesCount++;
                if (item.DidILike) rankingResponse.LikesCount++;
                rankingResponse.DidIFavor = item.DidIFavor;

                if (item.UserId == userId)
                {
                    rankingResponse.DidIDislike = item.DidIDislike;
                    rankingResponse.DidILike = item.DidILike;
                    rankingResponse.DidIFavor = item.DidIFavor;
                }
            }
            return rankingResponse;
        }

        #endregion

    }
}
