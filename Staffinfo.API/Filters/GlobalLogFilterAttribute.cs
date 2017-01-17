using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using log4net;

namespace Staffinfo.API.Filters
{
    public class GlobalLogFilterAttribute: ExceptionFilterAttribute
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override async Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                Log.Error("Ошибка", actionExecutedContext.Exception);
                actionExecutedContext.Response = new HttpResponseMessage()
                {
                    Content =
                        new StringContent($"Произошла ошибка: {GetNativeException(actionExecutedContext.Exception).Message}", Encoding.UTF8,
                            "text/plain"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }, cancellationToken);
        }

        private Exception GetNativeException(Exception ex)
        {
            if (ex.InnerException == null) return ex;
            return GetNativeException(ex.InnerException);
        }
    }
}