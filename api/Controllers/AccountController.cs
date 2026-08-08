using api.DTOS.Auth;
using api.Enum;
using api.Interfaces;
using api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/auth")]
public class AccountControllerController : ControllerBase
{

    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;


    public AccountControllerController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserDtoRequest registerUserDtoRequest)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUserNameOrEmailExist = await _userManager.Users.AnyAsync(u => u.UserName == registerUserDtoRequest.UserName || u.Email == registerUserDtoRequest.Email);
            if (isUserNameOrEmailExist)
            {
                return Conflict("User Already exists.");
            }

            var applicationUser = new AppUser()
            {
                Email = registerUserDtoRequest.Email,
                UserName = registerUserDtoRequest.UserName
            };

            var result = await _userManager.CreateAsync(applicationUser, registerUserDtoRequest.Password);
            if (result.Succeeded)
            {
                var response = await _userManager.AddToRoleAsync(applicationUser, nameof(UserRoleEnum.User));
                if (response.Succeeded)
                {
                    var userResponse = new RegisterUserResponse()
                    {
                        Username = applicationUser.UserName,
                        Email = applicationUser.Email,
                        Token = _tokenService.CreateJwtTokenService(applicationUser)
                    };
                    return Ok(userResponse);
                }
                else
                {
                    return StatusCode(500, response.Errors);
                }
            }
            else
            {
                return StatusCode(500, result.Errors);
            }

        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost]
    [Route("login")]

    public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserRequestDTO loginUserRequestDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingUser = await _userManager.FindByEmailAsync(loginUserRequestDTO.Email);
            if (existingUser == null)
            {
                return Unauthorized("Invalid Credentials");
            }

            var isPasswordMatch = await _signInManager.CheckPasswordSignInAsync(existingUser, loginUserRequestDTO.Password,false);
            if (!isPasswordMatch.Succeeded)
            {
                return Unauthorized("Invalid Credentials");
            }

            var userResponse = new RegisterUserResponse()
            {
                Username = existingUser.UserName,
                Email = existingUser.Email,
                Token = _tokenService.CreateJwtTokenService(existingUser)
            };

            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
