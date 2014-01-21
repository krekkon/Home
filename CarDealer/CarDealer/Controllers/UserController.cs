using CarDealerProject.Models;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace CarDealerProject.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Login/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (!WebSecurity.Login(user.UserName, user.Password, user.PersistCookie))
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            return RedirectBackOrIndex(); //TODO Return a simple view for display model level error messages
        }

        //Prevent Open Redirection Attacks, for safe
        private ActionResult RedirectBackOrIndex()
        {
            ActionResult action = RedirectToAction("Index", "Home");

            if (Request.UrlReferrer != null)
            {
                var localPath = Request.UrlReferrer.LocalPath;
                if (Url.IsLocalUrl(localPath))
                    action = Redirect(Request.UrlReferrer.LocalPath);   
            }

            return action;
        }

        public ActionResult LogOut()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
