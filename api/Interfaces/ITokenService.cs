using api.Model;

namespace api.Interfaces;

public interface ITokenService
{
    public string CreateJwtTokenService(AppUser appUser);
}
