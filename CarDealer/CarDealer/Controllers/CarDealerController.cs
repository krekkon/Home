using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models;
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

        #region Comment
        //TODO LATER, post is arrived with the ids of cheked boxes
        //public ActionResult DeleteAll(string[] ids)
        //{
        //    NHibernateSession.Delete();
        //    return RedirectToAction("All");
        //} 
        #endregion

        public CarDealerController(INHibernateSession nHibernateSession) 
            : base(nHibernateSession)
        {
        }

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

                if (Request.IsAjaxRequest())
                {
                    return PartialView("ListDataContainer", result);
                }
                else
                {
                    return View("All", result);
                }
            }
            catch (NotImplementedException)
            {
                var result = CreateFakeData.GetCarDealers(10);

                return Request.IsAjaxRequest()
                       ? (ActionResult)PartialView("ListDataContainer", result)
                       : View("All", result);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'Search' procedure. Error Message: ", ex);
                return View("All", new List<CarDealer>());
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
                //Fill empty values for avoid view errors
                var emptyEnumbeable = new[] { new { Country = "", City = "" } }.ToList();
                TempData["Countries"] = new SelectList(emptyEnumbeable.Select(item => item.City).Distinct());
                TempData["Cities"] = new SelectList(emptyEnumbeable.Select(item => item.Country).Distinct());

                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'FillTempData' procedure. Please refresh the page. Error Message:", ex);
            }

            base.FillTempData();
        }
    }
}
