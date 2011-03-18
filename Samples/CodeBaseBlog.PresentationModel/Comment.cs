using System;
using System.Collections.Generic;
using System.Linq;
using CodeBaseBlog.DomainModel;
using Precio.Domain;

namespace CodeBaseBlog.PresentationModel
{
    public class CommentProjection
    {
        public string Body { get; set; }
        public bool Approved { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserWebSite { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public static class CommentProjections
    {
        public static IList<CommentProjection> GetAllUnapproved()
        {
            return UoW
                .Query<Comment>()
                .Where(c => c.Approved == false)
                .AsProjection()
                .ToList();
        }

        public static IQueryable<CommentProjection> AsProjection(this IQueryable<Comment> self)
        {
            return self.Select(c => new CommentProjection
                                        {
                                            Approved = c.Approved,
                                            Body = c.Body,
                                            CreationDate = c.CreationDate,
                                            UserEmail = c.User.Email,
                                            UserName = c.User.Name,
                                            UserWebSite = c.User.WebSite
                                        });
        }
    }
}