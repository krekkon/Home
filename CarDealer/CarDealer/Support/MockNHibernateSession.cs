using System;
using System.Collections.Generic;
using CarDealerProject.Models.Business.Exceptions;
using CarDealerProject.Models.Nhibernate;
using NHibernate;

namespace CarDealerProject.Support
{
    class MockNHibernateSession : INHibernateSession
    {
        public ISession OpenSession(string modelTypeName)
        {
            throw new FakeImplementationException();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return new List<T> { Get<T>(0), Get<T>(1), Get<T>(2), Get<T>(3) };
        }

        public void Delete<T>(T carDealer)
        {
            //of course I delete it, or course. just a minute
        }

        public void Delete<T>(List<T> carDealer)
        {
            //of course I delete it, or course. just a minute
        }

        public void Update<T>(int id, T carDealer)
        {
            //of course I update it, or course. just a minute
        }

        public T Get<T>(int id)
        {
            var fakeItem = Activator.CreateInstance<T>();

            foreach (var prop in (typeof(T)).GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(fakeItem, "Fake" + id);
            }

            return fakeItem;
        }

        public void AddItem<T>(T carDealer)
        {
            //TOD NOTIMPLEMENTED EXPECITON?
            //of course I add it, or course. just a minute
        }

        public int DeleteAllByIds<T>(string[] entityIds)
        {
            return entityIds.Length;
        }
    }
}