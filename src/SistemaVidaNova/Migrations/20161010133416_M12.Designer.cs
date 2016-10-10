using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SistemaVidaNova.Models;

namespace SistemaVidaNova.Migrations
{
    [DbContext(typeof(VidaNovaContext))]
    [Migration("20161010133416_M12")]
    partial class M12
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.Property<int>("IdInformativo");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("IdInformativo");

                    b.ToTable("Attachment");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.ConhecimentoProficional", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CodFavorecido");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CodFavorecido");

                    b.ToTable("ConhecimentoProficional");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Despesa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DataDaCompra");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<int>("IdItem");

                    b.Property<string>("IdUsuario")
                        .IsRequired();

                    b.Property<double>("Quantidade");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<double>("ValorUnitario");

                    b.HasKey("Id");

                    b.HasIndex("IdItem");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Despesa");

                    b.HasDiscriminator<string>("Tipo").HasValue("Despesa");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Doador", b =>
                {
                    b.Property<int>("CodDoador")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Celular");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("EnderecoId");

                    b.Property<string>("Telefone");

                    b.Property<string>("doador_type")
                        .IsRequired();

                    b.HasKey("CodDoador");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("EnderecoId");

                    b.ToTable("Doador");

                    b.HasDiscriminator<string>("doador_type").HasValue("Doador");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Endereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bairro")
                        .IsRequired();

                    b.Property<string>("Cep")
                        .IsRequired();

                    b.Property<string>("Cidade")
                        .IsRequired();

                    b.Property<string>("Complemento");

                    b.Property<string>("Estado")
                        .IsRequired();

                    b.Property<string>("Logradouro");

                    b.Property<string>("Numero")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Endereco");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Evento", b =>
                {
                    b.Property<int>("CodEvento")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cor")
                        .IsRequired();

                    b.Property<string>("CorDaFonte")
                        .IsRequired();

                    b.Property<DateTime>("DataFim");

                    b.Property<DateTime>("DataInicio");

                    b.Property<string>("Descricao")
                        .IsRequired();

                    b.Property<string>("IdUsuario")
                        .IsRequired();

                    b.Property<int?>("InteressadoCodInteressado");

                    b.Property<string>("Relato");

                    b.Property<string>("Titulo")
                        .IsRequired();

                    b.Property<double>("ValorArrecadado");

                    b.Property<int?>("VoluntarioId");

                    b.HasKey("CodEvento");

                    b.HasIndex("IdUsuario");

                    b.HasIndex("InteressadoCodInteressado");

                    b.HasIndex("VoluntarioId");

                    b.ToTable("Evento");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Familia", b =>
                {
                    b.Property<int>("CodFamilia")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Celular");

                    b.Property<int>("CodFavorecido");

                    b.Property<string>("Email");

                    b.Property<int>("IdEndereco");

                    b.Property<string>("Nome")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("Telefone");

                    b.HasKey("CodFamilia");

                    b.HasIndex("CodFavorecido")
                        .IsUnique();

                    b.HasIndex("IdEndereco");

                    b.ToTable("Familia");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Favorecido", b =>
                {
                    b.Property<int>("CodFavorecido")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Apelido");

                    b.Property<string>("Cpf");

                    b.Property<DateTime>("DataDeCadastro");

                    b.Property<DateTime?>("DataNascimento");

                    b.Property<string>("IdUsuario")
                        .IsRequired();

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<string>("Rg");

                    b.Property<string>("Sexo")
                        .IsRequired();

                    b.HasKey("CodFavorecido");

                    b.HasIndex("Cpf")
                        .IsUnique();

                    b.HasIndex("IdUsuario");

                    b.ToTable("Favorecido");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Informativo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<string>("IdUsuario");

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Informativo");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.InformativoDoador", b =>
                {
                    b.Property<int>("CodDoador");

                    b.Property<int>("IdInformativo");

                    b.HasKey("CodDoador", "IdInformativo");

                    b.HasIndex("CodDoador");

                    b.HasIndex("IdInformativo");

                    b.ToTable("InformativoDoador");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.InformativoUsuario", b =>
                {
                    b.Property<string>("IdUsuario");

                    b.Property<int>("IdInformativo");

                    b.HasKey("IdUsuario", "IdInformativo");

                    b.HasIndex("IdInformativo");

                    b.HasIndex("IdUsuario");

                    b.ToTable("InformativoUsuario");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.InformativoVoluntario", b =>
                {
                    b.Property<int>("IdVoluntario");

                    b.Property<int>("IdInformativo");

                    b.HasKey("IdVoluntario", "IdInformativo");

                    b.HasIndex("IdInformativo");

                    b.HasIndex("IdVoluntario");

                    b.ToTable("InformativoVoluntario");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Interessado", b =>
                {
                    b.Property<int>("CodInteressado")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Celular");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<string>("Telefone");

                    b.HasKey("CodInteressado");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Interessado");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.InteressadoEvento", b =>
                {
                    b.Property<int>("CodEvento");

                    b.Property<int>("CodInteressado");

                    b.HasKey("CodEvento", "CodInteressado");

                    b.HasIndex("CodEvento");

                    b.HasIndex("CodInteressado");

                    b.ToTable("InteressadoEvento");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Destino")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("UnidadeDeMedida")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.HasKey("Id");

                    b.HasIndex("Nome", "Destino")
                        .IsUnique();

                    b.ToTable("Item");

                    b.HasDiscriminator<string>("Destino").HasValue("Item");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Usuario", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Cpf")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsAtivo");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("Cpf")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Voluntario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Celular");

                    b.Property<string>("Cpf")
                        .IsRequired();

                    b.Property<DateTime>("DataDeCadastro");

                    b.Property<DateTime>("DataNascimento");

                    b.Property<bool>("Domingo");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int?>("EnderecoId");

                    b.Property<string>("Funcao");

                    b.Property<string>("IdUsuario");

                    b.Property<bool>("IsDeletado");

                    b.Property<string>("Nome")
                        .IsRequired();

                    b.Property<bool>("QuartaFeira");

                    b.Property<bool>("QuintaFeira");

                    b.Property<string>("Rg")
                        .IsRequired();

                    b.Property<bool>("Sabado");

                    b.Property<bool>("SegundaFeira");

                    b.Property<string>("Sexo")
                        .IsRequired();

                    b.Property<bool>("SextaFeira");

                    b.Property<string>("Telefone");

                    b.Property<bool>("TercaFeira");

                    b.HasKey("Id");

                    b.HasIndex("Cpf")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("EnderecoId");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Voluntario");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.VoluntarioEvento", b =>
                {
                    b.Property<int>("CodEvento");

                    b.Property<int>("IdVoluntario");

                    b.HasKey("CodEvento", "IdVoluntario");

                    b.HasIndex("CodEvento");

                    b.HasIndex("IdVoluntario");

                    b.ToTable("VoluntarioEvento");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.DespesaAssociacao", b =>
                {
                    b.HasBaseType("SistemaVidaNova.Models.Despesa");


                    b.ToTable("DespesaAssociacao");

                    b.HasDiscriminator().HasValue("ASSOCIACAO");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.DespesaFavorecido", b =>
                {
                    b.HasBaseType("SistemaVidaNova.Models.Despesa");

                    b.Property<int>("CodFavorecido");

                    b.HasIndex("CodFavorecido");

                    b.ToTable("DespesaFavorecido");

                    b.HasDiscriminator().HasValue("FAVORECIDO");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.DespesaSopa", b =>
                {
                    b.HasBaseType("SistemaVidaNova.Models.Despesa");


                    b.ToTable("DespesaSopa");

                    b.HasDiscriminator().HasValue("SOPA");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.PessoaFisica", b =>
                {
                    b.HasBaseType("SistemaVidaNova.Models.Doador");

                    b.Property<string>("Cpf")
                        .HasAnnotation("MaxLength", 11);

                    b.Property<string>("Nome")
                        .HasAnnotation("MaxLength", 200);

                    b.ToTable("PessoaFisica");

                    b.HasDiscriminator().HasValue("PF");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.PessoaJuridica", b =>
                {
                    b.HasBaseType("SistemaVidaNova.Models.Doador");

                    b.Property<string>("Cnpj")
                        .HasAnnotation("MaxLength", 14);

                    b.Property<string>("RazaoSocial")
                        .HasAnnotation("MaxLength", 500);

                    b.ToTable("PessoaJuridica");

                    b.HasDiscriminator().HasValue("PJ");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.ItemAssociacao", b =>
                {
                    b.HasBaseType("SistemaVidaNova.Models.Item");


                    b.ToTable("ItemAssociacao");

                    b.HasDiscriminator().HasValue("ASSOCIACAO");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.ItemFavorecido", b =>
                {
                    b.HasBaseType("SistemaVidaNova.Models.Item");

                    b.Property<int>("CodFavorecido");

                    b.ToTable("ItemFavorecido");

                    b.HasDiscriminator().HasValue("FAVORECIDO");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.ItemSopa", b =>
                {
                    b.HasBaseType("SistemaVidaNova.Models.Item");


                    b.ToTable("ItemSopa");

                    b.HasDiscriminator().HasValue("SOPA");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Usuario")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Usuario")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SistemaVidaNova.Models.Usuario")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Attachment", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Informativo", "Informativo")
                        .WithMany("Attachments")
                        .HasForeignKey("IdInformativo")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.ConhecimentoProficional", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Favorecido", "Favorecido")
                        .WithMany("ConhecimentosProfissionais")
                        .HasForeignKey("CodFavorecido")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Despesa", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("IdItem")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SistemaVidaNova.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Doador", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Evento", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SistemaVidaNova.Models.Interessado")
                        .WithMany("Eventos")
                        .HasForeignKey("InteressadoCodInteressado");

                    b.HasOne("SistemaVidaNova.Models.Voluntario")
                        .WithMany("Eventos")
                        .HasForeignKey("VoluntarioId");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Familia", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Favorecido", "Favorecido")
                        .WithOne("Familia")
                        .HasForeignKey("SistemaVidaNova.Models.Familia", "CodFavorecido")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SistemaVidaNova.Models.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("IdEndereco")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Favorecido", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Informativo", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.InformativoDoador", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Doador", "Doador")
                        .WithMany()
                        .HasForeignKey("CodDoador")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SistemaVidaNova.Models.Informativo", "Informativo")
                        .WithMany("Doadores")
                        .HasForeignKey("IdInformativo")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.InformativoUsuario", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Informativo", "Informativo")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdInformativo")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SistemaVidaNova.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.InformativoVoluntario", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Informativo", "Informativo")
                        .WithMany("Voluntarios")
                        .HasForeignKey("IdInformativo")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SistemaVidaNova.Models.Voluntario", "Voluntario")
                        .WithMany()
                        .HasForeignKey("IdVoluntario")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.InteressadoEvento", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Evento", "Evento")
                        .WithMany("Interessados")
                        .HasForeignKey("CodEvento")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SistemaVidaNova.Models.Interessado", "Interessado")
                        .WithMany()
                        .HasForeignKey("CodInteressado")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.Voluntario", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Endereco", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoId");

                    b.HasOne("SistemaVidaNova.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario");
                });

            modelBuilder.Entity("SistemaVidaNova.Models.VoluntarioEvento", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Evento", "Evento")
                        .WithMany("Voluntarios")
                        .HasForeignKey("CodEvento")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SistemaVidaNova.Models.Voluntario", "Voluntario")
                        .WithMany()
                        .HasForeignKey("IdVoluntario")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SistemaVidaNova.Models.DespesaFavorecido", b =>
                {
                    b.HasOne("SistemaVidaNova.Models.Favorecido", "Favorecido")
                        .WithMany()
                        .HasForeignKey("CodFavorecido")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
