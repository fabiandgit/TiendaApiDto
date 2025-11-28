using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using TiendaApiDto.Data;
using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;
using TiendaApiDto.Mappers;
using TiendaApiDto.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<Producto?> UpdateAsync(int id, Producto producto)
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

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<ProductoDto>> GetPagedAsync(PaginationParams pagination)
        {
            //var query = _context.Productos.Include(p => p.Ventas).AsQueryable();
            var query = _context.Productos.AsQueryable();
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var itemsDto = items.Select(p => p.ToDto()).ToList();

            return new PagedResult<ProductoDto>
            {
                Items = itemsDto,
                TotalCount = totalCount
            };
        }

        public async Task<(IEnumerable<Producto?> productos, List<string?> errores)> ImportFileExcel(IFormFile archivoExcel)
        {
            var productos = new List<Producto>();
            var errores = new List<string>();
            try
            {
                using var stream = new MemoryStream();
                await archivoExcel.CopyToAsync(stream);

                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet == null)
                    throw new Exception("La plantilla no contiene hojas");

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // encabezados en la fila 1
                {
                    var nombre = worksheet.Cells[row, 1].Text;
                    var descripcion = worksheet.Cells[row, 2].Text;
                    var precioTexto = worksheet.Cells[row, 3].Text;
                    var stockTexto = worksheet.Cells[row, 4].Text;

                    if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(precioTexto) || string.IsNullOrWhiteSpace(stockTexto))
                    {
                        errores.Add($"Fila {row}: campos obligatorios vacíos (nombre, precio o stock)");
                        continue;
                    }

                    if (!decimal.TryParse(precioTexto, out var precio))
                    {
                        errores.Add($"Fila {row}: precio inválido ('{precioTexto}')");
                        continue;
                    }

                    if (!int.TryParse(stockTexto, out var stock))
                    {
                        errores.Add($"Fila {row}: stock inválido ('{stockTexto}')");
                        continue;
                    }

                    //  Validar existencia en BD
                    var existe = await _context.Productos.AnyAsync(p => p.Nombre == nombre);
                    if (existe)
                    {
                        errores.Add($"Fila {row}: el producto '{nombre}' ya existe en la base de datos");
                        continue;
                    }


                    var producto = new Producto
                    {
                        Nombre = nombre,
                        Descripcion = descripcion,
                        Precio = precio,
                        Stock = stock
                    };

                    productos.Add(producto);
                }

                _context.Productos.AddRange(productos);
                await _context.SaveChangesAsync();

                return (productos, errores);
            }
            catch(Exception ex)
            {
                errores.Add($"Error al procesar el archivo: {ex.Message}");
                return (new List<Producto>(), errores);
            }

        }

     
    }

}