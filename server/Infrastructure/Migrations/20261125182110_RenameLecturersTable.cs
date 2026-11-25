using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameLecturersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "academic_teachers",
                newName: "lecturers");

            migrationBuilder.Sql(
                "ALTER TABLE lecturers RENAME CONSTRAINT pk_academic_teachers TO pk_lecturers;");

            migrationBuilder.RenameIndex(
                name: "ix_academic_teachers_email",
                table: "lecturers",
                newName: "ix_lecturers_email");

            migrationBuilder.RenameIndex(
                name: "ix_academic_teachers_slug",
                table: "lecturers",
                newName: "ix_lecturers_slug");

            migrationBuilder.RenameIndex(
                name: "ix_academic_teachers_usos_id",
                table: "lecturers",
                newName: "ix_lecturers_usos_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ix_lecturers_email",
                table: "lecturers",
                newName: "ix_academic_teachers_email");

            migrationBuilder.RenameIndex(
                name: "ix_lecturers_slug",
                table: "lecturers",
                newName: "ix_academic_teachers_slug");

            migrationBuilder.RenameIndex(
                name: "ix_lecturers_usos_id",
                table: "lecturers",
                newName: "ix_academic_teachers_usos_id");

            migrationBuilder.Sql(
                "ALTER TABLE lecturers RENAME CONSTRAINT pk_lecturers TO pk_academic_teachers;");

            migrationBuilder.RenameTable(
                name: "lecturers",
                newName: "academic_teachers");
        }
    }
}