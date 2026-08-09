using api.DTOS.Portfolio;
using api.Model;

namespace api.Interfaces;

public interface IPortfolioRepository
{
    public Task<List<Stock>> GetUserPortfolio(AppUser appUser);
    public Task<PortFolio> AddPortFolioAsync(PortFolio portFolio);

    public Task<PortFolio> UpdateAsync(PortFolio portFolio,AddStockPortfolioRequestDTO addStockPortfolioRequestDTO);

    public Task<PortFolio?> GetByStockId(int stockId, string appUserId);
    public Task<PortFolio?> DeletePortfolioAsync(int stockId, string appUserId);
    Task<List<Stock>> GetUserPortfolio(string userId);


}
