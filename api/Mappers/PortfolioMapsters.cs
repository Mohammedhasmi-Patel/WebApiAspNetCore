using api.DTOS.Portfolio;
using api.Model;

namespace api.Mappers;

public static class PortfolioMapsters
{
    public static PortFolio ToPortfolio(this AddStockPortfolioRequestDTO addStockPortfolioRequestDTO,string userId)
    {
        return new PortFolio()
        {
            AppUserId = userId,
            StockId = addStockPortfolioRequestDTO.StockId,
            Quantity = addStockPortfolioRequestDTO.Quantity
        };
    }
    public static PortfolioResponseDTO ToPortfolioResponse(this PortFolio portFolio)
    {
        return new PortfolioResponseDTO()
        {
            StockId = portFolio.StockId,
            UserId = portFolio.AppUserId
        };
    }  

}
