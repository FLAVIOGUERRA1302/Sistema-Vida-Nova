using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaVidaNova.Migrations
{
    public partial class migration_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Interessado",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Interessado",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Interessado",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Celular",
                table: "Interessado",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Telefone",
                table: "Interessado",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Nome",
                table: "Interessado",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Email",
                table: "Interessado",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Celular",
                table: "Interessado",
                nullable: false);
        }
    }
}
