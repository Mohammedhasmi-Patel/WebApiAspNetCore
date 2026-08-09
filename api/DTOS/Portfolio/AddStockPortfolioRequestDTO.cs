using System.ComponentModel.DataAnnotations;

namespace api.DTOS.Portfolio;

public class AddStockPortfolioRequestDTO
{
    [Required]
    public int StockId { get; set; }

    [Required]
    [Range(1,int.MaxValue,ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }
}
