using AutoMapper;
using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Mappers
{
    public class TiendaProfile : Profile
    {
        public TiendaProfile()
        {
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoDto>().ReverseMap();
            CreateMap<Venta, VentaDto>().ReverseMap();
        }
    }
}