using Microsoft.EntityFrameworkCore;
using System;
using TiendaApiDto.Data;
using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;
using TiendaApiDto.Mappers;

namespace TiendaApiDto.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly TiendaApiContext _context;

        public VentaRepository(TiendaApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Venta>> GetAllAsync()
        {
            return await _context.Ventas
                .Include(v => v.Producto)
                .Include(v => v.Empleado)
                .ToListAsync();
        }

        public async Task<Venta?> GetByIdAsync(int id)
        {
            return await _context.Ventas
                .Include(v => v.Producto)
                .Include(v => v.Empleado)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Venta> AddAsync(Venta venta)
        {
             _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();
            return venta;
        }

        public async Task<Venta?> UpdateAsync(int id, Venta venta)
        {
            var existente = await _context.Ventas.FindAsync(id);
            if (existente == null) return null;

            existente.ProductoId = venta.ProductoId;
            existente.EmpleadoId = venta.EmpleadoId;
            existente.Cantidad = venta.Cantidad;
            existente.Total = venta.Total;
            existente.FechaVenta = venta.FechaVenta;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return false;

            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<VentaDto>> GetPagedAsync(PaginationParams pagination)
        {
            try
            {
                var query = _context.Ventas.Include(x => x.Producto)
                        .Include(x => x.Empleado)
                        .AsQueryable();

                var totalCount = await query.CountAsync();

                var items = await query.
                                  Skip((pagination.PageNumber - 1) * pagination.PageSize).
                                  Take(pagination.PageSize)
                                  .ToListAsync();

                var itemsDto = items.Select((v) => v.ToDto()).ToList();

                return new PagedResult<VentaDto>
                {
                    Items = itemsDto,
                    TotalCount = totalCount
                };
            }
            catch (Exception ex) {
                return null;
            }
        }
    }
}