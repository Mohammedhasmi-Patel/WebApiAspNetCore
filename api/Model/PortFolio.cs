using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model;

public class PortFolio
{
    public string AppUserId { get; set; } = null!;
    public int StockId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    public AppUser AppUser { get; set; } = null!;
    public Stock Stock { get; set; } = null!;

}
