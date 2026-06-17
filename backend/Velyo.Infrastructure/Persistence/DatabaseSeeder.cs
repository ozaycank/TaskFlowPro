using Microsoft.EntityFrameworkCore;
using Velyo.Domain.Entities;
using Velyo.Domain.Common.Models;

namespace Velyo.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedDevelopmentDataAsync(ApplicationDbContext context)
    {
        var devUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        var userExists = await context.Users.AnyAsync(u => u.Id == devUserId);

        if (!userExists)
        {
            // "Password123!" şifresinin BCrypt karması üretiliyor
            var defaultPasswordHash = BCrypt.Net.BCrypt.HashPassword("dotnet run");

            var devUser = User.Create(
                email: "testadmin@velyo.local",
                firstName: "Ozay Can",
                lastName: "Kirli",
                passwordHash: defaultPasswordHash // FIXED: Eksik olan parametre eklendi
            );

            typeof(Entity).GetProperty(nameof(Entity.Id))?
                .SetValue(devUser, devUserId);

            context.Users.Add(devUser);
            await context.SaveChangesAsync();
        }
    }
}