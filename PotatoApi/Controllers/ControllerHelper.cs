using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PotatoApi.Controllers
{
    public static class ControllerHelper
    {
        public static HttpResponseMessage Response<T>(this ApiController controller, Func<T> func, T failResult)
        {
            try
            {
                return controller.Request.CreateResponse(HttpStatusCode.OK, func());
            }
            catch (Exception ex)
            {
                //note(htoma): log exception
                return controller.Request.CreateResponse(HttpStatusCode.InternalServerError, failResult);
            }
        }

        public static HttpResponseMessage Response(this ApiController controller, Func<bool> func)
        {
            try
            {
                var res = func();
                return controller.Request.CreateResponse(res ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                //note(htoma): log exception
                return controller.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        public static HttpResponseMessage Response(this ApiController controller, Action func)
        {
            try
            {
                return controller.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //note(htoma): log exception
                return controller.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}