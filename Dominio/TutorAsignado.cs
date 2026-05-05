using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_H.Dominio
{
    public class TutorAsignado
    {
        public int IdEstudiante { get; set; }
        public int IdDocente { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        [ForeignKey("IdEstudiante")]
        public Usuario Estudiante { get; set; } = null!;

        [ForeignKey("IdDocente")]
        public Usuario Docente { get; set; } = null!;
    }
}