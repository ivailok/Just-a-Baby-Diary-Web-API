using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JustABabyDiaryWebAPI.Controllers
{
    public class BaseController : ApiController
    {
        protected T PerformOperationAndHandleExceptions<T>(Func<T> operation)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                throw new HttpResponseException(errResponse);
            }
        }
    }
}