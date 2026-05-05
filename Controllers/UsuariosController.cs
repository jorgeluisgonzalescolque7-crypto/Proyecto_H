using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly HospitalContext _context;

        public UsuariosController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        // Lista usuarios activos (DTO)
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            return Ok(
                await (
                    from u in _context.Usuario
                    where u.Estado != "Inactivo"
                    select new UsuarioDTO
                    {
                        Codigo = u.Codigo,
                        NombreCompleto = u.Nombre + " " + u.Apellido
                    }
                ).ToListAsync()
            );
        }

        // GET api/Usuarios/U001
        // Buscar por código (DTO)
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetUsuario(string codigo)
        {
            var usuario = await (
                from u in _context.Usuario
                where u.Codigo == codigo && u.Estado != "Inactivo"
                select new UsuarioDTO
                {
                    Codigo = u.Codigo,
                    NombreCompleto = u.Nombre + " " + u.Apellido
                }
            ).FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        // POST api/Usuarios
        [HttpPost]
        public async Task<IActionResult> PostUsuario(
            string nombre,
            string apellido
        )
        {
            string codigoGenerado =
                "USR" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            Usuario u = new Usuario()
            {
                Codigo = codigoGenerado,
                Nombre = nombre,
                Apellido = apellido,
                Estado = "Activo"
            };

            _context.Usuario.Add(u);

            await _context.SaveChangesAsync();

            return Ok("Creado correctamente");
        }

        // PUT api/Usuarios/U001
        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutUsuario(
            string codigo,
            string nombre,
            string apellido
        )
        {
            var usuario = await (
                from u in _context.Usuario
                where u.Codigo == codigo
                select u
            ).FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound();

            usuario.Nombre = nombre;
            usuario.Apellido = apellido;

            _context.Usuario.Update(usuario);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/Usuarios/U001
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteUsuario(string codigo)
        {
            var usuario = await (
                from u in _context.Usuario
                where u.Codigo == codigo
                select u
            ).FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound();

            usuario.Estado = "Inactivo";

            _context.Usuario.Update(usuario);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}