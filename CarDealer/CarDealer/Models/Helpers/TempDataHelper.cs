using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarDealerProject.Models.Nhibernate;

namespace CarDealerProject.Models.Helpers
{
    public class TempDataHelper
    {
        private readonly INHibertnateSession nHibernateSession;

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
        public IEnumerable<SelectListItem> CarDealers { get { return GetCarDealersForCars(); }}

        /// <summary>
        /// Car Dealer Property
        /// </summary>
        public IEnumerable<SelectListItem> Countries { get; private set; }

        /// <summary>
        /// Car Dealer Property
        /// </summary>
        public IEnumerable<SelectListItem> Cities { get; private set; }

        public TempDataHelper()
        {
            nHibernateSession = new NHibertnateSession();
        }

        /// <summary>
        /// Car Method
        /// </summary>
        public void CreateCarProperties()
        {
            using (var session = nHibernateSession.OpenSession(Activator.CreateInstance<Car>().GetType().Name))
            {

                var result = session.QueryOver<Car>()
                                    .Select(c => c.Brand, c => c.Model, c => c.State, c => c.Color)
                                    .List<object[]>()
                                    .Select(properties => new
                                        {
                                            Brand = (string) properties[0],
                                            Model = (string) properties[1],
                                            State = (string) properties[2],
                                            Color = (string) properties[3]
                                        });

                var enumerable = result.ToList();
                enumerable.Insert(0, new { Brand = "", Model = "", State = "", Color = "" });

                Brands = new SelectList(enumerable.Select(item => item.Brand).Distinct());
                Models = new SelectList(enumerable.Select(item => item.Model).Distinct());
                States = new SelectList(enumerable.Select(item => item.State).Distinct());
                Colors = new SelectList(enumerable.Select(item => item.Color).Distinct());
            }
        }

        /// <summary>
        /// Car Method
        /// </summary>
        private IEnumerable<SelectListItem> GetCarDealersForCars()
        {
            using (var session = nHibernateSession.OpenSession(Activator.CreateInstance<CarDealer>().GetType().Name))
            {
                var result = session.QueryOver<CarDealer>()
                                    .Select(c => c.Id, c => c.Name)
                                    .List<object[]>()
                                    .Select(properties => new
                                        {
                                            Id = (int) properties[0],
                                            Name = (string) properties[1],
                                        });
                return new SelectList(result.ToList(), "Id", "Name");
            }
        }

        /// <summary>
        /// Car Dealer Method
        /// </summary>
        public void CreateCarDealerProperties()
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
    }
}