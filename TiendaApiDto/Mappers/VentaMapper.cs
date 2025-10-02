using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Mappers
{
    public static class VentaMapper
    {
        public static VentaDto ToDto(this Venta venta)
        {
            return new VentaDto
            {
                Id = venta.Id,
                ProductoId = venta.ProductoId,
                ProductoNombre = venta.Producto?.Nombre ?? "No disponible",
                EmpleadoId = venta.EmpleadoId,
                EmpleadoNombre = $"{venta.Empleado?.Nombre} {venta.Empleado?.Apellido}",
                Cantidad = venta.Cantidad,
                Total = venta.Total,
                FechaVenta = venta.FechaVenta

            };
        }

        public static Venta ToEntity(this VentaDto dto)
        {
            return new Venta
            {
                Id = dto.Id,
                ProductoId = dto.ProductoId,
                EmpleadoId = dto.EmpleadoId,
                Cantidad = dto.Cantidad,
                Total = dto.Total,
                FechaVenta = dto.FechaVenta
            };
        }

        public static void UpdateEntity(this Venta venta, VentaDto dto)
        {
            if (venta == null || dto == null) return;

            venta.ProductoId = dto.ProductoId;
            venta.EmpleadoId = dto.EmpleadoId;
            venta.Cantidad = dto.Cantidad;
            venta.Total = dto.Total;
            venta.FechaVenta = dto.FechaVenta;
        }
    }
}