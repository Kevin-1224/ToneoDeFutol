using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ToneoDeFutol.Api.Migrations
{
    /// <inheritdoc />
    public partial class v01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Entrenador = table.Column<string>(type: "text", nullable: false),
                    Ciudad = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jugador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Posicion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Torneo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoTorneo = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Torneo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InscripcionJugador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EquipoID = table.Column<int>(type: "integer", nullable: false),
                    JugadorID = table.Column<int>(type: "integer", nullable: false),
                    TorneoID = table.Column<int>(type: "integer", nullable: false),
                    NumeroCamiseta = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscripcionJugador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InscripcionJugador_Equipo_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InscripcionJugador_Jugador_JugadorID",
                        column: x => x.JugadorID,
                        principalTable: "Jugador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InscripcionEquipo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EquipoID = table.Column<int>(type: "integer", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GrupoFaseGrupos = table.Column<string>(type: "text", nullable: false),
                    TorneoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscripcionEquipo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InscripcionEquipo_Equipo_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InscripcionEquipo_Torneo_TorneoId",
                        column: x => x.TorneoId,
                        principalTable: "Torneo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Partido",
                columns: table => new
                {
                    PartidoID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TorneoID = table.Column<int>(type: "integer", nullable: false),
                    EquipoLocalID = table.Column<int>(type: "integer", nullable: false),
                    EquipoVisitanteID = table.Column<int>(type: "integer", nullable: false),
                    GanadorPenalesID = table.Column<int>(type: "integer", nullable: true),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Fase = table.Column<string>(type: "text", nullable: false),
                    ResultadoLocal = table.Column<int>(type: "integer", nullable: true),
                    ResultadoVisitante = table.Column<int>(type: "integer", nullable: true),
                    PuntosLocal = table.Column<int>(type: "integer", nullable: false),
                    PuntosVisitante = table.Column<int>(type: "integer", nullable: false),
                    EstadoPartido = table.Column<string>(type: "text", nullable: false),
                    VersionResultado = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partido", x => x.PartidoID);
                    table.ForeignKey(
                        name: "FK_Partido_Equipo_EquipoLocalID",
                        column: x => x.EquipoLocalID,
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Partido_Equipo_EquipoVisitanteID",
                        column: x => x.EquipoVisitanteID,
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Partido_Equipo_GanadorPenalesID",
                        column: x => x.GanadorPenalesID,
                        principalTable: "Equipo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Partido_Torneo_TorneoID",
                        column: x => x.TorneoID,
                        principalTable: "Torneo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventoPartido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartidoID = table.Column<int>(type: "integer", nullable: false),
                    JugadorID = table.Column<int>(type: "integer", nullable: false),
                    EquipoID = table.Column<int>(type: "integer", nullable: false),
                    TipoEvento = table.Column<string>(type: "text", nullable: false),
                    Minuto = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoPartido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventoPartido_Equipo_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventoPartido_Jugador_JugadorID",
                        column: x => x.JugadorID,
                        principalTable: "Jugador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventoPartido_Partido_PartidoID",
                        column: x => x.PartidoID,
                        principalTable: "Partido",
                        principalColumn: "PartidoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventoPartido_EquipoID",
                table: "EventoPartido",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_EventoPartido_JugadorID",
                table: "EventoPartido",
                column: "JugadorID");

            migrationBuilder.CreateIndex(
                name: "IX_EventoPartido_PartidoID",
                table: "EventoPartido",
                column: "PartidoID");

            migrationBuilder.CreateIndex(
                name: "IX_InscripcionEquipo_EquipoID",
                table: "InscripcionEquipo",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_InscripcionEquipo_TorneoId",
                table: "InscripcionEquipo",
                column: "TorneoId");

            migrationBuilder.CreateIndex(
                name: "IX_InscripcionJugador_EquipoID",
                table: "InscripcionJugador",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_InscripcionJugador_JugadorID",
                table: "InscripcionJugador",
                column: "JugadorID");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_EquipoLocalID",
                table: "Partido",
                column: "EquipoLocalID");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_EquipoVisitanteID",
                table: "Partido",
                column: "EquipoVisitanteID");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_GanadorPenalesID",
                table: "Partido",
                column: "GanadorPenalesID");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_TorneoID",
                table: "Partido",
                column: "TorneoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventoPartido");

            migrationBuilder.DropTable(
                name: "InscripcionEquipo");

            migrationBuilder.DropTable(
                name: "InscripcionJugador");

            migrationBuilder.DropTable(
                name: "Partido");

            migrationBuilder.DropTable(
                name: "Jugador");

            migrationBuilder.DropTable(
                name: "Equipo");

            migrationBuilder.DropTable(
                name: "Torneo");
        }
    }
}
