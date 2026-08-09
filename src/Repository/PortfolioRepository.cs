using api.Data;
using api.DTOS.Portfolio;
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

    public async Task<PortFolio> AddPortFolioAsync(PortFolio portFolio)
    {
        await _context.PortFolios.AddAsync(portFolio);
        await _context.SaveChangesAsync();
        return portFolio;
    }

    public Task<PortFolio?> DeletePortfolioAsync(int stockId, string appUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<PortFolio?> GetByStockId(int stockId, string appUserId)
    {
        // throw new NotImplementedException();
        var res = await _context.PortFolios.FirstOrDefaultAsync(x => x.AppUserId == appUserId && x.StockId == stockId);
        return res;
    }

    public Task<List<Stock>> GetUserPortfolio(AppUser appUser)
    {
        return _context.PortFolios.Where(u => u.AppUserId == appUser.Id)
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

    public async Task<List<Stock>> GetUserPortfolio(string userId)
    {
        var portfolios = await _context.PortFolios
                            .Where(p => p.AppUserId == userId)
                            .Include(p => p.Stock)
                            .ToListAsync();

        return portfolios.ConvertAll(p => 
        {
            p.Stock.PortFolios = new List<PortFolio> { p };
            return p.Stock;
        });
    }


    public async Task<PortFolio> UpdateAsync(PortFolio portFolio, AddStockPortfolioRequestDTO addStockPortfolioRequestDTO)
    {
        portFolio.Quantity += addStockPortfolioRequestDTO.Quantity;
        await _context.SaveChangesAsync();
        return portFolio;
    }

}
