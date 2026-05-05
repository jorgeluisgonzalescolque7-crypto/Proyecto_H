using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_H.Dominio
{
    public class UsuarioRotacion
    {
        public int IdUsuario { get; set; }
        public int IdRotacion { get; set; }

        public DateTime FechaAsignacion { get; set; }
        public DateTime FechaCambio { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey("IdRotacion")]
        public Rotacion Rotacion { get; set; } = null!;
    }
}