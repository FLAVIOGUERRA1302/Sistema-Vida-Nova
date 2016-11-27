using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaVidaNova.Migrations
{
    public partial class M6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doador_Informativo_InformativoId",
                table: "Doador");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Informativo_InformativoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Voluntario_Informativo_InformativoId",
                table: "Voluntario");

            migrationBuilder.DropIndex(
                name: "IX_Voluntario_InformativoId",
                table: "Voluntario");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InformativoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Doador_InformativoId",
                table: "Doador");

            migrationBuilder.DropColumn(
                name: "InformativoId",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "InformativoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InformativoId",
                table: "Doador");

            migrationBuilder.CreateTable(
                name: "InformativoDoador",
                columns: table => new
                {
                    CodDoador = table.Column<int>(nullable: false),
                    IdInformativo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformativoDoador", x => new { x.CodDoador, x.IdInformativo });
                    table.ForeignKey(
                        name: "FK_InformativoDoador_Doador_CodDoador",
                        column: x => x.CodDoador,
                        principalTable: "Doador",
                        principalColumn: "CodDoador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InformativoDoador_Informativo_IdInformativo",
                        column: x => x.IdInformativo,
                        principalTable: "Informativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InformativoUsuario",
                columns: table => new
                {
                    IdUsuario = table.Column<string>(nullable: false),
                    IdInformativo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformativoUsuario", x => new { x.IdUsuario, x.IdInformativo });
                    table.ForeignKey(
                        name: "FK_InformativoUsuario_Informativo_IdInformativo",
                        column: x => x.IdInformativo,
                        principalTable: "Informativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InformativoUsuario_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "InformativoVoluntario",
                columns: table => new
                {
                    IdVoluntario = table.Column<int>(nullable: false),
                    IdInformativo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformativoVoluntario", x => new { x.IdVoluntario, x.IdInformativo });
                    table.ForeignKey(
                        name: "FK_InformativoVoluntario_Informativo_IdInformativo",
                        column: x => x.IdInformativo,
                        principalTable: "Informativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InformativoVoluntario_Voluntario_IdVoluntario",
                        column: x => x.IdVoluntario,
                        principalTable: "Voluntario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InformativoDoador_CodDoador",
                table: "InformativoDoador",
                column: "CodDoador");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoDoador_IdInformativo",
                table: "InformativoDoador",
                column: "IdInformativo");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoUsuario_IdInformativo",
                table: "InformativoUsuario",
                column: "IdInformativo");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoUsuario_IdUsuario",
                table: "InformativoUsuario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoVoluntario_IdInformativo",
                table: "InformativoVoluntario",
                column: "IdInformativo");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoVoluntario_IdVoluntario",
                table: "InformativoVoluntario",
                column: "IdVoluntario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InformativoDoador");

            migrationBuilder.DropTable(
                name: "InformativoUsuario");

            migrationBuilder.DropTable(
                name: "InformativoVoluntario");

            migrationBuilder.AddColumn<int>(
                name: "InformativoId",
                table: "Voluntario",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InformativoId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InformativoId",
                table: "Doador",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voluntario_InformativoId",
                table: "Voluntario",
                column: "InformativoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InformativoId",
                table: "AspNetUsers",
                column: "InformativoId");

            migrationBuilder.CreateIndex(
                name: "IX_Doador_InformativoId",
                table: "Doador",
                column: "InformativoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doador_Informativo_InformativoId",
                table: "Doador",
                column: "InformativoId",
                principalTable: "Informativo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Informativo_InformativoId",
                table: "AspNetUsers",
                column: "InformativoId",
                principalTable: "Informativo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Voluntario_Informativo_InformativoId",
                table: "Voluntario",
                column: "InformativoId",
                principalTable: "Informativo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
