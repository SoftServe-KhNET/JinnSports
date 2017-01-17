using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;

namespace JinnSports.WEB
{
    public class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            // Register Web Api controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Regiser MVC controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Configuring from BL
            BLL.Utilities.AutofacConfig.Configure(ref builder);

            var container = builder.Build();

            // Setting the dependency resolver for Web Api
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;

            // Setting the dependency resolver for MVC
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);
        }
    }
}