using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Blog
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

            var context = new MyDbContext(loggerFactory);
            context.Database.EnsureCreated();
            InitializeData(context);

            Console.WriteLine("All posts:");
            var data = context.BlogPosts.Select(x => x.Title).ToList();
            Console.WriteLine(JsonSerializer.Serialize(data));


            Console.WriteLine("\n\n\n");


            Console.WriteLine("--- raw data output ---");
            Console.WriteLine();

            Console.WriteLine("How many comments each user left:");
            // Expected result (format could be different, e.g. object serialized to JSON is ok):
            Console.WriteLine(JsonSerializer.Serialize(BlogService.NumberOfCommentsPerUser(context)));
            Console.WriteLine();

            Console.WriteLine("Posts ordered by date of last comment. Result should include text of last comment:");
            // Expected result (format could be different, e.g. object serialized to JSON is ok):
            Console.WriteLine(JsonSerializer.Serialize(BlogService.PostsOrderedByLastCommentDate(context)));
            Console.WriteLine();

            Console.WriteLine("How many last comments each user left:");
            // Expected result (format could be different, e.g. object serialized to JSON is ok):
            Console.WriteLine(JsonSerializer.Serialize(BlogService.NumberOfLastCommentsLeftByUser(context)));

            Console.WriteLine("\n\n\n");



            var loggerFactoryWithoutLog = LoggerFactory.Create(builder => { });

            var contextForForamatedOutput = new MyDbContext(loggerFactoryWithoutLog);
            contextForForamatedOutput.Database.EnsureCreated();
            InitializeData(contextForForamatedOutput);

            Console.WriteLine("--- formated data output without logs ---");
            Console.WriteLine();


            Console.WriteLine("How many comments each user left:");
            Console.WriteLine(BlogService.NumberOfCommentsPerUserString(contextForForamatedOutput));
            // formated output:
            // Elena: 3
            // Ivan: 4
            // Petr: 2

            Console.WriteLine("Posts ordered by date of last comment. Result should include text of last comment:");
            Console.WriteLine(BlogService.PostsOrderedByLastCommentDateString(contextForForamatedOutput));
            // formated output:
            // Post2: '2020-03-06', '4'
            // Post1: '2020-03-05', '8'
            // Post3: '2020-02-14', '9'

            Console.WriteLine("How many last comments each user left:");
            Console.WriteLine(BlogService.NumberOfLastCommentsLeftByUserString(contextForForamatedOutput));
            // formated output:
            // Ivan: 2
            // Petr: 1
        }

        private static void InitializeData(MyDbContext context)
        {
            context.BlogPosts.Add(new BlogPost("Post1")
            {
                Comments = new List<BlogComment>()
                {
                    new BlogComment("1", new DateTime(2020, 3, 2), "Petr"),
                    new BlogComment("2", new DateTime(2020, 3, 4), "Elena"),
                    new BlogComment("8", new DateTime(2020, 3, 5), "Ivan"),
                }
            });
            context.BlogPosts.Add(new BlogPost("Post2")
            {
                Comments = new List<BlogComment>()
                {
                    new BlogComment("3", new DateTime(2020, 3, 5), "Elena"),
                    new BlogComment("4", new DateTime(2020, 3, 6), "Ivan"),
                }
            });
            context.BlogPosts.Add(new BlogPost("Post3")
            {
                Comments = new List<BlogComment>()
                {
                    new BlogComment("5", new DateTime(2020, 2, 7), "Ivan"),
                    new BlogComment("6", new DateTime(2020, 2, 9), "Elena"),
                    new BlogComment("7", new DateTime(2020, 2, 10), "Ivan"),
                    new BlogComment("9", new DateTime(2020, 2, 14), "Petr"),
                }
            });
            context.SaveChanges();
        }
    }
}