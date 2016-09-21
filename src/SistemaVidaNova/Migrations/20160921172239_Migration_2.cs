using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaVidaNova.Migrations
{
    public partial class Migration_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interessado",
                columns: table => new
                {
                    CodInteressado = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Celular = table.Column<int>(nullable: false),
                    Email = table.Column<int>(nullable: false),
                    Nome = table.Column<int>(nullable: false),
                    Telefone = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interessado", x => x.CodInteressado);
                });

            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    CodEvento = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cor = table.Column<string>(nullable: false),
                    CorDaFonte = table.Column<string>(nullable: false),
                    DataFim = table.Column<DateTime>(nullable: false),
                    DataInicio = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(nullable: false),
                    InteressadoCodInteressado = table.Column<int>(nullable: true),
                    Relato = table.Column<string>(nullable: true),
                    Titulo = table.Column<string>(nullable: false),
                    ValorArrecadado = table.Column<double>(nullable: false),
                    ValorDeEntrada = table.Column<double>(nullable: false),
                    VoluntarioId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.CodEvento);
                    table.ForeignKey(
                        name: "FK_Evento_Interessado_InteressadoCodInteressado",
                        column: x => x.InteressadoCodInteressado,
                        principalTable: "Interessado",
                        principalColumn: "CodInteressado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Evento_AspNetUsers_VoluntarioId",
                        column: x => x.VoluntarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InteressadoEvento",
                columns: table => new
                {
                    CodEvento = table.Column<int>(nullable: false),
                    CodInetessado = table.Column<int>(nullable: false),
                    CodInteressado = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteressadoEvento", x => new { x.CodEvento, x.CodInetessado });
                    table.ForeignKey(
                        name: "FK_InteressadoEvento_Evento_CodEvento",
                        column: x => x.CodEvento,
                        principalTable: "Evento",
                        principalColumn: "CodEvento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteressadoEvento_Interessado_CodInteressado",
                        column: x => x.CodInteressado,
                        principalTable: "Interessado",
                        principalColumn: "CodInteressado",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Endereco",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_InteressadoCodInteressado",
                table: "Evento",
                column: "InteressadoCodInteressado");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_VoluntarioId",
                table: "Evento",
                column: "VoluntarioId");

            migrationBuilder.CreateIndex(
                name: "IX_InteressadoEvento_CodEvento",
                table: "InteressadoEvento",
                column: "CodEvento");

            migrationBuilder.CreateIndex(
                name: "IX_InteressadoEvento_CodInteressado",
                table: "InteressadoEvento",
                column: "CodInteressado");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Endereco");

            migrationBuilder.DropTable(
                name: "InteressadoEvento");

            migrationBuilder.DropTable(
                name: "Evento");

            migrationBuilder.DropTable(
                name: "Interessado");
        }
    }
}
