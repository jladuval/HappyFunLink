using System.Web.Mvc;

namespace HappyFunLink.Controllers
{
    using WebCore.Security.Interfaces;

    public class AdminController : Controller
    {
        private readonly MembershipProviderBase accounts;

        public AdminController(MembershipProviderBase _accounts)
        {
            accounts = _accounts;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
