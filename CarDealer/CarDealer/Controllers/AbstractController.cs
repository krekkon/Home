using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models.Logger;
using CarDealerProject.Models.Nhibernate;

namespace CarDealerProject.Controllers
{
    [Authorize]
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

        protected override ViewResult View(IView view, object model)
        {
            if (view.GetType().Name == "All")
                FillTempData();

            return base.View(view, model);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            if (viewName == "All")
                FillTempData();

            return base.View(viewName, masterName, model);
        }

        [HttpGet]
        public ActionResult All()
        {
            try
            {
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
        [Authorize(Roles = "Admin, Sale")]
        [ValidateAntiForgeryToken]
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

        [Authorize(Roles = "Admin, Sale")]
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
        [Authorize(Roles = "Admin, Sale")]
        [ValidateAntiForgeryToken]
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

        [Authorize(Roles = "Admin, Sale")]
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
        [Authorize(Roles = "Admin, Sale")]
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

        [HttpPost]
        [Authorize(Roles = "Admin, Sale")]
        //[ValidateAntiForgeryToken] //Todo add aax antiforgey token
        public ActionResult DeleteAllByIds(string idsForDelete)
        {
            if (string.IsNullOrWhiteSpace(idsForDelete))
            {
                TempData["ErrorMessage"] += Logger.LogWarning("Please select at least one row to delete");
                return RedirectToAction("All");
            }

            try
            {
                var deletedRows = NHibernateSession.DeleteAllByIds<T>(idsForDelete.Split(','));
                if (deletedRows > 0)
                {
                    var message = (deletedRows > 1) ? "The selected rows (" + deletedRows + ") have been deleted."
                                                    : "The selected row has been deleted.";

                    TempData["Message"] += message;
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'Delete selected' procedure. Please refresh the page. Error Message: ", ex);
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