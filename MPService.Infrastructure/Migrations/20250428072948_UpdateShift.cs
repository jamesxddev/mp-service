using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MPService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Users_UserId",
                table: "Shift");

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Users_UserId",
                table: "Shift",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shift_Users_UserId",
                table: "Shift");

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_Users_UserId",
                table: "Shift",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
