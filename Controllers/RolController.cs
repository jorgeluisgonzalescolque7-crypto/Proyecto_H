using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_H.Data;
using Proyecto_H.Dominio;
using Proyecto_H.DTOs;

namespace Proyecto_H.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly HospitalContext _context;

        public RolController(HospitalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(
                await (
                    from r in _context.Rol
                    where r.Estado != "Inactivo"
                    select new RolDTO
                    {
                        Codigo = r.Codigo,
                        NombreRol = r.NombreRol
                    }
                ).ToListAsync()
            );
        }

        [HttpPost]
        public async Task<IActionResult> PostRol(string nombreRol)
        {
            string codigo = "ROL" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            Rol rol = new()
            {
                Codigo = codigo,
                NombreRol = nombreRol,
                Estado = "Activo"
            };

            _context.Rol.Add(rol);
            await _context.SaveChangesAsync();

            return Ok("Rol creado");
        }
    }
}