using ESportsTournament.Api.Models;

namespace ESportsTournament.Api.Repositories
{
    public interface IEquipeRepository
    {
        Task<(IEnumerable<Equipe> Itens, int Total)> ObterTodasAsync(int pagina, int tamanhoPagina, string? nome = null);
        Task<Equipe?> ObterPorIdAsync(int id);
        Task<Equipe?> ObterEquipePorCapitaoIdAsync(int capitaoId);
        Task<Equipe?> ObterEquipePorNomeCandidatoAsync(string nome);

        Task AdicionarAsync(Equipe equipe);
        Task AtualizarAsync(Equipe equipe);
        Task RemoverAsync(Equipe equipe);
        Task SalvarAlteracoesAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
