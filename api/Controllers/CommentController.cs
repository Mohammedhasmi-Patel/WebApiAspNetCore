using api.DTOS.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentControllerController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;

    public CommentControllerController(ICommentRepository commentRepository,IStockRepository stockRepository)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var comments = await _commentRepository.GetAllAsync();
        var commentDto = comments.Select(c => c.ToCommentDto()).ToList();
        return Ok(commentDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCommentDTO createCommentDTO)
    {
        if (createCommentDTO.StockId == null)
        {
            return NotFound("Stock not found.");
        }

        var stock = await _stockRepository.GetByIdAsync(createCommentDTO.StockId ?? 0);
        if (stock is null)
        {
            return NotFound("Stock not found.");
        }
        var stockModal = createCommentDTO.ToCommentFromCreateDto();
        var createdComment = await _commentRepository.CreateAsync(stockModal);
        var response = createdComment.ToCommentDto();
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var response = await _commentRepository.DeleteAsync(id);
        if (!response)
        {
            return NotFound("Invalid comment");
        }

        return Ok("Comment deleted successfully.");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateCoomentDTO updateCoomentDTO)
    {
        var response = await _commentRepository.UpdateAsync(updateCoomentDTO);
        if (response is null)
        {
            return NotFound("Invalid comment");
        }

        return Ok("Comment updated successfully.");
    }
}
