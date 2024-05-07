using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Migrations
{
    /// <inheritdoc />
    public partial class userIdFKUtilizadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Vendedores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Gestores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Vendedores");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Gestores");
        }
    }
}
