using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Proyecto_H.Migrations
{
    /// <inheritdoc />
    public partial class m3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoCapacitacionIdTipo",
                table: "Capacitacion",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocenteCapacitacion",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdCapacitacion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocenteCapacitacion", x => new { x.IdUsuario, x.IdCapacitacion });
                    table.ForeignKey(
                        name: "FK_DocenteCapacitacion_Capacitacion_IdCapacitacion",
                        column: x => x.IdCapacitacion,
                        principalTable: "Capacitacion",
                        principalColumn: "IdCapacitacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocenteCapacitacion_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Especialidad",
                columns: table => new
                {
                    IdEspecialidad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidad", x => x.IdEspecialidad);
                });

            migrationBuilder.CreateTable(
                name: "Evaluacion",
                columns: table => new
                {
                    IdEvaluacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    IdInscripcion = table.Column<int>(type: "integer", nullable: false),
                    Nota = table.Column<decimal>(type: "numeric", nullable: false),
                    Aprobado = table.Column<bool>(type: "boolean", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluacion", x => x.IdEvaluacion);
                    table.ForeignKey(
                        name: "FK_Evaluacion_Inscripcion_IdInscripcion",
                        column: x => x.IdInscripcion,
                        principalTable: "Inscripcion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerfilAcademico",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    Universidad = table.Column<string>(type: "text", nullable: false),
                    Carrera = table.Column<string>(type: "text", nullable: false),
                    NivelAcademico = table.Column<string>(type: "text", nullable: false),
                    AñoFormacion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilAcademico", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_PerfilAcademico_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    NombreRol = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Rotacion",
                columns: table => new
                {
                    IdRotacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Servicio = table.Column<string>(type: "text", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rotacion", x => x.IdRotacion);
                });

            migrationBuilder.CreateTable(
                name: "TipoCapacitacion",
                columns: table => new
                {
                    IdTipo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCapacitacion", x => x.IdTipo);
                });

            migrationBuilder.CreateTable(
                name: "TutorAsignado",
                columns: table => new
                {
                    IdEstudiante = table.Column<int>(type: "integer", nullable: false),
                    IdDocente = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorAsignado", x => new { x.IdEstudiante, x.IdDocente });
                    table.ForeignKey(
                        name: "FK_TutorAsignado_Usuario_IdDocente",
                        column: x => x.IdDocente,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TutorAsignado_Usuario_IdEstudiante",
                        column: x => x.IdEstudiante,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocenteEspecialidad",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdEspecialidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocenteEspecialidad", x => new { x.IdUsuario, x.IdEspecialidad });
                    table.ForeignKey(
                        name: "FK_DocenteEspecialidad_Especialidad_IdEspecialidad",
                        column: x => x.IdEspecialidad,
                        principalTable: "Especialidad",
                        principalColumn: "IdEspecialidad",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocenteEspecialidad_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certificado",
                columns: table => new
                {
                    IdCertificado = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    IdEvaluacion = table.Column<int>(type: "integer", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificado", x => x.IdCertificado);
                    table.ForeignKey(
                        name: "FK_Certificado_Evaluacion_IdEvaluacion",
                        column: x => x.IdEvaluacion,
                        principalTable: "Evaluacion",
                        principalColumn: "IdEvaluacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRol",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRol", x => new { x.IdUsuario, x.IdRol });
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Rol_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRotacion",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdRotacion = table.Column<int>(type: "integer", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCambio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRotacion", x => new { x.IdUsuario, x.IdRotacion });
                    table.ForeignKey(
                        name: "FK_UsuarioRotacion_Rotacion_IdRotacion",
                        column: x => x.IdRotacion,
                        principalTable: "Rotacion",
                        principalColumn: "IdRotacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRotacion_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Capacitacion_TipoCapacitacionIdTipo",
                table: "Capacitacion",
                column: "TipoCapacitacionIdTipo");

            migrationBuilder.CreateIndex(
                name: "IX_Certificado_IdEvaluacion",
                table: "Certificado",
                column: "IdEvaluacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocenteCapacitacion_IdCapacitacion",
                table: "DocenteCapacitacion",
                column: "IdCapacitacion");

            migrationBuilder.CreateIndex(
                name: "IX_DocenteEspecialidad_IdEspecialidad",
                table: "DocenteEspecialidad",
                column: "IdEspecialidad");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_IdInscripcion",
                table: "Evaluacion",
                column: "IdInscripcion");

            migrationBuilder.CreateIndex(
                name: "IX_TutorAsignado_IdDocente",
                table: "TutorAsignado",
                column: "IdDocente");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_IdRol",
                table: "UsuarioRol",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRotacion_IdRotacion",
                table: "UsuarioRotacion",
                column: "IdRotacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Capacitacion_TipoCapacitacion_TipoCapacitacionIdTipo",
                table: "Capacitacion",
                column: "TipoCapacitacionIdTipo",
                principalTable: "TipoCapacitacion",
                principalColumn: "IdTipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Capacitacion_TipoCapacitacion_TipoCapacitacionIdTipo",
                table: "Capacitacion");

            migrationBuilder.DropTable(
                name: "Certificado");

            migrationBuilder.DropTable(
                name: "DocenteCapacitacion");

            migrationBuilder.DropTable(
                name: "DocenteEspecialidad");

            migrationBuilder.DropTable(
                name: "PerfilAcademico");

            migrationBuilder.DropTable(
                name: "TipoCapacitacion");

            migrationBuilder.DropTable(
                name: "TutorAsignado");

            migrationBuilder.DropTable(
                name: "UsuarioRol");

            migrationBuilder.DropTable(
                name: "UsuarioRotacion");

            migrationBuilder.DropTable(
                name: "Evaluacion");

            migrationBuilder.DropTable(
                name: "Especialidad");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Rotacion");

            migrationBuilder.DropIndex(
                name: "IX_Capacitacion_TipoCapacitacionIdTipo",
                table: "Capacitacion");

            migrationBuilder.DropColumn(
                name: "TipoCapacitacionIdTipo",
                table: "Capacitacion");
        }
    }
}
