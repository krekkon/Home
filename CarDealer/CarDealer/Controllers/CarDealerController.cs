using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models;
using CarDealerProject.Models.Logger;
using CarDealerProject.Models.Nhibernate;
using NHibernate.Criterion;

namespace CarDealerProject.Controllers
{
    public class CarDealerController : Controller
    {
        //
        // GET: /CarDealer/

        private readonly INHibertnateSession nHibertnateSession;

        public CarDealerController()
        {
            nHibertnateSession = new NHibertnateSession();
            //return View("GetCarDealers", CreateMockData.GetCarDealers(30));
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Create(CarDealer carDealer)
        {
            try
            {
                nHibertnateSession.AddItem(carDealer);
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
            var data = nHibertnateSession.GetAll<CarDealer>().ToList();
            return View("All", data);
        }

        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Edit(int id)
        {
            var data = nHibertnateSession.Get<CarDealer>(id);
            return View("Edit", data);
        }

        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Edit(int id, CarDealer carDealer)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Edit");

            try
            {
                nHibertnateSession.Update<CarDealer>(id, carDealer);
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
            //Todo null-e?
            var result = nHibertnateSession.Get<CarDealer>(id);

            if (result == null)
                return RedirectToAction("All"); //TODO not found
            else
                return View("Details", nHibertnateSession.Get<CarDealer>(id));
        }

        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Delete(int id)
        {
            return View("Delete", nHibertnateSession.Get<CarDealer>(id));
        }

        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Delete(int id, CarDealer carDealer)
        {
            try
            {
                nHibertnateSession.Delete<CarDealer>(carDealer);
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Delete procedure. Error Message:", ex);
                return View("Index");
            }
        }

        public ActionResult GetAllSimilar(CarDealer sampleItem)
        {
            List<CarDealer> result;
            //.Where(Restrictions.On<Person>(p => p.Name).IsLike("Smith%"))
            //.WhereRestrictionOn(c => c.Name).IsLike("%anna%")
            //.WhereRestrictionOn(p => Restrictions.On<Person>(r => r.Email).IsLike("%" + sampleItem.Email + "%")))
            using (var session = nHibertnateSession.OpenSession(Activator.CreateInstance<CarDealer>().GetType().Name))
            {
                result = session.QueryOver<CarDealer>()
                    .Where(p => p.ParkingPlaces >= sampleItem.ParkingPlaces)
                    .Where(Restrictions.On<CarDealer>(p => p.Name).IsLike("%" + sampleItem.Name + "%"))
                    .List<CarDealer>().ToList();
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


    }
}
