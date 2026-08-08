using api.Data;
using api.Model;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace api.Seeders.Data;

public static class StockSeeder
{
    public static async Task SeedAsync(ApplicationDbContext dbContext , CancellationToken cancellationToken)
    {
        if (await dbContext.Stocks.AnyAsync(cancellationToken)) return;

        var symbols = new[]
                {
                    "AAPL",
                    "MSFT",
                    "GOOGL",
                    "AMZN",
                    "META",
                    "TSLA",
                    "NVDA",
                    "NFLX",
                    "ORCL",
                    "IBM"
                };

        var faker = new Faker<Stock>()
                .RuleFor(s => s.Symbol,f => f.PickRandom(symbols))
                .RuleFor(s => s.CompanyName, f => f.Company.CompanyName())
                .RuleFor(s => s.Purchase, f => f.Finance.Amount(10, 5000))
                .RuleFor(s => s.LastDiv, f => f.Finance.Amount(0, 100))
                .RuleFor(s => s.Industry, f => f.Commerce.Department())
                .RuleFor(s => s.MarketCap, f => f.Random.Long(1_000_000, 1_000_000_000_000));

        var stocks = faker.Generate(1200);
        await dbContext.Stocks.AddRangeAsync(stocks, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

    }

}
