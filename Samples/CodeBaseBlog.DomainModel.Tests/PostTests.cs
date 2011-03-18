using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Precio.Domain;

namespace CodeBaseBlog.DomainModel.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
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
        public void can_find_post_by_id()
        {
            var p1 = new Post();
            SetProperty(p1, "Id", 1);
            var p2 = new Post();
            SetProperty(p2, "Id", 2);
            var p3 = new Post();
            SetProperty(p3, "Id", 3);

            testdata.Add(p1);
            testdata.Add(p2);
            testdata.Add(p3);

            using (UoW.Create())
            {
                Post post = Posts.GetById(2); //find post #2
                Assert.AreEqual(2, post.Id); //assert that we got the correct post back
            }
        }

        [ExpectedException(typeof (Exception))]
        [TestMethod]
        public void can_not_reply_to_post_when_comments_are_disabled()
        {
            using (UoW.Create())
            {
                new Post()
                    .DisableComments()
                    .ReplyTo(new UserInfo("Roger", "", "", ""), "This should not work");
            }
        }

        [TestMethod]
        public void can_reply_to_post_when_comments_are_enabled()
        {
            using (UoW.Create())
            {
                bool eventWasFired = false;
                DomainEvents.When<UserCommentedOnPost>().Then(e => eventWasFired = true);

                new Post()
                    .EnableComments()
                    .ReplyTo(new UserInfo("Roger", "", "", ""), "This should work");

                UoW.Commit();
                Assert.IsTrue(eventWasFired);
            }
        }

        [TestMethod]
        public void can_publish_post()
        {
            using (UoW.Create())
            {
                var post = new Post();
                Assert.IsNull(post.PublishDate); //unpublished
                post.Publish();
                Assert.IsNotNull(post.PublishDate); //published
            }
        }

        private static void SetProperty(object obj, string property, object value)
        {
            obj.GetType().GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).
                SetValue(obj, value, null);
        }
    }
}