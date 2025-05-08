using Api_Sport.Controllers;
using Api_Sport_Business_Logic.Models.Dtos;
using Api_Sport_Business_Logic.Services.Interfaces;
using Api_Sport_Business_Logic_Business_Logic.Services.Interfaces;
using Api_Sport_DataLayer_DataLayer.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Moq;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IAuthHelperService> _mockAuthHelper;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthenticateController _controller;

    public UsersControllerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockMapper = new Mock<IMapper>();
        _mockAuthHelper = new Mock<IAuthHelperService>();
        _mockConfiguration = new Mock<IConfiguration>();

        _controller = new AuthenticateController(
            _mockConfiguration.Object,
            _mockMapper.Object,
            _mockUserService.Object,
            _mockAuthHelper.Object
        );
    }

    [Fact]
    public async Task Register_ValidUser_ReturnsOkWithToken()
    {
        // Arrange
        var userDto = new UserDto
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

        var mappedUserDto = new UserDto
        {
            Id = 1,
            UserName = "testuser",
            Email = "test@example.com"
        };

        var token = "fake_jwt_token";

        var mockUserService = new Mock<IUserService>();
        var mockMapper = new Mock<IMapper>();
        var mockAuthHelper = new Mock<IAuthHelperService>();

        mockAuthHelper.Setup(x => x.UserExistsAsync(userDto.UserName, userDto.Email))
                      .ReturnsAsync(false);

        mockUserService.Setup(x => x.AddUserAsync(userDto))
                       .ReturnsAsync(createdUser);

        mockMapper.Setup(x => x.Map<UserDto>(createdUser))
                  .Returns(mappedUserDto);

        mockAuthHelper.Setup(x => x.GenerateJwtTokenAsync(mappedUserDto))
                      .ReturnsAsync(token);

        var controller = new AuthenticateController(_mockConfiguration.Object, mockMapper.Object, mockUserService.Object, mockAuthHelper.Object);


        // ترفند برای دادن دستی AuthHelper اگر تو سازنده نیست:
        typeof(AuthenticateController)
            .GetField("_authHelper", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(controller, mockAuthHelper.Object);

        // Act
        var result = await controller.Register(userDto);
        var okResult = Assert.IsType<OkObjectResult>(result);
        // خروجی از نوع dynamic
        var resultValue = okResult.Value;
        var returnedToken = resultValue?.GetType().GetProperty("token")?.GetValue(resultValue, null)?.ToString();
        // Assert
        Assert.Equal(token, returnedToken);
    }


    [Fact]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        // Arrange
        var loginDto = new LoginUserDto
        {
            UserName = "testuser",
            Password = "1234"
        };

        var user = new User
        {
            Id = 1,
            UserName = "testuser",
            Email = "test@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("1234") // رمز هَش شده برای تست
        };

        var userDto = new UserDto
        {
            Id = 1,
            UserName = "testuser",
            Email = "test@example.com"
        };

        var token = "fake_jwt_token";

        var mockUserService = new Mock<IUserService>();
        var mockMapper = new Mock<IMapper>();
        var mockAuthHelper = new Mock<IAuthHelperService>();
        var mockConfig = new Mock<IConfiguration>();

        // شبیه‌سازی رفتار سرویس‌ها
        mockUserService.Setup(x => x.GetUserByUsernameAsync(loginDto.UserName))
                       .ReturnsAsync(user);

        mockMapper.Setup(x => x.Map<UserDto>(user))
                  .Returns(userDto);

        mockAuthHelper.Setup(x => x.GenerateJwtTokenAsync(userDto))
                      .ReturnsAsync(token);

        var controller = new AuthenticateController(mockConfig.Object, mockMapper.Object, mockUserService.Object, mockAuthHelper.Object);

        // Act
        var result = await controller.Login(loginDto);
        var okResult = Assert.IsType<OkObjectResult>(result);
        // خروجی از نوع dynamic
        var resultValue = okResult.Value;
        var returnedToken = resultValue?.GetType().GetProperty("token")?.GetValue(resultValue, null)?.ToString();

        // Assert
        Assert.Equal(token, returnedToken);
    }


}
