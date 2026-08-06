using api.DTOS.Comment;
using api.Model;

namespace api.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment comment)
    {
        return new CommentDto()
        {
            Id = comment.Id,
            Content = comment.Content,
            Title = comment.Title,
            CreatedOn = comment.CreatedOn,
            StockId = comment.StockId
        };
    }

    public static Comment ToCommentFromCreateDto(this CreateCommentDTO createCommentDTO)
    {
        return new Comment()
        {
            Title = createCommentDTO.Title,
            Content = createCommentDTO.Content,
            CreatedOn = createCommentDTO.CreatedOn,
            StockId = createCommentDTO.StockId
        };
    }

}
