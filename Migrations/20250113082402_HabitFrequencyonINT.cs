using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
   public partial class HabitFrequencyonINT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Преобразуем столбец 'frequency' из interval в integer с явным преобразованием
            migrationBuilder.Sql(
                "ALTER TABLE habit " +
                "ALTER COLUMN frequency TYPE integer USING EXTRACT(EPOCH FROM frequency)::integer;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Возвращаем столбец 'frequency' обратно в тип interval
            migrationBuilder.Sql(
                "ALTER TABLE habit " +
                "ALTER COLUMN frequency TYPE interval USING frequency::interval;");
        }
    }
}
