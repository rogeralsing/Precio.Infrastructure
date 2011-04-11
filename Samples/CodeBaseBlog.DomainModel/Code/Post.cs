using System;
using System.Linq;
using Precio.Domain;

namespace CodeBaseBlog.DomainModel
{
    public static class Posts 
    {
        public static void Add(Post post)
        {
            UoW
                .Add(post);
        }

        public static void Remove(Post post)
        {
            UoW
                .Delete(post);
        }

        public static Post GetById(int postId)
        {
            return UoW
                .Query<Post>()
                //   .Include(p => p.Comments)
                .FirstOrDefault(p => p.Id == postId);
        }
    }


    public partial class Post : AggregateRoot<Post, int>
    {
        public Comment ReplyTo(UserInfo user, string body)
        {
            if (!CommentsEnabled)
                throw new Exception("Comments are not allowed");

            var reply = new Comment(this, user, body);
            Comments.Add(reply);
            DomainEvents.Raise(new UserCommentedOnPost
                                   {
                                       User = user,
                                       Comment = reply,
                                       Post = this,
                                   });

            return reply;
        }


        public Post DisableComments()
        {
            CommentsEnabled = false;
            return this;
        }

        public Post EnableComments()
        {
            CommentsEnabled = true;
            return this;
        }

        public Post Publish()
        {
            PublishDate = DateTime.Now;
            return this;
        }
    }
}