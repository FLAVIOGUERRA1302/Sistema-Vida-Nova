using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaVidaNova.Migrations
{
    public partial class MyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doador",
                columns: table => new
                {
                    CodDoador = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Celular = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Telefone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doador", x => x.CodDoador);
                });

            migrationBuilder.CreateTable(
                name: "Voluntario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Celular = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: false),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    Domingo = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    QuartaFeira = table.Column<bool>(nullable: false),
                    QuintaFeira = table.Column<bool>(nullable: false),
                    Rg = table.Column<string>(nullable: false),
                    Sabado = table.Column<bool>(nullable: false),
                    SegundaFeira = table.Column<bool>(nullable: false),
                    Senha = table.Column<string>(nullable: false),
                    SextaFeira = table.Column<bool>(nullable: false),
                    Telefone = table.Column<string>(nullable: true),
                    TercaFeira = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntario", x => x.Id);
                    table.UniqueConstraint("AK_Voluntario_Email", x => x.Email);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doador");

            migrationBuilder.DropTable(
                name: "Voluntario");
        }
    }
}
