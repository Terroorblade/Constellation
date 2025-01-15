using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToHabit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "habit",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_habit_UserId",
                table: "habit",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_habit_users_UserId",
                table: "habit",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_habit_users_UserId",
                table: "habit");

            migrationBuilder.DropIndex(
                name: "IX_habit_UserId",
                table: "habit");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "habit");
        }
    }
}
