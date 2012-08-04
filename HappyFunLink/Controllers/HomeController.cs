using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HappyFunLink.Models.Home;

namespace HappyFunLink.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new LinkModel());
        }

        [HttpPost]
        public ActionResult GenerateLink(LinkModel model)
        {
			if (ModelState.IsValid) {
						
			}
			
			return View("Index", new LinkModel());
        }
    }
}
