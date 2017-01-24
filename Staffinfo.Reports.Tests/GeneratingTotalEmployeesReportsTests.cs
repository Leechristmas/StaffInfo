using System;
using System.Diagnostics;
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
            const string filename =
            @"d:\\excel.xlsx";
            MemoryStream docStream = ReportsGenerator.GetTotalEmployeesListAsXlsx().Result;

            FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write);
            docStream.WriteTo(file);
            file.Close();
            docStream.Close();
            Process.Start(filename);
        }
    }
}
