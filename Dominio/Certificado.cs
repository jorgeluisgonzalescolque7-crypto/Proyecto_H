using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_H.Dominio
{
    public class Certificado
    {
        [Key]
        public int IdCertificado { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public int IdEvaluacion { get; set; }

        public DateTime FechaEmision { get; set; }

        public string Estado { get; set; } = "Activo";

        [ForeignKey("IdEvaluacion")]
        public Evaluacion Evaluacion { get; set; } = null!;
    }
}