using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TiendaApiDto.Dtos;

namespace TiendaApiDto.Entities
{
    public class Venta
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;

        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor que 0")]
        [Precision(18, 2)]
        public decimal Total { get; set; }

        public DateTime FechaVenta { get; set; } = DateTime.Now;
    }
}