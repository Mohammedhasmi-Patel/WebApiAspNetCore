using System.ComponentModel.DataAnnotations;

namespace api.DTOS.Stock;

public class CreateStockRequestDto
{
    [Required]
    public string Symbol { get; set; } = string.Empty;
    [Required]
    [MinLength(3, ErrorMessage = "The companyname must be atleast 3 characters long ")]
    public string CompanyName { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "The purchase must be greater than 0 ")]
    public decimal Purchase { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "The lastDiv must be greater than 0 ")]
    public decimal LastDiv { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "The totalQuantity must be greater than 0 ")]
    public decimal TotalQuantity { get; set; }

    [Required]
    public string Industry { get; set; } = string.Empty;

    [Required]
    [Range(0, long.MaxValue, ErrorMessage = "The marketCap must be greater than 0 ")]
    public long MarketCap { get; set; }

}
