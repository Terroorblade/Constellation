using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddUserGoals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "goal",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_goal_UserId",
                table: "goal",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_goal_users_UserId",
                table: "goal",
                column: "UserId",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_goal_users_UserId",
                table: "goal");

            migrationBuilder.DropIndex(
                name: "IX_goal_UserId",
                table: "goal");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "goal");
        }
    }
}
