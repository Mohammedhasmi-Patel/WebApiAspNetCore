using api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<PortFolio> PortFolios { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var adminRoleId = "7f8c9a10-1234-4567-8901-123456789abc";
        var userRoleId = "8a9b0c1d-2345-6789-0123-456789abcdef";

        List<IdentityRole> roles =
        [
                new IdentityRole
                    {
                        Id = adminRoleId,
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                                ConcurrencyStamp = "11111111-1111-1111-1111-111111111111"

                    },
                new IdentityRole
                    {
                        Id = userRoleId,
                        Name = "User",
                        NormalizedName = "USER",
                        ConcurrencyStamp = "22222222-2222-2222-2222-222222222222"
                    }
        ];

        builder.Entity<PortFolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));
        builder.Entity<PortFolio>()
                .HasOne(u => u.AppUser)
                .WithMany(p => p.PortFolios)
                .HasForeignKey(p => p.AppUserId);

        builder.Entity<PortFolio>()
                .HasOne(u => u.Stock)
                .WithMany(p => p.PortFolios)
                .HasForeignKey(p => p.StockId);




        builder.Entity<IdentityRole>().HasData(roles);
    }

}
