using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionController : ControllerBase
    {
        private readonly HospitalContext _context;

        public EvaluacionController(HospitalContext context)
        {
            _context = context;
        }

        // GET con DTO
        [HttpGet]
        public async Task<IActionResult> GetEvaluaciones()
        {
            var query = await (
                from e in _context.Evaluacion
                join i in _context.Inscripcion on e.IdInscripcion equals i.Id
                join u in _context.Usuario on i.IdUsuario equals u.IdUsuario
                join c in _context.Capacitacion on i.IdCapacitacion equals c.IdCapacitacion
                where e.Estado != "Inactivo"
                select new EvaluacionDTO
                {
                    Evaluacion = e.Codigo,
                    Usuario = u.Nombre + " " + u.Apellido,
                    Capacitacion = c.Nombre,
                    Nota = e.Nota,
                    AprobadoTexto = e.Nota >= 51 ? "SI" : "NO"
                }
            ).ToListAsync();

            return Ok(query);
        }

        // POST (no usa DTO, está bien así)
        [HttpPost]
        public async Task<IActionResult> PostEvaluacion(
            string codigoInscripcion,
            decimal nota
        )
        {
            var ins = await (
                from i in _context.Inscripcion
                where i.Codigo == codigoInscripcion
                select i
            ).FirstOrDefaultAsync();

            if (ins == null)
                return BadRequest("Inscripción no encontrada");

            string codigo = "EVA" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            Evaluacion e = new()
            {
                Codigo = codigo,
                IdInscripcion = ins.Id,
                Nota = nota,
                Aprobado = nota >= 51,
                Estado = "Activo"
            };

            _context.Evaluacion.Add(e);
            await _context.SaveChangesAsync();

            return Ok("Evaluación creada");
        }
    }
}