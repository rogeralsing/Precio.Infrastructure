using System;
using System.Web.UI;
using CodeBaseBlog.DomainModel;
using CodeBaseBlog.PresentationModel;
using Precio.Domain;

namespace CodeBaseBlog
{
    public partial class ShowPost : Page
    {
        private int PostId
        {
            get { return int.Parse(Request.QueryString["postId"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ReplyButton_Click(object sender, EventArgs e)
        {
            string userId = Request.UserHostAddress;
            using (UoW.Create())
            {
                Post post = Posts.GetById(PostId);
                string name = NameTextbox.Text;
                string email = EmailTextbox.Text;
                string website = WebsiteTextbox.Text;
                string body = CommentTextbox.Text;

                post.ReplyTo(new UserInfo(name, website, email, userId), body);

                UoW.Commit();
                NameTextbox.Text = "";
                EmailTextbox.Text = "";
                WebsiteTextbox.Text = "";
                CommentTextbox.Text = "";
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string userId = Request.UserHostAddress;
            using (UoW.Create())
            {
                PostProjectionWithComments post = PostProjections.GetById(PostId, userId);
                PublshDateLabel.Text = post.PublishDateString;
                SubjectLabel.Text = post.Subject;
                BodyLabel.Text = post.Body;
                CategoryLabel.Text = post.CategoryString;
                CommentRepeater.DataSource = post.Comments;
                CommentRepeater.DataBind();
            }
        }
    }
}