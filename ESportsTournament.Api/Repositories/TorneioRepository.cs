using ESportsTournament.Api.Data;
using ESportsTournament.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ESportsTournament.Api.Repositories
{
    public class TorneioRepository : ITorneioRepository
    {
        private readonly AppDbContext _context;

        public TorneioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Torneio>> ObterTodosAsync()
        {
            return await _context.Torneios
                .Include(t => t.Equipes)
                .ToListAsync();
        }

        public async Task<Torneio> ObterPorIdAsync(int id)
        {
            return await _context.Torneios
                .Include(t => t.Equipes)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AdicionarAsync(Torneio torneio)
        {
            await _context.Torneios.AddAsync(torneio);
        }

        public async Task AtualizarAsync(Torneio torneio)
        {

            _context.Torneios.Update(torneio);
            await Task.CompletedTask;
        }

        public async Task RemoverAsync(Torneio torneio)
        {
            _context.Torneios.Remove(torneio);
            await Task.CompletedTask;
        }

        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

