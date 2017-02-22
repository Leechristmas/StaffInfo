using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Tests
{
    [TestClass]
    public class RepositoriesTests
    {
        private IUnitRepository _repository;
        
        /// <summary>
        /// Setup an implementation of IQueryable
        /// </summary>
        /// <typeparam name="T">type of the set</typeparam>
        /// <param name="set">DbSet of items</param>
        /// <param name="data">fake data for the set</param>
        private void ConfigureTheDbSet<T>(Mock<DbSet<T>> set, IQueryable<T> data) where T : class
        {
            set.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(() => new TestDbAsyncEnumerator<T>(data.GetEnumerator()));//lambda is necessary for giving a new enumerator (otherwise items will not be yielded)
            set.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(data.Provider));
            set.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            set.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            set.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());//one more necessary lambda
        }

        [TestInitialize]
        public void SetUp()
        {
            var addressesData = new List<Address>
            {
                new Address
                {
                    Id = 1,
                    City = "Gomel",
                    Area = "Gomelskaya",
                    DetailedAddress = "Vladzimirova st., 19/2",
                    ZipCode = "247022"
                },
                new Address
                {
                    Id = 1,
                    City = "Gomel",
                    Area = "Gomelskaya",
                    DetailedAddress = "Bochkina st., 9/12",
                    ZipCode = "247021"
                },
                new Address
                {
                    Id = 1,
                    City = "Gomel",
                    Area = "Gomelskaya",
                    DetailedAddress = "Petrova st., 7/3",
                    ZipCode = "247020"
                }
            }.AsQueryable();

            //configuration of IQueryable sets
            var addresses = new Mock<DbSet<Address>>();
            ConfigureTheDbSet(addresses, addressesData);
            addresses.Setup(b => b.FindAsync(It.IsAny<object[]>()))    //substitution of the .SelectAsync(id) method
                .Returns<object[]>(ids => addresses.Object.FirstOrDefaultAsync(b => b.Id == (int)ids[0]));

            var context = new Mock<StaffContext>();
            context.Setup(c => c.Set<Address>()).Returns(() => addresses.Object);
            context.Setup(c => c.Addresses).Returns(() => addresses.Object);

            _repository = new StaffUnitRepository(new Repository<Address>(context.Object), null, null, null, null, null,
                null, null, null, null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public void GetAllItems_ShouldReturns3Items()
        {
            IEnumerable<Address> addresses = null;
            Address address = null;

            addresses = _repository.AddressRepository.SelectAsync().Result;
            address = _repository.AddressRepository.SelectAsync(1).Result;

            Assert.IsNotNull(address);
            Assert.IsNotNull(addresses);
            Assert.IsTrue(addresses.Count() == 3);
        }
    }

    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }

}