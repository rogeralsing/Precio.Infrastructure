using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CodeBaseBlog.DomainModel;
using Precio.Domain;

namespace CodeBaseBlog.PresentationModel
{
    public class PostProjection
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; }
        public int CommentCount { get; set; }
        public IEnumerable<CategoryProjection> Categories { get; set; }

        public string CategoryString
        {
            get { return string.Join(", ", Categories.Select(c => string.Format("<a href=\"{0}\">{1}</a>", c.Id, c.Name))); }
        }

        public string PublishDateString
        {
            get { return PublishDate.ToShortDateString(); }
        }
    }

    public class PostProjectionWithComments
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; }
        public IEnumerable<CommentProjection> Comments { get; set; }

        public IEnumerable<CategoryProjection> Categories { get; set; }

        public string CategoryString
        {
            get { return string.Join(", ", Categories.Select(c => string.Format("<a href=\"{0}\">{1}</a>", c.Id, c.Name))); }
        }

        public string PublishDateString
        {
            get { return PublishDate.ToShortDateString(); }
        }
    }

    public static class PostProjections
    {
        public static IList<PostProjection> GetLatestPosts(int pageSize, int page, string userId)
        {
            return UoW
                .Query<Post>()
                .Where(p => p.PublishDate != null)
                .OrderByDescending(p => p.PublishDate)
                .Skip(page*pageSize)
                .Take(pageSize)
                .AsProjection(userId)
                .ToList();
        }

        private static IQueryable<Post> GetByDateDesc(int pageSize, int page, Expression<Func<Post, bool>> predicate)
        {
            return UoW
                .Query<Post>()
                .Where(p => p.PublishDate != null)
                .Where(predicate)
                .OrderByDescending(p => p.PublishDate)
                .Skip(page*pageSize)
                .Take(pageSize);
        }

        public static IList<PostProjection> GetByCategory(int categoryId, int pageSize, int page, string userId)
        {
            return GetByDateDesc(pageSize, page, p => p.Categories.Any(c => c.Id == categoryId))
                .AsProjection(userId)
                .ToList();
        }

        private static IQueryable<PostProjection> AsProjection(this IQueryable<Post> source, string userId)
        {
            return source
                .Select(p =>
                        new PostProjection
                            {
                                Id = p.Id,
                                Body = p.Body,
                                Categories = p.Categories.Select(c => new CategoryProjection {Id = c.Id, Name = c.Name}),
                                CommentCount = p.Comments.Where(c => c.Approved || c.User.UserId == userId).Count(),
                                //count of approved comments
                                PublishDate = p.PublishDate.Value,
                                Subject = p.Subject,
                            });
        }

        public static PostProjectionWithComments GetById(int postId, string userId)
        {
            return UoW
                .Query<Post>()
                .Select(p =>
                        new PostProjectionWithComments
                            {
                                Id = p.Id,
                                Body = p.Body,
                                Categories = p.Categories.Select(c => new CategoryProjection {Id = c.Id, Name = c.Name}),
                                PublishDate = p.PublishDate.Value,
                                Subject = p.Subject,
                                Comments = p.Comments
                                    .Where(c => c.Approved || c.User.UserId == userId)
                                    .Select(c => new CommentProjection
                                                     {
                                                         Approved = c.Approved,
                                                         Body = c.Body,
                                                         CreationDate = c.CreationDate,
                                                         UserEmail = c.User.Email,
                                                         UserName = c.User.Name,
                                                         UserWebSite = c.User.WebSite
                                                     })
                            })
                .First(p => p.Id == postId);
        }
    }
}