using System;
using System.Collections.Generic;
using System.Web.UI;
using CodeBaseBlog.PresentationModel;
using Precio.Domain;

namespace CodeBaseBlog
{
    public partial class ShowPosts : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = Request.UserHostAddress;
            using (UoW.Create())
            {
                IList<PostProjection> posts = GetPosts(userId);
                PostRepeater.DataSource = posts;
                PostRepeater.DataBind();
            }
        }

        private IList<PostProjection> GetPosts(string userId)
        {
            string categoryId = Request["CategoryId"];
            if (categoryId != null)
                return PostProjections.GetByCategory(int.Parse(categoryId), 10, 0, userId);
            else
                return PostProjections.GetLatestPosts(10, 0, userId);
        }
    }
}