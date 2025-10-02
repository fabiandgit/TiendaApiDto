using System.ComponentModel.DataAnnotations;

namespace TiendaApiDto.Dtos
{
    public class VentaDto
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;

        public int EmpleadoId { get; set; }
        public string EmpleadoNombre { get; set; } = string.Empty;

        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaVenta { get; set; }
    }
}