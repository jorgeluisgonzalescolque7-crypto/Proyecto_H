using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_H.Dominio
{
    public class Inscripcion
    {
        [Key]
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public int IdUsuario { get; set; }

        public int IdCapacitacion { get; set; }

        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        public string Estado { get; set; } = "Activo";

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey("IdCapacitacion")]
        public Capacitacion Capacitacion { get; set; } = null!;
    }
}