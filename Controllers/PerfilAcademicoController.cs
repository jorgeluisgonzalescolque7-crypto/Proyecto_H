using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilAcademicoController : ControllerBase
    {
        private readonly HospitalContext _context;

        public PerfilAcademicoController(HospitalContext context)
        {
            _context = context;
        }

        // 🔹 GET → ver perfiles académicos
        [HttpGet]
        public async Task<IActionResult> GetPerfilAcademico()
        {
            var query = await (
                from p in _context.PerfilAcademico
                join u in _context.Usuario
                    on p.IdUsuario equals u.IdUsuario
                where u.Estado != "Inactivo"
                select new PerfilAcademicoDTO
                {
                    Usuario = u.Nombre + " " + u.Apellido,
                    Universidad = p.Universidad,
                    Carrera = p.Carrera,
                    NivelAcademico = p.NivelAcademico,
                    AñoFormacion = p.AñoFormacion
                }
            ).ToListAsync();

            return Ok(query);
        }

        // 🔹 GET → perfil por código de usuario
        [HttpGet("por-usuario/{codigo}")]
        public async Task<IActionResult> GetPerfilPorUsuario(string codigo)
        {
            var query = await (
                from p in _context.PerfilAcademico
                join u in _context.Usuario
                    on p.IdUsuario equals u.IdUsuario
                where u.Codigo == codigo
                select new PerfilAcademicoDTO
                {
                    Usuario = u.Nombre + " " + u.Apellido,
                    Universidad = p.Universidad,
                    Carrera = p.Carrera,
                    NivelAcademico = p.NivelAcademico,
                    AñoFormacion = p.AñoFormacion
                }
            ).FirstOrDefaultAsync();

            if (query == null)
                return NotFound();

            return Ok(query);
        }

        // 🔹 POST → crear perfil académico
        [HttpPost]
        public async Task<IActionResult> PostPerfilAcademico(
            string codigoUsuario,
            string universidad,
            string carrera,
            string nivelAcademico,
            int anioFormacion
        )
        {
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Codigo == codigoUsuario);

            if (usuario == null)
                return BadRequest("Usuario no encontrado");

            var existe = await _context.PerfilAcademico
                .FirstOrDefaultAsync(p => p.IdUsuario == usuario.IdUsuario);

            if (existe != null)
                return BadRequest("El usuario ya tiene perfil académico");

            PerfilAcademico perfil = new()
            {
                IdUsuario = usuario.IdUsuario,
                Universidad = universidad,
                Carrera = carrera,
                NivelAcademico = nivelAcademico,
                AñoFormacion = anioFormacion
            };

            _context.PerfilAcademico.Add(perfil);
            await _context.SaveChangesAsync();

            return Ok("Perfil académico creado");
        }
    }
}