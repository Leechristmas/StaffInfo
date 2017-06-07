using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using NLog;

namespace Staffinfo.API.Filters
{
    /// <summary>
    /// Exceptions filter
    /// </summary>
    public class GlobalLogFilterAttribute: ExceptionFilterAttribute
    {
        private static readonly ILogger _logger =
            LogManager.GetLogger("ExceptionFilterLogger");

        /// <summary>
        /// Process a raised exception
        /// </summary>
        /// <param name="actionExecutedContext">exception</param>
        /// <param name="cancellationToken">token to cancel the process</param>
        /// <returns></returns>
        public override async Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                var ex = GetNativeException(actionExecutedContext.Exception);
                var exType = ex.GetType();

                string message = String.Empty;

                //typifying of exceptions
                if (exType == typeof (SqlException))
                    message = "Ошибка базы данных: ";

                message += ex.Message;  //for debug

                _logger.Log(LogLevel.Error, ex, $"Произошло исключение типа {exType.Name}. Подробнее: {ex.Message}");
                actionExecutedContext.Response = new HttpResponseMessage()
                {
                    Content =
                        new StringContent($"Произошла ошибка! {message}", Encoding.UTF8,
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