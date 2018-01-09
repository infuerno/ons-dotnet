using System;
using Xunit;
using Dot.Kitchen.Ons.Application;
using Dot.Kitchen.Ons.Domain;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Dot.Kitchen.Ons.Application.Interfaces;
using Dot.Kitchen.Ons.Application.Queries;

namespace Dot.Kitchen.Ons.Application.Tests
{
    public class GetScrapeListQueryTests
    {
        [Fact]
        public void Instantiate_NoRepository_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { new GetScrapeListQuery(null); });
        }

        [Fact]
        public void Execute_ScrapeHasFinished_ReturnsScrapeList()
        {
            var startedAt = DateTime.UtcNow;
            var timeTaken = new TimeSpan(0, 10, 0);
            var source = new Source() { Id = 1, Name = "FreeBmdBirths", FriendlyName = "FreeBMD Births" };
            var scrape = new Scrape() { Id = 1, Source = source, StartedAt = startedAt, FinishedAt = startedAt.Add(timeTaken), NumberOfRecordsScraped = 5 };
            var scrapeList = new List<Scrape>() { scrape };
            var mockRepository = new Mock<IScrapeRepository>();
            mockRepository.Setup(q => q.GetAll()).Returns(scrapeList.AsQueryable());

            var listQuery = new GetScrapeListQuery(mockRepository.Object);

            var result = listQuery.Execute();

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("FreeBMD Births", result[0].SourceName);
            Assert.Equal(startedAt, result[0].StartedAt);
            Assert.Equal(timeTaken, result[0].TimeTaken.Value, new TimeSpanComparer());
            Assert.Equal(5, result[0].NumberOfRecordsScraped);
        }

        [Fact]
        public void Execute_ScrapeHasNotFinished_ReturnsScrapeList()
        {
            var startedAt = DateTime.UtcNow;
            var source = new Source() { Id = 1, Name = "FreeBmdBirths", FriendlyName = "FreeBMD Births" };
            var scrape = new Scrape() { Id = 1, Source = source, StartedAt = startedAt };
            var scrapeList = new List<Scrape>() { scrape };
            var mockRepository = new Mock<IScrapeRepository>();
            mockRepository.Setup(q => q.GetAll()).Returns(scrapeList.AsQueryable());

            var listQuery = new GetScrapeListQuery(mockRepository.Object);

            var result = listQuery.Execute();

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("FreeBMD Births", result[0].SourceName);
            Assert.Equal(startedAt, result[0].StartedAt);
            Assert.False(result[0].TimeTaken.HasValue);
            Assert.Equal(0, result[0].NumberOfRecordsScraped);
        }

        public class TimeSpanComparer : IEqualityComparer<TimeSpan>
        {
            public bool Equals(TimeSpan x, TimeSpan y)
            {
                return x == y;
            }

            public int GetHashCode(TimeSpan obj)
            {
                return obj.GetHashCode();
            }
        }
    }

}
