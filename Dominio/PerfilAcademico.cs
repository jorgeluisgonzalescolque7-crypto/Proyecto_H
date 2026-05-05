using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_H.Dominio
{
    public class PerfilAcademico
    {
        [Key]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        public string Universidad { get; set; } = string.Empty;
        public string Carrera { get; set; } = string.Empty;
        public string NivelAcademico { get; set; } = string.Empty;
        public int AñoFormacion { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}