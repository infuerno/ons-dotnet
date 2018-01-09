using System;
using System.Collections.Generic;
using System.Text;
using Dot.Kitchen.Ons.Application.Commands;
using Dot.Kitchen.Ons.Application.Interfaces;
using Dot.Kitchen.Ons.Application.Models;
using Dot.Kitchen.Ons.Domain;
using Moq;
using Xunit;

namespace Dot.Kitchen.Ons.Application.Tests
{
    public class CreateSourceCommandTests
    {
        [Fact]
        public void Intialise_NoRepository_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new CreateSourceCommand(null));
        }

        [Fact]
        public void Execute_NullSourceModel_ThrowsException()
        {
            var mockSourceRepository = new Mock<ISourceRepository>();
            var createSourceCommand = new CreateSourceCommand(mockSourceRepository.Object);
            Assert.Throws<ArgumentNullException>(() => createSourceCommand.Execute(null));
        }

        [Fact]
        public void Execute_AddsSourceToDatabase()
        {
            var mockSourceRepository = new Mock<ISourceRepository>();
            var createSourceCommand = new CreateSourceCommand(mockSourceRepository.Object);
            var sourceModel = new SourceModel();
            createSourceCommand.Execute(sourceModel);
            mockSourceRepository.Verify(r => r.Add(It.IsAny<Source>()), Times.Once);
        }

    }
}
