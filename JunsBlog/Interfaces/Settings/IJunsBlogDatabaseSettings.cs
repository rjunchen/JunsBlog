using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Interfaces.Settings
{
    public interface IJunsBlogDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string UserTokensCollectionName { get; set; }
        string ArticleCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string RankingCollectionName { get; set; }
        string CommentCollectionName { get; set; }
    }
}
