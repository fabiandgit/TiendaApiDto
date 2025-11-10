using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TiendaApiDto.Dtos;
using TiendaApiDto.Entities;
using TiendaApiDto.Mappers;
using TiendaApiDto.Repositories;

namespace TiendaApiDto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _config;

        public AuthController(IUsuarioRepository usuarioRepository, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            _config = config;
        }

        // 🔹 POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            // Verificar si el usuario ya existe
            var existente = await _usuarioRepository.GetByUsernameAsync(dto.NombreUsuario);
            if (existente != null)
                return BadRequest("El usuario ya existe.");

            var nuevoUsuario = dto.ToEntity();

            // Encriptar contraseña
            nuevoUsuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _usuarioRepository.AddAsync(nuevoUsuario);

            return Ok("Usuario registrado correctamente ✅");
        }

        // 🔹 POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto dto)
        {
            var usuario = await _usuarioRepository.GetByUsernameAsync(dto.NombreUsuario);
            if (usuario == null)
                return Unauthorized("Usuario o contraseña incorrectos.");

            // Validar contraseña
            bool valido = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);
            if (!valido)
                return Unauthorized("Usuario o contraseña incorrectos.");

            var token = GenerarToken(usuario);

            return Ok(new
            {
                token,
                usuario = usuario.NombreUsuario,
                rol = usuario.Rol
            });

        }

            private string GenerarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            //- Se obtiene la clave secreta desde la configuración (appsettings.json) Jwt:key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            //- Se usa esa clave para firmar el token con el algoritmo HmacSha512
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //se crea el token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"], // issuer: quién emite el token (tu API)
                audience: _config["Jwt:Audience"],// audience: quién puede usar el token (los clientes)
                claims: claims, // claims: la información del usuario
                expires: DateTime.Now.AddHours(3), // expires: cuándo expira el token (en 3 horas)
                signingCredentials: creds // signingCredentials: cómo se firma el toke
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            // Esto convierte el objeto JwtSecurityToken en un string codificado, que es lo que se envía al cliente.
        }
    }

}
