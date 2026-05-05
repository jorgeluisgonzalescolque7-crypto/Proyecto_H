using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocenteCapacitacionController : ControllerBase
    {
        private readonly HospitalContext _context;

        public DocenteCapacitacionController(HospitalContext context)
        {
            _context = context;
        }

        // GET → docentes por capacitación
        [HttpGet("docentes-por-capacitacion")]
        public async Task<IActionResult> GetDocentesPorCapacitacion()
        {
            var query = await (
                from dc in _context.DocenteCapacitacion
                join u in _context.Usuario on dc.IdUsuario equals u.IdUsuario
                join c in _context.Capacitacion on dc.IdCapacitacion equals c.IdCapacitacion
                select new DocenteCapacitacionDTO
                {
                    Capacitacion = c.Nombre,
                    Docente = u.Nombre + " " + u.Apellido
                }
            ).ToListAsync();

            return Ok(query);
        }

        // POST → asignar docente a capacitación
        [HttpPost("asignar")]
        public async Task<IActionResult> AsignarDocente(string codigoUsuario, string codigoCapacitacion)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Codigo == codigoUsuario);
            var cap = await _context.Capacitacion.FirstOrDefaultAsync(x => x.Codigo == codigoCapacitacion);

            if (usuario == null || cap == null)
                return BadRequest("No encontrado");

            var existe = await _context.DocenteCapacitacion
                .FirstOrDefaultAsync(x => x.IdUsuario == usuario.IdUsuario && x.IdCapacitacion == cap.IdCapacitacion);

            if (existe != null)
                return BadRequest("Ya existe relación");

            DocenteCapacitacion dc = new()
            {
                IdUsuario = usuario.IdUsuario,
                IdCapacitacion = cap.IdCapacitacion
            };

            _context.DocenteCapacitacion.Add(dc);
            await _context.SaveChangesAsync();

            return Ok("Docente asignado");
        }
    }
}