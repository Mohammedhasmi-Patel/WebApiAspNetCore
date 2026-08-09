using api.Data;
using api.DTOS.Comment;
using api.Interfaces;
using api.Model;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository : ICommentRepository
{

    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var res = await _context.Comments.FindAsync(id);
        if (res == null)
        {
            return false;
        }

        _context.Comments.Remove(res);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.AsNoTracking().ToListAsync();
    }

    public async Task<Comment?> GetCommentByIdAsync(int id)
    {
        // throw new NotImplementedException();
        return await _context.Comments.FindAsync(id);

    }


    public async Task<Comment> UpdateAsync(UpdateCoomentDTO updateCoomentDTO)
    {
        var stock = await _context.Stocks.FindAsync(updateCoomentDTO.StockId);
        if (stock is null)
        {
            return null;
        }
        var commentModal = await _context.Comments.FindAsync(updateCoomentDTO.Id);
        if (commentModal is null)
        {
            return null;
        }

        commentModal.Title = updateCoomentDTO.Title;
        commentModal.Content = updateCoomentDTO.Content;
        await _context.SaveChangesAsync();
        return commentModal;
    }

}
