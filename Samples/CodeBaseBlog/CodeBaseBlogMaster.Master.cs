using System;
using System.Collections.Generic;
using System.Web.UI;
using CodeBaseBlog.PresentationModel;
using Precio.Domain;

namespace CodeBaseBlog
{
    public partial class CodeBaseBlogMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (UoW.Create())
            {
                IList<FlattenedCategoryProjection> categories = CategoryProjections.GetAll();
                CategoryRepeater.DataSource = categories;
                CategoryRepeater.DataBind();
            }
        }
    }
}