using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staffinfo.DAL.Context;

namespace Staffinfo.DAL.Tests
{
    [TestClass]
    public class DataCrudTests
    {
        private readonly StaffContext _db = new StaffContext();

        [TestMethod]
        public void Get_AllAddresses()
        {
            var addr = _db.Addresses.ToList();

            Assert.IsTrue(addr.Count > 0);
        }

        [TestMethod]
        public void Get_AllPosts()
        {
            var t = _db.Posts.Find(1);
            var posts = _db.Posts.ToList();

            var t2 = posts[0].Service;

            Assert.IsTrue(posts.Count > 0);
        }
    }
}