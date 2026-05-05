using System.ComponentModel.DataAnnotations;

namespace Proyecto_H.Dominio
{
    public class Capacitacion
    {
        [Key]
        public int IdCapacitacion { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Estado { get; set; } = "Activo";

        public List<Inscripcion> Inscripciones { get; set; } = new();
    }
}
