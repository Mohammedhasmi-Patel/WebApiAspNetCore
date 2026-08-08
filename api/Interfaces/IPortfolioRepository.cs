using api.Model;

namespace api.Interfaces;

public interface IPortfolioRepository
{
    public Task<List<Stock>> GetUserPortfolio(AppUser appUser);
}
