using api.Data;
using api.Interfaces;
using api.Model;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly ApplicationDbContext _context;

    public PortfolioRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public  Task<List<Stock>> GetUserPortfolio(AppUser appUser)
    {
        return  _context.PortFolios.Where(u => u.AppUserId == appUser.Id)
                                .Select(stock => new Stock()
                                {
                                    Id = stock.Stock.Id,
                                    Symbol = stock.Stock.Symbol,
                                    CompanyName = stock.Stock.CompanyName,
                                    Purchase = stock.Stock.Purchase,
                                    LastDiv = stock.Stock.Purchase,
                                    Industry = stock.Stock.Industry,
                                    MarketCap = stock.Stock.MarketCap
                                }).ToListAsync();

    }

}
