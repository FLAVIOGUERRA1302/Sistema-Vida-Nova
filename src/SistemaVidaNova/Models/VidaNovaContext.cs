using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models
{
    public class VidaNovaContext : DbContext
    {
        public VidaNovaContext(DbContextOptions<VidaNovaContext> options)
            : base(options)
        { }

        public DbSet<Doador> Doador { get; set; }
        public DbSet<Voluntario> Voluntario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Voluntario>()
                .HasAlternateKey(c => c.Email);
        }

    }
}
