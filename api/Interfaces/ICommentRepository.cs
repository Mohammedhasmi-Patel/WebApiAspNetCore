using api.DTOS.Comment;
using api.Model;

namespace api.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
    Task<Comment> CreateAsync(Comment comment);
}
