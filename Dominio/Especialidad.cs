using System.ComponentModel.DataAnnotations;

namespace Proyecto_H.Dominio
{
    public class Especialidad
    {
        [Key]
        public int IdEspecialidad { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Estado { get; set; } = "Activo";

        public List<DocenteEspecialidad> DocenteEspecialidades { get; set; } = new();
    }
}