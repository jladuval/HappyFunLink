using System.Web.Mvc;

namespace HappyFunLink.Controllers
{
    using WebCore.Security.Interfaces;

    public class AdminController : Controller
    {
        private readonly MembershipProviderBase _accounts;

        public AdminController(MembershipProviderBase accounts)
        {
            accounts = accounts;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
