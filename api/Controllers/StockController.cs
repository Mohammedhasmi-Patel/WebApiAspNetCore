using api.Data;
using api.DTOS.Stock;
using api.Helper;
using api.Interfaces;
using  api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stocks")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IStockRepository _stockRepository;
    public StockController(ApplicationDbContext context,IStockRepository stockRepository)
    {
        _context = context;
        _stockRepository = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        var stocks = await _stockRepository.GetAllAsync(query);
        return Ok(stocks);
    }


    [HttpGet("{id}", Name = "GetStockById")]
    public async Task<IActionResult> GetStockByIdAsync([FromRoute] int id)
    {
        var stock = await _context.Stocks.Include(s => s.Cooments).FirstOrDefaultAsync(s => s.Id == id);
        if (stock == null)
        {
            return NotFound();
        }
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto request)
    {
        var stockModal = request.ToStockFromCreateDto();
        await _context.Stocks.AddAsync(stockModal);
        await _context.SaveChangesAsync();
        return CreatedAtRoute("GetStockById", new { id = stockModal.Id }, stockModal.ToStockDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockRequest)
    {
        var stockModal = await _context.Stocks.FindAsync(id);
        if (stockModal is null)
        {
            return NotFound();
        }

        stockModal.Symbol = updateStockRequest.Symbol;
        stockModal.CompanyName = updateStockRequest.CompanyName;
        stockModal.Purchase = updateStockRequest.Purchase;
        stockModal.LastDiv = updateStockRequest.LastDiv;
        stockModal.Industry = updateStockRequest.Industry;
        stockModal.MarketCap = updateStockRequest.MarketCap;

        await _context.SaveChangesAsync();
        return Ok(stockModal.ToStockDto());

    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModal = await _context.Stocks.FindAsync(id);
        if (stockModal == null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stockModal);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}
