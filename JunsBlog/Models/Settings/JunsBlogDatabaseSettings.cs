using JunsBlog.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Settings
{
    public class JunsBlogDatabaseSettings : IJunsBlogDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string UserTokensCollectionName { get; set; }
        public string ArticleCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string RankingCollectionName { get; set; }
        public string CommentCollectionName { get; set; }
    }
}
