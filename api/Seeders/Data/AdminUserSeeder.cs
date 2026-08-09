using api.Enum;
using api.Model;
using Microsoft.AspNetCore.Identity;

namespace api.Seeders.Data;

public static class AdminUserSeeder
{
    public static async Task SeedAsync(UserManager<AppUser> userManager, CancellationToken cancellationToken)
    {
        var adminExist = await userManager.FindByEmailAsync("adminhasmi@gmail.com");
        if (adminExist != null) return;
        var admin = new AppUser
        {
            UserName = "adminhasmi",
            Email = "adminhasmi@gmail.com",
            EmailConfirmed = true
        };

        var res = await userManager.CreateAsync(
            admin,
            "Hajju@2003#insta"
        );

        await userManager.AddToRoleAsync(
            admin,
            nameof(UserRoleEnum.Admin)
        );
    }

}
