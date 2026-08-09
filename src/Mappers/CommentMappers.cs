using api.DTOS.Comment;
using api.Model;

namespace api.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment comment, string userId)
    {
        return new CommentDto()
        {
            Id = comment.Id,
            Content = comment.Content,
            Title = comment.Title,
            UserId = userId,
            CreatedOn = comment.CreatedOn,
            StockId = comment.StockId
        };
    }

    public static Comment ToCommentFromCreateDto(this CreateCommentDTO createCommentDTO, string userId)
    {
        return new Comment()
        {
            Title = createCommentDTO.Title,
            Content = createCommentDTO.Content,
            AppUserId = userId,
            CreatedOn = DateTime.UtcNow,
            StockId = createCommentDTO.StockId
        };
    }

}
