using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market_manager.Migrations
{
    /// <inheritdoc />
    public partial class reservabanca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservaBanca");

            migrationBuilder.CreateTable(
                name: "BancasReservas",
                columns: table => new
                {
                    ListaBancasBancaId = table.Column<int>(type: "int", nullable: false),
                    ReservasReservaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BancasReservas", x => new { x.ListaBancasBancaId, x.ReservasReservaId });
                    table.ForeignKey(
                        name: "FK_BancasReservas_Bancas_ListaBancasBancaId",
                        column: x => x.ListaBancasBancaId,
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

            migrationBuilder.CreateIndex(
                name: "IX_BancasReservas_ReservasReservaId",
                table: "BancasReservas",
                column: "ReservasReservaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BancasReservas");

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
        }
    }
}
