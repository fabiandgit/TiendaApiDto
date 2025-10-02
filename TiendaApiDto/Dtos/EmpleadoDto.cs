using System.ComponentModel.DataAnnotations;

namespace TiendaApiDto.Dtos
{
    public class EmpleadoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La edad es obligatoria")]
        [Range(18, int.MaxValue, ErrorMessage = "Debe ser mayor de 18")]
        public int Edad { get; set; }

        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(15)]
        [Phone(ErrorMessage = "El número de celular no es válido")]
        public string Celular { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cargo es obligatorio")]
        public string Cargo { get; set; } = string.Empty;

        public List<VentaDto>? Ventas { get; set; }
    }
}