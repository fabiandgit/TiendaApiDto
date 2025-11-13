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
        Task<Producto?> UpdateAsync(int id, Producto producto);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<ProductoDto>> GetPagedAsync(PaginationParams pagination);
    }
}
