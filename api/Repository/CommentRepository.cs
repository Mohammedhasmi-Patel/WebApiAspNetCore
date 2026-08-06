using api.Data;
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


    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.AsNoTracking().ToListAsync();
    }

}
