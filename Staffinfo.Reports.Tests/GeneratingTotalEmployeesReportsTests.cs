using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Staffinfo.Reports.Tests
{
    [TestClass]
    public class GeneratingTotalEmployeesReportsTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            MemoryStream docStream = ReportDataManager.GetTotalEmployeesListAsAReport();

            FileStream file = new FileStream("d:\\excel.xlxs", FileMode.Create, FileAccess.Write);
            docStream.WriteTo(file);
            file.Close();
            docStream.Close();
        }
    }
}
