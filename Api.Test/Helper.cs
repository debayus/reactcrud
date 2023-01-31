using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using Api.Services;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Persistence.Models;

namespace Api.Test;

public class Helper
{
    public static Mock<UserManager<AppUser>> MockUserManager(List<AppUser>? users = null)
    {
        if (users == null)
        {
            users = new List<AppUser>()
            {
                new AppUser()
                {
                    UserName = "test",
                    Email = "test@test.com"
                }
            };
        }

        var store = new Mock<IUserStore<AppUser>>();
        var mgr = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Object.UserValidators.Add(new UserValidator<AppUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<AppUser>());

        mgr.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<AppUser, string>((x, y) => users.Add(x));
        mgr.Setup(x => x.Users).Returns(users.AsQueryable());
        mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(true);

        return mgr;
    }

    public static Mock<ITokenService> MockTokenService()
    {
        var mgr = new Mock<ITokenService>();
        mgr.Setup(x => x.CreateToken(It.IsIn<AppUser>())).Returns("TOKEN");
        return mgr;
    }

    public static IList<ValidationResult> ValidateModel(object model, string? containMessage = null)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        if (containMessage != null)
        {
            validationResults = validationResults.Where(x => x.ErrorMessage!.Contains(containMessage)).ToList();
        }
        return validationResults;
    }

    public static ControllerContext GetClaimControllerContext(string email = "test@test.com")
    {
        return new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new List<ClaimsIdentity>()
                    {
                        new ClaimsIdentity(new List<Claim>()
                        {
                            new Claim(ClaimTypes.Email, email)
                        })
                    })
            }
        };
    }
}

