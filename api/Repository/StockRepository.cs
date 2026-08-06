using api.Data;
using api.DTOS.Stock;
using api.Helper;
using api.Interfaces;
using api.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Stock> CreateAsync(Stock stockModal)
    {
        var result = await _context.Stocks.AddAsync(stockModal);
        await _context.SaveChangesAsync();
        return stockModal;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var task = await _context.Stocks.FindAsync(id);
        if (task is null)
        {
            return null;
        }

        _context.Stocks.Remove(task);
        await _context.SaveChangesAsync();
        return task;
    }


    public async Task<IEnumerable<Stock>> GetAllAsync(QueryObject queryObject)
    {
        var query = _context.Stocks.AsQueryable();

        if (queryObject.CompanyName != null)
        {
            query = query.Where(s => s.CompanyName != null && s.CompanyName.ToLower().Contains(queryObject.CompanyName.ToLower()));
        }

        if (queryObject.Symbol != null)
        {
            query = query.Where( s => s.Symbol!=null && s.Symbol.ToLower().Contains(queryObject.Symbol.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
        {
            switch (queryObject.SortBy)
            {
                case "Symbol":
                    query = queryObject.IsDescending ? query.OrderByDescending(s => s.Symbol) : query.OrderBy(s => s.Symbol);
                    break;
            }
        }

        int skipNumber = (queryObject.PageNumber-1) * queryObject.PageSize;
        return await query.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        var result = await _context.Stocks.FindAsync(id);
        return result;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockRequestDto)
    {
        var existingStock = await _context.Stocks.FindAsync(id);

        if (existingStock == null)
        {
            return null;
        }

        existingStock.Symbol = stockRequestDto.Symbol;
        existingStock.CompanyName = stockRequestDto.CompanyName;
        existingStock.Purchase = stockRequestDto.Purchase;
        existingStock.LastDiv = stockRequestDto.LastDiv;
        existingStock.Industry = stockRequestDto.Industry;
        existingStock.MarketCap = stockRequestDto.MarketCap;

        await _context.SaveChangesAsync();
        return existingStock;
    }
}
