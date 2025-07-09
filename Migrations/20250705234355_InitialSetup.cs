using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AkaryakitOtomasyonu.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fuelings_AspNetUsers_PerformedByUserId",
                table: "Fuelings");

            migrationBuilder.DropForeignKey(
                name: "FK_Fuelings_Tanks_TankId",
                table: "Fuelings");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Fuelings",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fuelings_UserId",
                table: "Fuelings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fuelings_AspNetUsers_PerformedByUserId",
                table: "Fuelings",
                column: "PerformedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fuelings_AspNetUsers_UserId",
                table: "Fuelings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fuelings_Tanks_TankId",
                table: "Fuelings",
                column: "TankId",
                principalTable: "Tanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fuelings_AspNetUsers_PerformedByUserId",
                table: "Fuelings");

            migrationBuilder.DropForeignKey(
                name: "FK_Fuelings_AspNetUsers_UserId",
                table: "Fuelings");

            migrationBuilder.DropForeignKey(
                name: "FK_Fuelings_Tanks_TankId",
                table: "Fuelings");

            migrationBuilder.DropIndex(
                name: "IX_Fuelings_UserId",
                table: "Fuelings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Fuelings");

            migrationBuilder.AddForeignKey(
                name: "FK_Fuelings_AspNetUsers_PerformedByUserId",
                table: "Fuelings",
                column: "PerformedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fuelings_Tanks_TankId",
                table: "Fuelings",
                column: "TankId",
                principalTable: "Tanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
