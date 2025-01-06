using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class FixUserScheduleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "daily_schedule_user_schedule_fkey",
                table: "daily_schedule");

            migrationBuilder.DropForeignKey(
                name: "user_sphere_satisfaction_user_spheres_fkey",
                table: "user_sphere_satisfaction");

            migrationBuilder.DropTable(
                name: "AspNetRoleAspNetUser");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "AspNetRole");

            migrationBuilder.DropTable(
                name: "AspNetUser");

            migrationBuilder.AlterColumn<string>(
                name: "user_spheres",
                table: "user_sphere_satisfaction",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_schedule",
                table: "daily_schedule",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Birthday",
                table: "AspNetUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "daily_schedule_user_schedule_fkey",
                table: "daily_schedule",
                column: "user_schedule",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "user_sphere_satisfaction_user_spheres_fkey",
                table: "user_sphere_satisfaction",
                column: "user_spheres",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "daily_schedule_user_schedule_fkey",
                table: "daily_schedule");

            migrationBuilder.DropForeignKey(
                name: "user_sphere_satisfaction_user_spheres_fkey",
                table: "user_sphere_satisfaction");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "user_spheres",
                table: "user_sphere_satisfaction",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_schedule",
                table: "daily_schedule",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AspNetRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleAspNetUser",
                columns: table => new
                {
                    RolesId = table.Column<string>(type: "text", nullable: false),
                    UsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleAspNetUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AspNetRoleAspNetUser_AspNetRole_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetRoleAspNetUser_AspNetUser_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityUserId1 = table.Column<string>(type: "text", nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    identity_user_id = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_users_AspNetUser_IdentityUserId1",
                        column: x => x.IdentityUserId1,
                        principalTable: "AspNetUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_identity_user_id",
                        column: x => x.identity_user_id,
                        principalTable: "AspNetUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleAspNetUser_UsersId",
                table: "AspNetRoleAspNetUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_users_identity_user_id",
                table: "users",
                column: "identity_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_IdentityUserId1",
                table: "users",
                column: "IdentityUserId1");

            migrationBuilder.AddForeignKey(
                name: "daily_schedule_user_schedule_fkey",
                table: "daily_schedule",
                column: "user_schedule",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "user_sphere_satisfaction_user_spheres_fkey",
                table: "user_sphere_satisfaction",
                column: "user_spheres",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
