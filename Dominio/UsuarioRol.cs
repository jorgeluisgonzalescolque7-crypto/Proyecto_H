using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_H.Dominio
{
    public class UsuarioRol
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey("IdRol")]
        public Rol Rol { get; set; } = null!;
    }
}