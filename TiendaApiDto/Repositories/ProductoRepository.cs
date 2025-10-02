using Microsoft.EntityFrameworkCore;
using System;
using TiendaApiDto.Data;
using TiendaApiDto.Entities;
using TiendaApiDto.Repositories;

namespace TiendaApiDto.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly TiendaApiContext _context;

        public ProductoRepository(TiendaApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos
                .Include(p => p.Ventas) // si quieres incluir ventas relacionadas
                .ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.Ventas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Producto> AddAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto?> UpdateAsync(long id, Producto producto)
        {
            var existente = await _context.Productos.FindAsync(id);
            if (existente == null) return null;

            existente.Nombre = producto.Nombre;
            existente.Descripcion = producto.Descripcion;
            existente.Precio = producto.Precio;
            existente.Stock = producto.Stock;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}