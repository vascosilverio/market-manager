using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Migrations
{
    /// <inheritdoc />
    public partial class atributoVendedores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentoCC",
                table: "Vendedores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentoCC",
                table: "Vendedores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
