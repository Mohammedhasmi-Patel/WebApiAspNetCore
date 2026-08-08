using api.Enum;
using api.Model;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Seeders.Data;

public static class AppUserSeeder
{
    public static async Task SeedAsync(UserManager<AppUser> userManager,CancellationToken cancellationToken)
    {
        if (await userManager.Users.AnyAsync())
            return;

        var users = new Faker<AppUser>()
            .RuleFor(x => x.UserName, f => f.Internet.Email())
            .RuleFor(x => x.Email, (f, u) => u.UserName)
            .RuleFor(x => x.EmailConfirmed, _ => true)
            .Generate(100);

        foreach (var user in users)
        {
            var result = await userManager.CreateAsync(
                user,
                "Test@123456"
            );

            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(
                        ", ",
                        result.Errors.Select(x => x.Description)
                    )
                );
            }

            var roleResult = await userManager.AddToRoleAsync(
                user,
                nameof(UserRoleEnum.User)
            );

            if (!roleResult.Succeeded)
            {
                throw new Exception(
                    string.Join(
                        ", ",
                        roleResult.Errors.Select(x => x.Description)
                    )
                );
            }
        }
    }
}