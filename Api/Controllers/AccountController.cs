﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly TokenService _tokenService;
    public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AccountUserModel>> Login(AccountLoginParamModel loginDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null) return Unauthorized();

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (result)
        {
            return CreateUserObject(user);
        }

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AccountUserModel>> Register(AccountRegisterParamModel registerDto)
    {
        if (_userManager.Users.Any(x => x.UserName == registerDto.Username))
        {
            ModelState.AddModelError("username", "Username taken");
            return ValidationProblem();
        }

        if (_userManager.Users.Any(x => x.Email == registerDto.Email))
        {
            ModelState.AddModelError("email", "Email taken");
            return ValidationProblem();
        }

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Username
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return CreateUserObject(user);
        }

        return BadRequest(result.Errors);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<AccountUserModel>> GetCurrentUser()
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
        if (user == null) return NotFound();
        return CreateUserObject(user);
    }

    private AccountUserModel CreateUserObject(AppUser user)
    {
        return new AccountUserModel
        {
            DisplayName = user.DisplayName!,
            Token = _tokenService.CreateToken(user),
            Username = user.UserName
        };
    }
}