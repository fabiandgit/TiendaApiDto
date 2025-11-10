using System.ComponentModel.DataAnnotations;

namespace TiendaApiDto.Dtos
{
    namespace TiendaApiDto.Dtos
    {
        public class UsuarioDto
        {
            public int Id { get; set; }
            public string NombreUsuario { get; set; } = string.Empty;
            public string Rol { get; set; } = "Empleado";
        }
    }

}
