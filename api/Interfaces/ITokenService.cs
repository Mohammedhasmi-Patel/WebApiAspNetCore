using api.Model;

namespace api.Interfaces;

public interface ITokenService
{
    public Task<string> CreateJwtTokenService(AppUser appUser);
}
