using Microsoft.EntityFrameworkCore;
using System;
using TiendaApiDto.Data;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly TiendaApiContext _context;

        public EmpleadoRepository(TiendaApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Empleado>> GetAllAsync()
        {
            return await _context.Empleados.Include(e => e.Ventas).ToListAsync();
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await _context.Empleados.Include(e => e.Ventas).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Empleado> AddAsync(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<Empleado?> UpdateAsync(int id, Empleado empleado)
        {
            var existente = await _context.Empleados.FindAsync(id);
            if (existente == null) return null;

            existente.Nombre = empleado.Nombre;
            existente.Apellido = empleado.Apellido;
            existente.Edad = empleado.Edad;
            existente.Email = empleado.Email;
            existente.Cargo = empleado.Cargo;

            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return false;

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}