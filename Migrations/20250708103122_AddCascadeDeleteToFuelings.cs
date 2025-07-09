using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AkaryakitOtomasyonu.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToFuelings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fuelings_Tanks_TankId",
                table: "Fuelings");

            migrationBuilder.AddForeignKey(
                name: "FK_Fuelings_Tanks_TankId",
                table: "Fuelings",
                column: "TankId",
                principalTable: "Tanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fuelings_Tanks_TankId",
                table: "Fuelings");

            migrationBuilder.AddForeignKey(
                name: "FK_Fuelings_Tanks_TankId",
                table: "Fuelings",
                column: "TankId",
                principalTable: "Tanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
