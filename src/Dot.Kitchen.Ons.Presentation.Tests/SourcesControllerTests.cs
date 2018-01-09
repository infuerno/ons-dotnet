using System;
using System.Collections.Generic;
using System.Text;
using Dot.Kitchen.Ons.Application.Commands;
using Dot.Kitchen.Ons.Application.Models;
using Dot.Kitchen.Ons.Application.Queries;
using Xunit;
using Moq;
using Dot.Kitchen.Ons.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Kitchen.Ons.Presentation.Tests
{
    public class SourcesControllerTests
    {
        [Fact]
        public void Instantiate_NoIGetSourcesListQuery_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => { new SourcesController(null, new Mock<ICreateSourceCommand>().Object); });
        }

        [Fact]
        public void Instantiate_NoICreateSourceCommand_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => { new SourcesController(new Mock<IGetSourceListQuery>().Object, null); });
        }
        [Fact]
        public void Index_NoSources_ModelShouldNotBeNull()
        {
            var mockListQuery = new Mock<IGetSourceListQuery>();
            var mockCreateCommand = new Mock<ICreateSourceCommand>();
            mockListQuery.Setup(s => s.Execute()).Returns(new List<SourceModel>());
            var controller = new SourcesController(mockListQuery.Object, mockCreateCommand.Object);
            var viewResult = (ViewResult)controller.Index();
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.Model);
        }

        [Fact]
        public void Index_NoSources_ModelShouldBeOfCorrectType()
        {
            var mockListQuery = new Mock<IGetSourceListQuery>();
            var mockCreateCommand = new Mock<ICreateSourceCommand>();
            mockListQuery.Setup(s => s.Execute()).Returns(new List<SourceModel>());
            var controller = new SourcesController(mockListQuery.Object, mockCreateCommand.Object);
            var viewResult = (ViewResult)controller.Index();
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.Model);
            Assert.IsType<List<SourceModel>>(viewResult.Model);
        }


        [Fact]
        public void Index_SingleSource_ModelPopulated()
        {
            var source = new SourceModel() {Id = 1};
            var mockListQuery = new Mock<IGetSourceListQuery>();
            var mockCreateCommand = new Mock<ICreateSourceCommand>();
            mockListQuery.Setup(s => s.Execute()).Returns(new List<SourceModel>() {source});
            var controller = new SourcesController(mockListQuery.Object, mockCreateCommand.Object);
            var viewResult = (ViewResult)controller.Index();
            Assert.NotNull(viewResult);
            Assert.NotNull(viewResult.Model);
            var sources = (List<SourceModel>) viewResult.Model;
            Assert.Equal(source, sources[0]);
        }

        [Fact]
        public void Create_ModelNotValid_ReturnsCreateView()
        {
            var mockListQuery = new Mock<IGetSourceListQuery>();
            var mockCreateCommand = new Mock<ICreateSourceCommand>();
            var controller = new SourcesController(mockListQuery.Object, mockCreateCommand.Object);
            var viewResult = (ViewResult)controller.Create(null);
            Assert.NotNull(viewResult);
            Assert.Null(viewResult.ViewName); // same action
        }

        [Fact]
        public void Create_ModelNotValid_ReturnsErrors()
        {
            var mockListQuery = new Mock<IGetSourceListQuery>();
            var mockCreateCommand = new Mock<ICreateSourceCommand>();
            var controller = new SourcesController(mockListQuery.Object, mockCreateCommand.Object);
            var viewResult = (ViewResult)controller.Create(null);
            Assert.NotNull(viewResult);
            Assert.Null(viewResult.ViewName); // same action
        }

        [Fact]
        public void Create_ExecuteThrowsException_ReturnsCreateView()
        {
            var mockListQuery = new Mock<IGetSourceListQuery>();
            var mockCreateCommand = new Mock<ICreateSourceCommand>();

            mockCreateCommand.Setup(s => s.Execute(null)).Throws(new Exception());
            var controller = new SourcesController(mockListQuery.Object, mockCreateCommand.Object);
            var viewResult = (ViewResult)controller.Create(null);
            Assert.NotNull(viewResult);
            Assert.Null(viewResult.ViewName); // same action
        }

        [Fact]
        public void Create_ExecuteCompletesWithoutException_ReturnsIndexView()
        {
            var mockListQuery = new Mock<IGetSourceListQuery>();
            var mockCreateCommand = new Mock<ICreateSourceCommand>();

            mockCreateCommand.Setup(s => s.Execute(null));
            var controller = new SourcesController(mockListQuery.Object, mockCreateCommand.Object);
            var redirectToActionResult = (RedirectToActionResult)controller.Create(null);
            Assert.NotNull(redirectToActionResult);
            Assert.Null(redirectToActionResult.ControllerName); // same controller
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        //[Fact]
        //public void Create_ExecuteCompletes_ReturnsIndexWithMessage()
        //{
            
        //}

    }
}
