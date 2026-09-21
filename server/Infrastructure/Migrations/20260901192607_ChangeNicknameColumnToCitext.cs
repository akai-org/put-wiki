using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNicknameColumnToCitext : Migration
    {
        private static readonly string[] _nicknameCollation = ["und-x-icu"];
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Nickname",
                table: "Users");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Users",
                type: "citext",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Nickname",
                table: "Users",
                column: "Nickname",
                unique: true,
                filter: "\"Nickname\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Nickname",
                table: "Users");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Users",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Nickname",
                table: "Users",
                column: "Nickname",
                unique: true,
                filter: "\"Nickname\" IS NOT NULL")
                .Annotation("Relational:Collation", _nicknameCollation);
        }
    }
}