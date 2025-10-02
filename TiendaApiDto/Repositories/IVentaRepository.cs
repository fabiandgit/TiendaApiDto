using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Repositories
{
    public interface IVentaRepository
    {
        Task<IEnumerable<Venta>> GetAllAsync();
        Task<Venta?> GetByIdAsync(int id);
        Task<Venta> AddAsync(Venta venta);
        Task<Venta?> UpdateAsync(long id, Venta venta);
        Task<bool> DeleteAsync(long id);

    }
}