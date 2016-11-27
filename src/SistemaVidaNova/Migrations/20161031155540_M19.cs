using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaVidaNova.Migrations
{
    public partial class M19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataAgendamentoCurso",
                table: "Voluntario");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCurso",
                table: "Voluntario",
                nullable: false,
                defaultValue: new DateTime(2016, 10, 31, 0, 0, 0, 0, DateTimeKind.Local));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataAgendamentoCurso",
                table: "Voluntario",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCurso",
                table: "Voluntario",
                nullable: false,
                defaultValue: new DateTime(2016, 10, 25, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
