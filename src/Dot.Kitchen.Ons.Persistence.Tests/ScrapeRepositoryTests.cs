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
    public class ScrapeRepositoryTests
    {
        [Fact]
        public void Initialise_NoDatabaseContext_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => { new ScrapeRepository(null); });
        }

        [Fact]
        public void Initialise_ProvideValidDatabaseContext_RepositoryShouldNotBeNull()
        {
            var context = new Mock<IDatabaseContext>();
            var repository = new ScrapeRepository(context.Object);
            Assert.NotNull(repository);
        }

        [Fact]
        public void GetAll_SingleRecordToReturn_ReturnsOneRecords()
        {
            var source = new Source() { Id = 1, Name = "name", FriendlyName = "FriendlyName" };
            var scrape1 = new Scrape() { Id = 1, SourceId = 1, Source = source, Surname = "surname", StartedAt = DateTime.UtcNow };
            var data = new List<Scrape> { scrape1 }.AsQueryable();
            var mockDbSet = GetMockDbSet(data);
            var mockContext = new Mock<IDatabaseContext>();
            mockContext.Setup(c => c.Scrapes).Returns(mockDbSet.Object);

            var repository = new ScrapeRepository(mockContext.Object);
            var result = repository.GetAll();
            Assert.NotNull(result);
            Assert.Equal(1, result.Count<Scrape>());
        }

        [Fact]
        public void GetAll_MultipleRecordsToReturn_ReturnsAllRecords()
        {
            var source = new Source() { Id = 1, Name = "name", FriendlyName = "FriendlyName" };
            var scrape1 = new Scrape() { Id = 1, SourceId = 1, Source = source, Surname = "surname", StartedAt = DateTime.UtcNow };
            var scrape2 = new Scrape() { Id = 2, SourceId = 1, Source = source, Surname = "surname2", StartedAt = DateTime.UtcNow };
            var data = new List<Scrape> { scrape1, scrape2 }.AsQueryable();
            var mockDbSet = GetMockDbSet(data);
            var mockContext = new Mock<IDatabaseContext>();
            mockContext.Setup(c => c.Scrapes).Returns(mockDbSet.Object);

            var repository = new ScrapeRepository(mockContext.Object);
            var result = repository.GetAll();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count<Scrape>());
        }

        private Mock<DbSet<Scrape>> GetMockDbSet(IQueryable<Scrape> data)
        {
            var mockDbSet = new Mock<DbSet<Scrape>>();
            mockDbSet.As<IQueryable<Scrape>>().Setup(m => m.Provider).Returns(data.Provider); 
            mockDbSet.As<IQueryable<Scrape>>().Setup(m => m.Expression).Returns(data.Expression); 
            mockDbSet.As<IQueryable<Scrape>>().Setup(m => m.ElementType).Returns(data.ElementType); 
            mockDbSet.As<IQueryable<Scrape>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockDbSet;            
        }
    }
}
