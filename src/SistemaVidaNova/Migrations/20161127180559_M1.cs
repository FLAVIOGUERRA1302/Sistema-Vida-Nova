using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SistemaVidaNova.Migrations
{
    public partial class M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Endereco",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Bairro = table.Column<string>(nullable: false),
                    Cep = table.Column<string>(nullable: false),
                    Cidade = table.Column<string>(nullable: false),
                    Complemento = table.Column<string>(nullable: true),
                    Estado = table.Column<string>(nullable: false),
                    Logradouro = table.Column<string>(nullable: true),
                    Numero = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endereco", x => x.Id);
                });
            /*
            migrationBuilder.CreateTable(
                name: "MelhorDoador",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CpfCnpj = table.Column<string>(nullable: true),
                    NomeRazaoSocial = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: true),
                    ValorDoado = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MelhorDoador", x => x.Id);
                });
            
            migrationBuilder.CreateTable(
                name: "EventoMaisProcurado",
                columns: table => new
                {
                    CodEvento = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
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
                });

            migrationBuilder.CreateTable(
                name: "FavorecidoComGasto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Nome = table.Column<string>(nullable: true),
                    ValorGasto = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavorecidoComGasto", x => x.Id);
                });
                */
            migrationBuilder.CreateTable(
                name: "Interessado",
                columns: table => new
                {
                    CodInteressado = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Celular = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    Telefone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interessado", x => x.CodInteressado);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Destino = table.Column<string>(maxLength: 10, nullable: false),
                    Nome = table.Column<string>(maxLength: 200, nullable: false),
                    QuantidadeEmEstoque = table.Column<double>(nullable: false, defaultValue: 0.0d),
                        //.Annotation("MySQL:AutoIncrement", true),
                    UnidadeDeMedida = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModeloDeReceita",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    Nome = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloDeReceita", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    IsAtivo = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    Nome = table.Column<string>(nullable: false),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doador",
                columns: table => new
                {
                    CodDoador = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Celular = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    EnderecoId = table.Column<int>(nullable: false),
                    Telefone = table.Column<string>(nullable: true),
                    doador_type = table.Column<string>(nullable: false),
                    Cpf = table.Column<string>(maxLength: 11, nullable: true),
                    Nome = table.Column<string>(maxLength: 200, nullable: true),
                    Cnpj = table.Column<string>(maxLength: 14, nullable: true),
                    RazaoSocial = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doador", x => x.CodDoador);
                    table.ForeignKey(
                        name: "FK_Doador_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModeloDeReceitaItem",
                columns: table => new
                {
                    IdItem = table.Column<int>(nullable: false),
                    IdModeloDeReceita = table.Column<int>(nullable: false),
                    Quantidade = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloDeReceitaItem", x => new { x.IdItem, x.IdModeloDeReceita });
                    table.ForeignKey(
                        name: "FK_ModeloDeReceitaItem_Item_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModeloDeReceitaItem_ModeloDeReceita_IdModeloDeReceita",
                        column: x => x.IdModeloDeReceita,
                        principalTable: "ModeloDeReceita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoSopa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Data = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    IdModeloDeReceita = table.Column<int>(nullable: false),
                    LitrosProduzidos = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoSopa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadoSopa_ModeloDeReceita_IdModeloDeReceita",
                        column: x => x.IdModeloDeReceita,
                        principalTable: "ModeloDeReceita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorecido",
                columns: table => new
                {
                    CodFavorecido = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Apelido = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    DataDeCadastro = table.Column<DateTime>(nullable: false),
                    DataNascimento = table.Column<DateTime>(nullable: true),
                    IdUsuario = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    Rg = table.Column<string>(nullable: true),
                    Sexo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorecido", x => x.CodFavorecido);
                    table.ForeignKey(
                        name: "FK_Favorecido_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Informativo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Body = table.Column<string>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Informativo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Informativo_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Voluntario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Celular = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: false),
                    DataCurso = table.Column<DateTime>(nullable: false),
                    DataDeCadastro = table.Column<DateTime>(nullable: false),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    Domingo = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    EnderecoId = table.Column<int>(nullable: true),
                    Funcao = table.Column<string>(nullable: true),
                    IdUsuario = table.Column<string>(nullable: true),
                    IsDeletado = table.Column<bool>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    QuartaFeira = table.Column<bool>(nullable: false),
                    QuintaFeira = table.Column<bool>(nullable: false),
                    Rg = table.Column<string>(nullable: false),
                    Sabado = table.Column<bool>(nullable: false),
                    SegundaFeira = table.Column<bool>(nullable: false),
                    Sexo = table.Column<string>(nullable: false),
                    SextaFeira = table.Column<bool>(nullable: false),
                    Telefone = table.Column<string>(nullable: true),
                    TercaFeira = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voluntario_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Voluntario_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoacaoDinheiro",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CodDoador = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    Valor = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoacaoDinheiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoacaoDinheiro_Doador_CodDoador",
                        column: x => x.CodDoador,
                        principalTable: "Doador",
                        principalColumn: "CodDoador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoacaoSopa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CodDoador = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    IdItem = table.Column<int>(nullable: false),
                    Quantidade = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoacaoSopa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoacaoSopa_Doador_CodDoador",
                        column: x => x.CodDoador,
                        principalTable: "Doador",
                        principalColumn: "CodDoador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoacaoSopa_Item_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoSopaItem",
                columns: table => new
                {
                    IdItem = table.Column<int>(nullable: false),
                    IdResultadoSopa = table.Column<int>(nullable: false),
                    Quantidade = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoSopaItem", x => new { x.IdItem, x.IdResultadoSopa });
                    table.ForeignKey(
                        name: "FK_ResultadoSopaItem_Item_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultadoSopaItem_ResultadoSopa_IdResultadoSopa",
                        column: x => x.IdResultadoSopa,
                        principalTable: "ResultadoSopa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConhecimentoProficional",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CodFavorecido = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConhecimentoProficional", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConhecimentoProficional_Favorecido_CodFavorecido",
                        column: x => x.CodFavorecido,
                        principalTable: "Favorecido",
                        principalColumn: "CodFavorecido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Despesa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    DataDaCompra = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    IdItem = table.Column<int>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: false),
                    Quantidade = table.Column<double>(nullable: false),
                    Tipo = table.Column<string>(maxLength: 10, nullable: false),
                    ValorUnitario = table.Column<double>(nullable: false),
                    CodFavorecido = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Despesa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Despesa_Item_IdItem",
                        column: x => x.IdItem,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Despesa_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Despesa_Favorecido_CodFavorecido",
                        column: x => x.CodFavorecido,
                        principalTable: "Favorecido",
                        principalColumn: "CodFavorecido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Familia",
                columns: table => new
                {
                    CodFamilia = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Celular = table.Column<string>(nullable: true),
                    CodFavorecido = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    IdEndereco = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(maxLength: 200, nullable: true),
                    Telefone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familia", x => x.CodFamilia);
                    table.ForeignKey(
                        name: "FK_Familia_Favorecido_CodFavorecido",
                        column: x => x.CodFavorecido,
                        principalTable: "Favorecido",
                        principalColumn: "CodFavorecido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Familia_Endereco_IdEndereco",
                        column: x => x.IdEndereco,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    FileName = table.Column<string>(nullable: false),
                    IdInformativo = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_Informativo_IdInformativo",
                        column: x => x.IdInformativo,
                        principalTable: "Informativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InformativoDoador",
                columns: table => new
                {
                    CodDoador = table.Column<int>(nullable: false),
                    IdInformativo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformativoDoador", x => new { x.CodDoador, x.IdInformativo });
                    table.ForeignKey(
                        name: "FK_InformativoDoador_Doador_CodDoador",
                        column: x => x.CodDoador,
                        principalTable: "Doador",
                        principalColumn: "CodDoador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InformativoDoador_Informativo_IdInformativo",
                        column: x => x.IdInformativo,
                        principalTable: "Informativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InformativoUsuario",
                columns: table => new
                {
                    IdUsuario = table.Column<string>(nullable: false),
                    IdInformativo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformativoUsuario", x => new { x.IdUsuario, x.IdInformativo });
                    table.ForeignKey(
                        name: "FK_InformativoUsuario_Informativo_IdInformativo",
                        column: x => x.IdInformativo,
                        principalTable: "Informativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InformativoUsuario_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoacaoObjeto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    CodDoador = table.Column<int>(nullable: false),
                    DataDaDoacao = table.Column<DateTime>(nullable: false),
                    DataDeRetirada = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 200, nullable: false),
                    IdEndereco = table.Column<int>(nullable: false),
                    IdVoluntario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoacaoObjeto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoacaoObjeto_Doador_CodDoador",
                        column: x => x.CodDoador,
                        principalTable: "Doador",
                        principalColumn: "CodDoador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoacaoObjeto_Endereco_IdEndereco",
                        column: x => x.IdEndereco,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoacaoObjeto_Voluntario_IdVoluntario",
                        column: x => x.IdVoluntario,
                        principalTable: "Voluntario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    CodEvento = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Cor = table.Column<string>(nullable: false),
                    CorDaFonte = table.Column<string>(nullable: false),
                    DataFim = table.Column<DateTime>(nullable: false),
                    DataInicio = table.Column<DateTime>(nullable: false),
                    Descricao = table.Column<string>(nullable: false),
                    IdUsuario = table.Column<string>(nullable: false),
                    InteressadoCodInteressado = table.Column<int>(nullable: true),
                    Relato = table.Column<string>(nullable: true),
                    Titulo = table.Column<string>(nullable: false),
                    ValorArrecadado = table.Column<double>(nullable: false),
                    VoluntarioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.CodEvento);
                    table.ForeignKey(
                        name: "FK_Evento_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evento_Interessado_InteressadoCodInteressado",
                        column: x => x.InteressadoCodInteressado,
                        principalTable: "Interessado",
                        principalColumn: "CodInteressado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Evento_Voluntario_VoluntarioId",
                        column: x => x.VoluntarioId,
                        principalTable: "Voluntario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InformativoVoluntario",
                columns: table => new
                {
                    IdVoluntario = table.Column<int>(nullable: false),
                    IdInformativo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformativoVoluntario", x => new { x.IdVoluntario, x.IdInformativo });
                    table.ForeignKey(
                        name: "FK_InformativoVoluntario_Informativo_IdInformativo",
                        column: x => x.IdInformativo,
                        principalTable: "Informativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InformativoVoluntario_Voluntario_IdVoluntario",
                        column: x => x.IdVoluntario,
                        principalTable: "Voluntario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InteressadoEvento",
                columns: table => new
                {
                    CodEvento = table.Column<int>(nullable: false),
                    CodInteressado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteressadoEvento", x => new { x.CodEvento, x.CodInteressado });
                    table.ForeignKey(
                        name: "FK_InteressadoEvento_Evento_CodEvento",
                        column: x => x.CodEvento,
                        principalTable: "Evento",
                        principalColumn: "CodEvento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InteressadoEvento_Interessado_CodInteressado",
                        column: x => x.CodInteressado,
                        principalTable: "Interessado",
                        principalColumn: "CodInteressado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoluntarioEvento",
                columns: table => new
                {
                    CodEvento = table.Column<int>(nullable: false),
                    IdVoluntario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoluntarioEvento", x => new { x.CodEvento, x.IdVoluntario });
                    table.ForeignKey(
                        name: "FK_VoluntarioEvento_Evento_CodEvento",
                        column: x => x.CodEvento,
                        principalTable: "Evento",
                        principalColumn: "CodEvento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoluntarioEvento_Voluntario_IdVoluntario",
                        column: x => x.IdVoluntario,
                        principalTable: "Voluntario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_IdInformativo",
                table: "Attachment",
                column: "IdInformativo");

            migrationBuilder.CreateIndex(
                name: "IX_ConhecimentoProficional_CodFavorecido",
                table: "ConhecimentoProficional",
                column: "CodFavorecido");

            migrationBuilder.CreateIndex(
                name: "IX_Despesa_IdItem",
                table: "Despesa",
                column: "IdItem");

            migrationBuilder.CreateIndex(
                name: "IX_Despesa_IdUsuario",
                table: "Despesa",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Despesa_CodFavorecido",
                table: "Despesa",
                column: "CodFavorecido");

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoDinheiro_CodDoador",
                table: "DoacaoDinheiro",
                column: "CodDoador");

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoObjeto_CodDoador",
                table: "DoacaoObjeto",
                column: "CodDoador");

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoObjeto_IdEndereco",
                table: "DoacaoObjeto",
                column: "IdEndereco");

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoObjeto_IdVoluntario",
                table: "DoacaoObjeto",
                column: "IdVoluntario");

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoSopa_CodDoador",
                table: "DoacaoSopa",
                column: "CodDoador");

            migrationBuilder.CreateIndex(
                name: "IX_DoacaoSopa_IdItem",
                table: "DoacaoSopa",
                column: "IdItem");

            migrationBuilder.CreateIndex(
                name: "IX_Doador_Email",
                table: "Doador",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doador_EnderecoId",
                table: "Doador",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_DoadorEvento_CodDoador",
                table: "DoadorEvento",
                column: "CodDoador");

            migrationBuilder.CreateIndex(
                name: "IX_DoadorEvento_CodEvento",
                table: "DoadorEvento",
                column: "CodEvento");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_IdUsuario",
                table: "Evento",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_InteressadoCodInteressado",
                table: "Evento",
                column: "InteressadoCodInteressado");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_VoluntarioId",
                table: "Evento",
                column: "VoluntarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Familia_CodFavorecido",
                table: "Familia",
                column: "CodFavorecido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Familia_IdEndereco",
                table: "Familia",
                column: "IdEndereco");

            migrationBuilder.CreateIndex(
                name: "IX_Favorecido_Cpf",
                table: "Favorecido",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorecido_IdUsuario",
                table: "Favorecido",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_FavorecidoEvento_CodEvento",
                table: "FavorecidoEvento",
                column: "CodEvento");

            migrationBuilder.CreateIndex(
                name: "IX_FavorecidoEvento_CodFavorecido",
                table: "FavorecidoEvento",
                column: "CodFavorecido");

            migrationBuilder.CreateIndex(
                name: "IX_Informativo_IdUsuario",
                table: "Informativo",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoDoador_CodDoador",
                table: "InformativoDoador",
                column: "CodDoador");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoDoador_IdInformativo",
                table: "InformativoDoador",
                column: "IdInformativo");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoUsuario_IdInformativo",
                table: "InformativoUsuario",
                column: "IdInformativo");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoUsuario_IdUsuario",
                table: "InformativoUsuario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoVoluntario_IdInformativo",
                table: "InformativoVoluntario",
                column: "IdInformativo");

            migrationBuilder.CreateIndex(
                name: "IX_InformativoVoluntario_IdVoluntario",
                table: "InformativoVoluntario",
                column: "IdVoluntario");

            migrationBuilder.CreateIndex(
                name: "IX_Interessado_Email",
                table: "Interessado",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InteressadoEvento_CodEvento",
                table: "InteressadoEvento",
                column: "CodEvento");

            migrationBuilder.CreateIndex(
                name: "IX_InteressadoEvento_CodInteressado",
                table: "InteressadoEvento",
                column: "CodInteressado");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Nome_Destino",
                table: "Item",
                columns: new[] { "Nome", "Destino" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModeloDeReceita_Nome",
                table: "ModeloDeReceita",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModeloDeReceitaItem_IdItem",
                table: "ModeloDeReceitaItem",
                column: "IdItem");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloDeReceitaItem_IdModeloDeReceita",
                table: "ModeloDeReceitaItem",
                column: "IdModeloDeReceita");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoSopa_IdModeloDeReceita",
                table: "ResultadoSopa",
                column: "IdModeloDeReceita");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoSopaItem_IdItem",
                table: "ResultadoSopaItem",
                column: "IdItem");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoSopaItem_IdResultadoSopa",
                table: "ResultadoSopaItem",
                column: "IdResultadoSopa");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Cpf",
                table: "AspNetUsers",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

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
                name: "IX_Voluntario_EnderecoId",
                table: "Voluntario",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_Voluntario_IdUsuario",
                table: "Voluntario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_VoluntarioEvento_CodEvento",
                table: "VoluntarioEvento",
                column: "CodEvento");

            migrationBuilder.CreateIndex(
                name: "IX_VoluntarioEvento_IdVoluntario",
                table: "VoluntarioEvento",
                column: "IdVoluntario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "ConhecimentoProficional");

            migrationBuilder.DropTable(
                name: "Despesa");

            migrationBuilder.DropTable(
                name: "DoacaoDinheiro");

            migrationBuilder.DropTable(
                name: "DoacaoObjeto");

            migrationBuilder.DropTable(
                name: "DoacaoSopa");

            migrationBuilder.DropTable(
                name: "DoadorEvento");

            migrationBuilder.DropTable(
                name: "Familia");

            migrationBuilder.DropTable(
                name: "FavorecidoEvento");

            /*migrationBuilder.DropTable(
                name: "MelhorDoador");

            migrationBuilder.DropTable(
                name: "EventoMaisProcurado");

            migrationBuilder.DropTable(
                name: "FavorecidoComGasto");
                */
            migrationBuilder.DropTable(
                name: "InformativoDoador");

            migrationBuilder.DropTable(
                name: "InformativoUsuario");

            migrationBuilder.DropTable(
                name: "InformativoVoluntario");

            migrationBuilder.DropTable(
                name: "InteressadoEvento");

            migrationBuilder.DropTable(
                name: "ModeloDeReceitaItem");

            migrationBuilder.DropTable(
                name: "ResultadoSopaItem");

            migrationBuilder.DropTable(
                name: "VoluntarioEvento");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Favorecido");

            migrationBuilder.DropTable(
                name: "Doador");

            migrationBuilder.DropTable(
                name: "Informativo");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "ResultadoSopa");

            migrationBuilder.DropTable(
                name: "Evento");

            migrationBuilder.DropTable(
                name: "ModeloDeReceita");

            migrationBuilder.DropTable(
                name: "Interessado");

            migrationBuilder.DropTable(
                name: "Voluntario");

            migrationBuilder.DropTable(
                name: "Endereco");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
