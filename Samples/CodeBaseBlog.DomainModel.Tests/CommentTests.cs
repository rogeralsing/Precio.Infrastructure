using Microsoft.VisualStudio.TestTools.UnitTesting;
using Precio.Domain;

namespace CodeBaseBlog.DomainModel.Tests
{
    [TestClass]
    public class CommentTests
    {
        private InMemUoW testdata;

        [TestInitialize]
        public void SetupTest()
        {
            testdata = new InMemUoW();
            UoW.UoWFactory = () => testdata;
        }

        public void can_approve_comments()
        {
            using (UoW.Create())
            {
                bool eventWasFired = false;
                DomainEvents.When<CommentGotApproved>().Then(e => eventWasFired = true);

                Comment comment =
                    NewPost()
                        .ReplyTo(new UserInfo("Roger", "", "", "myid"), "this should fire an event")
                        .Approve();

                Assert.IsTrue(comment.Approved);
                Assert.IsTrue(eventWasFired);
            }
        }

        private static Post NewPost()
        {
            var post = new Post();
            Posts.Add(post);

            post.EnableComments();
            post.Publish();
            return post;
        }
    }
}