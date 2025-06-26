using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameSomeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecretCode",
                table: "Urls",
                newName: "StatsUrl");

            migrationBuilder.RenameColumn(
                name: "urlId",
                table: "UrlOpens",
                newName: "UrlId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatsUrl",
                table: "Urls",
                newName: "SecretCode");

            migrationBuilder.RenameColumn(
                name: "UrlId",
                table: "UrlOpens",
                newName: "urlId");
        }
    }
}
