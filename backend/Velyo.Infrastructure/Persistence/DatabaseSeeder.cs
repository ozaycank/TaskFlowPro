using Microsoft.EntityFrameworkCore;
using Velyo.Domain.Entities;
using Velyo.Domain.Common.Models;

namespace Velyo.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedDevelopmentDataAsync(ApplicationDbContext context)
    {
        // Global test kullanÄ±cÄ±sÄ± ID'si (DevelopmentCurrentUserService ile birebir eÅŸleÅŸmeli)
        var devUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        // KullanÄ±cÄ± zaten veritabanÄ±nda var mÄ± kontrol et
        var userExists = await context.Users.AnyAsync(u => u.Id == devUserId);

        if (!userExists)
        {
            var devUser = User.Create(
                email: "testadmin@velyo.local",
                firstName: "Ozay Can",
                lastName: "Kirli"
            );

            // Domain katmanÄ±nda Id protected set olduÄŸu iÃ§in, Reflection ile test ID'sini zorla atÄ±yoruz 
            // veya User modelinde Id ezilebilir. En temizi domain'i bozmamak iÃ§in burada atama simÃ¼lasyonu yapmak:
            typeof(Entity).GetProperty(nameof(Entity.Id))?
                .SetValue(devUser, devUserId);

            context.Users.Add(devUser);
            await context.SaveChangesAsync();
        }
    }
}