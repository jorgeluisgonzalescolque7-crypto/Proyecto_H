using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocenteEspecialidadController : ControllerBase
    {
        private readonly HospitalContext _context;

        public DocenteEspecialidadController(HospitalContext context)
        {
            _context = context;
        }

        // GET con DTO
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = await (
                from de in _context.DocenteEspecialidad
                join u in _context.Usuario on de.IdUsuario equals u.IdUsuario
                join e in _context.Especialidad on de.IdEspecialidad equals e.IdEspecialidad
                select new DocenteEspecialidadDTO
                {
                    Docente = u.Nombre + " " + u.Apellido,
                    Especialidad = e.Nombre
                }
            ).ToListAsync();

            return Ok(query);
        }

        // POST (igual que antes)
        [HttpPost]
        public async Task<IActionResult> Post(string codigoUsuario, string codigoEspecialidad)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Codigo == codigoUsuario);
            var esp = await _context.Especialidad.FirstOrDefaultAsync(x => x.Codigo == codigoEspecialidad);

            if (usuario == null || esp == null)
                return BadRequest("No encontrado");

            var existe = await _context.DocenteEspecialidad
                .FirstOrDefaultAsync(x => x.IdUsuario == usuario.IdUsuario && x.IdEspecialidad == esp.IdEspecialidad);

            if (existe != null)
                return BadRequest("Ya existe");

            DocenteEspecialidad de = new()
            {
                IdUsuario = usuario.IdUsuario,
                IdEspecialidad = esp.IdEspecialidad
            };

            _context.DocenteEspecialidad.Add(de);
            await _context.SaveChangesAsync();

            return Ok("Asignado");
        }
    }
}