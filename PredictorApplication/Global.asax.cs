using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace PredictorApplication
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoMapperConfiguration.Configure();
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
