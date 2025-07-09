using AkaryakitOtomasyonu.Models;
using Microsoft.AspNetCore.Identity;

public static class AdminPasswordResetHelper
{
    public static async Task ResetAdminPasswordAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<User>>();

        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser != null)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
            var result = await userManager.ResetPasswordAsync(adminUser, token, "Admin123!");

            if (result.Succeeded)
            {
                Console.WriteLine("✅ Admin şifresi başarıyla güncellendi: Admin123!");
            }
            else
            {
                Console.WriteLine("❌ Şifre güncellenemedi:");
                foreach (var error in result.Errors)
                    Console.WriteLine($"- {error.Description}");
            }
        }
        else
        {
            Console.WriteLine("❌ 'admin' kullanıcısı bulunamadı.");
        }
    }
}
