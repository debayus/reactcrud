using System;
using Persistence.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Persistence;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ItemDbModel> Items { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

