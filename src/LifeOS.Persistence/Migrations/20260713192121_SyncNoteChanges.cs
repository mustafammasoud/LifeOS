using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeOS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncNoteChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_NoteFolders_FolderId",
                table: "Notes");

            migrationBuilder.DropTable(
                name: "NoteFolders");

            migrationBuilder.DropIndex(
                name: "IX_Notes_FolderId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Notes");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Notes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Notes");

            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                table: "Notes",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "NoteFolders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteFolders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_FolderId",
                table: "Notes",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_NoteFolders_FolderId",
                table: "Notes",
                column: "FolderId",
                principalTable: "NoteFolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
