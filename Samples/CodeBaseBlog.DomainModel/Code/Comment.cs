using System;
using System.Linq;
using Precio.Domain;

namespace CodeBaseBlog.DomainModel
{
    public static class Comments
    {
        public static Comment GetById(int commentId)
        {
            return UoW
                .Query<Comment>()
                .FirstOrDefault(c => c.Id == commentId);
        }

        public static void Remove(Comment comment)
        {
            UoW.Delete(comment);
        }
    }

    public partial class Comment
    {
        protected Comment()
        {
        }

        public Comment(Post belongsTo, UserInfo author, string body)
        {
            Post = belongsTo;
            User = author;
            Body = body;
            CreationDate = DateTime.Now;
        }

        public Comment Approve()
        {
            Approved = true;

            DomainEvents.Raise(new CommentGotApproved
                                   {
                                       Comment = this
                                   });
            return this;
        }
    }
}