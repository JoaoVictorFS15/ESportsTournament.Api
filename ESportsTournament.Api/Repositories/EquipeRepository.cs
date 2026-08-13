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

        public async Task<(IEnumerable<Equipe> Itens, int Total)> ObterTodasAsync(int pagina, int tamanhoPagina, string? nome = null)
        {
            var query = _context.Equipes.Include(x => x.Capitao).AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(e => e.Nome.Contains(nome));
            }

            var total = await query.CountAsync();

            var itens = await query
                        .Skip((pagina - 1) * tamanhoPagina)
                        .Take(tamanhoPagina)
                        .ToListAsync();

            return (itens, total);
        }

        public async Task<Equipe?> ObterPorIdAsync(int id)
        {
            return await _context.Equipes
                .Include(e => e.Capitao)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Equipe?> ObterEquipePorCapitaoIdAsync(int capitaoId)
        {
            return await _context.Equipes.FirstOrDefaultAsync(e => e.CapitaoId == capitaoId);
        }
        public async Task<Equipe?> ObterEquipePorNomeCandidatoAsync(string nome)
        {
            return await _context.Equipes.FirstOrDefaultAsync(e => e.Nome.ToLower() == nome.ToLower());
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
