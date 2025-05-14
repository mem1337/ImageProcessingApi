using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageProcessingService.Migrations
{
    /// <inheritdoc />
    public partial class ImageModelEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBytes",
                table: "image");

            migrationBuilder.AddColumn<string>(
                name: "ImageLocation",
                table: "image",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "image",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageLocation",
                table: "image");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "image");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageBytes",
                table: "image",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
