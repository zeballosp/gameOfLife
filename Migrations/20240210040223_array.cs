using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOfLife_A.Migrations
{
    /// <inheritdoc />
    public partial class array : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalColumns",
                table: "Boards");

            migrationBuilder.RenameColumn(
                name: "InternalRows",
                table: "Boards",
                newName: "InternalArray");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InternalArray",
                table: "Boards",
                newName: "InternalRows");

            migrationBuilder.AddColumn<string>(
                name: "InternalColumns",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
