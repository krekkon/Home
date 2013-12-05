using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models;
using CarDealerProject.Models.Helpers;
using NHibernate.Criterion;

namespace CarDealerProject.Controllers
{
    public class CarController : AbstractController<Car>
    {
        //
        // GET: /Cars/

        public ActionResult GetAllSimilar(Car sampleItem)
        {
            sampleItem.Brand = sampleItem.Brand ?? "";
            sampleItem.Color = sampleItem.Color ?? "";
            sampleItem.State = sampleItem.State ?? "";
            sampleItem.Model = sampleItem.Model ?? "";
            sampleItem.CarNumber = sampleItem.CarNumber ?? "";

            List<Car> result;
            using (var session = NHibertnateSession.OpenSession(Activator.CreateInstance<Car>().GetType().Name))
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

        public override ActionResult Edit(int id)
        {
            FillTempData();
            return base.Edit(id);
        } 

        protected override void FillTempData()
        {
            var tempDataHelper = new TempDataHelper();
            tempDataHelper.CreateCarProperties();

            TempData["CarDealersData"] = tempDataHelper.CarDealers;
            TempData["Brands"] = tempDataHelper.Brands;
            TempData["Models"] = tempDataHelper.Models;
            TempData["States"] = tempDataHelper.States;
            TempData["Colors"] = tempDataHelper.Colors;

            base.FillTempData();
        }
    }
}
