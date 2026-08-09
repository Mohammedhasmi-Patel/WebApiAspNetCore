namespace api.DTOS.Comment;

public class CreateCommentDTO
{

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int? StockId { get; set; }

}
