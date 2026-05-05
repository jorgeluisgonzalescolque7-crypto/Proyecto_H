using System.ComponentModel.DataAnnotations;

namespace Proyecto_H.Dominio
{
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string NombreRol { get; set; } = string.Empty;

        public string Estado { get; set; } = "Activo";

        public List<UsuarioRol> UsuarioRoles { get; set; } = new();
    }
}