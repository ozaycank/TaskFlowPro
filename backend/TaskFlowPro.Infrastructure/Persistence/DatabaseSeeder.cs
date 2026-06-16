using Microsoft.EntityFrameworkCore;
using TaskFlowPro.Domain.Entities;
using TaskFlowPro.Domain.Common.Models;

namespace TaskFlowPro.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedDevelopmentDataAsync(ApplicationDbContext context)
    {
        // Global test kullanıcısı ID'si (DevelopmentCurrentUserService ile birebir eşleşmeli)
        var devUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        // Kullanıcı zaten veritabanında var mı kontrol et
        var userExists = await context.Users.AnyAsync(u => u.Id == devUserId);

        if (!userExists)
        {
            var devUser = User.Create(
                email: "testadmin@taskflowpro.local",
                firstName: "Ozay Can",
                lastName: "Kirli"
            );

            // Domain katmanında Id protected set olduğu için, Reflection ile test ID'sini zorla atıyoruz 
            // veya User modelinde Id ezilebilir. En temizi domain'i bozmamak için burada atama simülasyonu yapmak:
            typeof(Entity).GetProperty(nameof(Entity.Id))?
                .SetValue(devUser, devUserId);

            context.Users.Add(devUser);
            await context.SaveChangesAsync();
        }
    }
}