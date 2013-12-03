using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models;
using CarDealerProject.Models.Helpers;
using CarDealerProject.Models.Logger;
using CarDealerProject.Models.Nhibernate;
using NHibernate.Criterion;

namespace CarDealerProject.Controllers
{
    public class CarController : Controller
    {
        //
        // GET: /Cars/

        private readonly INHibertnateSession nHibertnateSession;

        public CarController()
        {
            nHibertnateSession = new NHibertnateSession();
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Create(Car car)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = Logger.LogWarning("Invalid data.");
                return View("Index");
            }

            try
            {
                nHibertnateSession.AddItem(car);
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Create procedure. Error Message:", ex);
                return View("Index");
            }
        }

        public ActionResult All()
        {
            FillTempData();

            var data = nHibertnateSession.GetAll<Car>().ToList();
            return View("All", data);
        }



        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Edit(int id)
        {
            var data = nHibertnateSession.Get<Car>(id);
            return View("Edit", data);
        }


        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Edit(int id, Car car)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = Logger.LogWarning("Invalid data.");
                return View("Index");
            }

            try
            {
                nHibertnateSession.Update<Car>(id, car);

                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Edit procedure. Error Message:", ex);
                return View("Index");
            }
        }

        public ActionResult Details(int id)
        {
            return View("Details", nHibertnateSession.Get<Car>(id));
        }

        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Delete(int id)
        {
            return View("Delete", nHibertnateSession.Get<Car>(id));
        }

        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Delete(int id, Car car)
        {
            try
            {
                nHibertnateSession.Delete<Car>(car);
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Delete procedure. Error Message:", ex);
                return View("Index");
            }
        }

        public ActionResult GetAllSimilar(Car sampleItem)
        {
            sampleItem.Brand = sampleItem.Brand ?? "";
            sampleItem.Color = sampleItem.Color ?? "";
            sampleItem.State = sampleItem.State ?? "";
            sampleItem.Model = sampleItem.Model ?? "";
            sampleItem.CarNumber = sampleItem.CarNumber ?? "";

            List<Car> result;
            using (var session = nHibertnateSession.OpenSession(Activator.CreateInstance<Car>().GetType().Name))
            {
                result = session.QueryOver<Car>()
                    .Where(p => (sampleItem.CarDealerId == -1 || p.CarDealerId == sampleItem.CarDealerId) &&
                                (sampleItem.Brand == "" || sampleItem.Brand == p.Brand) &&
                                (sampleItem.Color == "" || sampleItem.Color == p.Color) &&
                                (sampleItem.State == "" || sampleItem.State == p.State) &&
                                (sampleItem.Model == "" || sampleItem.Model == p.Model))
                    .Where(Restrictions.On<Car>(p => p.CarNumber).IsLike("%" + sampleItem.CarNumber + "%"))
                    .List<Car>().ToList();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("ListDataContainer", result);
            }
            else
            {
                return View("All", result);
            }
        }

        private void FillTempData()
        {
            var tempDataHelper = new TempDataHelper();
            tempDataHelper.CreatePropertiesFromData();

            TempData["CarDealersData"] = tempDataHelper.CarDealers;
            TempData["Brands"] = tempDataHelper.Brands;
            TempData["Models"] = tempDataHelper.Models;
            TempData["States"] = tempDataHelper.States;
            TempData["Colors"] = tempDataHelper.Colors;
        }
    }
}
