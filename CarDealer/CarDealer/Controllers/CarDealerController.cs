using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models;
using CarDealerProject.Models.Helpers;
using NHibernate.Criterion;

namespace CarDealerProject.Controllers
{
    public class CarDealerController : AbstractController<CarDealer>
    {
        //
        // GET: /CarDealer/

        //TODO LATER, post is arrived with the ids of cheked boxes
        //public ActionResult DeleteAll(string[] ids)
        //{
        //    NHibertnateSession.Delete();
        //    return RedirectToAction("All");
        //}

        public ActionResult GetAllSimilar(CarDealer sampleItem)
        {
            
            sampleItem.Country = sampleItem.Country ?? "";
            sampleItem.City = sampleItem.City ?? "";

            sampleItem.Name = sampleItem.Name ?? "";
            sampleItem.Street = sampleItem.Street ?? "";

            List<CarDealer> result;
            using (var session = NHibertnateSession.OpenSession(Activator.CreateInstance<CarDealer>().GetType().Name))
            {
                result = session.QueryOver<CarDealer>()
                    .Where(p =>
                                (sampleItem.Country == "" || sampleItem.Country == p.Country) &&
                                (sampleItem.City == "" || sampleItem.City == p.City))
                    .Where(Restrictions.On<CarDealer>(p => p.Name).IsLike("%" + sampleItem.Name + "%"))
                    .Where(Restrictions.On<CarDealer>(p => p.Street).IsLike("%" + sampleItem.Street + "%"))

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

        protected override void FillTempData()
        {
            var tempDataHelper = new TempDataHelper();
            tempDataHelper.CreateCarDealerProperties();

            TempData["Countries"] = tempDataHelper.Countries;
            TempData["Cities"] = tempDataHelper.Cities;

            base.FillTempData();
        }


    }
}
