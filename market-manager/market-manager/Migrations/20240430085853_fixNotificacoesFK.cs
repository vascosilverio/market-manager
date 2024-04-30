using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Migrations
{
    /// <inheritdoc />
    public partial class fixNotificacoesFK : Migration
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

            migrationBuilder.RenameColumn(
                name: "VendedorId",
                table: "Notificacoes",
                newName: "VendedorUtilizadorId");

            migrationBuilder.RenameColumn(
                name: "GestorId",
                table: "Notificacoes",
                newName: "GestorUtilizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Notificacoes_VendedorId",
                table: "Notificacoes",
                newName: "IX_Notificacoes_VendedorUtilizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Notificacoes_GestorId",
                table: "Notificacoes",
                newName: "IX_Notificacoes_GestorUtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Gestores_GestorUtilizadorId",
                table: "Notificacoes",
                column: "GestorUtilizadorId",
                principalTable: "Gestores",
                principalColumn: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Vendedores_VendedorUtilizadorId",
                table: "Notificacoes",
                column: "VendedorUtilizadorId",
                principalTable: "Vendedores",
                principalColumn: "UtilizadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Gestores_GestorUtilizadorId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Vendedores_VendedorUtilizadorId",
                table: "Notificacoes");

            migrationBuilder.RenameColumn(
                name: "VendedorUtilizadorId",
                table: "Notificacoes",
                newName: "VendedorId");

            migrationBuilder.RenameColumn(
                name: "GestorUtilizadorId",
                table: "Notificacoes",
                newName: "GestorId");

            migrationBuilder.RenameIndex(
                name: "IX_Notificacoes_VendedorUtilizadorId",
                table: "Notificacoes",
                newName: "IX_Notificacoes_VendedorId");

            migrationBuilder.RenameIndex(
                name: "IX_Notificacoes_GestorUtilizadorId",
                table: "Notificacoes",
                newName: "IX_Notificacoes_GestorId");

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
    }
}
