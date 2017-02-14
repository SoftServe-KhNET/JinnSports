using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Web.Configuration;

namespace JinnSports.WEB
{
    public partial class Startup
    {        
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(50),
                SlidingExpiration = true                
            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            if (WebConfigurationManager.AppSettings["appId"] != string.Empty && WebConfigurationManager.AppSettings["appSecret"] != string.Empty)
            {
                app.UseFacebookAuthentication(
                       appId: WebConfigurationManager.AppSettings["appId"],
                       appSecret: WebConfigurationManager.AppSettings["appSecret"]);
            }
        }
    }
}