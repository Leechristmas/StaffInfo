using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using iTextSharp.text;
using Staffinfo.DAL.Repositories.Interfaces;
using Staffinfo.Reports;

namespace Staffinfo.API.Controllers
{
    [Route("api/reports")]
    //[Authorize]
    public class ReportingController : ApiController
    {
        private readonly IUnitRepository _repository;

        public ReportingController(IUnitRepository repository)
        {
            _repository = repository;
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
                    stream = new MemoryStream((await ReportsGenerator.GetTotalEmployeesListAsPdf()).ToArray());
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
                    stream = await ReportsGenerator.GetTotalEmployeesListAsXlsx();
                    stream.Position = 0;
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = filename
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
