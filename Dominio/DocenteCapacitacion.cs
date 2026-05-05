using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_H.Dominio
{
    public class DocenteCapacitacion
    {
        public int IdUsuario { get; set; }
        public int IdCapacitacion { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey("IdCapacitacion")]
        public Capacitacion Capacitacion { get; set; } = null!;
    }
}