using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TiendaApiDto.Dtos;

public class ProductoDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Precision(18, 2)]

    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El stock es obligatorio")]
    public int Stock { get; set; }

    public List<VentaDto>? Ventas { get; set; }
}