//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Data.EntityClient;
using System.Data.Objects;

namespace CodeBaseBlog.DomainModel
{
    public class MyBlogConnection : ObjectContext
    {
        public const string ConnectionString = "name=MyBlogConnection";
        public const string ContainerName = "MyBlogConnection";

        #region Constructors

        public MyBlogConnection()
            : base(ConnectionString, ContainerName)
        {
            ContextOptions.LazyLoadingEnabled = true;
        }

        public MyBlogConnection(string connectionString)
            : base(connectionString, ContainerName)
        {
            ContextOptions.LazyLoadingEnabled = true;
        }

        public MyBlogConnection(EntityConnection connection)
            : base(connection, ContainerName)
        {
            ContextOptions.LazyLoadingEnabled = true;
        }

        #endregion

        #region ObjectSet Properties

        private ObjectSet<Category> _categories;

        private ObjectSet<Comment> _comments;

        private ObjectSet<Post> _posts;

        public ObjectSet<Category> Categories
        {
            get { return _categories ?? (_categories = CreateObjectSet<Category>("Categories")); }
        }

        public ObjectSet<Comment> Comments
        {
            get { return _comments ?? (_comments = CreateObjectSet<Comment>("Comments")); }
        }

        public ObjectSet<Post> Posts
        {
            get { return _posts ?? (_posts = CreateObjectSet<Post>("Posts")); }
        }

        #endregion
    }
}