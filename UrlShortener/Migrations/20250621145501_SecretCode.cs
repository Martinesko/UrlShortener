using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Migrations
{
    /// <inheritdoc />
    public partial class SecretCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_urls",
                table: "urls");

            migrationBuilder.RenameTable(
                name: "urls",
                newName: "Urls");

            migrationBuilder.AddColumn<string>(
                name: "SecretCode",
                table: "Urls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Urls",
                table: "Urls",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Urls",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "SecretCode",
                table: "Urls");

            migrationBuilder.RenameTable(
                name: "Urls",
                newName: "urls");

            migrationBuilder.AddPrimaryKey(
                name: "PK_urls",
                table: "urls",
                column: "Id");
        }
    }
}
