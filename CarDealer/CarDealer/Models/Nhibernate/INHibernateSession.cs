using System.Collections.Generic;
using NHibernate;

namespace CarDealerProject.Models.Nhibernate
{
    public interface INHibernateSession
    {
        ISession OpenSession(string modelTypeName);
        IEnumerable<T> GetAll<T>();
        void Delete<T>(T carDealer);
        void Update<T>(int id, T carDealer);
        T Get<T>(int id);
        void AddItem<T>(T carDealer);
        int DeleteAllByIds<T>(string[] entityIds);
    }
}