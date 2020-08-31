using JunsBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Profile
{
    public class Profile
    {
        public User User { get; set; }
        public int FavorsCount { get; set; }
        public int LikesCount { get; set; }
        public int ArticlesCount { get; set; }
    }
}
