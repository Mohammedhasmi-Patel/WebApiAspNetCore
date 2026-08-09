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
    public DbSet<IdentityRole> Roles { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        

        builder.Entity<PortFolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));
        builder.Entity<PortFolio>()
                .HasOne(u => u.AppUser)
                .WithMany(p => p.PortFolios)
                .HasForeignKey(p => p.AppUserId);

        builder.Entity<PortFolio>()
                .HasOne(u => u.Stock)
                .WithMany(p => p.PortFolios)
                .HasForeignKey(p => p.StockId);
    }

}
