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

        public ActionResult Index(LinkModel model)
        {
            if(model == null) model = new LinkModel();
            ModelState.Clear();
            return View("Index", model);
        }

        public ActionResult Route()
        {
            var happyLink = Request.Path.Substring(1);
            if (string.IsNullOrEmpty(happyLink)) return View("Index", new LinkModel());
            var link = _links.GetOriginalLink(happyLink);
            if (string.IsNullOrEmpty(link)) return View("Index", new LinkModel());
            return Redirect(link);
        }

        [HttpPost]
        public ActionResult GenerateLink(LinkModel model)
        {
			if (ModelState.IsValid) {
				return RedirectToAction("Index", new LinkModel{ HappyLink = _links.GetHappyLink(model.OriginalLink)});
			}
			return View("Index", model);
        }
    }
}
