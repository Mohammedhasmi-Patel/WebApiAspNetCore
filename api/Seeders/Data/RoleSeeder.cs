using System;


using api.Data;
using api.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace api.Seeders.Data;



public static class RoleSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Roles.Any())
            return;

        var roles = System.Enum.GetValues<UserRoleEnum>()
            .Select(role => new IdentityRole
            {
                Name = role.ToString(),
                NormalizedName = role.ToString().ToUpperInvariant()
            })
            .ToList();

        context.Roles.AddRange(roles);
        context.SaveChanges();
    }


    public static async Task SeedAsync(ApplicationDbContext _context,CancellationToken cancellationToken)
    {
        if (await _context.Roles.AnyAsync()) return;
        var roles = System.Enum.GetValues<UserRoleEnum>()
        .Select(role => new IdentityRole
        {
            Name = role.ToString(),
            NormalizedName = role.ToString().ToUpper()
        })
        .ToList();

        await _context.Roles.AddRangeAsync(roles, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }   
}
