using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorAsignadoController : ControllerBase
    {
        private readonly HospitalContext _context;

        public TutorAsignadoController(HospitalContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = await (
                from t in _context.TutorAsignado
                join est in _context.Usuario on t.IdEstudiante equals est.IdUsuario
                join doc in _context.Usuario on t.IdDocente equals doc.IdUsuario
                select new TutorAsignadoDTO
                {
                    Estudiante = est.Nombre + " " + est.Apellido,
                    Tutor = doc.Nombre + " " + doc.Apellido,
                    FechaInicio = t.FechaInicio.Date
                }
            ).ToListAsync();

            return Ok(query);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Post(string codigoEstudiante, string codigoDocente)
        {
            var est = await _context.Usuario
                .FirstOrDefaultAsync(x => x.Codigo == codigoEstudiante);

            var doc = await _context.Usuario
                .FirstOrDefaultAsync(x => x.Codigo == codigoDocente);

            if (est == null || doc == null)
                return BadRequest("No encontrado");

            // 🔥 VALIDACIÓN IMPORTANTE
            if (est.IdUsuario == doc.IdUsuario)
                return BadRequest("Un usuario no puede ser su propio tutor");

            var existe = await _context.TutorAsignado
                .FirstOrDefaultAsync(x =>
                    x.IdEstudiante == est.IdUsuario &&
                    x.IdDocente == doc.IdUsuario);

            if (existe != null)
                return BadRequest("Ya existe esta asignación");

            TutorAsignado t = new()
            {
                IdEstudiante = est.IdUsuario,
                IdDocente = doc.IdUsuario,
                FechaInicio = DateTime.UtcNow
            };

            _context.TutorAsignado.Add(t);
            await _context.SaveChangesAsync();

            return Ok("Tutor asignado");
        }
    }
}