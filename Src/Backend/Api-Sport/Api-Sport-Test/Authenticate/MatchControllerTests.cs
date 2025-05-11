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
        [Theory(DisplayName = "MatchDetailInvalid")]
        [InlineData(2)]
        public async Task MatchDetailsInvalid(int Id)
        {
            //arrange
            var matchInstance = new Api_Sport_DataLayer_DataLayer.Models.Match
            {
                Id = 1,
                Description = "Test",
                Title = "Test",
            };
            var mockUserService = new Mock<IMatcheService>();
            mockUserService.Setup(x => x.GetMatchDetailsByIdAsync(Id)).ReturnsAsync(matchInstance);
            var controller = new MatchController(mockUserService.Object);
            //act

            var result = controller.MatchDetails(Id);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = okResult.Value;

            //assert
            Assert.Equal(resultValue, matchInstance);

        }
    }
}
