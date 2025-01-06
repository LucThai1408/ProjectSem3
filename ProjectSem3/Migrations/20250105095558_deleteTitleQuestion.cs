using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectSem3.Migrations
{
    /// <inheritdoc />
    public partial class deleteTitleQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Question");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Question",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
