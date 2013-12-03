using System.Web.Mvc;

namespace CarDealerProject.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(string searchFilter)
        {
            //using (ISession session = NHibertnateSession.OpenSession())
            //{
            //    var employees = session.Query<Employee>().ToList();
            //    return View(employees);
            //}
            if (searchFilter == null)
                return View();
            else
                return RedirectToAction("GetCars", "Cars");
        }
    }
}
