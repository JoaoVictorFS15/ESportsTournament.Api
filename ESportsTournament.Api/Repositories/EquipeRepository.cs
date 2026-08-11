using ESportsTournament.Api.Data;
using ESportsTournament.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ESportsTournament.Api.Repositories
{
    public class EquipeRepository : IEquipeRepository
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public EquipeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Equipe>> ObterTodasAsync()
        {
            return await _context.Equipes.ToListAsync();
        }

        public async Task<Equipe?> ObterPorIdAsync(int id)
        {
            return await _context.Equipes.FindAsync(id);
        }

        public async Task<List<Equipe>> ObterPorNomeAsync(string nome)
        {
            return await _context.Equipes.Where(e => e.Nome.Contains(nome)).ToListAsync();
        }

        public async Task AdicionarAsync(Equipe equipe)
        {
            await _context.Equipes.AddAsync(equipe);
        }

        public async Task AtualizarAsync(Equipe equipe)
        {
            _context.Equipes.Update(equipe);
            await Task.CompletedTask;
        }

        public async Task RemoverAsync(Equipe equipe)
        {
            _context.Equipes.Remove(equipe);
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
