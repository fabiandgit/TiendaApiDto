using System.Threading.Tasks;
using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Repositories
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(int id);
        Task<Producto> AddAsync(Producto producto);
        Task<Producto?> UpdateAsync(long id, Producto producto);
        Task<bool> DeleteAsync(long id);
    }
}
