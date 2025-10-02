using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Mappers
{
    public static class EmpleadoMapper
    {
        public static EmpleadoDto ToDto(this Empleado empleado)
        {
            return new EmpleadoDto
            {
                Id = empleado.Id,
                Nombre = empleado.Nombre,
                Apellido = empleado.Apellido,
                Edad = empleado.Edad,
                Celular = empleado.Celular,
                Email = empleado.Email,
                Cargo = empleado.Cargo,
                Ventas = empleado.Ventas?.Select(v => v.ToDto()).ToList()
            };
        }

        public static Empleado ToEntity(this EmpleadoDto dto)
        {
            return new Empleado
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Edad = dto.Edad,
                Celular = dto.Celular,
                Email = dto.Email,
                Cargo = dto.Cargo
            };
        }

        public static void UpdateEntity(this Empleado empleado, EmpleadoDto dto)
        {
            if (empleado == null || dto == null) return;

            empleado.Nombre = dto.Nombre;
            empleado.Apellido = dto.Apellido;
            empleado.Edad = dto.Edad;
            empleado.Celular = dto.Celular;
            empleado.Email = dto.Email;
            empleado.Cargo = dto.Cargo;
        }
    }
}