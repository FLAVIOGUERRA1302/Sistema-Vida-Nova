using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaVidaNova.Migrations
{
    public partial class M7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Informativo_AspNetUsers_IdUsuario",
                table: "Informativo");

            migrationBuilder.DropColumn(
                name: "MediaSubType",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "path",
                table: "Attachment");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Attachment",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Attachment",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "AspNetUsers",
                nullable: false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_Cpf",
                table: "AspNetUsers",
                column: "Cpf");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Interessado",
                nullable: false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Interessado_Email",
                table: "Interessado",
                column: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "Informativo",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Informativo_AspNetUsers_IdUsuario",
                table: "Informativo",
                column: "IdUsuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Informativo_AspNetUsers_IdUsuario",
                table: "Informativo");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_Cpf",
                table: "AspNetUsers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Interessado_Email",
                table: "Interessado");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Attachment");

            migrationBuilder.AddColumn<string>(
                name: "MediaSubType",
                table: "Attachment",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "Attachment",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "path",
                table: "Attachment",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "AspNetUsers",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Interessado",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "Informativo",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Informativo_AspNetUsers_IdUsuario",
                table: "Informativo",
                column: "IdUsuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
