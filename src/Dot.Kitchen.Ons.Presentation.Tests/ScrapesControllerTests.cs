using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Dot.Kitchen.Ons.Application;
using Dot.Kitchen.Ons.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Dot.Kitchen.Ons.Application.Queries;
using Dot.Kitchen.Ons.Application.Models;

namespace Dot.Kitchen.Ons.Presentation.Tests
{
    public class ScrapesControllerTests
    {
        [Fact]
        public void Instantiate_NoIGetScrapesListQuery_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => { new ScrapesController(null, new Mock<IGetScrapeDetailQuery>().Object, null); });
        }

        [Fact]
        public void Instantiate_NoIGetScrapeDetailsQuery_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => { new ScrapesController(new Mock<IGetScrapeListQuery>().Object, null, null); });
        }

        [Fact]
        public void Index_NoItems_ShouldNotBeNull()
        {
            var mockListQuery = new Mock<IGetScrapeListQuery>();
            var mockDetailsQuery = new Mock<IGetScrapeDetailQuery>();
            mockListQuery.Setup(q => q.Execute()).Returns(new List<ScrapeModel> { });

            var controller = new ScrapesController(mockListQuery.Object, mockDetailsQuery.Object, null);
            var viewResult = controller.Index();

            Assert.NotNull(viewResult);
        }

        [Fact]
        public void Index_NoItems_ShouldBeOfTypeViewResult()
        {
            var mockListQuery = new Mock<IGetScrapeListQuery>();
            var mockDetailsQuery = new Mock<IGetScrapeDetailQuery>();
            mockListQuery.Setup(q => q.Execute()).Returns(new List<ScrapeModel> { });

            var controller = new ScrapesController(mockListQuery.Object, mockDetailsQuery.Object, null);
            var viewResult = controller.Index();

            Assert.IsType<ViewResult>(viewResult);
        }

        [Fact]
        public void Index_NoItems_ShouldBeEmptyList()
        {
            var mockListQuery = new Mock<IGetScrapeListQuery>();
            var mockDetailsQuery = new Mock<IGetScrapeDetailQuery>();
            mockListQuery.Setup(q => q.Execute()).Returns(new List<ScrapeModel> { });

            var controller = new ScrapesController(mockListQuery.Object, mockDetailsQuery.Object, null);
            var viewResult = (ViewResult)controller.Index();
            var items = (List<ScrapeModel>)viewResult.Model;

            Assert.Equal(0, items.Count);
        }

        [Fact]
        public void Index_OneItem_ShouldReturnListOfOneScrapeItem()
        {
            var scrape = new ScrapeModel();
            var mockListQuery = new Mock<IGetScrapeListQuery>();
            var mockDetailsQuery = new Mock<IGetScrapeDetailQuery>();
            mockListQuery.Setup(q => q.Execute()).Returns(new List<ScrapeModel> { scrape });

            var controller = new ScrapesController(mockListQuery.Object, mockDetailsQuery.Object, null);

            var viewResult = (ViewResult)controller.Index();
            var items = (List<ScrapeModel>)viewResult.Model;

            Assert.Equal(1,items.Count);
            Assert.Contains(scrape, items);
        }

        [Fact]
        public void Index_MultipleItems_ShouldReturnListOfScrapeItems()
        {
            var scrape1 = new ScrapeModel();
            var scrape2 = new ScrapeModel();
            var mockListQuery = new Mock<IGetScrapeListQuery>();
            var mockDetailsQuery = new Mock<IGetScrapeDetailQuery>();
            mockListQuery.Setup(q => q.Execute()).Returns(new List<ScrapeModel> { scrape1, scrape2 });

            var controller = new ScrapesController(mockListQuery.Object, mockDetailsQuery.Object, null);

            var viewResult = (ViewResult)controller.Index();
            var items = (List<ScrapeModel>)viewResult.Model;

            Assert.Equal(2, items.Count);
            Assert.Contains(scrape1, items);
            Assert.Contains(scrape2, items);
        }

    }
}
