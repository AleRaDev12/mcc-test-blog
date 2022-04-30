using System.Collections.Generic;
using System.Linq;

namespace Blog
{
    public class BlogService
    {
        private static string UserCommentsCountToString(List<UserCommentsCountDto> objects)
        {
            string stringResult = "";

            foreach (var item in objects)
            {
                stringResult += item.UserName + ": " + item.Count + System.Environment.NewLine;
            }

            return stringResult;
        }

        public static List<UserCommentsCountDto> NumberOfCommentsPerUser(MyDbContext context)
        {
            return context.BlogComments.GroupBy(c => c.UserName).Select(c => new UserCommentsCountDto
            { 
                UserName = c.Key,
                Count = c.Count() 
            }).ToList();
        }

        public static string NumberOfCommentsPerUserString(MyDbContext context)
        {
            return UserCommentsCountToString(NumberOfCommentsPerUser(context));
        }


        public static List<BlogPostDto> PostsOrderedByLastCommentDate(MyDbContext context)
        {
            return context.BlogPosts.Select(p => new BlogPostDto
            {
                Title = p.Title,
                LastComment = p.Comments.OrderByDescending(c => c.CreatedDate).Select(c => new CommentDto
                {
                    CreatedDate = c.CreatedDate,
                    Text = c.Text
                }).FirstOrDefault()
            }).OrderByDescending(p => p.LastComment.CreatedDate).ToList();
        }

        public static string PostsOrderedByLastCommentDateString(MyDbContext context)
        {
            var objects = PostsOrderedByLastCommentDate(context);
            string stringResult = "";

            foreach (var item in objects)
            {
                stringResult += item.Title + ": '" + item.LastComment.CreatedDate.ToString("yyyy-MM-dd") + "', '" + item.LastComment.Text + "'" + System.Environment.NewLine;
            }

            return stringResult;
        }



        public static List<UserCommentsCountDto> NumberOfLastCommentsLeftByUser(MyDbContext context)
        {
            var lastComments = context.BlogPosts
                .Select(p => p.Comments.OrderByDescending(c => c.CreatedDate).FirstOrDefault()).ToList();

            return lastComments.GroupBy(c => c.UserName)
                .Select(g => new UserCommentsCountDto
                {
                    UserName = g.Key,
                    Count = g.Count()
                }).ToList();

        }

        public static string NumberOfLastCommentsLeftByUserString(MyDbContext context)
        {
            return UserCommentsCountToString(NumberOfLastCommentsLeftByUser(context));

        }
    }
}
