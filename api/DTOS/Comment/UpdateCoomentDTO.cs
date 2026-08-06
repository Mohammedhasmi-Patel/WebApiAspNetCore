namespace api.DTOS.Comment;

public class UpdateCoomentDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int? StockId { get; set; }

}
