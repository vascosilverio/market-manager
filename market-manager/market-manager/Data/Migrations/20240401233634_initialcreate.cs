using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Reservas_ReservaId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Utilizadores_DestinatarioId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Utilizadores_RegistoId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Utilizadores_UtilizadoresUtilizadorId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Utilizadores_UtilizadoresUtilizadorId1",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Utilizadores_UtilizadorId",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "BancasReservas");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_RegistoId",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_ReservaId",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_UtilizadoresUtilizadorId",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "DestinatarioTipo",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "RegistoId",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "ReservaId",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "CategoriaProdutos",
                table: "Bancas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Bancas");

            migrationBuilder.DropColumn(
                name: "NomeBanca",
                table: "Bancas");

            migrationBuilder.RenameColumn(
                name: "UtilizadoresUtilizadorId1",
                table: "Notificacoes",
                newName: "ReservasReservaId");

            migrationBuilder.RenameColumn(
                name: "UtilizadoresUtilizadorId",
                table: "Notificacoes",
                newName: "EstadoActualNotificacao");

            migrationBuilder.RenameIndex(
                name: "IX_Notificacoes_UtilizadoresUtilizadorId1",
                table: "Notificacoes",
                newName: "IX_Notificacoes_ReservasReservaId");

            migrationBuilder.AlterColumn<string>(
                name: "UtilizadorId",
                table: "Reservas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "Reservas",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "EstadoActualReserva",
                table: "Reservas",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DestinatarioId",
                table: "Notificacoes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "Notificacoes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaBanca",
                table: "Bancas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstadoActualBanca",
                table: "Bancas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NomeIdentificadorBanca",
                table: "Bancas",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CC",
                table: "AspNetUsers",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cargo",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "AspNetUsers",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAdmissao",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DataNascimento",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Departamento",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "DocumentoCC",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "DocumentoCartaoComerciante",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstadoActualRegisto",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Localidade",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Morada",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NIF",
                table: "AspNetUsers",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NISS",
                table: "AspNetUsers",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroIdentificacaoFuncionario",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroTelemovel",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimeiroNome",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UltimoNome",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ReservaBanca",
                columns: table => new
                {
                    BancaId = table.Column<int>(type: "int", nullable: false),
                    ReservaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaBanca", x => new { x.BancaId, x.ReservaId });
                    table.ForeignKey(
                        name: "FK_ReservaBanca_Bancas_BancaId",
                        column: x => x.BancaId,
                        principalTable: "Bancas",
                        principalColumn: "BancaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservaBanca_Reservas_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reservas",
                        principalColumn: "ReservaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservaBanca_ReservaId",
                table: "ReservaBanca",
                column: "ReservaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_AspNetUsers_DestinatarioId",
                table: "Notificacoes",
                column: "DestinatarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Reservas_ReservasReservaId",
                table: "Notificacoes",
                column: "ReservasReservaId",
                principalTable: "Reservas",
                principalColumn: "ReservaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_AspNetUsers_UtilizadorId",
                table: "Reservas",
                column: "UtilizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_AspNetUsers_DestinatarioId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Reservas_ReservasReservaId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_AspNetUsers_UtilizadorId",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "ReservaBanca");

            migrationBuilder.DropColumn(
                name: "EstadoActualReserva",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "CategoriaBanca",
                table: "Bancas");

            migrationBuilder.DropColumn(
                name: "EstadoActualBanca",
                table: "Bancas");

            migrationBuilder.DropColumn(
                name: "NomeIdentificadorBanca",
                table: "Bancas");

            migrationBuilder.DropColumn(
                name: "CC",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Cargo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataAdmissao",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DocumentoCC",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DocumentoCartaoComerciante",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EstadoActualRegisto",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Localidade",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Morada",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NIF",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NISS",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NumeroIdentificacaoFuncionario",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NumeroTelemovel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimeiroNome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UltimoNome",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ReservasReservaId",
                table: "Notificacoes",
                newName: "UtilizadoresUtilizadorId1");

            migrationBuilder.RenameColumn(
                name: "EstadoActualNotificacao",
                table: "Notificacoes",
                newName: "UtilizadoresUtilizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Notificacoes_ReservasReservaId",
                table: "Notificacoes",
                newName: "IX_Notificacoes_UtilizadoresUtilizadorId1");

            migrationBuilder.AlterColumn<int>(
                name: "UtilizadorId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "Reservas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "DestinatarioId",
                table: "Notificacoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCriacao",
                table: "Notificacoes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinatarioTipo",
                table: "Notificacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Notificacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RegistoId",
                table: "Notificacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReservaId",
                table: "Notificacoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Notificacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CategoriaProdutos",
                table: "Bancas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Bancas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeBanca",
                table: "Bancas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "Utilizadores",
                columns: table => new
                {
                    UtilizadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NIF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeUtilizador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PalavraPasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoUtilizador = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.UtilizadorId);
                });

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
                name: "IX_BancasReservas_ReservasReservaId",
                table: "BancasReservas",
                column: "ReservasReservaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Reservas_ReservaId",
                table: "Notificacoes",
                column: "ReservaId",
                principalTable: "Reservas",
                principalColumn: "ReservaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Utilizadores_DestinatarioId",
                table: "Notificacoes",
                column: "DestinatarioId",
                principalTable: "Utilizadores",
                principalColumn: "UtilizadorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Utilizadores_RegistoId",
                table: "Notificacoes",
                column: "RegistoId",
                principalTable: "Utilizadores",
                principalColumn: "UtilizadorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Utilizadores_UtilizadoresUtilizadorId",
                table: "Notificacoes",
                column: "UtilizadoresUtilizadorId",
                principalTable: "Utilizadores",
                principalColumn: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Utilizadores_UtilizadoresUtilizadorId1",
                table: "Notificacoes",
                column: "UtilizadoresUtilizadorId1",
                principalTable: "Utilizadores",
                principalColumn: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Utilizadores_UtilizadorId",
                table: "Reservas",
                column: "UtilizadorId",
                principalTable: "Utilizadores",
                principalColumn: "UtilizadorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
