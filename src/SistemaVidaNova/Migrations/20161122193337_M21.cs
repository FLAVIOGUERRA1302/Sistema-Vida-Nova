using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemaVidaNova.Migrations
{
    public partial class M21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodFavorecido",
                table: "Item");

            migrationBuilder.CreateTable(
                name: "DoadorEvento",
                columns: table => new
                {
                    CodEvento = table.Column<int>(nullable: false),
                    CodDoador = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoadorEvento", x => new { x.CodEvento, x.CodDoador });
                    table.ForeignKey(
                        name: "FK_DoadorEvento_Doador_CodDoador",
                        column: x => x.CodDoador,
                        principalTable: "Doador",
                        principalColumn: "CodDoador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoadorEvento_Evento_CodEvento",
                        column: x => x.CodEvento,
                        principalTable: "Evento",
                        principalColumn: "CodEvento",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FavorecidoEvento",
                columns: table => new
                {
                    CodEvento = table.Column<int>(nullable: false),
                    CodFavorecido = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavorecidoEvento", x => new { x.CodEvento, x.CodFavorecido });
                    table.ForeignKey(
                        name: "FK_FavorecidoEvento_Evento_CodEvento",
                        column: x => x.CodEvento,
                        principalTable: "Evento",
                        principalColumn: "CodEvento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavorecidoEvento_Favorecido_CodFavorecido",
                        column: x => x.CodFavorecido,
                        principalTable: "Favorecido",
                        principalColumn: "CodFavorecido",
                        onDelete: ReferentialAction.Restrict);
                });
            /*
            migrationBuilder.CreateTable(
                name: "MelhorDoador",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CpfCnpj = table.Column<string>(nullable: true),
                    NomeRazaoSocial = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: true),
                    ValorDoado = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MelhorDoador", x => x.Id);
                });*/
                /*
            migrationBuilder.CreateTable(
                name: "EventoMaisProcurado",
                columns: table => new
                {
                    CodEvento = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cor = table.Column<string>(nullable: true),
                    CorDaFonte = table.Column<string>(nullable: true),
                    DataFim = table.Column<DateTime>(nullable: false),
                    DataInicio = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(nullable: true),
                    QuantidadeDePessoas = table.Column<int>(nullable: false),
                    Relato = table.Column<string>(nullable: true),
                    Titulo = table.Column<string>(nullable: true),
                    ValorArrecadado = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoMaisProcurado", x => x.CodEvento);
                });*/
                /*
            migrationBuilder.CreateTable(
                name: "FavorecidoComGasto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    ValorGasto = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavorecidoComGasto", x => x.Id);
                });
            */
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCurso",
                table: "Voluntario",
                nullable: false,
                defaultValue: new DateTime(2016, 11, 22, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_DoadorEvento_CodDoador",
                table: "DoadorEvento",
                column: "CodDoador");

            migrationBuilder.CreateIndex(
                name: "IX_DoadorEvento_CodEvento",
                table: "DoadorEvento",
                column: "CodEvento");

            migrationBuilder.CreateIndex(
                name: "IX_FavorecidoEvento_CodEvento",
                table: "FavorecidoEvento",
                column: "CodEvento");

            migrationBuilder.CreateIndex(
                name: "IX_FavorecidoEvento_CodFavorecido",
                table: "FavorecidoEvento",
                column: "CodFavorecido");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoadorEvento");

            migrationBuilder.DropTable(
                name: "FavorecidoEvento");

            migrationBuilder.DropTable(
                name: "MelhorDoador");

            migrationBuilder.DropTable(
                name: "EventoMaisProcurado");

            migrationBuilder.DropTable(
                name: "FavorecidoComGasto");

            migrationBuilder.AddColumn<int>(
                name: "CodFavorecido",
                table: "Item",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCurso",
                table: "Voluntario",
                nullable: false,
                defaultValue: new DateTime(2016, 11, 4, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
