using ESportsTournament.Api.Models;

namespace ESportsTournament.Api.Repositories
{
    public interface ITorneioRepository
    {
        Task<(IEnumerable<Torneio> Itens, int Total)> ObterTodosAsync(int pagina, int tamanhoPagina, string? nome = null);
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
