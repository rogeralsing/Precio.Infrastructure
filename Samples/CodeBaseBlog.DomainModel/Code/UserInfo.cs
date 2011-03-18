using Precio.Domain;

namespace CodeBaseBlog.DomainModel
{
    //immutable VO
    public partial class UserInfo : ValueObject<UserInfo>
    {
        internal UserInfo()
        {
        }

        public UserInfo(string name, string website, string email, string userId)
        {
            Name = name;
            WebSite = website;
            Email = email;
            UserId = userId;
        }
    }
}