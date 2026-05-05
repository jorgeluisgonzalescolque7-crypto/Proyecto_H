using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;
using Proyecto_H.Helpers;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificadoController : ControllerBase
    {
        private readonly HospitalContext _context;

        public CertificadoController(HospitalContext context)
        {
            _context = context;
        }

        // GET con JOIN + DTO
        [HttpGet]
        public async Task<IActionResult> GetCertificados()
        {
            var query = await (
                from cer in _context.Certificado
                join e in _context.Evaluacion on cer.IdEvaluacion equals e.IdEvaluacion
                join i in _context.Inscripcion on e.IdInscripcion equals i.Id
                join u in _context.Usuario on i.IdUsuario equals u.IdUsuario
                join c in _context.Capacitacion on i.IdCapacitacion equals c.IdCapacitacion
                where cer.Estado != "Inactivo"
                select new CertificadoDTO
                {
                    Certificado = cer.Codigo,
                    Usuario = u.Nombre + " " + u.Apellido,
                    Capacitacion = c.Nombre,
                    FechaEmision = DateTimeHelper.ToBolivia(cer.FechaEmision)
                }
            ).ToListAsync();

            return Ok(query);
        }

        [HttpPost]
        public async Task<IActionResult> PostCertificado(string codigoEvaluacion)
        {
            var eva = await (
                from e in _context.Evaluacion
                where e.Codigo == codigoEvaluacion
                select e
            ).FirstOrDefaultAsync();

            if (eva == null)
                return BadRequest("Evaluación no encontrada");

            string codigo = "CER" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            Certificado c = new()
            {
                Codigo = codigo,
                IdEvaluacion = eva.IdEvaluacion,
                FechaEmision = DateTime.UtcNow,
                Estado = "Activo"
            };

            _context.Certificado.Add(c);
            await _context.SaveChangesAsync();

            return Ok("Certificado generado");
        }
    }
}