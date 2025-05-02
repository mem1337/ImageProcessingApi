using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageProcessingService.Migrations
{
    /// <inheritdoc />
    public partial class Salt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserSalt",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserSalt",
                table: "user");
        }
    }
}
