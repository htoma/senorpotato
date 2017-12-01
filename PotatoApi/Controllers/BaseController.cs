using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PotatoApi.Controllers
{
    public class BaseController : ApiController
    {
        protected async Task<HttpResponseMessage> Reply(Func<Task<HttpResponseMessage>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                return Error(HttpStatusCode.InternalServerError);
            }
        }

        protected new HttpResponseMessage Ok()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        protected new HttpResponseMessage Ok<T>(T content)
        {
            return Request.CreateResponse(HttpStatusCode.OK, content);
        }

        protected HttpResponseMessage Error(HttpStatusCode code, string message = null)
        {
            return Request.CreateErrorResponse(code, message ?? HttpStatusCode.InternalServerError.ToString());
        }
    }
}