using System;
using System.Linq;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http.Filters;

namespace JinnSports.WEB.Filters
{
    public sealed class ValidateCustomAntiForgeryTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            var headers = actionContext.Request.Headers;
            var cookie = headers
                .GetCookies()
                .Select(c => c[AntiForgeryConfig.CookieName])
                .FirstOrDefault();

            var tokenFromHeader = string.Empty;
            if (headers.Contains("X-XSRF-Token"))
            {
                tokenFromHeader = headers.GetValues("X-XSRF-Token").FirstOrDefault();
            }
            AntiForgery.Validate(cookie != null ? cookie.Value : null, tokenFromHeader);
            //try
            //{
            //    AntiForgery.Validate(cookie != null ? cookie.Value : null, tokenFromHeader);
            //}
            //catch
            //{
                            
            //}

            base.OnActionExecuting(actionContext);
        }
    }
}
