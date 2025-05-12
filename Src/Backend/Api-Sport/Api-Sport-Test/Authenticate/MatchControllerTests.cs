using Api_Sport.Controllers;
using Api_Sport_Business_Logic.Services.Interfaces;
using Api_Sport_Business_Logic_Business_Logic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Sport_Test.Authenticate
{
    public class MatchControllerTests
    {
        private readonly Mock<IMatcheService> _mockMatchService;
        private readonly MatchController _matchController;
        public MatchControllerTests()
        {
            _mockMatchService = new Mock<IMatcheService>();
            _matchController = new MatchController(
            _mockMatchService.Object
        );
        }

        #region match detail
        [Theory(DisplayName = "MatchDetails_ReturnsOk_WhenMatchExists")]
        [InlineData(1)]
        public async Task MatchDetails_ReturnsOk_WhenMatchExists(int id)
        {
            // Arrange
            var match = new Api_Sport_DataLayer_DataLayer.Models.Match { Id = id, Title = "Title", Description = "Desc" };
            var mockService = new Mock<IMatcheService>();
            mockService.Setup(x => x.GetMatchDetailsByIdAsync(id)).ReturnsAsync(match);
            var controller = new MatchController(mockService.Object);

            // Act
            var result = await controller.MatchDetails(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(match, okResult.Value);
        }
        [Theory(DisplayName = "MatchDetails_ReturnsNotFound_WhenMatchDoesNotExist")]
        [InlineData(99)]
        public async Task MatchDetails_ReturnsNotFound_WhenMatchDoesNotExist(int id)
        {
            // Arrange
            var mockService = new Mock<IMatcheService>();
            mockService.Setup(x => x.GetMatchDetailsByIdAsync(id)).ReturnsAsync((Api_Sport_DataLayer_DataLayer.Models.Match)null);
            var controller = new MatchController(mockService.Object);

            // Act
            var result = await controller.MatchDetails(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion
        #region match create
        [Fact(DisplayName = "CreateMatch_ReturnsOk_WhenValidMatch")]
        public async Task CreateValidMatchReturnOk()
        {
            // Arrange
            var match = new Api_Sport_DataLayer_DataLayer.Models.Match
            {
                Id = 1,
                Title = "Title",
                Description = "Desc"
            };

            var mockService = new Mock<IMatcheService>();
            mockService.Setup(c => c.CreateMatchAsync(match)).Returns(Task.CompletedTask);

            var controller = new MatchController(mockService.Object);

            // Act
            var result = await controller.CreateMatch(match);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(match, okResult.Value);
        }

        #endregion
    }
}
