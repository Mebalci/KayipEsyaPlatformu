using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KayipEsyaPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class AddKonumKoordinatlari : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Boylam",
                table: "Esyalar",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Enlem",
                table: "Esyalar",
                type: "double",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Boylam",
                table: "Esyalar");

            migrationBuilder.DropColumn(
                name: "Enlem",
                table: "Esyalar");
        }
    }
}
