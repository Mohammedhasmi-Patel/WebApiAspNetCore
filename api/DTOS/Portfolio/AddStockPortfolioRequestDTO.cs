using System.ComponentModel.DataAnnotations;

namespace api.DTOS.Portfolio;

public class AddStockPortfolioRequestDTO
{
    [Required]
    public int StockId { get; set; }
}
