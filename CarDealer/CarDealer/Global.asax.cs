using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using CarDealerProject.App_Start;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Unity.Mvc4;
using WebMatrix.WebData;

namespace CarDealerProject
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new UnityContainer().LoadConfiguration();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("dbContext", "webpages_User", "Id", "UserName", true);
        }
    }
}