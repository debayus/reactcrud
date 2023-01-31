using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class AccountUserModel
{
    public string DisplayName { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string Username { get; set; } = default!;
}

public class AccountLoginParamModel
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}

public class AccountRegisterParamModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    [Required]
    public string DisplayName { get; set; } = default!;

    [Required]
    public string Username { get; set; } = default!;
}

