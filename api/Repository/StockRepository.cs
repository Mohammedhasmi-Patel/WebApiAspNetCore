using api.Data;
using api.DTOS.Stock;
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


    public async Task<IEnumerable<Stock>> GetAllAsync()
    {
         return await _context.Stocks.ToListAsync();
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
