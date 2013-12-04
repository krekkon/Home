using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models.Logger;
using CarDealerProject.Models.Nhibernate;

namespace CarDealerProject.Controllers
{
    public abstract class AbstractController<T> : Controller where T : class
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
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Create procedure. Error Message:", ex);
            }
            return RedirectToAction("All");
        }

        public ActionResult All()
        {
            FillTempData();
            try
            {
                var data = NHibertnateSession.GetAll<T>().ToList();
                return View("All", data);
            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the 'get all' procedure. Error Message:", ex);
                return View("All", new List<T>());
            }
        }

        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Edit(int id)
        {
            try
            {
                var result = NHibertnateSession.Get<T>(id);

                if (result != null)
                    return View("Edit", result);
                else
                    TempData["ErrorMessage"] = Logger.LogWarning("Unavaliable data.");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Edit procedure. Error Message:", ex);
                return View("All", new List<T>());
            }

            return RedirectToAction("All");
        }


        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Edit(int id, T item)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = Logger.LogWarning("Invalid data.");
                return RedirectToAction("All");
            }

            try
            {
                NHibertnateSession.Update<T>(id, item);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Edit procedure. Error Message:", ex);
            }
            return RedirectToAction("All");
        }

        public ActionResult Details(int id)
        {
            try
            {
                var result = NHibertnateSession.Get<T>(id);

                if (result != null)
                    return View("Details", result);
                else
                    TempData["ErrorMessage"] = Logger.LogWarning("Unavaliable data.");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the 'Get details' procedure. Error Message:", ex);
                return View("All", new List<T>());
            }

            return RedirectToAction("All");
        }

        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = NHibertnateSession.Get<T>(id);

                if (result != null)
                    return View("Delete", result);
                else
                    TempData["ErrorMessage"] = Logger.LogWarning("Unavaliable data.");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the 'Get' procedure. Error Message:", ex);
                return View("All", new List<T>());
            }

            return RedirectToAction("All");
        }

        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Delete(int id, T item)
        {
            try
            {
                NHibertnateSession.Delete<T>(item);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Delete procedure. Error Message:", ex);
            }
            return RedirectToAction("All");
        }

        protected virtual void FillTempData()
        {
            //Do Nothing Here. Fill the TempDada[] variable in the inherited classes if needed.
        }
    }
}