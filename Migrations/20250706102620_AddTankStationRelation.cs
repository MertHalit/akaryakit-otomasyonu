using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AkaryakitOtomasyonu.Migrations
{
    /// <inheritdoc />
    public partial class AddTankStationRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "Tanks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tanks_StationId",
                table: "Tanks",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tanks_Stations_StationId",
                table: "Tanks",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tanks_Stations_StationId",
                table: "Tanks");

            migrationBuilder.DropIndex(
                name: "IX_Tanks_StationId",
                table: "Tanks");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Tanks");
        }
    }
}
