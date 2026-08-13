using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWholeSchemaNamingToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AcademicTeachers",
                table: "AcademicTeachers");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "AcademicTeachers",
                newName: "academic_teachers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "JoinedDate",
                table: "users",
                newName: "joined_date");

            migrationBuilder.RenameColumn(
                name: "HashedUsosId",
                table: "users",
                newName: "hashed_usos_id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_HashedUsosId",
                table: "users",
                newName: "IX_users_hashed_usos_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "academic_teachers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "academic_teachers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "academic_teachers",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Degrees",
                table: "academic_teachers",
                newName: "degrees");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "academic_teachers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WebsiteUrl",
                table: "academic_teachers",
                newName: "website_url");

            migrationBuilder.RenameColumn(
                name: "UsosId",
                table: "academic_teachers",
                newName: "usos_id");

            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "academic_teachers",
                newName: "photo_url");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "academic_teachers",
                newName: "phone_number");

            migrationBuilder.RenameIndex(
                name: "IX_AcademicTeachers_UsosId",
                table: "academic_teachers",
                newName: "IX_academic_teachers_usos_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_academic_teachers",
                table: "academic_teachers",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_academic_teachers",
                table: "academic_teachers");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "academic_teachers",
                newName: "AcademicTeachers");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "joined_date",
                table: "Users",
                newName: "JoinedDate");

            migrationBuilder.RenameColumn(
                name: "hashed_usos_id",
                table: "Users",
                newName: "HashedUsosId");

            migrationBuilder.RenameIndex(
                name: "IX_users_hashed_usos_id",
                table: "Users",
                newName: "IX_Users_HashedUsosId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AcademicTeachers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AcademicTeachers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "AcademicTeachers",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "degrees",
                table: "AcademicTeachers",
                newName: "Degrees");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AcademicTeachers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "website_url",
                table: "AcademicTeachers",
                newName: "WebsiteUrl");

            migrationBuilder.RenameColumn(
                name: "usos_id",
                table: "AcademicTeachers",
                newName: "UsosId");

            migrationBuilder.RenameColumn(
                name: "photo_url",
                table: "AcademicTeachers",
                newName: "PhotoUrl");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "AcademicTeachers",
                newName: "PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_academic_teachers_usos_id",
                table: "AcademicTeachers",
                newName: "IX_AcademicTeachers_UsosId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcademicTeachers",
                table: "AcademicTeachers",
                column: "Id");
        }
    }
}