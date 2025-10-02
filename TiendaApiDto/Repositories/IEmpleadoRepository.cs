using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Repositories
{
    public interface IEmpleadoRepository 
    {
        Task<IEnumerable<Empleado>> GetAllAsync();
        Task<Empleado?> GetByIdAsync(int id);
        Task<Empleado> AddAsync(Empleado empleado);
        Task<Empleado?> UpdateAsync(long id, Empleado empleado);
        Task<bool> DeleteAsync(long id);

    }
}
