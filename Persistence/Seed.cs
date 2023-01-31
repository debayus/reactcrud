using System;
using Microsoft.AspNetCore.Identity;
using Persistence.Models;
using System.Diagnostics;

namespace Persistence;

public class Seed
{
    public static async Task SeedData(DataContext context,
            UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any() && !context.Items.Any())
        {
            var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Bob",
                        UserName = "bob",
                        Email = "test@test.com"
                    },
                };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "123456");
            }

            var items = new List<ItemDbModel>
                {
                    new ItemDbModel
                    {
                        Title = "Guitar",
                    },
                    new ItemDbModel
                    {
                        Title = "Drum",
                    },
                    new ItemDbModel
                    {
                        Title = "Microphone",
                    },
                };

            await context.Items.AddRangeAsync(items);
            await context.SaveChangesAsync();
        }
    }
}

