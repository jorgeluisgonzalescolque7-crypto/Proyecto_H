using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapacitacionesController : ControllerBase
    {
        private readonly HospitalContext _context;

        public CapacitacionesController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Capacitaciones
        [HttpGet]
        public async Task<IActionResult> GetCapacitaciones()
        {
            return Ok(
                await (
                    from c in _context.Capacitacion
                    where c.Estado != "Inactivo"
                    select new CapacitacionConteoDTO
                    {
                        CodigoCapacitacion = c.Codigo,
                        Capacitacion = c.Nombre,
                        Total = 0
                    }
                ).ToListAsync()
            );
        }

        // GET api/Capacitaciones/CAP001
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetCapacitacion(string codigo)
        {
            var cap = await (
                from c in _context.Capacitacion
                where c.Codigo == codigo && c.Estado != "Inactivo"
                select new CapacitacionConteoDTO
                {
                    CodigoCapacitacion = c.Codigo,
                    Capacitacion = c.Nombre,
                    Total = 0
                }
            ).FirstOrDefaultAsync();

            if (cap == null)
                return NotFound();

            return Ok(cap);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> PostCapacitacion(string nombre)
        {
            string codigoGenerado =
               "CAP" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            Capacitacion c1 = new()
            {
                Codigo = codigoGenerado,
                Nombre = nombre,
                Estado = "Activo"
            };

            _context.Capacitacion.Add(c1);

            await _context.SaveChangesAsync();

            return Ok("Creada");
        }

        // PUT
        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutCapacitacion(
            string codigo,
            string nombre
        )
        {
            Capacitacion? cap = await (
                from c in _context.Capacitacion
                where c.Codigo == codigo
                select c
            ).FirstOrDefaultAsync();

            if (cap == null)
                return NotFound();

            cap.Nombre = nombre;

            _context.Capacitacion.Update(cap);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE (soft)
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteCapacitacion(string codigo)
        {
            Capacitacion? cap = await (
                from c in _context.Capacitacion
                where c.Codigo == codigo
                select c
            ).FirstOrDefaultAsync();

            if (cap == null)
                return NotFound();

            cap.Estado = "Inactivo";

            _context.Capacitacion.Update(cap);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// CONSULTA MIS
        [HttpGet("conteo-inscritos")]
        public async Task<IActionResult> ConteoInscritos()
        {
            var query = await (
                from c in _context.Capacitacion
                join i in _context.Inscripcion
                    on c.IdCapacitacion equals i.IdCapacitacion
                where i.Estado != "Inactivo"
                group i by new { c.Codigo, c.Nombre } into g
                select new CapacitacionConteoDTO
                {
                    CodigoCapacitacion = g.Key.Codigo,
                    Capacitacion = g.Key.Nombre,
                    Total = g.Count()
                }
            ).ToListAsync();

            return Ok(query);
        }
    }
}