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
        protected readonly INHibernateSession NHibernateSession;

        protected AbstractController(INHibernateSession nHibernateSession)
        {
            NHibernateSession = nHibernateSession;
        }

        public ActionResult Index()
        {
            return RedirectToAction("All");
        }

        public ActionResult All()
        {
            try
            {
                FillTempData();
                var data = NHibernateSession.GetAll<T>().ToList();
                return View("All", data);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'Get all' procedure. Error Message:", ex);
                return View("All", new List<T>());
            }
        }

        [HttpPost]
        public ActionResult Create(T item)
        {
            var validationResult = ModelStateValidation();
            if (validationResult != null)
                return validationResult;

            try
            {
                NHibernateSession.AddItem(item);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the Create procedure. Error Message: ", ex);
            }
            return RedirectToAction("All");
        }

        public virtual ActionResult Edit(int id)
        {
            try
            {
                var result = NHibernateSession.Get<T>(id);

                if (result == null)
                    TempData["ErrorMessage"] += Logger.LogWarning("Unavaliable data.");
                else
                    return View("Edit", result);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'Edit' procedure. Error Message: ", ex);
            }

            return RedirectToAction("All");
        }


        [HttpPost]
        public ActionResult Edit(int id, T item)
        {
            var validationResult = ModelStateValidation();
            if (validationResult != null)
                return validationResult;

            try
            {
                NHibernateSession.Update<T>(id, item);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the Edit procedure. Error Message:", ex);
            }
            return RedirectToAction("All");
        }

        public ActionResult Details(int id)
        {
            try
            {
                var result = NHibernateSession.Get<T>(id);

                if (result == null)
                    TempData["ErrorMessage"] += Logger.LogWarning("Unavaliable data.");
                else
                    return View("Details", result);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'Get details' procedure. Error Message:", ex);
            }

            return RedirectToAction("All");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var result = NHibernateSession.Get<T>(id);

                if (result == null)
                    TempData["ErrorMessage"] += Logger.LogWarning("Unavaliable data.");
                else
                    return View("Delete", result);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'Get' procedure. Error Message:", ex);
            }

            return RedirectToAction("All");
        }

        [HttpPost]
        public ActionResult Delete(int id, T item)
        {
            try
            {
                NHibernateSession.Delete<T>(item);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the Delete procedure. Error Message:", ex);
            }
            return RedirectToAction("All");
        }

        protected virtual void FillTempData()
        {
            //Do Nothing Here. Fill the TempDada[] variable in the inherited classes if needed.
        }

        private ActionResult ModelStateValidation()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] += Logger.LogWarning("Invalid data.");

                foreach (var modelStateError in ModelState.Keys.SelectMany(key => ModelState[key].Errors))
                    TempData["ErrorMessage"] += Logger.LogWarning(modelStateError.ErrorMessage);

                return RedirectToAction("All");
            }
            return null;
        }
    }
}