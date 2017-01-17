using System;
using System.Web.Http.Filters;

namespace Staffinfo.API.Filters
{
    public class NotEmplExceptionFilterAttribute: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            throw new NotImplementedException("This method has not yet been implemented!");
        }
    }
}