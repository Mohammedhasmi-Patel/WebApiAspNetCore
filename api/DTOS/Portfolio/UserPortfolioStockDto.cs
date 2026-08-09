namespace api.DTOS.Portfolio;

public class UserPortfolioStockDto
{

    public int StockId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal Purchase { get; set; }
    public decimal LastDiv { get; set; }
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; }
    public decimal UserQuantity { get; set; }
    
}
