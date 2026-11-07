using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNickname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AddColumn<string>(
                name: "nickname",
                table: "users",
                type: "citext",
                maxLength: 30,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_nickname",
                table: "users",
                column: "nickname",
                unique: true,
                filter: "\"nickname\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_nickname",
                table: "users");

            migrationBuilder.DropColumn(
                name: "nickname",
                table: "users");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");
        }
    }
}