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

        public ActionResult Route()
        {
            var happylink = Request.Path.Substring(1);
            if (string.IsNullOrEmpty(happylink)) return RedirectToAction("Index");
            var link = _links.GetOriginalLink(happylink);
            if (string.IsNullOrEmpty(link)) return RedirectToAction("Index");
            return Redirect("www.google.com");
        }

        [HttpPost]
        public ActionResult GenerateLink(LinkModel model)
        {
			if (ModelState.IsValid) {
				return View("Index", new LinkModel{ HappyLink = _links.GetHappyLink(model.OriginalLink)});
			}
			return View("Index", model);
        }
    }
}
