namespace api.Model;

public class PortFolio
{
    public string AppUserId { get; set; }
    public int StockId { get; set; }

    public AppUser AppUser { get; set; } = null!;
    public Stock Stock { get; set; } = null!;

}
