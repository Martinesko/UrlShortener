using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUrlReferenceToOpenUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UrlOpens_UrlId",
                table: "UrlOpens",
                column: "UrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_UrlOpens_Urls_UrlId",
                table: "UrlOpens",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UrlOpens_Urls_UrlId",
                table: "UrlOpens");

            migrationBuilder.DropIndex(
                name: "IX_UrlOpens_UrlId",
                table: "UrlOpens");
        }
    }
}
