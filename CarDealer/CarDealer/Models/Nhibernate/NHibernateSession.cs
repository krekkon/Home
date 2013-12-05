using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;

namespace CarDealerProject.Models.Nhibernate
{
    public class NHibernateSession : INHibernateSession
    {
        public ISession OpenSession(string modelTypeName)
        {
            if (!File.Exists(HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\" + modelTypeName + ".hbm.xml")))
                throw new ArgumentException(
                    "There is no mapping file (" + modelTypeName + ".hbm.xml) for the " + modelTypeName +
                    " class in the ~\\Models\\NHibernate folder.", modelTypeName);

            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\hibernate.cfg.xml");
            configuration.Configure(configurationPath);

            var configurationFile = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\" + modelTypeName + ".hbm.xml");
            configuration.AddFile(configurationFile);
            var sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }

        public void AddItem<T>(T item)
        {
            using (var session = OpenSession(item.GetType().Name))
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(item);
                    transaction.Commit();
                }
            }
        }

        public T Get<T>(int id)
        {
            using (var session = OpenSession(Activator.CreateInstance<T>().GetType().Name))
            {
                return session.Get<T>(id);
            }
        }

        public void Update<T>(int id, T modifiedItem)
        {
            using (var session = OpenSession(modifiedItem.GetType().Name))
            {
                var itemtoUpdate = session.Get<T>(id);

                foreach (var prop in (typeof(T)).GetProperties())
                    prop.SetValue(itemtoUpdate,prop.GetValue(modifiedItem));

                using (var transaction = session.BeginTransaction())
                {
                    session.Save(itemtoUpdate);
                    transaction.Commit();
                }
            }
        }

        public void Delete<T>(T item)
        {
            using (var session = OpenSession(item.GetType().Name))
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(item);
                    transaction.Commit();
                }
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            using (var session = OpenSession(Activator.CreateInstance<T>().GetType().Name))
            {
                return session.Query<T>().ToList();
            }
        }
    }
}