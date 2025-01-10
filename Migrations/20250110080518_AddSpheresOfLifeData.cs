using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddSpheresOfLifeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    INSERT INTO Spheres_Of_Life (sphere_id, name) VALUES
                    (1, 'Саморазвитие'),
                    (2, 'Здоровье'),
                    (3, 'Отдых'),
                    (4, 'Окружение'),
                    (5, 'Любовь'),
                    (6, 'Карьера'),
                    (7, 'Финансы'),
                    (8, 'Духовность');
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.Sql(@"
            //         DELETE FROM Spheres_Of_Life WHERE sphere_id BETWEEN 1 AND 8;
            //     ");
        }
    }
}
