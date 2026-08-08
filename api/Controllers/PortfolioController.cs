using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.DTOS.Portfolio;
using api.Interfaces;
using api.Mappers;
using api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/portfolios")]
[Authorize]
public class PortfolioControllerController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPortfolioRepository _portfolioRepository;
    //IStockRepository
    private readonly IStockRepository _stockRepository;


    public PortfolioControllerController(UserManager<AppUser> userManager,IPortfolioRepository portfolioRepository,IStockRepository stockRepository)
    {
        _userManager = userManager;
        _portfolioRepository = portfolioRepository;
        _stockRepository = stockRepository;
    }
    [HttpGet]
    public async Task<IActionResult> GetUserPortfolio()
    {
        try
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email is null)
        {
            return Unauthorized("Unauthorized");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Unauthorized("Invalid token");
        }

            var response = await _portfolioRepository.GetUserPortfolio(user);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddPortfolio(AddStockPortfolioRequestDTO addStockPortfolioRequestDTO)
    {
        try
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized("Invalid token");
            }

            var stock = await _stockRepository.GetByIdAsync(addStockPortfolioRequestDTO.StockId);
            if (stock == null)
            {
                return BadRequest("Stock not found.");
            }

            var portfolioModal = addStockPortfolioRequestDTO.ToPortfolio(user.Id);
            var res = await _portfolioRepository.AddPortFolioAsync(portfolioModal);
            return Ok(new
            {
                StockId = res.StockId,
                UserId = res.AppUserId
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}
