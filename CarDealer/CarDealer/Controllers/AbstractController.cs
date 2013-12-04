using System;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models.Logger;
using CarDealerProject.Models.Nhibernate;

namespace CarDealerProject.Controllers
{
    public abstract class AbstractController<T> : Controller where T:class 
    {
        protected readonly INHibertnateSession NHibertnateSession;

        protected AbstractController()
        {
            NHibertnateSession = new NHibertnateSession();
        }

        public ActionResult Index()
        {
            return RedirectToAction("All");
        }

        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Create(T item)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = Logger.LogWarning("Invalid data.");
                return RedirectToAction("All");
            }

            try
            {
                NHibertnateSession.AddItem(item);
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Create procedure. Error Message:", ex);
                return RedirectToAction("All");
            }
        }

        public ActionResult All()
        {
            FillTempData();

            var data = NHibertnateSession.GetAll<T>().ToList();
            return View("All", data);
        }

        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Edit(int id)
        {
            var data = NHibertnateSession.Get<T>(id);
            return View("Edit", data);
        }


        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Edit(int id, T item)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = Logger.LogWarning("Invalid data.");
                return RedirectToAction("All");//TODO not found 
            }

            try
            {
                //TODO NULL? Other usages check too
                NHibertnateSession.Update<T>(id, item);
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Edit procedure. Error Message:", ex);
                return RedirectToAction("All");
            }
        }

        public ActionResult Details(int id)
        {
            return View("Details", NHibertnateSession.Get<T>(id));
        }

        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Delete(int id)
        {
            return View("Delete", NHibertnateSession.Get<T>(id));
        }

        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Delete(int id, T item)
        {
            try
            {
                NHibertnateSession.Delete<T>(item);
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Delete procedure. Error Message:", ex);
                return RedirectToAction("All");
            }
        }

        protected virtual void FillTempData()
        {
            //Do Nothing Here. Fill the TempDada[] variable in the inherited classes if needed.
        }
    }
}