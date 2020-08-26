using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Comments
{
    public class CommentRequest
    {
        public string TargetId { get; set; }
        public string  Content { get; set; }
    }
}
