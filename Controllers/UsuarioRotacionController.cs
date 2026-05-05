using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioRotacionController : ControllerBase
    {
        private readonly HospitalContext _context;

        public UsuarioRotacionController(HospitalContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = await (
                from ur in _context.UsuarioRotacion
                join u in _context.Usuario on ur.IdUsuario equals u.IdUsuario
                join r in _context.Rotacion on ur.IdRotacion equals r.IdRotacion
                select new UsuarioRotacionDTO
                {
                    Usuario = u.Nombre + " " + u.Apellido,
                    Rotacion = r.Servicio,
                    FechaAsignacion = ur.FechaAsignacion.Date
                }
            ).ToListAsync();

            return Ok(query);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Post(string codigoUsuario, string codigoRotacion)
        {
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(x => x.Codigo == codigoUsuario);

            var rot = await _context.Rotacion
                .FirstOrDefaultAsync(x => x.Codigo == codigoRotacion);

            if (usuario == null || rot == null)
                return BadRequest("No encontrado");

            var existe = await _context.UsuarioRotacion
                .FirstOrDefaultAsync(x =>
                    x.IdUsuario == usuario.IdUsuario &&
                    x.IdRotacion == rot.IdRotacion);

            if (existe != null)
                return BadRequest("Ya existe relación");

            UsuarioRotacion ur = new()
            {
                IdUsuario = usuario.IdUsuario,
                IdRotacion = rot.IdRotacion,
                FechaAsignacion = DateTime.UtcNow
            };

            _context.UsuarioRotacion.Add(ur);
            await _context.SaveChangesAsync();

            return Ok("Asignado");
        }
    }
}