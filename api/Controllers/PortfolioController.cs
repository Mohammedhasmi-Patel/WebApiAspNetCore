using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Interfaces;
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

    public PortfolioControllerController(UserManager<AppUser> userManager,IPortfolioRepository portfolioRepository)
    {
        _userManager = userManager;
        _portfolioRepository = portfolioRepository;
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

}
