using Api_Sport.Controllers;
using Api_Sport_Business_Logic.Models.Dtos;
using Api_Sport_Business_Logic.Services.Interfaces;
using Api_Sport_DataLayer.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthenticateController _controller;

    public UsersControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockMapper = new Mock<IMapper>();
        _mockConfiguration = new Mock<IConfiguration>();

        _controller = new AuthenticateController(
            _mockConfiguration.Object,
            _mockMapper.Object,
            _mockUserService.Object
        );
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Email", "Required");

        var user = new UserDto();

        // Act
        var result = await _controller.Register(user);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    }
    [Fact]
    public async Task Register_ReturnsOk_WithToken_WhenUserIsValid()
    {
        // Arrange
        var inputUserDto = new UserDto
        {
            Id = 0,
            UserName = "testuser",
            Email = "test@example.com",
            Password = "1234"
        };

        var createdUser = new User
        {
            Id = 1,
            UserName = "testuser",
            Email = "test@example.com",
            Password = "hashed_password"
        };

        var outputUserDto = new UserDto
        {
            Id = 1,
            UserName = "testuser",
            Email = "test@example.com"
        };

        _mockUserService.Setup(x => x.ValidationCredentialAsync(inputUserDto.UserName, inputUserDto.Email))
                        .ReturnsAsync((User)null); // یعنی کاربر قبلاً وجود نداره

        _mockUserService.Setup(x => x.AddUserAsync(inputUserDto))
                        .ReturnsAsync(createdUser); // کاربر با موفقیت ساخته شد

        _mockMapper.Setup(x => x.Map<UserDto>(createdUser))
                   .Returns(outputUserDto);

        _mockConfiguration.Setup(x => x["Authentication:SecretForKey"])
                          .Returns("THIS_IS_A_TEST_SECRET_1234567890"); // حتماً طولش زیاد باشه

        _mockConfiguration.Setup(x => x["Authentication:Issuer"])
                          .Returns("TestIssuer");

        _mockConfiguration.Setup(x => x["Authentication:Audience"])
                          .Returns("TestAudience");

        // Act
        var result = await _controller.Register(inputUserDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        Assert.IsType<string>(okResult.Value); // چون خروجی JWT Token هست
    }

}
