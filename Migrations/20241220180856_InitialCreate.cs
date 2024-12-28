using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace КурсоваяБД5.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fivesembd");

            migrationBuilder.CreateTable(
                name: "spheres_of_life",
                schema: "fivesembd",
                columns: table => new
                {
                    sphere_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("spheres_of_life_pkey", x => x.sphere_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "fivesembd",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "goal",
                schema: "fivesembd",
                columns: table => new
                {
                    goal_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    create_date = table.Column<DateOnly>(type: "date", nullable: false),
                    deadline = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    goal_sphere = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("goal_pkey", x => x.goal_id);
                    table.ForeignKey(
                        name: "goal_goal_sphere_fkey",
                        column: x => x.goal_sphere,
                        principalSchema: "fivesembd",
                        principalTable: "spheres_of_life",
                        principalColumn: "sphere_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "daily_schedule",
                schema: "fivesembd",
                columns: table => new
                {
                    schedule_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    schedule_data = table.Column<DateOnly>(type: "date", nullable: false),
                    user_schedule = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("daily_schedule_pkey", x => x.schedule_id);
                    table.ForeignKey(
                        name: "daily_schedule_user_schedule_fkey",
                        column: x => x.user_schedule,
                        principalSchema: "fivesembd",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_sphere_satisfaction",
                schema: "fivesembd",
                columns: table => new
                {
                    satisfaction_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    satisfaction_level = table.Column<short>(type: "smallint", nullable: true),
                    user_spheres = table.Column<int>(type: "integer", nullable: true),
                    sphere_ids = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_sphere_satisfaction_pkey", x => x.satisfaction_id);
                    table.ForeignKey(
                        name: "user_sphere_satisfaction_sphere_ids_fkey",
                        column: x => x.sphere_ids,
                        principalSchema: "fivesembd",
                        principalTable: "spheres_of_life",
                        principalColumn: "sphere_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "user_sphere_satisfaction_user_spheres_fkey",
                        column: x => x.user_spheres,
                        principalSchema: "fivesembd",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "habit",
                schema: "fivesembd",
                columns: table => new
                {
                    habit_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    frequency = table.Column<TimeSpan>(type: "interval", nullable: false),
                    reminder = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    goal_habit = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("habit_pkey", x => x.habit_id);
                    table.ForeignKey(
                        name: "habit_goal_habit_fkey",
                        column: x => x.goal_habit,
                        principalSchema: "fivesembd",
                        principalTable: "goal",
                        principalColumn: "goal_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "event",
                schema: "fivesembd",
                columns: table => new
                {
                    event_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    event_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    priority = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true, defaultValueSql: "'low'::character varying"),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    event_schedule = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("event_pkey", x => x.event_id);
                    table.ForeignKey(
                        name: "event_event_schedule_fkey",
                        column: x => x.event_schedule,
                        principalSchema: "fivesembd",
                        principalTable: "daily_schedule",
                        principalColumn: "schedule_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "habit_of_the_day",
                schema: "fivesembd",
                columns: table => new
                {
                    habit_day_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    habit_day = table.Column<int>(type: "integer", nullable: true),
                    schedule_day = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("habit_of_the_day_pkey", x => x.habit_day_id);
                    table.ForeignKey(
                        name: "habit_of_the_day_habit_day_fkey",
                        column: x => x.habit_day,
                        principalSchema: "fivesembd",
                        principalTable: "habit",
                        principalColumn: "habit_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "habit_of_the_day_schedule_day_fkey",
                        column: x => x.schedule_day,
                        principalSchema: "fivesembd",
                        principalTable: "daily_schedule",
                        principalColumn: "schedule_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_daily_schedule_user_schedule",
                schema: "fivesembd",
                table: "daily_schedule",
                column: "user_schedule");

            migrationBuilder.CreateIndex(
                name: "IX_event_event_schedule",
                schema: "fivesembd",
                table: "event",
                column: "event_schedule");

            migrationBuilder.CreateIndex(
                name: "IX_goal_goal_sphere",
                schema: "fivesembd",
                table: "goal",
                column: "goal_sphere");

            migrationBuilder.CreateIndex(
                name: "IX_habit_goal_habit",
                schema: "fivesembd",
                table: "habit",
                column: "goal_habit");

            migrationBuilder.CreateIndex(
                name: "IX_habit_of_the_day_habit_day",
                schema: "fivesembd",
                table: "habit_of_the_day",
                column: "habit_day");

            migrationBuilder.CreateIndex(
                name: "IX_habit_of_the_day_schedule_day",
                schema: "fivesembd",
                table: "habit_of_the_day",
                column: "schedule_day");

            migrationBuilder.CreateIndex(
                name: "IX_user_sphere_satisfaction_sphere_ids",
                schema: "fivesembd",
                table: "user_sphere_satisfaction",
                column: "sphere_ids");

            migrationBuilder.CreateIndex(
                name: "IX_user_sphere_satisfaction_user_spheres",
                schema: "fivesembd",
                table: "user_sphere_satisfaction",
                column: "user_spheres");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                schema: "fivesembd",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_username_key",
                schema: "fivesembd",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event",
                schema: "fivesembd");

            migrationBuilder.DropTable(
                name: "habit_of_the_day",
                schema: "fivesembd");

            migrationBuilder.DropTable(
                name: "user_sphere_satisfaction",
                schema: "fivesembd");

            migrationBuilder.DropTable(
                name: "habit",
                schema: "fivesembd");

            migrationBuilder.DropTable(
                name: "daily_schedule",
                schema: "fivesembd");

            migrationBuilder.DropTable(
                name: "goal",
                schema: "fivesembd");

            migrationBuilder.DropTable(
                name: "users",
                schema: "fivesembd");

            migrationBuilder.DropTable(
                name: "spheres_of_life",
                schema: "fivesembd");
        }
    }
}
