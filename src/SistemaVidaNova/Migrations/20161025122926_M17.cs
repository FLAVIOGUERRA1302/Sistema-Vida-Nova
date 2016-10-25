using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaVidaNova.Migrations
{
    public partial class M17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataAgendamentoCurso",
                table: "Voluntario",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCurso",
                table: "Voluntario",
                nullable: false,
                defaultValue: new DateTime(2016, 10, 25, 0, 0, 0, 0, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataAgendamentoCurso",
                table: "Voluntario");

            migrationBuilder.DropColumn(
                name: "DataCurso",
                table: "Voluntario");
        }
    }
}
