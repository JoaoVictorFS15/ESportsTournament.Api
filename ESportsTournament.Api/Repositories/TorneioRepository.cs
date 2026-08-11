using ESportsTournament.Api.Data;
using ESportsTournament.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ESportsTournament.Api.Repositories
{
    public class TorneioRepository : ITorneioRepository
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

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

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }
    }
}

