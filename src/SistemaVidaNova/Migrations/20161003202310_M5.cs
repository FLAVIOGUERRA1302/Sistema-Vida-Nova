using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaVidaNova.Migrations
{
    public partial class M5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Informativo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Body = table.Column<string>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Informativo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Informativo_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdInformativo = table.Column<int>(nullable: false),
                    MediaSubType = table.Column<string>(nullable: false),
                    MediaType = table.Column<string>(nullable: false),
                    path = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_Informativo_IdInformativo",
                        column: x => x.IdInformativo,
                        principalTable: "Informativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_IdInformativo",
                table: "Attachment",
                column: "IdInformativo");

            migrationBuilder.CreateIndex(
                name: "IX_Informativo_IdUsuario",
                table: "Informativo",
                column: "IdUsuario");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Informativo");
        }
    }
}
