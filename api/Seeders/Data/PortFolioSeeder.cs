using api.Data;
using api.Model;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace api.Seeders.Data;

public static class PortFolioSeeder
{
    public static async Task SeedAsync(ApplicationDbContext _context, CancellationToken cancellationToken)
    {
        if (await _context.PortFolios.AnyAsync()) return;

        var stockIds = await _context.Stocks
                            .Select(s => s.Id)
                            .ToListAsync(cancellationToken);

        var userIds = await _context.Users
                    .Select(u => u.Id)
                    .ToListAsync(cancellationToken);

        if (stockIds.Count == 0 || userIds.Count == 0) return;
        var faker = new Faker<PortFolio>()
                    .RuleFor(p => p.AppUserId, f => f.PickRandom(userIds))
                    .RuleFor(p => p.StockId, f => f.PickRandom(stockIds));


        var portfolios = faker.Generate(100);
        await _context.PortFolios.AddRangeAsync(portfolios,cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

    }

}
