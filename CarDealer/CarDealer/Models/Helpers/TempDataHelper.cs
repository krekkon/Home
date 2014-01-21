using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models.Business.Exceptions;
using CarDealerProject.Models.Nhibernate;

namespace CarDealerProject.Models.Helpers
{
    public class TempDataHelper
    {
        private readonly INHibernateSession nHibernateSession;

        /// <summary>
        /// Car Property
        /// </summary>
        public IEnumerable<SelectListItem> Colors { get; private set; }

        /// <summary>
        /// Car Property
        /// </summary>
        public IEnumerable<SelectListItem> Brands { get; private set; }

        /// <summary>
        /// Car Property
        /// </summary>
        public IEnumerable<SelectListItem> Models { get; private set; }

        /// <summary>
        /// Car Property
        /// </summary>
        public IEnumerable<SelectListItem> States { get; private set; }

        /// <summary>
        /// Car Property
        /// </summary>
        public IEnumerable<SelectListItem> CarDealers { get { return GetCarDealersForCars(); } }

        /// <summary>
        /// Car Dealer Property
        /// </summary>
        public IEnumerable<SelectListItem> Countries { get; private set; }

        /// <summary>
        /// Car Dealer Property
        /// </summary>
        public IEnumerable<SelectListItem> Cities { get; private set; }

        public TempDataHelper(INHibernateSession nHibernateSession)
        {
            this.nHibernateSession = nHibernateSession;
        }

        /// <summary>
        /// Car Method
        /// </summary>
        public void CreateCarProperties()
        {
            try
            {
                using (var session = nHibernateSession.OpenSession(Activator.CreateInstance<Car>().GetType().Name))
                {

                    var result = session.QueryOver<Car>()
                                        .Select(c => c.Brand, c => c.Model, c => c.State, c => c.Color)
                                        .List<object[]>()
                                        .Select(properties => new
                                            {
                                                Brand = (string)properties[0],
                                                Model = (string)properties[1],
                                                State = (string)properties[2],
                                                Color = (string)properties[3]
                                            });

                    var enumerable = result.ToList();
                    enumerable.Insert(0, new { Brand = "", Model = "", State = "", Color = "" });

                    Brands = new SelectList(enumerable.Select(item => item.Brand).Distinct());
                    Models = new SelectList(enumerable.Select(item => item.Model).Distinct());
                    States = new SelectList(enumerable.Select(item => item.State).Distinct());
                    Colors = new SelectList(enumerable.Select(item => item.Color).Distinct());
                }
            }
            catch (FakeImplementationException)
            {
                var mockEmumerable = new[]
                { 
                    new { Brand = "", Model = "", State = "", Color = "" },
                    new {Brand = "FBran_1", Model = "FMode_1", State = "FStat_1", Color = "FCol_1"},
                    new {Brand = "FBran_2", Model = "FMode_2", State = "FStat_2", Color = "FCol_2"},
                    new {Brand = "FBran_3", Model = "FMode_3", State = "FStat_3", Color = "FCol_3"}
                }.ToList();


                Brands = new SelectList(mockEmumerable.Select(item => item.Brand).Distinct());
                Models = new SelectList(mockEmumerable.Select(item => item.Model).Distinct());
                States = new SelectList(mockEmumerable.Select(item => item.State).Distinct());
                Colors = new SelectList(mockEmumerable.Select(item => item.Color).Distinct());
            }
        }

        /// <summary>
        /// Car Method
        /// </summary>
        private IEnumerable<SelectListItem> GetCarDealersForCars()
        {
            try
            {
                using (var session = nHibernateSession.OpenSession(Activator.CreateInstance<CarDealer>().GetType().Name))
                {
                    var result = session.QueryOver<CarDealer>()
                                        .Select(c => c.Id, c => c.Name)
                                        .List<object[]>()
                                        .Select(properties => new
                                            {
                                                Id = (int)properties[0],
                                                Name = (string)properties[1],
                                            });
                    return new SelectList(result.ToList(), "Id", "Name");
                }
            }
            catch (FakeImplementationException)
            {
                var mockEmumerable = new[]
                { 
                    new {Id = "FId_1", Name = "FName_1"},
                    new {Id = "FId_2", Name = "FName_2"},
                    new {Id = "FId_3", Name = "FName_3"}
                }.ToList();

                return new SelectList(mockEmumerable, "Id", "Name");
            }
        }

        /// <summary>
        /// Car Dealer Method
        /// </summary>
        public void CreateCarDealerProperties()
        {
            try
            {
                using (var session = nHibernateSession.OpenSession(Activator.CreateInstance<CarDealer>().GetType().Name))
                {

                    var result = session.QueryOver<CarDealer>()
                                        .Select(c => c.Country, c => c.City)
                                        .List<object[]>()
                                        .Select(properties => new
                                        {
                                            Country = (string)properties[0],
                                            City = (string)properties[1],
                                        });

                    var enumerable = result.ToList();
                    enumerable.Insert(0, new { Country = "", City = "" });

                    Countries = new SelectList(enumerable.Select(item => item.Country).Distinct());
                    Cities = new SelectList(enumerable.Select(item => item.City).Distinct());
                }
            }
            catch (FakeImplementationException)
            {
                var mockEmumerable = new[]
                { 
                    new { Country = "", City = "" },
                    new {Country = "FCtry_1", City = "FCity_1"},
                    new {Country = "FCtry_2", City = "FCity_2"},
                    new {Country = "FCtry_3", City = "FCity_3"}
                }.ToList();

                Countries = new SelectList(mockEmumerable.Select(item => item.City).Distinct());
                Cities = new SelectList(mockEmumerable.Select(item => item.Country).Distinct());
            }
        }
    }
}