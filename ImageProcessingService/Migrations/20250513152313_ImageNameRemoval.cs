using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageProcessingService.Migrations
{
    /// <inheritdoc />
    public partial class ImageNameRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "image",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
