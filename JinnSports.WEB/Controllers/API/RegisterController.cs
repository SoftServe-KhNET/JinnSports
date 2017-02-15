using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JinnSports.WEB.Controllers
{
    public class RegisterController : ApiController
    {
        public HttpResponseMessage Post(Api.Credentials credentials)
        {
            if (credentials.Login == "mylogin")
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

        }
    }
}

public class Credentials
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}