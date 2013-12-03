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
            //if (!ModelState.IsValid)
            //    return RedirectToAction("Edit");

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
            List<Car> result;
            //.Where(Restrictions.On<Person>(p => p.Name).IsLike("Smith%"))
            //.WhereRestrictionOn(c => c.Name).IsLike("%anna%")
            //.WhereRestrictionOn(p => Restrictions.On<Person>(r => r.Email).IsLike("%" + sampleItem.Email + "%")))
            using (var session = nHibertnateSession.OpenSession(Activator.CreateInstance<Car>().GetType().Name))
            {
                result = session.QueryOver<Car>()
                    .Where(p =>(sampleItem.ManufactureDate == DateTime.MinValue || p.ManufactureDate >= sampleItem.ManufactureDate) &&
                               (sampleItem.Year == 0 || p.Year >= sampleItem.Year))
                    .Where(Restrictions.On<Car>(p => p.Brand).IsLike("%" + sampleItem.Brand + "%") &&
                           Restrictions.On<Car>(p => p.Color).IsLike("%" + sampleItem.Color + "%") &&
                           Restrictions.On<Car>(p => p.Model).IsLike("%" + sampleItem.Model + "%"))
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

        
    }
}
