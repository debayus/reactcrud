using Api.Controllers;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;

namespace Api.Test;

public class AccountControllerTest
{
    [Theory]
    [InlineData("test", "123456", 0)]
    [InlineData("", "", 2)]
    public void LoginParam_Validation(string username, string pass, int expectedResult)
    {
        var model = new AccountLoginModel
        {
            Username = username,
            Password = pass,
        };

        Assert.Equal(expectedResult, Helper.ValidateModel(model).Count);
    }

    [Theory]
    [InlineData("", "" , "", "", 4)]
    [InlineData("test", "test" , "test", "test", 1)]
    [InlineData("test", "test@test.com", "test", "test", 0)]
    public void RegisterParam_Validation(string displayName, string email, string username, string pass, int expectedResult)
    {
        var model = new AccountRegisterModel()
        {
            DisplayName = displayName,
            Email = email,
            Username = username,
            Password = pass,
        };

        Assert.Equal(expectedResult, Helper.ValidateModel(model).Count);
    }

    [Fact]
    public async void Login_Success()
    {
        var userManager = Helper.MockUserManager().Object;
        var tokenService = Helper.GetTokenService();
        var controller = new AccountController(userManager, tokenService);

        var response = await controller.Login(new AccountLoginModel()
        {
            Username = "test",
            Password = "123456",
        });

        Assert.NotNull(response.Value);
    }

    [Fact]
    public async void Login_Unauthorized()
    {
        var userManager = Helper.MockUserManager().Object;
        var tokenService = Helper.GetTokenService();
        var controller = new AccountController(userManager, tokenService);

        var response = await controller.Login(new AccountLoginModel()
        {
            Username = "test2",
            Password = "123456",
        });

        Assert.Equal(StatusCodes.Status401Unauthorized, ((UnauthorizedResult)response.Result!).StatusCode);
    }

    [Fact]
    public async void Register_Success()
    {
        var userManager = Helper.MockUserManager().Object;
        var tokenService = Helper.GetTokenService();
        var controller = new AccountController(userManager, tokenService);

        var response = await controller.Register(new AccountRegisterModel()
        {
            Email = "test2@test.com",
            Password = "123456",
            DisplayName = "Test",
            Username = "test2",
        });

        Assert.NotNull(response.Value);
    }

    [Theory]
    [InlineData("test@test.com", "empty")]
    [InlineData("empty@empty.com", "test")]
    public async void Register_BadRequest(string email, string username)
    {
        var userManager = Helper.MockUserManager().Object;
        var tokenService = Helper.GetTokenService();
        var controller = new AccountController(userManager, tokenService);

        var response = await controller.Register(new AccountRegisterModel()
        {
            Email = email,
            Password = "123456",
            DisplayName = "Test",
            Username = username,
        });

        Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)response.Result!).StatusCode);
    }

    [Fact]
    public void GetCurrentUser_Success()
    {
        var userManager = Helper.MockUserManager().Object;
        var tokenService = Helper.GetTokenService();

        var controller = new AccountController(userManager, tokenService)
        {
            ControllerContext = Helper.GetClaimControllerContext()
        };

        var response = controller.GetCurrentUser();

        Assert.NotNull(response.Value);
    }

    [Fact]
    public void GetCurrentUser_Unauthorized()
    {
        var userManager = Helper.MockUserManager().Object;
        var tokenService = Helper.GetTokenService();

        var controller = new AccountController(userManager, tokenService)
        {
            ControllerContext = Helper.GetClaimControllerContext("empty@empty.com")
        };

        var response = controller.GetCurrentUser();

        Assert.Equal(StatusCodes.Status404NotFound, ((NotFoundResult)response.Result!).StatusCode);
    }
}

