using System.Security.Claims;
using api.DTOS.Comment;
using api.Enum;
using api.Interfaces;
using api.Mappers;
using api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/comments")]
[Authorize]
public class CommentControllerController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    private readonly UserManager<AppUser> _userManager;

    public CommentControllerController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
        _userManager = userManager;
    }
    [HttpGet]
    [Authorize(Roles = nameof(UserRoleEnum.Admin))]
    public async Task<IActionResult> GetAllAsync()
    {
        var comments = await _commentRepository.GetAllAsync();
        var commentDto = comments.Select(c => c.ToCommentDto(c.AppUserId)).ToList();
        return Ok(commentDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCommentDTO createCommentDTO)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser is null)
        {
            return Unauthorized("User not found.");
        }

        if (createCommentDTO.StockId == null)
        {
            return NotFound("Stock not found.");
        }

        var stock = await _stockRepository.GetByIdAsync(createCommentDTO.StockId ?? 0);
        if (stock is null)
        {
            return NotFound("Stock not found.");
        }
        var stockModal = createCommentDTO.ToCommentFromCreateDto(appUser.Id);
        var createdComment = await _commentRepository.CreateAsync(stockModal);
        var response = createdComment.ToCommentDto(appUser.Id);
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
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser is null)
        {
            return Unauthorized("User not found.");
        }

        if (updateCoomentDTO.StockId == null)
        {
            return NotFound("Stock not found.");
        }

        // check the current user update is actual associated with that comment 
        var comment = await _commentRepository.GetCommentByIdAsync(updateCoomentDTO.Id);
        if (comment is null)
        {
            return NotFound("Comment not found.");
        }
        if (comment.AppUserId != appUser.Id)
        {
            return Unauthorized("You are not authorized to update this comment.");
        }

        var stock = await _stockRepository.GetByIdAsync(updateCoomentDTO.StockId ?? 0);
        if (stock is null)
        {
            return NotFound("Stock not found.");
        }
        var response = await _commentRepository.UpdateAsync(updateCoomentDTO);
        if (response is null)
        {
            return NotFound("Invalid comment");
        }

        return Ok("Comment updated successfully.");
    }
}
