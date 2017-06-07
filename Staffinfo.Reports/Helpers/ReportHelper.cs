using System.IO;

namespace Staffinfo.Reports.Helpers
{
    public class ReportHelper
    {
        /// <summary>
        /// Возвращает путь временной папки пользователя для сохранения отчета
        /// </summary>
        /// <param name="fileName">имя файла</param>
        /// <returns></returns>
        static public string GetTempPathForSave(string fileName)
        {
            var tempPath = Path.GetTempPath();
            return Path.Combine(tempPath, fileName);
        }
    }
}