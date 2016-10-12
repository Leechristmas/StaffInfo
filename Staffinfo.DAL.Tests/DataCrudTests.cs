using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Repositories;

namespace Staffinfo.DAL.Tests
{
    /// <summary>
    /// Includes unit-tests for Unit repository
    /// </summary>
    [TestClass]
    public class DataCrudTests
    {
        private StaffUnitRepository _staffUnitRepository = new StaffUnitRepository();

        [TestMethod]
        public void Select_GeyAllAddresses()
        {
            
        }
    }
}