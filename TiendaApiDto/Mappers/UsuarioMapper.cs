using TiendaApiDto.Dtos;
using TiendaApiDto.Dtos.TiendaApiDto.Dtos;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Mappers
{
    public static class UsuarioMapper
    {
        public static Usuario ToEntity(this RegisterDto dto)
        {
            return new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                Rol = dto.Rol
            };
        }

        public static UsuarioDto ToDto(this Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol
            };
        }
    }
}
