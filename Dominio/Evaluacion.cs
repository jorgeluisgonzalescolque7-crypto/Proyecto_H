using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_H.Dominio
{
    public class Evaluacion
    {
        [Key]
        public int IdEvaluacion { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public int IdInscripcion { get; set; }

        public decimal Nota { get; set; }

        public bool Aprobado { get; set; }

        public string Estado { get; set; } = "Activo";

        [ForeignKey("IdInscripcion")]
        public Inscripcion Inscripcion { get; set; } = null!;

        public Certificado? Certificado { get; set; }
    }
}