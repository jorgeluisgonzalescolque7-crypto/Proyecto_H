using System.ComponentModel.DataAnnotations;

namespace Proyecto_H.Dominio
{
    public class Rotacion
    {
        [Key]
        public int IdRotacion { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Servicio { get; set; } = string.Empty;

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string Estado { get; set; } = "Activo";

        public List<UsuarioRotacion> UsuarioRotaciones { get; set; } = new();
    }
}