using Microsoft.EntityFrameworkCore;
using Proyecto_H.Dominio;

namespace Proyecto_H.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(
            DbContextOptions<HospitalContext> options
        ) : base(options)
        {

        }

        // ta
        public DbSet<Usuario> Usuario { get; set; } = default!;
        public DbSet<Capacitacion> Capacitacion { get; set; } = default!;
        public DbSet<Inscripcion> Inscripcion { get; set; } = default!;

        // tn
        public DbSet<Rol> Rol { get; set; } = default!;
        public DbSet<UsuarioRol> UsuarioRol { get; set; } = default!;
        public DbSet<TipoCapacitacion> TipoCapacitacion { get; set; } = default!;
        public DbSet<DocenteCapacitacion> DocenteCapacitacion { get; set; } = default!;
        public DbSet<Evaluacion> Evaluacion { get; set; } = default!;
        public DbSet<Certificado> Certificado { get; set; } = default!;
        public DbSet<Especialidad> Especialidad { get; set; } = default!;
        public DbSet<DocenteEspecialidad> DocenteEspecialidad { get; set; } = default!;
        public DbSet<Rotacion> Rotacion { get; set; } = default!;
        public DbSet<UsuarioRotacion> UsuarioRotacion { get; set; } = default!;
        public DbSet<PerfilAcademico> PerfilAcademico { get; set; } = default!;
        public DbSet<TutorAsignado> TutorAsignado { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //TABLAS INTERMEDIAS

            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.IdUsuario, ur.IdRol });

            modelBuilder.Entity<DocenteCapacitacion>()
                .HasKey(dc => new { dc.IdUsuario, dc.IdCapacitacion });

            modelBuilder.Entity<DocenteEspecialidad>()
                .HasKey(de => new { de.IdUsuario, de.IdEspecialidad });

            modelBuilder.Entity<UsuarioRotacion>()
                .HasKey(ur => new { ur.IdUsuario, ur.IdRotacion });

            modelBuilder.Entity<TutorAsignado>()
                .HasKey(t => new { t.IdEstudiante, t.IdDocente });
        }
    }
}