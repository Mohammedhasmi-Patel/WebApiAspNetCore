using api.DTOS.Stock;
using api.Helper;
using api.Model;

namespace api.Interfaces;

public interface IStockRepository
{
    Task<IEnumerable<Stock>> GetAllAsync(QueryObject queryObject);
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock> CreateAsync(Stock stockModal);
    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockRequestDto);
    Task<Stock?> DeleteAsync(int id);
}
