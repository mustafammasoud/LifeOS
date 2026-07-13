using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeOS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    TasksCreated = table.Column<int>(type: "INTEGER", nullable: false),
                    TasksCompleted = table.Column<int>(type: "INTEGER", nullable: false),
                    StudyMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    HabitsCompleted = table.Column<int>(type: "INTEGER", nullable: false),
                    GoalsCompleted = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyStatistics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyStatistics_Date",
                table: "DailyStatistics",
                column: "Date",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyStatistics");
        }
    }
}
