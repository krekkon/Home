using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models;
using CarDealerProject.Models.Business.Exceptions;
using CarDealerProject.Models.Helpers;
using CarDealerProject.Models.Logger;
using CarDealerProject.Models.Nhibernate;
using CarDealerProject.Support;
using NHibernate.Criterion;

namespace CarDealerProject.Controllers
{
    public class CarDealerController : AbstractController<CarDealer>
    {
        //
        // GET: /CarDealer/

        public CarDealerController(INHibernateSession nHibernateSession)
            : base(nHibernateSession)
        {
        }

        //TODO Refact
        public ActionResult GetAllSimilar(CarDealer sampleItem)
        {
            sampleItem.Country = sampleItem.Country ?? "";
            sampleItem.City = sampleItem.City ?? "";

            sampleItem.Name = sampleItem.Name ?? "";
            sampleItem.Street = sampleItem.Street ?? "";

            try
            {
                List<CarDealer> result;
                using (var session = NHibernateSession.OpenSession(Activator.CreateInstance<CarDealer>().GetType().Name))
                {
                    result = session.QueryOver<CarDealer>()
                        .Where(p =>
                                    (sampleItem.Country == "" || sampleItem.Country == p.Country) &&
                                    (sampleItem.City == "" || sampleItem.City == p.City))
                        .Where(Restrictions.On<CarDealer>(p => p.Name).IsLike("%" + sampleItem.Name + "%"))
                        .Where(Restrictions.On<CarDealer>(p => p.Street).IsLike("%" + sampleItem.Street + "%"))

                        .List<CarDealer>().ToList();
                }

                return Request.IsAjaxRequest()
                          ? (ActionResult)PartialView("ListDataContainer", result)
                          : RedirectToAction("All", new { listToView = result });
            }
            catch (FakeImplementationException)
            {
                var result = CreateFakeData.GetCarDealers(10);

                return Request.IsAjaxRequest()
                          ? (ActionResult)PartialView("ListDataContainer", result)
                          : RedirectToAction("All", new { listToView = result });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'Search' procedure. Error Message: ", ex);
                return RedirectToAction("All", new { listToView = new List<CarDealer>() });

            }
        }

        protected override void FillTempData()
        {
            try
            {
                var tempDataHelper = new TempDataHelper(NHibernateSession);
                tempDataHelper.CreateCarDealerProperties();

                TempData["Countries"] = tempDataHelper.Countries;
                TempData["Cities"] = tempDataHelper.Cities;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'FillTempData' procedure. Please refresh the page. Error Message:", ex);
            }

            base.FillTempData();
        }
    }
}
