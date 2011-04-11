using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeBaseBlog.PresentationModel;

namespace CodeBaseBlog.Razor.Models
{
    public class CategoriesViewModel
    {
        public CategoriesViewModel()
        {
            Categories = new List<CategoryProjection>();
        }

        public IEnumerable<CategoryProjection> Categories { get;private set; }        
    }
}