using System;
using Xunit;
using Moq;
using Todo.API.Controllers;
using Todo.API.Repository;
using Todo.API.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Threading.Tasks;

namespace Todo.Tests.Controllers
{
    public class TodoControllerTest
    {
        [Fact]
        public async Task InvalidTodoItem_ShouldReturn_NotFound()
        {
            //Arrange
            var mockRepository = new Mock<ITodoRepository>();
            //Establish that our repo doesn't contain any items with ID = -1
            mockRepository.Setup(repo => repo.GetByID(-1)).Returns(null as TodoItem);
            var controller = new TodoController(mockRepository.Object);

            //Act
            var result = await controller.Get(-1);

            //Assert
            var okResult = result.Should().BeOfType<NotFoundResult>().Subject;
        }

        [Fact]
        public async Task ValidTodoItem_ShouldReturn_OkResponse() {
            //Arrange
            var mockRepository = new Mock<ITodoRepository>();
            //Establish a response for an item of ID = 1
            mockRepository.Setup(repo => repo.GetByID(1)).Returns(new TodoItem {
                ItemId = 1,
                Title = "Blah",
                Description = "Hmmm",
                Completed = true
            });
            var controller = new TodoController(mockRepository.Object);

            //Act
            var result = await controller.Get(1);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

            (okResult.Value as TodoItem).Title.Should().BeEquivalentTo("Blah");
            (okResult.Value as TodoItem).Description.Should().BeEquivalentTo("Hmmm");
            (okResult.Value as TodoItem).ItemId.Should().Equals(1);
            (okResult.Value as TodoItem).Completed.Should().Be(true);
        }
    }
}
