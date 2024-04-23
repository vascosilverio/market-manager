using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Migrations
{
    /// <inheritdoc />
    public partial class ModelNotations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumeroIdentificacaoFuncionario",
                table: "Utilizadores",
                newName: "NumIdFuncionario");

            migrationBuilder.RenameColumn(
                name: "EstadoActualBanca",
                table: "Bancas",
                newName: "EstadoAtualBanca");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataAdmissao",
                table: "Utilizadores",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EstadoActualReserva",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataInicio",
                table: "Reservas",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataFim",
                table: "Reservas",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "EstadoActualNotificacao",
                table: "Notificacoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumIdFuncionario",
                table: "Utilizadores",
                newName: "NumeroIdentificacaoFuncionario");

            migrationBuilder.RenameColumn(
                name: "EstadoAtualBanca",
                table: "Bancas",
                newName: "EstadoActualBanca");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAdmissao",
                table: "Utilizadores",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EstadoActualReserva",
                table: "Reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataInicio",
                table: "Reservas",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "Reservas",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<int>(
                name: "EstadoActualNotificacao",
                table: "Notificacoes",
                type: "int",
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
        }
    }
}
