using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KayipEsyaPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class AddIlceMahalleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esyalar_Markalar_MarkaId",
                table: "Esyalar");

            migrationBuilder.DropForeignKey(
                name: "FK_Esyalar_Modeller_ModelId",
                table: "Esyalar");

            migrationBuilder.DropIndex(
                name: "IX_Esyalar_MarkaId",
                table: "Esyalar");

            migrationBuilder.DropIndex(
                name: "IX_Esyalar_ModelId",
                table: "Esyalar");

            migrationBuilder.DropColumn(
                name: "MarkaId",
                table: "Esyalar");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Esyalar");

            migrationBuilder.AddColumn<int>(
                name: "MarkaId",
                table: "Modeller",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Marka",
                table: "Esyalar",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Esyalar",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Modeller_MarkaId",
                table: "Modeller",
                column: "MarkaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modeller_Markalar_MarkaId",
                table: "Modeller",
                column: "MarkaId",
                principalTable: "Markalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modeller_Markalar_MarkaId",
                table: "Modeller");

            migrationBuilder.DropIndex(
                name: "IX_Modeller_MarkaId",
                table: "Modeller");

            migrationBuilder.DropColumn(
                name: "MarkaId",
                table: "Modeller");

            migrationBuilder.DropColumn(
                name: "Marka",
                table: "Esyalar");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Esyalar");

            migrationBuilder.AddColumn<int>(
                name: "MarkaId",
                table: "Esyalar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModelId",
                table: "Esyalar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Esyalar_MarkaId",
                table: "Esyalar",
                column: "MarkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Esyalar_ModelId",
                table: "Esyalar",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Esyalar_Markalar_MarkaId",
                table: "Esyalar",
                column: "MarkaId",
                principalTable: "Markalar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Esyalar_Modeller_ModelId",
                table: "Esyalar",
                column: "ModelId",
                principalTable: "Modeller",
                principalColumn: "Id");
        }
    }
}
