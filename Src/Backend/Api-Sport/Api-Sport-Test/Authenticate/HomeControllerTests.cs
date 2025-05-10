using Api_Sport.Controllers;
using Api_Sport_Business_Logic.Services.Interfaces;
using Api_Sport_Business_Logic_Business_Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Api_Sport_Test.Authenticate
{
    public class HomeControllerTests
    {
        private readonly HomeController _controller;
        private readonly Mock<IMatcheService> _mocmatchesServices;
        public HomeControllerTests()
        {
            _mocmatchesServices = new Mock<IMatcheService>();
            _controller = new HomeController(
            _mocmatchesServices.Object
        );
        }
        [Fact]
        public async Task Home_ReturnsOkWithListOfMatches()
        {
            // Arrange
            var mockMatchService = new Mock<IMatcheService>();
            var expectedMatches = new List<Api_Sport_DataLayer_DataLayer.Models.Match>
    {
        new Api_Sport_DataLayer_DataLayer.Models.Match
        {
            Id = 1, Title = "Ligua", Description = "Test", Text = "rtl", ImageName = "png", CreateDate = DateTime.Now
        },
        new Api_Sport_DataLayer_DataLayer.Models.Match
        {
            Id = 2, Title = "Ligua", Description = "Test", Text = "rtl", ImageName = "png", CreateDate = DateTime.Now
        }
    };

            mockMatchService.Setup(x => x.GetAllmatchesAsync())
                            .ReturnsAsync(expectedMatches);

            var controller = new HomeController(mockMatchService.Object);

            // Act
            var result = await controller.Home();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var matches = Assert.IsAssignableFrom<IEnumerable<Api_Sport_DataLayer_DataLayer.Models.Match>>(okResult.Value);
            Assert.Equal(2, matches.Count());
        }

    }
}
