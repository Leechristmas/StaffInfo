using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Staffinfo.API.Filters
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private class ErrorInformation
        {
            public string Message { get; set; }
            public DateTime ErrorDate { get; set; }
        }

        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new ResponseMessageResult(context.Request.CreateResponse(
                HttpStatusCode.InternalServerError, new ErrorInformation{ Message = "Error was occured!", ErrorDate = DateTime.Now}));
        }
    }
}