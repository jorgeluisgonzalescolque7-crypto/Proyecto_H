using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadController : ControllerBase
    {
        private readonly HospitalContext _context;

        public EspecialidadController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Especialidad
        [HttpGet]
        public async Task<IActionResult> GetEspecialidades()
        {
            return Ok(
                await (
                    from e in _context.Especialidad
                    where e.Estado != "Inactivo"
                    select new EspecialidadDTO
                    {
                        Codigo = e.Codigo,
                        Nombre = e.Nombre
                    }
                ).ToListAsync()
            );
        }

        // GET: api/Especialidad/{codigo}
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetEspecialidad(string codigo)
        {
            var esp = await (
                from e in _context.Especialidad
                where e.Codigo == codigo && e.Estado != "Inactivo"
                select new EspecialidadDTO
                {
                    Codigo = e.Codigo,
                    Nombre = e.Nombre
                }
            ).FirstOrDefaultAsync();

            if (esp == null)
                return NotFound();

            return Ok(esp);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> PostEspecialidad(string nombre)
        {
            string codigo = "ESP" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            Especialidad esp = new()
            {
                Codigo = codigo,
                Nombre = nombre,
                Estado = "Activo"
            };

            _context.Especialidad.Add(esp);
            await _context.SaveChangesAsync();

            return Ok("Especialidad creada");
        }

        // DELETE (soft delete)
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteEspecialidad(string codigo)
        {
            var esp = await _context.Especialidad
                .FirstOrDefaultAsync(e => e.Codigo == codigo);

            if (esp == null)
                return NotFound();

            esp.Estado = "Inactivo";

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}