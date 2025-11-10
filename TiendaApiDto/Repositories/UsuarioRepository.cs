using Microsoft.EntityFrameworkCore;
using TiendaApiDto.Data;
using TiendaApiDto.Entities;

namespace TiendaApiDto.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TiendaApiContext _context;
        public UsuarioRepository(TiendaApiContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByUsernameAsync(string username)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == username);
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
