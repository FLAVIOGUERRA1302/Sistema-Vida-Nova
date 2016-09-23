using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SistemaVidaNova.Models
{
    public class VidaNovaContext : IdentityDbContext<Usuario>
    {
        public VidaNovaContext(DbContextOptions<VidaNovaContext> options)
            : base(options)
        { }

        public DbSet<Doador> Doador { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Voluntario> Voluntario { get; set; }
        public DbSet<Interessado> Interessado { get; set; }
        public DbSet<Evento> Evento { get; set; }

        public DbSet<InteressadoEvento> InteressadoEvento { get; set; }
        public DbSet<VoluntarioEvento> VoluntarioEvento { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Voluntario>()
                .HasAlternateKey(c => c.Email);

            modelBuilder.Entity<InteressadoEvento>()
                .HasKey(t => new { t.CodEvento, t.CodInetessado });

            modelBuilder.Entity<VoluntarioEvento>()
                .HasKey(t => new { t.CodEvento, t.IdVoluntario });


            /*  modelBuilder.Entity<TUserRole>()
              .HasKey(r => new { r.UserId, r.RoleId })
              .ToTable("AspNetUserRoles");

              modelBuilder.Entity<TUserLogin>()
                  .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                  .ToTable("AspNetUserLogins");*/

        }

    }
}
