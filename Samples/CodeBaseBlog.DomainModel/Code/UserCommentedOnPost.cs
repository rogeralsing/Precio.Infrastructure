using Precio.Domain;

namespace CodeBaseBlog.DomainModel
{
    public class UserCommentedOnPost : IDomainEvent
    {
        public UserInfo User { get; set; }
        public Comment Comment { get; set; }
        public Post Post { get; set; }
    }
}