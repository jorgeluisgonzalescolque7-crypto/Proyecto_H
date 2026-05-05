using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoCapacitacionController : ControllerBase
    {
        private readonly HospitalContext _context;

        public TipoCapacitacionController(HospitalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTipos()
        {
            return Ok(
                await (
                    from t in _context.TipoCapacitacion
                    where t.Estado != "Inactivo"
                    select new TipoCapacitacionDTO
                    {
                        Codigo = t.Codigo,
                        Nombre = t.Nombre
                    }
                ).ToListAsync()
            );
        }

        [HttpPost]
        public async Task<IActionResult> PostTipo(string nombre)
        {
            string codigo = "TIP" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            TipoCapacitacion t = new()
            {
                Codigo = codigo,
                Nombre = nombre,
                Estado = "Activo"
            };

            _context.TipoCapacitacion.Add(t);
            await _context.SaveChangesAsync();

            return Ok("Tipo creado");
        }
    }
}