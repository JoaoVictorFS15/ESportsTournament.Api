using ESportsTournament.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ESportsTournament.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Torneio> Torneios { get; set; }
        public DbSet<Equipe> Equipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Nick)
                .IsUnique();

            modelBuilder.Entity<Equipe>()
                .HasIndex(e => e.Nome)
                .IsUnique();
        }
    }
}
