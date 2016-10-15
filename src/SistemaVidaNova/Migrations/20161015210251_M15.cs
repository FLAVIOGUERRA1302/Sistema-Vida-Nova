using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaVidaNova.Migrations
{
    public partial class M15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultadoSopa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    IdModeloDeReceita = table.Column<int>(nullable: false),
                    LitrosProduzidos = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoSopa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadoSopa_ModeloDeReceita_IdModeloDeReceita",
                        column: x => x.IdModeloDeReceita,
                        principalTable: "ModeloDeReceita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoSopaItem",
                columns: table => new
                {
                    IdItem = table.Column<int>(nullable: false),
                    IdResultadoSopa = table.Column<int>(nullable: false),
                    Quantidade = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoSopaItem", x => new { x.IdItem, x.IdResultadoSopa });
                    table.ForeignKey(
                        name: "FK_ResultadoSopaItem_Item_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultadoSopaItem_ResultadoSopa_IdResultadoSopa",
                        column: x => x.IdResultadoSopa,
                        principalTable: "ResultadoSopa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoSopa_IdModeloDeReceita",
                table: "ResultadoSopa",
                column: "IdModeloDeReceita");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoSopaItem_IdItem",
                table: "ResultadoSopaItem",
                column: "IdItem");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoSopaItem_IdResultadoSopa",
                table: "ResultadoSopaItem",
                column: "IdResultadoSopa");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultadoSopaItem");

            migrationBuilder.DropTable(
                name: "ResultadoSopa");
        }
    }
}
