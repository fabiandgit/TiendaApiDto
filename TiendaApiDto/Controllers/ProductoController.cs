using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;
using TiendaApiDto.Mappers;
using TiendaApiDto.Repositories;

namespace TiendaApiDto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoRepository _repository;

        public ProductoController(IProductoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll()
        {
            var productos = await _repository.GetAllAsync();
            var productosDto = productos.Select(p => p.ToDto());
            return Ok(productosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            var producto = await _repository.GetByIdAsync(id);
            if (producto == null) return NotFound();

            return Ok(producto.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create([FromBody] ProductoDto dto)
        {
            var producto = dto.ToEntity();
            var creado = await _repository.AddAsync(producto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductoDto>> Update(int id, [FromBody] ProductoDto dto)
        {
            if (id != dto.Id) return BadRequest("El ID no coincide");

            var productoExistente = await _repository.GetByIdAsync(id);
            if (productoExistente == null) return NotFound();

            productoExistente.UpdateEntity(dto);
            var actualizado = await _repository.UpdateAsync(id, productoExistente);
            return Ok(actualizado?.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var eliminado = await _repository.DeleteAsync(id);
            if (!eliminado) return NotFound();

            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<ProductoDto>>> GetPaged([FromQuery] PaginationParams pagination)
        {
            var result = await _repository.GetPagedAsync(pagination);
            return Ok(result);
        }

        [HttpGet("template")]
        public IActionResult DescargarPlantilla()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Plantillas", "PlantillaProductos.xlsx");

            if (!System.IO.File.Exists(filePath))
                return NotFound("Archivo no encontrado");

            var MIME = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return PhysicalFile(filePath, MIME, "PlantillaProductos.xlsx");
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportExcel(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
                return BadRequest("Archivo no válido");

            var (productos, errores) = await _repository.ImportFileExcel(archivoExcel);
            var productosDto = productos.Select(p => p.ToDto());

            return Ok(new
            {
                mensaje = errores.Any() ? "Importación completada con advertencias" : "Importación exitosa",
                cantidad = productosDto.Count(),
                productos = productosDto,
                errores = errores
            });
        }
    }
}