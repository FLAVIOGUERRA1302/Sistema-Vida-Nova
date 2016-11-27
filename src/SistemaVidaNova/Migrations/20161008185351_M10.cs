using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaVidaNova.Migrations
{
    public partial class M10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataDaCompra",
                table: "Despesa",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "IdUsuario",
                table: "Despesa",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Quantidade",
                table: "Despesa",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ValorUnitario",
                table: "Despesa",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Despesa_IdUsuario",
                table: "Despesa",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Despesa_CodFavorecido",
                table: "Despesa",
                column: "CodFavorecido");

            migrationBuilder.AddForeignKey(
                name: "FK_Despesa_AspNetUsers_IdUsuario",
                table: "Despesa",
                column: "IdUsuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Despesa_Favorecido_CodFavorecido",
                table: "Despesa",
                column: "CodFavorecido",
                principalTable: "Favorecido",
                principalColumn: "CodFavorecido",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Despesa_AspNetUsers_IdUsuario",
                table: "Despesa");

            migrationBuilder.DropForeignKey(
                name: "FK_Despesa_Favorecido_CodFavorecido",
                table: "Despesa");

            migrationBuilder.DropIndex(
                name: "IX_Despesa_IdUsuario",
                table: "Despesa");

            migrationBuilder.DropIndex(
                name: "IX_Despesa_CodFavorecido",
                table: "Despesa");

            migrationBuilder.DropColumn(
                name: "DataDaCompra",
                table: "Despesa");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Despesa");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "Despesa");

            migrationBuilder.DropColumn(
                name: "ValorUnitario",
                table: "Despesa");
        }
    }
}
