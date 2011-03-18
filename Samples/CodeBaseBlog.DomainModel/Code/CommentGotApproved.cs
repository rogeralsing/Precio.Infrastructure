using Precio.Domain;

namespace CodeBaseBlog.DomainModel
{
    public class CommentGotApproved : IDomainEvent
    {
        public Comment Comment { get; set; }
    }
}