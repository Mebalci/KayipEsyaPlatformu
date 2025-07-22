using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KayipEsyaPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class EsyaIliskileriEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IlceId",
                table: "Esyalar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MahalleId",
                table: "Esyalar",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "RenkId",
                table: "Esyalar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SehirId",
                table: "Esyalar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Markalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markalar", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Modeller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modeller", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Renkler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Renkler", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sehirler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sehirler", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ilceler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SehirId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilceler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ilceler_Sehirler_SehirId",
                        column: x => x.SehirId,
                        principalTable: "Sehirler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mahalleler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IlceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahalleler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mahalleler_Ilceler_IlceId",
                        column: x => x.IlceId,
                        principalTable: "Ilceler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Esyalar_IlceId",
                table: "Esyalar",
                column: "IlceId");

            migrationBuilder.CreateIndex(
                name: "IX_Esyalar_MahalleId",
                table: "Esyalar",
                column: "MahalleId");

            migrationBuilder.CreateIndex(
                name: "IX_Esyalar_MarkaId",
                table: "Esyalar",
                column: "MarkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Esyalar_ModelId",
                table: "Esyalar",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Esyalar_RenkId",
                table: "Esyalar",
                column: "RenkId");

            migrationBuilder.CreateIndex(
                name: "IX_Esyalar_SehirId",
                table: "Esyalar",
                column: "SehirId");

            migrationBuilder.CreateIndex(
                name: "IX_Ilceler_SehirId",
                table: "Ilceler",
                column: "SehirId");

            migrationBuilder.CreateIndex(
                name: "IX_Mahalleler_IlceId",
                table: "Mahalleler",
                column: "IlceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Esyalar_Ilceler_IlceId",
                table: "Esyalar",
                column: "IlceId",
                principalTable: "Ilceler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Esyalar_Mahalleler_MahalleId",
                table: "Esyalar",
                column: "MahalleId",
                principalTable: "Mahalleler",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Esyalar_Renkler_RenkId",
                table: "Esyalar",
                column: "RenkId",
                principalTable: "Renkler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Esyalar_Sehirler_SehirId",
                table: "Esyalar",
                column: "SehirId",
                principalTable: "Sehirler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esyalar_Ilceler_IlceId",
                table: "Esyalar");

            migrationBuilder.DropForeignKey(
                name: "FK_Esyalar_Mahalleler_MahalleId",
                table: "Esyalar");

            migrationBuilder.DropForeignKey(
                name: "FK_Esyalar_Markalar_MarkaId",
                table: "Esyalar");

            migrationBuilder.DropForeignKey(
                name: "FK_Esyalar_Modeller_ModelId",
                table: "Esyalar");

            migrationBuilder.DropForeignKey(
                name: "FK_Esyalar_Renkler_RenkId",
                table: "Esyalar");

            migrationBuilder.DropForeignKey(
                name: "FK_Esyalar_Sehirler_SehirId",
                table: "Esyalar");

            migrationBuilder.DropTable(
                name: "Mahalleler");

            migrationBuilder.DropTable(
                name: "Markalar");

            migrationBuilder.DropTable(
                name: "Modeller");

            migrationBuilder.DropTable(
                name: "Renkler");

            migrationBuilder.DropTable(
                name: "Ilceler");

            migrationBuilder.DropTable(
                name: "Sehirler");

            migrationBuilder.DropIndex(
                name: "IX_Esyalar_IlceId",
                table: "Esyalar");

            migrationBuilder.DropIndex(
                name: "IX_Esyalar_MahalleId",
                table: "Esyalar");

            migrationBuilder.DropIndex(
                name: "IX_Esyalar_MarkaId",
                table: "Esyalar");

            migrationBuilder.DropIndex(
                name: "IX_Esyalar_ModelId",
                table: "Esyalar");

            migrationBuilder.DropIndex(
                name: "IX_Esyalar_RenkId",
                table: "Esyalar");

            migrationBuilder.DropIndex(
                name: "IX_Esyalar_SehirId",
                table: "Esyalar");

            migrationBuilder.DropColumn(
                name: "IlceId",
                table: "Esyalar");

            migrationBuilder.DropColumn(
                name: "MahalleId",
                table: "Esyalar");

            migrationBuilder.DropColumn(
                name: "MarkaId",
                table: "Esyalar");

            migrationBuilder.DropColumn(
                name: "ModelId",
                table: "Esyalar");

            migrationBuilder.DropColumn(
                name: "RenkId",
                table: "Esyalar");

            migrationBuilder.DropColumn(
                name: "SehirId",
                table: "Esyalar");
        }
    }
}
