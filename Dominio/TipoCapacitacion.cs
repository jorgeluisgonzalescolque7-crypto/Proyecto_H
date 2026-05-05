using System.ComponentModel.DataAnnotations;

namespace Proyecto_H.Dominio
{
    public class TipoCapacitacion
    {
        [Key]
        public int IdTipo { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Estado { get; set; } = "Activo";

        public List<Capacitacion> Capacitaciones { get; set; } = new();
    }
}