using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioRolController : ControllerBase
    {
        private readonly HospitalContext _context;

        public UsuarioRolController(HospitalContext context)
        {
            _context = context;
        }

        // GET → ver roles por usuario
        [HttpGet("roles-por-usuario")]
        public async Task<IActionResult> GetRolesPorUsuario()
        {
            var query = await (
                from ur in _context.UsuarioRol
                join u in _context.Usuario on ur.IdUsuario equals u.IdUsuario
                join r in _context.Rol on ur.IdRol equals r.IdRol
                select new UsuarioRolDTO
                {
                    Usuario = u.Nombre + " " + u.Apellido,
                    Rol = r.NombreRol
                }
            ).ToListAsync();

            return Ok(query);
        }

        // POST → asignar rol
        [HttpPost("asignar")]
        public async Task<IActionResult> AsignarRol(string codigoUsuario, string codigoRol)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Codigo == codigoUsuario);
            var rol = await _context.Rol.FirstOrDefaultAsync(x => x.Codigo == codigoRol);

            if (usuario == null || rol == null)
                return BadRequest("No encontrado");

            var existe = await _context.UsuarioRol
                .FirstOrDefaultAsync(x => x.IdUsuario == usuario.IdUsuario && x.IdRol == rol.IdRol);

            if (existe != null)
                return BadRequest("Ya existe relación");

            UsuarioRol ur = new()
            {
                IdUsuario = usuario.IdUsuario,
                IdRol = rol.IdRol
            };

            _context.UsuarioRol.Add(ur);
            await _context.SaveChangesAsync();

            return Ok("Rol asignado");
        }
    }
}