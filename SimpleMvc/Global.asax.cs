using SimpleMvc.Controllers;
using SimpleMvc.Interfaces;
using SimpleMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Towers.DependencyInjection;

namespace SimpleMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Example of registering types.
            Dependency.Register<HomeController, HomeController>();
            Dependency.Register<ICalculator, Calculator>();
            Dependency.Register<IEmailService, EmailService>();

            DependencyResolver.SetResolver(new CustomDependencyResolver());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
