using System;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models;
using CarDealerProject.Models.Logger;
using CarDealerProject.Models.Nhibernate;

namespace CarDealerProject.Controllers
{
    public class PersonController : Controller
    {
        //
        // GET: /Persons/

        private readonly INHibertnateSession nHibertnateSession;

        public PersonController()
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
        public ActionResult Create(Person person)
        {
            try
            {
                nHibertnateSession.AddItem(person);

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
            var data = nHibertnateSession.GetAll<Person>().ToList();
            return View("All", data);
        }
        
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Edit(int id)
        {
            var data = nHibertnateSession.Get<Person>(id);
            return View("Edit", data);
        }


        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Edit(int id, Person person)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Edit");

            try
            {
                nHibertnateSession.Update<Person>(id, person);

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
            return View("Details", nHibertnateSession.Get<Person>(id));
        }

        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Delete(int id)
        {
            return View("Delete", nHibertnateSession.Get<Person>(id));
        }

        [HttpPost]
        //[Authorize(Users = "krekkon, John", Roles = "Officers, Admins")]
        public ActionResult Delete(int id, Person person)
        {
            try
            {
                nHibertnateSession.Delete<Person>(person);
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Logger.LogError("An error occured during the Delete procedure. Error Message:", ex);
                return View("Index");
            }
        }

        
    }
}
