using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Migrations
{
    /// <inheritdoc />
    public partial class fixReservasFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Vendedores_VendedorId",
                table: "Reservas");

            migrationBuilder.RenameColumn(
                name: "VendedorId",
                table: "Reservas",
                newName: "VendedorUtilizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_VendedorId",
                table: "Reservas",
                newName: "IX_Reservas_VendedorUtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Vendedores_VendedorUtilizadorId",
                table: "Reservas",
                column: "VendedorUtilizadorId",
                principalTable: "Vendedores",
                principalColumn: "UtilizadorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Vendedores_VendedorUtilizadorId",
                table: "Reservas");

            migrationBuilder.RenameColumn(
                name: "VendedorUtilizadorId",
                table: "Reservas",
                newName: "VendedorId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_VendedorUtilizadorId",
                table: "Reservas",
                newName: "IX_Reservas_VendedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Vendedores_VendedorId",
                table: "Reservas",
                column: "VendedorId",
                principalTable: "Vendedores",
                principalColumn: "UtilizadorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
