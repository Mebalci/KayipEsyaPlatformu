using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KayipEsyaPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class MahalleYapilandirma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mahalleler_Ilceler_IlceId",
                table: "Mahalleler");

            migrationBuilder.AlterColumn<int>(
                name: "IlceId",
                table: "Mahalleler",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "PostaKodu",
                table: "Mahalleler",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SehirId",
                table: "Mahalleler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SemtId",
                table: "Mahalleler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Mahalleler_SehirId",
                table: "Mahalleler",
                column: "SehirId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mahalleler_Ilceler_IlceId",
                table: "Mahalleler",
                column: "IlceId",
                principalTable: "Ilceler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mahalleler_Sehirler_SehirId",
                table: "Mahalleler",
                column: "SehirId",
                principalTable: "Sehirler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mahalleler_Ilceler_IlceId",
                table: "Mahalleler");

            migrationBuilder.DropForeignKey(
                name: "FK_Mahalleler_Sehirler_SehirId",
                table: "Mahalleler");

            migrationBuilder.DropIndex(
                name: "IX_Mahalleler_SehirId",
                table: "Mahalleler");

            migrationBuilder.DropColumn(
                name: "PostaKodu",
                table: "Mahalleler");

            migrationBuilder.DropColumn(
                name: "SehirId",
                table: "Mahalleler");

            migrationBuilder.DropColumn(
                name: "SemtId",
                table: "Mahalleler");

            migrationBuilder.AlterColumn<int>(
                name: "IlceId",
                table: "Mahalleler",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mahalleler_Ilceler_IlceId",
                table: "Mahalleler",
                column: "IlceId",
                principalTable: "Ilceler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
