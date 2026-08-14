using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExternalPackageHandlesSnakeCaseNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_users_hashed_usos_id",
                table: "users",
                newName: "ix_users_hashed_usos_id");

            migrationBuilder.RenameIndex(
                name: "IX_academic_teachers_usos_id",
                table: "academic_teachers",
                newName: "ix_academic_teachers_usos_id");

            migrationBuilder.RenameIndex(
                name: "IX_academic_teachers_slug",
                table: "academic_teachers",
                newName: "ix_academic_teachers_slug");

            migrationBuilder.RenameIndex(
                name: "IX_academic_teachers_email",
                table: "academic_teachers",
                newName: "ix_academic_teachers_email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ix_users_hashed_usos_id",
                table: "users",
                newName: "IX_users_hashed_usos_id");

            migrationBuilder.RenameIndex(
                name: "ix_academic_teachers_usos_id",
                table: "academic_teachers",
                newName: "IX_academic_teachers_usos_id");

            migrationBuilder.RenameIndex(
                name: "ix_academic_teachers_slug",
                table: "academic_teachers",
                newName: "IX_academic_teachers_slug");

            migrationBuilder.RenameIndex(
                name: "ix_academic_teachers_email",
                table: "academic_teachers",
                newName: "IX_academic_teachers_email");
        }
    }
}