using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOfLife_A.Migrations
{
    /// <inheritdoc />
    public partial class M2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InternalColumns",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternalRows",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalColumns",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "InternalRows",
                table: "Boards");
        }
    }
}
