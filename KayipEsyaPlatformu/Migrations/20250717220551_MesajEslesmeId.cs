using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KayipEsyaPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class MesajEslesmeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EslesmeId",
                table: "Mesajlar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Mesajlar_EslesmeId",
                table: "Mesajlar",
                column: "EslesmeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mesajlar_Eslesmeler_EslesmeId",
                table: "Mesajlar",
                column: "EslesmeId",
                principalTable: "Eslesmeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mesajlar_Eslesmeler_EslesmeId",
                table: "Mesajlar");

            migrationBuilder.DropIndex(
                name: "IX_Mesajlar_EslesmeId",
                table: "Mesajlar");

            migrationBuilder.DropColumn(
                name: "EslesmeId",
                table: "Mesajlar");
        }
    }
}
