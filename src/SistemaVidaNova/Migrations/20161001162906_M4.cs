using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaVidaNova.Migrations
{
    public partial class M4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Familia_Endereco_EnderecoId",
                table: "Familia");

            migrationBuilder.DropIndex(
                name: "IX_Familia_EnderecoId",
                table: "Familia");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Familia");

            migrationBuilder.CreateTable(
                name: "ConhecimentoProficional",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CodFavorecido = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConhecimentoProficional", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConhecimentoProficional_Favorecido_CodFavorecido",
                        column: x => x.CodFavorecido,
                        principalTable: "Favorecido",
                        principalColumn: "CodFavorecido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "IdEndereco",
                table: "Familia",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataNascimento",
                table: "Favorecido",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Familia_IdEndereco",
                table: "Familia",
                column: "IdEndereco");

            migrationBuilder.CreateIndex(
                name: "IX_ConhecimentoProficional_CodFavorecido",
                table: "ConhecimentoProficional",
                column: "CodFavorecido");

            migrationBuilder.AddForeignKey(
                name: "FK_Familia_Endereco_IdEndereco",
                table: "Familia",
                column: "IdEndereco",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Familia_Endereco_IdEndereco",
                table: "Familia");

            migrationBuilder.DropIndex(
                name: "IX_Familia_IdEndereco",
                table: "Familia");

            migrationBuilder.DropColumn(
                name: "IdEndereco",
                table: "Familia");

            migrationBuilder.DropTable(
                name: "ConhecimentoProficional");

            migrationBuilder.AddColumn<int>(
                name: "EnderecoId",
                table: "Familia",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataNascimento",
                table: "Favorecido",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Familia_EnderecoId",
                table: "Familia",
                column: "EnderecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Familia_Endereco_EnderecoId",
                table: "Familia",
                column: "EnderecoId",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
