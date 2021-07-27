using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HikingApp_RSWEB.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Planinar",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(nullable: true),
                    Vozrast = table.Column<int>(nullable: false),
                    ProfilePicture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planinar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vodich",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(nullable: true),
                    Pozicija = table.Column<string>(nullable: true),
                    Vozrast = table.Column<int>(nullable: false),
                    ProfilePicture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vodich", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tura",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mesto = table.Column<string>(nullable: true),
                    DatumPocetok = table.Column<DateTime>(nullable: false),
                    DatumKraj = table.Column<DateTime>(nullable: false),
                    Tezina = table.Column<string>(nullable: true),
                    Vremetraenje = table.Column<string>(nullable: true),
                    FirstVodichId = table.Column<int>(nullable: true),
                    SecoundVodichId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tura_Vodich_FirstVodichId",
                        column: x => x.FirstVodichId,
                        principalTable: "Vodich",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tura_Vodich_SecoundVodichId",
                        column: x => x.SecoundVodichId,
                        principalTable: "Vodich",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacii",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TuraId = table.Column<int>(nullable: false),
                    PlaninarId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacii", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervacii_Planinar_PlaninarId",
                        column: x => x.PlaninarId,
                        principalTable: "Planinar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervacii_Tura_TuraId",
                        column: x => x.TuraId,
                        principalTable: "Tura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacii_PlaninarId",
                table: "Rezervacii",
                column: "PlaninarId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacii_TuraId",
                table: "Rezervacii",
                column: "TuraId");

            migrationBuilder.CreateIndex(
                name: "IX_Tura_FirstVodichId",
                table: "Tura",
                column: "FirstVodichId");

            migrationBuilder.CreateIndex(
                name: "IX_Tura_SecoundVodichId",
                table: "Tura",
                column: "SecoundVodichId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rezervacii");

            migrationBuilder.DropTable(
                name: "Planinar");

            migrationBuilder.DropTable(
                name: "Tura");

            migrationBuilder.DropTable(
                name: "Vodich");
        }
    }
}
