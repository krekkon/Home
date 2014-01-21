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
    public class CarController : AbstractController<Car>
    {
        //
        // GET: /Cars/

        public CarController(INHibernateSession nHibernateSession) 
            : base(nHibernateSession)
        {
        }

        public ActionResult GetAllSimilar(Car sampleItem)
        {
            sampleItem.Brand = sampleItem.Brand ?? "";
            sampleItem.Color = sampleItem.Color ?? "";
            sampleItem.State = sampleItem.State ?? "";
            sampleItem.Model = sampleItem.Model ?? "";
            sampleItem.CarNumber = sampleItem.CarNumber ?? "";

            try
            {
                List<Car> result;
                using (var session = NHibernateSession.OpenSession(Activator.CreateInstance<Car>().GetType().Name))
                {
                    result = session.QueryOver<Car>()
                                    .Where(
                                        p =>
                                        (sampleItem.CarDealerId == -1 || p.CarDealerId == sampleItem.CarDealerId) &&
                                        (sampleItem.Brand == "" || sampleItem.Brand == p.Brand) &&
                                        (sampleItem.Color == "" || sampleItem.Color == p.Color) &&
                                        (sampleItem.State == "" || sampleItem.State == p.State) &&
                                        (sampleItem.Model == "" || sampleItem.Model == p.Model))
                                    .Where(
                                        Restrictions.On<Car>(p => p.CarNumber).IsLike("%" + sampleItem.CarNumber + "%"))
                                    .List<Car>().ToList();
                }

                return Request.IsAjaxRequest()
                           ? (ActionResult)PartialView("ListDataContainer", result)
                           : RedirectToAction("All", new { listToView = result });
            }
            catch (FakeImplementationException)
            {
                var result = CreateFakeData.GetCars(10);

                return Request.IsAjaxRequest()
                           ? (ActionResult) PartialView("ListDataContainer", result)
                           : RedirectToAction("All", new {listToView = result});
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'Search' procedure. Error Message:", ex);
                return RedirectToAction("All", new {listToView = new List<Car>()});
            }
        }

        [Authorize(Roles = "Admin, Sale")]
        public override ActionResult Edit(int id)
        {
            FillTempData();
            return base.Edit(id);
        }

        protected override void FillTempData()
        {
            try
            {
                var tempDataHelper = new TempDataHelper(NHibernateSession);
                tempDataHelper.CreateCarProperties();

                TempData["CarDealersData"] = tempDataHelper.CarDealers;
                TempData["Brands"] = tempDataHelper.Brands;
                TempData["Models"] = tempDataHelper.Models;
                TempData["States"] = tempDataHelper.States;
                TempData["Colors"] = tempDataHelper.Colors;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] += Logger.LogError("An error occured during the 'FillTempData' procedure. Please refresh the page. Error Message:", ex);
            }

            base.FillTempData();
        }
    }
}
