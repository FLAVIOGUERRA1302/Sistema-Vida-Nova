using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SistemaVidaNova.Models.FromSql;

namespace SistemaVidaNova.Models
{
    public class VidaNovaContext : IdentityDbContext<Usuario>
    {
        public VidaNovaContext(DbContextOptions<VidaNovaContext> options)
            : base(options)
        { }

        public DbSet<Doador> Doador { get; set; }
        public DbSet<PessoaFisica> DoadorPessoaFisica { get; set; }
        public DbSet<PessoaJuridica> DoadorPessoaJuridica { get; set; }
        public DbSet<Favorecido> Favorecido { get; set; }
        public DbSet<Familia> Familia { get; set; }
        public DbSet<ConhecimentoProficional> ConhecimentoProficional { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Voluntario> Voluntario { get; set; }
        public DbSet<Interessado> Interessado { get; set; }
        public DbSet<Evento> Evento { get; set; }

        public DbSet<InteressadoEvento> InteressadoEvento { get; set; }
        public DbSet<VoluntarioEvento> VoluntarioEvento { get; set; }
        public DbSet<DoadorEvento> DoadorEvento { get; set; }
        
        public DbSet<FavorecidoEvento> FavorecidoEvento { get; set; }
        public DbSet<Informativo> Informativo { get; set; }
        public DbSet<Attachment> Attachment { get; set; }
        
        public DbSet<Endereco> Endereco { get; set; }

        public DbSet<Item> Item { get; set; }
        public DbSet<ItemAssociacao> ItemAssociacao { get; set; }
        public DbSet<ItemFavorecido> ItemFavorecido { get; set; }
        public DbSet<ItemSopa> ItemSopa { get; set; }
        public DbSet<Despesa> Despesa { get; set; }
        public DbSet<DespesaAssociacao> DespesaAssociacao { get; set; }
        public DbSet<DespesaFavorecido> DespesaFavorecido { get; set; }
        public DbSet<DespesaSopa> DespesaSopa { get; set; }
        public DbSet<DoacaoDinheiro> DoacaoDinheiro { get; set; }
        public DbSet<DoacaoObjeto> DoacaoObjeto { get; set; }
        public DbSet<DoacaoSopa> DoacaoSopa { get; set; }

        public DbSet<ModeloDeReceita> ModeloDeReceita { get; set; }
        public DbSet<ModeloDeReceitaItem> ModeloDeReceitaItem { get; set; }

        
        public DbSet<ResultadoSopa> ResultadoSopa { get; set; }

        public DbSet<EventoMaisProcurado> EventoMaisProcurado { get; set; }

        public DbSet<DoadorComQuantidadeDeDoacoes> MelhorDoador { get; set; }
        public DbSet<FavorecidoComGasto> FavorecidoComGasto { get; set; }

        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Voluntario>()
                .HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<Interessado>()
                .HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<Voluntario>()
                .HasIndex(c => c.Cpf).IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(c => c.Cpf).IsUnique();

            modelBuilder.Entity<Doador>()
                .HasDiscriminator<string>("doador_type")
                .HasValue<PessoaFisica>("PF")
                .HasValue<PessoaJuridica>("PJ");

            modelBuilder.Entity<Doador>()
                .HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<Favorecido>()
                .HasIndex(c => c.Cpf).IsUnique();



            modelBuilder.Entity<InteressadoEvento>()
                .HasKey(t => new { t.CodEvento, t.CodInteressado });

            modelBuilder.Entity<VoluntarioEvento>()
                .HasKey(t => new { t.CodEvento, t.IdVoluntario });

            modelBuilder.Entity<FavorecidoEvento>()
                .HasKey(t => new { t.CodEvento, t.CodFavorecido });

            modelBuilder.Entity<DoadorEvento>()
                .HasKey(t => new { t.CodEvento, t.CodDoador });
            
            
            modelBuilder.Entity<InformativoDoador>()
                .HasKey(t => new { t.CodDoador, t.IdInformativo });

            modelBuilder.Entity<InformativoUsuario>()
                .HasKey(t => new { t.IdUsuario, t.IdInformativo });

            modelBuilder.Entity<InformativoVoluntario>()
                .HasKey(t => new { t.IdVoluntario, t.IdInformativo });

            //chave unica, somente uma descrição por destino
            modelBuilder.Entity<Item>()
                .HasIndex(c => new { c.Nome, c.Destino }).IsUnique();

            modelBuilder.Entity<Item>()
               .HasDiscriminator<string>("Destino")
               .HasValue<ItemAssociacao>("ASSOCIACAO")
               .HasValue<ItemFavorecido>("FAVORECIDO")
               .HasValue<ItemSopa>("SOPA");

            modelBuilder.Entity<Despesa>()
              .HasDiscriminator<string>("Tipo")
              .HasValue<DespesaAssociacao>("ASSOCIACAO")
              .HasValue<DespesaFavorecido>("FAVORECIDO")
              .HasValue<DespesaSopa>("SOPA");

            modelBuilder.Entity<ModeloDeReceitaItem>()
                .HasKey(t => new { t.IdItem, t.IdModeloDeReceita });

            modelBuilder.Entity<ModeloDeReceita>()
                .HasIndex(c => c.Nome).IsUnique();

            modelBuilder.Entity<ResultadoSopaItem>()
                .HasKey(t => new { t.IdItem, t.IdResultadoSopa });


            modelBuilder.Entity<Item>()
                .Property(b => b.QuantidadeEmEstoque)
                .HasDefaultValue(0.0d);

            modelBuilder.Entity<Voluntario>()
               .Property(b => b.DataCurso)
               .HasDefaultValue(DateTime.Today);

            //modelBuilder.Ignore<EventoMaisProcurado>();
            

        }

    }
}
