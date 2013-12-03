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

        public IEnumerable<SelectListItem> Colors { get; private set; }
        public IEnumerable<SelectListItem> Brands { get; private set; }
        public IEnumerable<SelectListItem> Models { get; private set; }
        public IEnumerable<SelectListItem> States { get; private set; }
        public IEnumerable<SelectListItem> CarDealers { get { return GetCarDealers(); }}

        public TempDataHelper()
        {
            nHibernateSession = new NHibertnateSession();
        }

        public void CreatePropertiesFromData()
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

        private IEnumerable<SelectListItem> GetCarDealers()
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
    }
}