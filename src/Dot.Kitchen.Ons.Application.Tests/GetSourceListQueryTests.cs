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
    public class GetSourceListQueryTests
    {
        [Fact]
        public void Instantiate_NoRepository_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { new GetSourceListQuery(null); });
        }

        [Fact]
        public void Execute_NoSources_ReturnsEmptySourceList()
        {
            var mockRepository = new Mock<ISourceRepository>();
            mockRepository.Setup(q => q.GetAll()).Returns(new List<Source>().AsQueryable());
            var listQuery = new GetSourceListQuery(mockRepository.Object);

            var result = listQuery.Execute();

            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void Execute_MultipleSources_ReturnsSourceListWithMultipleItems()
        {
            var source1 = new Source() { Id = 1, FriendlyName = "FriendlyName1", Description = "description1" };
            var source2 = new Source() { Id = 2, FriendlyName = "FriendlyName2", Description = "description2"};
            var sources = new List<Source>() {source1, source2};
            var mockRepository = new Mock<ISourceRepository>();
            mockRepository.Setup(q => q.GetAll()).Returns(sources.AsQueryable());
            var listQuery = new GetSourceListQuery(mockRepository.Object);

            var result = listQuery.Execute();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("FriendlyName1", result[0].FriendlyName);
            Assert.Equal("description1", result[0].Description);
        }

    }

}
