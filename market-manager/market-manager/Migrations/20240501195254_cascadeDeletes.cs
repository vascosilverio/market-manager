using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Migrations
{
    /// <inheritdoc />
    public partial class cascadeDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Gestores_GestorUtilizadorId",
                table: "Notificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Vendedores_VendedorUtilizadorId",
                table: "Notificacoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Gestores_GestorUtilizadorId",
                table: "Notificacoes",
                column: "GestorUtilizadorId",
                principalTable: "Gestores",
                principalColumn: "UtilizadorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Vendedores_VendedorUtilizadorId",
                table: "Notificacoes",
                column: "VendedorUtilizadorId",
                principalTable: "Vendedores",
                principalColumn: "UtilizadorId",
                onDelete: ReferentialAction.Cascade);
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
    }
}
