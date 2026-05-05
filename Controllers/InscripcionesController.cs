using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionesController : ControllerBase
    {
        private readonly HospitalContext _context;

        public InscripcionesController(HospitalContext context)
        {
            _context = context;
        }

        // Lista completa (puedes dejarla sin DTO porque es interna)
        [HttpGet]
        public async Task<IActionResult> GetInscripciones()
        {
            return Ok(
                await (
                    from i in _context.Inscripcion
                    where i.Estado != "Inactivo"
                    select new
                    {
                        i.Codigo,
                        i.FechaInscripcion
                    }
                ).ToListAsync()
            );
        }

        ////////////////////////////////////////////////////////////////

        // JOIN
        [HttpGet("join-usuarios-capacitaciones")]
        public async Task<IActionResult> JoinUsuariosCapacitaciones()
        {
            var query = await (
                from u in _context.Usuario
                join i in _context.Inscripcion on u.IdUsuario equals i.IdUsuario
                join c in _context.Capacitacion on i.IdCapacitacion equals c.IdCapacitacion
                where i.Estado != "Inactivo"
                select new InscripcionDTO
                {
                    Usuario = u.Nombre + " " + u.Apellido,
                    Capacitacion = c.Nombre,
                    Fecha = i.FechaInscripcion.Date
                }
            ).ToListAsync();

            return Ok(query);
        }

        // GROUP BY COUNT
        [HttpGet("count-inscritos-por-capacitacion")]
        public async Task<IActionResult> CountInscritosPorCapacitacion()
        {
            var query = await (
                from c in _context.Capacitacion
                join i in _context.Inscripcion on c.IdCapacitacion equals i.IdCapacitacion
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

        // GROUP BY SUM
        [HttpGet("sum-inscripciones-por-usuario")]
        public async Task<IActionResult> SumInscripcionesPorUsuario()
        {
            var query = await (
                from u in _context.Usuario
                join i in _context.Inscripcion on u.IdUsuario equals i.IdUsuario
                where i.Estado != "Inactivo"
                group i by new { u.Codigo, u.Nombre, u.Apellido } into g
                select new UsuarioInscripcionDTO
                {
                    CodigoUsuario = g.Key.Codigo,
                    Usuario = g.Key.Nombre + " " + g.Key.Apellido,
                    TotalInscripciones = g.Count()
                }
            ).ToListAsync();

            return Ok(query);
        }

        // BUSCAR POR CÓDIGO
        [HttpGet("buscar-inscripcion/{codigo}")]
        public async Task<IActionResult> BuscarInscripcion(string codigo)
        {
            var query = await (
                from i in _context.Inscripcion
                join u in _context.Usuario on i.IdUsuario equals u.IdUsuario
                join c in _context.Capacitacion on i.IdCapacitacion equals c.IdCapacitacion
                where i.Codigo == codigo
                select new HistorialDTO
                {
                    Usuario = u.Nombre + " " + u.Apellido,
                    Capacitacion = c.Nombre,
                    Fecha = i.FechaInscripcion.Date
                }
            ).FirstOrDefaultAsync();

            if (query == null)
                return NotFound();

            return Ok(query);
        }

        // NOT EXISTS
        [HttpGet("usuarios-sin-inscripciones")]
        public async Task<IActionResult> UsuariosSinInscripciones()
        {
            var query = await (
                from u in _context.Usuario
                where !(
                    from i in _context.Inscripcion
                    where i.IdUsuario == u.IdUsuario
                    select i
                ).Any()
                select new UsuarioDTO
                {
                    Codigo = u.Codigo,
                    NombreCompleto = u.Nombre + " " + u.Apellido
                }
            ).ToListAsync();

            return Ok(query);
        }

        ////////////////////////////////////////////////////////////////
        // POST

        [HttpPost("crear")]
        public async Task<IActionResult> PostInscripcion(string codigoUsuario, string codigoCapacitacion)
        {
            Usuario? usuario = await (
                from u in _context.Usuario
                where u.Codigo == codigoUsuario
                select u
            ).FirstOrDefaultAsync();

            Capacitacion? cap = await (
                from c in _context.Capacitacion
                where c.Codigo == codigoCapacitacion
                select c
            ).FirstOrDefaultAsync();

            if (usuario == null || cap == null)
                return BadRequest("No encontrado");

            Inscripcion? existe = await (
                from i in _context.Inscripcion
                where i.IdUsuario == usuario.IdUsuario && i.IdCapacitacion == cap.IdCapacitacion
                select i
            ).FirstOrDefaultAsync();

            if (existe != null)
                return BadRequest("Ya existe relación");

            Inscripcion ins = new()
            {
                Codigo = "INS" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"),
                IdUsuario = usuario.IdUsuario,
                IdCapacitacion = cap.IdCapacitacion,
                FechaInscripcion = DateTime.UtcNow,
                Estado = "Activo"
            };

            _context.Inscripcion.Add(ins);
            await _context.SaveChangesAsync();

            return Ok("Inscripción creada");
        }

        ////////////////////////////////////////////////////////////////
        // CONSULTAS MIS

        [HttpGet("usuarios-por-capacitacion")]
        public async Task<IActionResult> UsuariosPorCapacitacion()
        {
            var query = await (
                from c in _context.Capacitacion
                join i in _context.Inscripcion on c.IdCapacitacion equals i.IdCapacitacion
                join u in _context.Usuario on i.IdUsuario equals u.IdUsuario
                where i.Estado != "Inactivo"
                select new InscripcionDTO
                {
                    Capacitacion = c.Nombre,
                    Usuario = u.Nombre + " " + u.Apellido,
                    Fecha = i.FechaInscripcion.Date
                }
            ).ToListAsync();

            return Ok(query);
        }

        [HttpGet("capacitaciones-sin-inscritos")]
        public async Task<IActionResult> CapacitacionesSinInscritos()
        {
            var query = await (
                from c in _context.Capacitacion
                where !(
                    from i in _context.Inscripcion
                    where i.IdCapacitacion == c.IdCapacitacion
                    select i
                ).Any()
                select new UsuarioDTO
                {
                    Codigo = c.Codigo,
                    NombreCompleto = c.Nombre
                }
            ).ToListAsync();

            return Ok(query);
        }

        [HttpGet("inscripciones-por-fecha")]
        public async Task<IActionResult> InscripcionesPorFecha()
        {
            var query = await (
                from i in _context.Inscripcion
                group i by i.FechaInscripcion.Date into g
                select new FechaConteoDTO
                {
                    Fecha = g.Key,
                    Total = g.Count()
                }
            ).ToListAsync();

            return Ok(query);
        }

        [HttpGet("capacitaciones-mas-demandadas")]
        public async Task<IActionResult> CapacitacionesMasDemandadas()
        {
            var query = await (
                from c in _context.Capacitacion
                join i in _context.Inscripcion on c.IdCapacitacion equals i.IdCapacitacion
                where i.Estado != "Inactivo"
                group i by new { c.Codigo, c.Nombre } into g
                orderby g.Count() descending
                select new CapacitacionConteoDTO
                {
                    CodigoCapacitacion = g.Key.Codigo,
                    Capacitacion = g.Key.Nombre,
                    Total = g.Count()
                }
            ).ToListAsync();

            return Ok(query);
        }

        [HttpGet("usuarios-mas-activos")]
        public async Task<IActionResult> UsuariosMasActivos()
        {
            var query = await (
                from u in _context.Usuario
                join i in _context.Inscripcion on u.IdUsuario equals i.IdUsuario
                where i.Estado != "Inactivo"
                group i by new { u.Codigo, u.Nombre, u.Apellido } into g
                orderby g.Count() descending
                select new UsuarioInscripcionDTO
                {
                    CodigoUsuario = g.Key.Codigo,
                    Usuario = g.Key.Nombre + " " + g.Key.Apellido,
                    TotalInscripciones = g.Count()
                }
            ).ToListAsync();

            return Ok(query);
        }

        [HttpGet("historial-usuario/{codigo}")]
        public async Task<IActionResult> HistorialUsuario(string codigo)
        {
            var query = await (
                from u in _context.Usuario
                join i in _context.Inscripcion on u.IdUsuario equals i.IdUsuario
                join c in _context.Capacitacion on i.IdCapacitacion equals c.IdCapacitacion
                where u.Codigo == codigo
                select new HistorialDTO
                {
                    Usuario = u.Nombre + " " + u.Apellido,
                    Capacitacion = c.Nombre,
                    Fecha = i.FechaInscripcion.Date
                }
            ).ToListAsync();

            return Ok(query);
        }

        [HttpGet("usuarios-multiples-capacitaciones")]
        public async Task<IActionResult> UsuariosMultiplesCapacitaciones()
        {
            var query = await (
                from u in _context.Usuario
                join i in _context.Inscripcion on u.IdUsuario equals i.IdUsuario
                where i.Estado != "Inactivo"
                group i by new { u.Codigo, u.Nombre, u.Apellido } into g
                where g.Count() > 1
                select new UsuarioInscripcionDTO
                {
                    CodigoUsuario = g.Key.Codigo,
                    Usuario = g.Key.Nombre + " " + g.Key.Apellido,
                    TotalInscripciones = g.Count()
                }
            ).ToListAsync();

            return Ok(query);
        }

        ////////////////////////////////////////////////////////////////
        // DELETE

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteInscripcion(string codigo)
        {
            Inscripcion? ins = await (
                from i in _context.Inscripcion
                where i.Codigo == codigo
                select i
            ).FirstOrDefaultAsync();

            if (ins == null)
                return NotFound();

            ins.Estado = "Inactivo";
            _context.Inscripcion.Update(ins);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}