using api.Data;
using api.Model;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace api.Seeders.Data;

public static class CommentSeeder
{
    public static async Task SeedAsync(ApplicationDbContext _context,CancellationToken cancellationToken)
    {
        if (await _context.Comments.AnyAsync()) return;
        var stockIds = await _context.Stocks
            .Select(s => s.Id)
            .ToListAsync(cancellationToken);

        var userIds = await _context.Users
            .Select(u => u.Id)
            .ToListAsync(cancellationToken);

        if (stockIds.Count == 0 || userIds.Count == 0) return;

        var faker = new Faker<Comment>()
            .RuleFor(c => c.Title, f => f.Lorem.Sentence(5))
            .RuleFor(c => c.Content, f => f.Lorem.Paragraph())
            .RuleFor(c => c.CreatedOn, f => f.Date.Past(1))
            .RuleFor(c => c.CreatedOn,f => f.Date.Past(1).ToUniversalTime())
            .RuleFor(c => c.StockId, f => f.PickRandom(stockIds))
            .RuleFor(c => c.AppUserId, f => f.PickRandom(userIds));

        var comments = faker.Generate(200);

        await _context.Comments.AddRangeAsync(comments,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);


    }

}
