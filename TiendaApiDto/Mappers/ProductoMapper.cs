using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Mappers
{
    public static class ProductoMapper
    {
        public static ProductoDto ToDto(this Producto producto)
        {
            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock
            };
        }

        public static Producto ToEntity(this ProductoDto dto)
        {
            return new Producto
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock
            };
        }

        public static void UpdateEntity(this Producto producto, ProductoDto dto)
        {
            if (producto == null || dto == null) return;

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;
        }

    }
}