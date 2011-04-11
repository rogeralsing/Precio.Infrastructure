using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeBaseBlog.Razor.Models;

namespace CodeBaseBlog.Razor.Controllers
{
    public class CategoriesController : Controller
    {
        //
        // GET: /Categories/

        public ActionResult Index()
        {
            return View(new CategoriesViewModel());
        }

    }
}
