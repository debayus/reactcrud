using System;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Models;

public class AppUser : IdentityUser
{
    public string? DisplayName { get; set; }
}