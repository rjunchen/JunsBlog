using JunsBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Comments
{
    public class CommentRequest
    {
        public string TargetId { get; set; }
        public string  CommentText { get; set; }
        public CommentType CommentType { get; set; }
    }
}
