using ESportsTournament.Api.Models;

namespace ESportsTournament.Api.Repositories
{
    public interface ITorneioRepository
    {
        Task<List<Torneio>> ObterTodosAsync();
        Task<Torneio> ObterPorIdAsync(int id);
        Task AdicionarAsync(Torneio torneio);
        Task AtualizarAsync(Torneio torneio);
        Task RemoverAsync(Torneio torneio);
        Task SalvarAlteracoesAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
