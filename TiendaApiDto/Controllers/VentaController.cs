using Microsoft.AspNetCore.Mvc;
using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;
using TiendaApiDto.Mappers;
using TiendaApiDto.Repositories;

namespace TiendaApiDto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly IVentaRepository _repository;

        public VentaController(IVentaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetAll()
        {
            var ventas = await _repository.GetAllAsync();
            return Ok(ventas.Select(v => v.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDto>> GetById(int id)
        {
            var venta = await _repository.GetByIdAsync(id);
            if (venta == null) return NotFound();
            return Ok(venta.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<VentaDto>> Create([FromBody] VentaDto dto)
        {
            var venta = dto.ToEntity();
            var creada = await _repository.AddAsync(venta);
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VentaDto>> Update(int id, [FromBody] VentaDto dto)
        {
            if (id != dto.Id) return BadRequest("El ID no coincide");

            var existente = await _repository.GetByIdAsync(id);
            if (existente == null) return NotFound();

            existente.UpdateEntity(dto);
            var actualizada = await _repository.UpdateAsync(id, existente);
            return Ok(actualizada?.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var eliminado = await _repository.DeleteAsync(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<VentaDto>>> GetPaged([FromQuery] PaginationParams pagination)
        {
            var result = await _repository.GetPagedAsync(pagination);
            return Ok(result);
        }
    }
}