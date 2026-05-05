using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotacionController : ControllerBase
    {
        private readonly HospitalContext _context;

        public RotacionController(HospitalContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> GetRotaciones()
        {
            return Ok(
                await (
                    from r in _context.Rotacion
                    where r.Estado != "Inactivo"
                    select new RotacionDTO
                    {
                        Codigo = r.Codigo,
                        Servicio = r.Servicio,
                        FechaInicio = r.FechaInicio.Date,
                        FechaFin = r.FechaFin.Date
                    }
                ).ToListAsync()
            );
        }

        // GET por código (recomendado)
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetRotacion(string codigo)
        {
            var rot = await (
                from r in _context.Rotacion
                where r.Codigo == codigo && r.Estado != "Inactivo"
                select new RotacionDTO
                {
                    Codigo = r.Codigo,
                    Servicio = r.Servicio,
                    FechaInicio = r.FechaInicio.Date,
                    FechaFin = r.FechaFin.Date
                }
            ).FirstOrDefaultAsync();

            if (rot == null)
                return NotFound();

            return Ok(rot);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> PostRotacion(
            string servicio,
            DateTime fechaInicio,
            DateTime fechaFin
        )
        {
            string codigo = "ROT" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            Rotacion r = new()
            {
                Codigo = codigo,
                Servicio = servicio,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Estado = "Activo"
            };

            _context.Rotacion.Add(r);
            await _context.SaveChangesAsync();

            return Ok("Rotación creada");
        }

        // DELETE (soft delete)
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteRotacion(string codigo)
        {
            var rot = await _context.Rotacion
                .FirstOrDefaultAsync(r => r.Codigo == codigo);

            if (rot == null)
                return NotFound();

            rot.Estado = "Inactivo";

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}