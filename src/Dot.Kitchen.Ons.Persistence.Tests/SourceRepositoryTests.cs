using System;
using Xunit;
using Moq;
using Dot.Kitchen.Ons.Persistence;
using Dot.Kitchen.Ons.Domain;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dot.Kitchen.Ons.Persistence.Tests
{
    public class SourceRepositoryTests
    {
        [Fact]
        public void Initialise_NoDatabaseContext_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => { new SourceRepository(null); });
        }

        [Fact]
        public void Initialise_ProvideValidDatabaseContext_RepositoryShouldNotBeNull()
        {
            var context = new Mock<IDatabaseContext>();
            var repository = new SourceRepository(context.Object);
            Assert.NotNull(repository);
        }

        [Fact]
        public void GetAll_SingleRecordToReturn_ReturnsOneRecord()
        {
            var source = new Source() { Id = 1, Name = "name", FriendlyName = "FriendlyName" };
            var data = new List<Source> { source }.AsQueryable();
            var mockDbSet = GetMockDbSet(data);
            var mockContext = new Mock<IDatabaseContext>();
            mockContext.Setup(c => c.Sources).Returns(mockDbSet.Object);

            var repository = new SourceRepository(mockContext.Object);
            var result = repository.GetAll();
            Assert.NotNull(result);
            Assert.Equal(1, result.Count<Source>());
        }

        [Fact]
        public void GetAll_MultipleRecordsToReturn_ReturnsAllRecords()
        {
            var source1 = new Source() { Id = 1, Name = "name1", FriendlyName = "FriendlyName1" };
            var source2 = new Source() { Id = 2, Name = "name2", FriendlyName = "FriendlyName2" };
            var data = new List<Source> { source1, source2 }.AsQueryable();
            var mockDbSet = GetMockDbSet(data);
            var mockContext = new Mock<IDatabaseContext>();
            mockContext.Setup(c => c.Sources).Returns(mockDbSet.Object);

            var repository = new SourceRepository(mockContext.Object);
            var result = repository.GetAll();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count<Source>());
        }

        private Mock<DbSet<Source>> GetMockDbSet(IQueryable<Source> data)
        {
            var mockDbSet = new Mock<DbSet<Source>>();
            mockDbSet.As<IQueryable<Source>>().Setup(m => m.Provider).Returns(data.Provider); 
            mockDbSet.As<IQueryable<Source>>().Setup(m => m.Expression).Returns(data.Expression); 
            mockDbSet.As<IQueryable<Source>>().Setup(m => m.ElementType).Returns(data.ElementType); 
            mockDbSet.As<IQueryable<Source>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockDbSet;            
        }
    }
}
