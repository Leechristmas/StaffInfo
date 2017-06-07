using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;
using Staffinfo.Reports.Abstract;

namespace Staffinfo.API.Controllers
{
    [Route("api/reports")]
    //[Authorize]
    public class ReportingController : ApiController
    {
        private readonly IReportGenerator _generator;
        private readonly ILogger _logger;

        public ReportingController(IReportGenerator generator, ILogger logger)
        {
            _generator = generator;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/reports/total-employees/{format}")]
        public async Task<HttpResponseMessage> GetTotalEmployeesReport(string format)
        {
            try
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                string filename;
                MemoryStream stream;

                if (String.CompareOrdinal(format, "pdf") == 0)
                {
                    filename = $"Сотрудники-{DateTime.Now.ToString("d", new CultureInfo("ru-RU"))}.pdf";
                    stream = new MemoryStream((await _generator.GetTotalEmployeesListAsPdf()).ToArray());
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = filename
                    };
                }
                else
                {
                    filename = $"Сотрудники-{DateTime.Now.ToString("d", new CultureInfo("ru-RU"))}.xlsx";
                    stream = await _generator.GetTotalEmployeesListAsXlsx();
                    stream.Position = 0;
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = filename
                    };
                }
                _logger.Info(new CultureInfo("ru-RU"), "\"Комплексный отчет по сотрудникам\" был успешно сформирован");

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(new CultureInfo("ru-RU"), $"Произошла ошибка при формировании отчета: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };
            }
        }
    }
}
