using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_H.Dominio
{
    public class DocenteEspecialidad
    {
        public int IdUsuario { get; set; }
        public int IdEspecialidad { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!;

        [ForeignKey("IdEspecialidad")]
        public Especialidad Especialidad { get; set; } = null!;
    }
}