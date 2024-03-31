using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Data.Migrations
{
    /// <inheritdoc />
    public partial class marketmanager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bancas",
                columns: table => new
                {
                    BancaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeBanca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriaProdutos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Largura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comprimento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LocalizacaoX = table.Column<int>(type: "int", nullable: false),
                    LocalizacaoY = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bancas", x => x.BancaId);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    UtilizadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeUtilizador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PalavraPasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoUtilizador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NIF = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.UtilizadorId);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    ReservaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtilizadorId = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.ReservaId);
                    table.ForeignKey(
                        name: "FK_Reservas_Utilizadores_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "Utilizadores",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BancasReservas",
                columns: table => new
                {
                    BancasBancaId = table.Column<int>(type: "int", nullable: false),
                    ReservasReservaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BancasReservas", x => new { x.BancasBancaId, x.ReservasReservaId });
                    table.ForeignKey(
                        name: "FK_BancasReservas_Bancas_BancasBancaId",
                        column: x => x.BancasBancaId,
                        principalTable: "Bancas",
                        principalColumn: "BancaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BancasReservas_Reservas_ReservasReservaId",
                        column: x => x.ReservasReservaId,
                        principalTable: "Reservas",
                        principalColumn: "ReservaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    NotificacaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinatarioId = table.Column<int>(type: "int", nullable: false),
                    RegistoId = table.Column<int>(type: "int", nullable: false),
                    DestinatarioTipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservaId = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UtilizadoresUtilizadorId = table.Column<int>(type: "int", nullable: true),
                    UtilizadoresUtilizadorId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.NotificacaoId);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "ReservaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Utilizadores_DestinatarioId",
                        column: x => x.DestinatarioId,
                        principalTable: "Utilizadores",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Utilizadores_RegistoId",
                        column: x => x.RegistoId,
                        principalTable: "Utilizadores",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Utilizadores_UtilizadoresUtilizadorId",
                        column: x => x.UtilizadoresUtilizadorId,
                        principalTable: "Utilizadores",
                        principalColumn: "UtilizadorId");
                    table.ForeignKey(
                        name: "FK_Notificacoes_Utilizadores_UtilizadoresUtilizadorId1",
                        column: x => x.UtilizadoresUtilizadorId1,
                        principalTable: "Utilizadores",
                        principalColumn: "UtilizadorId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BancasReservas_ReservasReservaId",
                table: "BancasReservas",
                column: "ReservasReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_DestinatarioId",
                table: "Notificacoes",
                column: "DestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_RegistoId",
                table: "Notificacoes",
                column: "RegistoId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_ReservaId",
                table: "Notificacoes",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_UtilizadoresUtilizadorId",
                table: "Notificacoes",
                column: "UtilizadoresUtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_UtilizadoresUtilizadorId1",
                table: "Notificacoes",
                column: "UtilizadoresUtilizadorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_UtilizadorId",
                table: "Reservas",
                column: "UtilizadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BancasReservas");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "Bancas");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Utilizadores");
        }
    }
}
