using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog
{
    public class UserCommentsCountDto
    {
        public string UserName { get; set; }
        public int Count { get; set; }
    }

    public class BlogPostDto
    {
        public string Title { get; set; }
        public CommentDto LastComment { get; set; }
    }

    public class CommentDto
    {
        public DateTime CreatedDate { get; set; }
        public string Text { get; set; }


        public string UserName { get; set; }
    }
}
