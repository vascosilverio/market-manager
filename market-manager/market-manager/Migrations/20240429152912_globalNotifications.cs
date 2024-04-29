using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Migrations
{
    /// <inheritdoc />
    public partial class globalNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Gestores_GestorId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Vendedores_VendedorId",
                table: "Notificacoes");

            migrationBuilder.AlterColumn<int>(
                name: "VendedorId",
                table: "Notificacoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GestorId",
                table: "Notificacoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Gestores_GestorId",
                table: "Notificacoes",
                column: "GestorId",
                principalTable: "Gestores",
                principalColumn: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Vendedores_VendedorId",
                table: "Notificacoes",
                column: "VendedorId",
                principalTable: "Vendedores",
                principalColumn: "UtilizadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Gestores_GestorId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Vendedores_VendedorId",
                table: "Notificacoes");

            migrationBuilder.AlterColumn<int>(
                name: "VendedorId",
                table: "Notificacoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GestorId",
                table: "Notificacoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Gestores_GestorId",
                table: "Notificacoes",
                column: "GestorId",
                principalTable: "Gestores",
                principalColumn: "UtilizadorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Vendedores_VendedorId",
                table: "Notificacoes",
                column: "VendedorId",
                principalTable: "Vendedores",
                principalColumn: "UtilizadorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
