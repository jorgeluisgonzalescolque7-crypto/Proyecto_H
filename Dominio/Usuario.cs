using System.ComponentModel.DataAnnotations;

namespace Proyecto_H.Dominio
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Estado { get; set; } = "Activo";


        public List<Inscripcion> Inscripciones { get; set; } = new();
    }
}
