using Microsoft.AspNetCore.Mvc;
using TiendaApiDto.Dtos;
using TiendaApiDto.Mappers;
using TiendaApiDto.Repositories;

namespace TiendaApiDto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoRepository _repository;

        public EmpleadoController(IEmpleadoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> GetAll()
        {
            var empleados = await _repository.GetAllAsync();
            return Ok(empleados.Select(e => e.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpleadoDto>> GetById(int id)
        {
            var empleado = await _repository.GetByIdAsync(id);
            if (empleado == null) return NotFound();
            return Ok(empleado.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<EmpleadoDto>> Create([FromBody] EmpleadoDto dto)
        {
            var empleado = dto.ToEntity();
            var creado = await _repository.AddAsync(empleado);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmpleadoDto>> Update(int id, [FromBody] EmpleadoDto dto)
        {
            if (id != dto.Id) return BadRequest("El ID no coincide");

            var existente = await _repository.GetByIdAsync(id);
            if (existente == null) return NotFound();

            existente.UpdateEntity(dto);
            var actualizado = await _repository.UpdateAsync(id, existente);
            return Ok(actualizado?.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var eliminado = await _repository.DeleteAsync(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }
    }
}