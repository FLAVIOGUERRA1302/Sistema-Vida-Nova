using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaVidaNova.Migrations
{
    public partial class M20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdVoluntario",
                table: "DoacaoObjeto",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCurso",
                table: "Voluntario",
                nullable: false,
                defaultValue: new DateTime(2016, 11, 4, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoObjeto_IdVoluntario",
                table: "DoacaoObjeto",
                column: "IdVoluntario");

            migrationBuilder.AddForeignKey(
                name: "FK_DoacaoObjeto_Voluntario_IdVoluntario",
                table: "DoacaoObjeto",
                column: "IdVoluntario",
                principalTable: "Voluntario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoacaoObjeto_Voluntario_IdVoluntario",
                table: "DoacaoObjeto");

            migrationBuilder.DropIndex(
                name: "IX_DoacaoObjeto_IdVoluntario",
                table: "DoacaoObjeto");

            migrationBuilder.DropColumn(
                name: "IdVoluntario",
                table: "DoacaoObjeto");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCurso",
                table: "Voluntario",
                nullable: false,
                defaultValue: new DateTime(2016, 10, 31, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
