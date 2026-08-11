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
    }
}
