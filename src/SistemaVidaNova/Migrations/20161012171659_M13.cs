using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaVidaNova.Migrations
{
    public partial class M13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoacaoDinheiro",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CodDoador = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    Valor = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoacaoDinheiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoacaoDinheiro_Doador_CodDoador",
                        column: x => x.CodDoador,
                        principalTable: "Doador",
                        principalColumn: "CodDoador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoacaoObjeto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CodDoador = table.Column<int>(nullable: false),
                    DataDaDoacao = table.Column<DateTime>(nullable: false),
                    DataDeRetirada = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    IdEndereco = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoacaoObjeto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoacaoObjeto_Doador_CodDoador",
                        column: x => x.CodDoador,
                        principalTable: "Doador",
                        principalColumn: "CodDoador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoacaoObjeto_Endereco_IdEndereco",
                        column: x => x.IdEndereco,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoacaoSopa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CodDoador = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    IdItem = table.Column<int>(nullable: false),
                    Quantidade = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoacaoSopa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoacaoSopa_Doador_CodDoador",
                        column: x => x.CodDoador,
                        principalTable: "Doador",
                        principalColumn: "CodDoador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoacaoSopa_Item_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoDinheiro_CodDoador",
                table: "DoacaoDinheiro",
                column: "CodDoador");

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoObjeto_CodDoador",
                table: "DoacaoObjeto",
                column: "CodDoador");

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoObjeto_IdEndereco",
                table: "DoacaoObjeto",
                column: "IdEndereco");

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoSopa_CodDoador",
                table: "DoacaoSopa",
                column: "CodDoador");

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoSopa_IdItem",
                table: "DoacaoSopa",
                column: "IdItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoacaoDinheiro");

            migrationBuilder.DropTable(
                name: "DoacaoObjeto");

            migrationBuilder.DropTable(
                name: "DoacaoSopa");
        }
    }
}
