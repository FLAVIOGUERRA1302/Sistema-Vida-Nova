using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaVidaNova.Migrations
{
    public partial class M3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Endereco_Voluntario_VoluntarioId",
                table: "Endereco");

            migrationBuilder.DropIndex(
                name: "IX_Endereco_VoluntarioId",
                table: "Endereco");

            migrationBuilder.DropColumn(
                name: "VoluntarioId",
                table: "Endereco");

            migrationBuilder.CreateTable(
                name: "Favorecido",
                columns: table => new
                {
                    CodFavorecido = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Apelido = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    DataDeCadastro = table.Column<DateTime>(nullable: false),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    Rg = table.Column<string>(nullable: true),
                    Sexo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorecido", x => x.CodFavorecido);
                    table.ForeignKey(
                        name: "FK_Favorecido_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Familia",
                columns: table => new
                {
                    CodFamilia = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Celular = table.Column<string>(nullable: true),
                    CodFavorecido = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EnderecoId = table.Column<int>(nullable: true),
                    Nome = table.Column<string>(maxLength: 200, nullable: true),
                    Telefone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familia", x => x.CodFamilia);
                    table.ForeignKey(
                        name: "FK_Familia_Favorecido_CodFavorecido",
                        column: x => x.CodFavorecido,
                        principalTable: "Favorecido",
                        principalColumn: "CodFavorecido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Familia_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<int>(
                name: "EnderecoId",
                table: "Voluntario",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnderecoId",
                table: "Doador",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "doador_type",
                table: "Doador",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Doador",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Doador",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "Doador",
                maxLength: 14,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RazaoSocial",
                table: "Doador",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voluntario_EnderecoId",
                table: "Voluntario",
                column: "EnderecoId");

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                table: "Endereco",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doador",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doador_EnderecoId",
                table: "Doador",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Familia_CodFavorecido",
                table: "Familia",
                column: "CodFavorecido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Familia_EnderecoId",
                table: "Familia",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorecido_IdUsuario",
                table: "Favorecido",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Doador_Endereco_EnderecoId",
                table: "Doador",
                column: "EnderecoId",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Voluntario_Endereco_EnderecoId",
                table: "Voluntario",
                column: "EnderecoId",
                principalTable: "Endereco",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doador_Endereco_EnderecoId",
                table: "Doador");

            migrationBuilder.DropForeignKey(
                name: "FK_Voluntario_Endereco_EnderecoId",
                table: "Voluntario");

            migrationBuilder.DropIndex(
                name: "IX_Voluntario_EnderecoId",
                table: "Voluntario");

            migrationBuilder.DropIndex(
                name: "IX_Doador_EnderecoId",
                table: "Doador");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Doador");

            migrationBuilder.DropColumn(
                name: "doador_type",
                table: "Doador");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Doador");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Doador");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "Doador");

            migrationBuilder.DropColumn(
                name: "RazaoSocial",
                table: "Doador");

            migrationBuilder.DropTable(
                name: "Familia");

            migrationBuilder.DropTable(
                name: "Favorecido");

            migrationBuilder.AddColumn<int>(
                name: "VoluntarioId",
                table: "Endereco",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                table: "Endereco",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Endereco_VoluntarioId",
                table: "Endereco",
                column: "VoluntarioId",
                unique: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doador",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Endereco_Voluntario_VoluntarioId",
                table: "Endereco",
                column: "VoluntarioId",
                principalTable: "Voluntario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
