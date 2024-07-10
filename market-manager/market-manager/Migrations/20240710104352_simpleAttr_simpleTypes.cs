using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Migrations
{
    /// <inheritdoc />
    public partial class simpleAttr_simpleTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Vendedores_VendedorUtilizadorId",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "Gestores");

            migrationBuilder.DropTable(
                name: "Vendedores");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_VendedorUtilizadorId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "VendedorUtilizadorId",
                table: "Reservas");

            migrationBuilder.AddColumn<string>(
                name: "UtilizadorId",
                table: "Reservas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FotografiaBanca",
                table: "Bancas",
                type: "nvarchar(max)",
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
                name: "CodigoPostal",
                table: "AspNetUsers",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DataNascimento",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

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
                name: "PrimeiroNome",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telemovel",
                table: "AspNetUsers",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UltimoNome",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UtilizadorId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_UtilizadorId",
                table: "Reservas",
                column: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_AspNetUsers_UtilizadorId",
                table: "Reservas",
                column: "UtilizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_AspNetUsers_UtilizadorId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_UtilizadorId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "FotografiaBanca",
                table: "Bancas");

            migrationBuilder.DropColumn(
                name: "CC",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
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
                name: "PrimeiroNome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Telemovel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UltimoNome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "VendedorUtilizadorId",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Gestores",
                columns: table => new
                {
                    UtilizadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CC = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    DataAdmissao = table.Column<DateOnly>(type: "date", nullable: false),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Localidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NIF = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    NumIdFuncionario = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PrimeiroNome = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    UltimoNome = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gestores", x => x.UtilizadorId);
                });

            migrationBuilder.CreateTable(
                name: "Vendedores",
                columns: table => new
                {
                    UtilizadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CC = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    DataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    DocumentoCartaoComerciante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoActualRegisto = table.Column<int>(type: "int", nullable: false),
                    Localidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NIF = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    NISS = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PrimeiroNome = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    UltimoNome = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendedores", x => x.UtilizadorId);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    NotificacaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GestorUtilizadorId = table.Column<int>(type: "int", nullable: true),
                    VendedorUtilizadorId = table.Column<int>(type: "int", nullable: true),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoActualNotificacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.NotificacaoId);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Gestores_GestorUtilizadorId",
                        column: x => x.GestorUtilizadorId,
                        principalTable: "Gestores",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Vendedores_VendedorUtilizadorId",
                        column: x => x.VendedorUtilizadorId,
                        principalTable: "Vendedores",
                        principalColumn: "UtilizadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_VendedorUtilizadorId",
                table: "Reservas",
                column: "VendedorUtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_GestorUtilizadorId",
                table: "Notificacoes",
                column: "GestorUtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_VendedorUtilizadorId",
                table: "Notificacoes",
                column: "VendedorUtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Vendedores_VendedorUtilizadorId",
                table: "Reservas",
                column: "VendedorUtilizadorId",
                principalTable: "Vendedores",
                principalColumn: "UtilizadorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
