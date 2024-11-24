using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ristorante360.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTemporaryPasswordToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTemporaryPassword",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Exception_Message",
                table: "Error",
                type: "varchar(4000)",
                unicode: false,
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Error_Message",
                table: "Error",
                type: "varchar(4000)",
                unicode: false,
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldUnicode: false,
                oldMaxLength: 500,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTemporaryPassword",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Exception_Message",
                table: "Error",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldUnicode: false,
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Error_Message",
                table: "Error",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(4000)",
                oldUnicode: false,
                oldMaxLength: 4000,
                oldNullable: true);
        }
    }
}
