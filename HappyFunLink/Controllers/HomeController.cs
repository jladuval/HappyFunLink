using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HappyFunLink.Models.Home;

namespace HappyFunLink.Controllers
{
    using Domain.Services.Interfaces;

    public class HomeController : Controller
    {
        private readonly ILinkService _links;

        public HomeController(ILinkService links)
        {
            _links = links;
        }

        public ActionResult Index()
        {
            return View(new LinkModel());
        }

        [HttpPost]
        public ActionResult GenerateLink(LinkModel model)
        {
			if (ModelState.IsValid) {
				return View("Index", new LinkModel{ HappyLink = _links.GetHappyLink(model.OriginalLink)});
			}
			return View("Index");
        }
    }
}
