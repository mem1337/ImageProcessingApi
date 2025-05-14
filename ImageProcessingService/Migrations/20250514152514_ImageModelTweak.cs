using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageProcessingService.Migrations
{
    /// <inheritdoc />
    public partial class ImageModelTweak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageSize",
                table: "image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageSize",
                table: "image",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
