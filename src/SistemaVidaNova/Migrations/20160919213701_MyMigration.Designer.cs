using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SistemaVidaNova.Models;

namespace SistemaVidaNova.Migrations
{
    [DbContext(typeof(VidaNovaContext))]
    [Migration("20160919213701_MyMigration")]
    partial class MyMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SistemaVidaNova.Models.Doador", b =>
                {
                    b.Property<int>("CodDoador")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Celular");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Telefone");

                    b.HasKey("CodDoador");

                    b.ToTable("Doador");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Voluntario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Celular");

                    b.Property<string>("Cpf")
                        .IsRequired();

                    b.Property<DateTime>("DataNascimento");

                    b.Property<bool>("Domingo");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<bool>("QuartaFeira");

                    b.Property<bool>("QuintaFeira");

                    b.Property<string>("Rg")
                        .IsRequired();

                    b.Property<bool>("Sabado");

                    b.Property<bool>("SegundaFeira");

                    b.Property<string>("Senha")
                        .IsRequired();

                    b.Property<bool>("SextaFeira");

                    b.Property<string>("Telefone");

                    b.Property<bool>("TercaFeira");

                    b.HasKey("Id");

                    b.HasAlternateKey("Email");

                    b.ToTable("Voluntario");
                });
        }
    }
}
