using System.Linq;
using CodeBaseBlog.DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Precio.Domain;

namespace CodeBaseBlog.PresentationModel.Tests
{
    [TestClass]
    public class PostTests
    {
        private InMemUoW testdata;

        [TestInitialize]
        public void SetupTest()
        {
            testdata = new InMemUoW();
            UoW.UoWFactory = () => testdata;
        }

        [TestMethod]
        public void user_can_see_own_unapproved_comments()
        {
            using (UoW.Create())
            {
                NewPost()
                    .ReplyTo(new UserInfo("Roger", "", "", "myid"), "this comment should be listed");


                PostProjectionWithComments presentedPost = PostProjections.GetById(0, "myid");

                //the comment is unapproved but created by the viewing user, we should get one
                Assert.AreEqual(1, presentedPost.Comments.Count());
            }
        }

        [TestMethod]
        public void user_can_not_see_other_users_unapproved_comments()
        {
            using (UoW.Create())
            {
                NewPost()
                    .ReplyTo(new UserInfo("Roger", "", "", "myid"), "this comment should not be listed");


                PostProjectionWithComments presentedPost = PostProjections.GetById(0, "otherid");

                //the comment is unapproved and by another user, we should get none
                Assert.AreEqual(0, presentedPost.Comments.Count());
            }
        }

        [TestMethod]
        public void user_can_see_other_users_approved_comments()
        {
            using (UoW.Create())
            {
                NewPost()
                    .ReplyTo(new UserInfo("Roger", "", "", "myid"), "this comment should be listed")
                    .Approve();

                PostProjectionWithComments presentedPost = PostProjections.GetById(0, "otherid");

                //the comment is approved, we should get one
                Assert.AreEqual(1, presentedPost.Comments.Count());
            }
        }

        private static Post NewPost()
        {
            Post post =
                new Post()
                    .EnableComments()
                    .Publish();

            Posts.Add(post);
            return post;
        }
    }
}