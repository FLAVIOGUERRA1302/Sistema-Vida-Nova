using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaVidaNova.Migrations
{
    public partial class M14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModeloDeReceita",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    Nome = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloDeReceita", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModeloDeReceitaItem",
                columns: table => new
                {
                    IdItem = table.Column<int>(nullable: false),
                    IdModeloDeReceita = table.Column<int>(nullable: false),
                    Quantidade = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloDeReceitaItem", x => new { x.IdItem, x.IdModeloDeReceita });
                    table.ForeignKey(
                        name: "FK_ModeloDeReceitaItem_Item_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModeloDeReceitaItem_ModeloDeReceita_IdModeloDeReceita",
                        column: x => x.IdModeloDeReceita,
                        principalTable: "ModeloDeReceita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModeloDeReceita_Nome",
                table: "ModeloDeReceita",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModeloDeReceitaItem_IdItem",
                table: "ModeloDeReceitaItem",
                column: "IdItem");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloDeReceitaItem_IdModeloDeReceita",
                table: "ModeloDeReceitaItem",
                column: "IdModeloDeReceita");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModeloDeReceitaItem");

            migrationBuilder.DropTable(
                name: "ModeloDeReceita");
        }
    }
}
