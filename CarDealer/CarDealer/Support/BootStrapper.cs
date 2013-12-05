using System.Web.Mvc;
using CarDealerProject.Models.Nhibernate;
using Microsoft.Practices.Unity;
using Unity.Mvc4;

namespace CarDealerProject.Support
{
    #region Unused but maybe later needed
    //public static class Bootstrapper
    //{
    //    public static IUnityContainer Initialise()
    //    {
    //        var container = BuildUnityContainer();

    //        DependencyResolver.SetResolver(new UnityDependencyResolver(container));

    //        return container;
    //    }

    //    private static IUnityContainer BuildUnityContainer()
    //    {
    //        var container = new UnityContainer();

    //        container.RegisterType<INHibernateSession, MockNHibernateSession>();

    //        RegisterTypes(container);

    //        return container;
    //    }

    //    public static void RegisterTypes(IUnityContainer container)
    //    {

    //    }
    //} 
    #endregion
}
