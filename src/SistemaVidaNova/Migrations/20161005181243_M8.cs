using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaVidaNova.Migrations
{
    public partial class M8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Voluntario_Cpf",
                table: "Voluntario");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Voluntario_Email",
                table: "Voluntario");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_Cpf",
                table: "AspNetUsers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Interessado_Email",
                table: "Interessado");

            migrationBuilder.AddColumn<string>(
                name: "Funcao",
                table: "Voluntario",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voluntario_Cpf",
                table: "Voluntario",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voluntario_Email",
                table: "Voluntario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Cpf",
                table: "AspNetUsers",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Interessado_Email",
                table: "Interessado",
                column: "Email",
                unique: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Favorecido",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorecido_Cpf",
                table: "Favorecido",
                column: "Cpf",
                unique: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doador",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Doador_Email",
                table: "Doador",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Voluntario_Cpf",
                table: "Voluntario");

            migrationBuilder.DropIndex(
                name: "IX_Voluntario_Email",
                table: "Voluntario");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Cpf",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Interessado_Email",
                table: "Interessado");

            migrationBuilder.DropIndex(
                name: "IX_Favorecido_Cpf",
                table: "Favorecido");

            migrationBuilder.DropIndex(
                name: "IX_Doador_Email",
                table: "Doador");

            migrationBuilder.DropColumn(
                name: "Funcao",
                table: "Voluntario");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Voluntario_Cpf",
                table: "Voluntario",
                column: "Cpf");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Voluntario_Email",
                table: "Voluntario",
                column: "Email");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_Cpf",
                table: "AspNetUsers",
                column: "Cpf");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Interessado_Email",
                table: "Interessado",
                column: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Favorecido",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doador",
                nullable: true);
        }
    }
}
