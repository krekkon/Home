using System.Collections.Generic;
using NHibernate;

namespace CarDealerProject.Models.Nhibernate
{
    class MockNHibertnateSession : INHibertnateSession
    {
        public ISession OpenSession(string modelTypeName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>()
        {
            throw new System.NotImplementedException();
        }

        public void Delete<T>(T carDealer)
        {
            throw new System.NotImplementedException();
        }

        public void Delete<T>(List<T> carDealer)
        {
            throw new System.NotImplementedException();
        }

        public void Update<T>(int id, T carDealer)
        {
            throw new System.NotImplementedException();
        }

        public T Get<T>(int id)
        {
            throw new System.NotImplementedException();
        }

        public void AddItem<T>(T carDealer)
        {
            throw new System.NotImplementedException();
        }
    }
}