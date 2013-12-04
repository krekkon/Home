using System.Web.Mvc;

namespace CarDealerProject.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(string searchFilter)
        {
            return View();
        }
    }
}
