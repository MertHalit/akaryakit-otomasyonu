using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using AkaryakitOtomasyonu.Models;

namespace AkaryakitOtomasyonu.Data
{
    public static class AdminPasswordResetHelper
    {
        public static async Task ResetAdminPasswordAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");

            if (adminUser != null)
            {
                if (!adminUser.EmailConfirmed)
                {
                    adminUser.EmailConfirmed = true;
                    await userManager.UpdateAsync(adminUser);
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                var result = await userManager.ResetPasswordAsync(adminUser, token, "Admin123!");

                if (result.Succeeded)
                    Console.WriteLine("✅ Admin şifresi 'Admin123!' olarak sıfırlandı.");
                else
                {
                    Console.WriteLine("❌ Şifre sıfırlanamadı:");
                    foreach (var error in result.Errors)
                        Console.WriteLine($" - {error.Description}");
                }
            }
            else
            {
                Console.WriteLine("❗ admin@admin.com bulunamadı.");
            }
        }
    }
}
