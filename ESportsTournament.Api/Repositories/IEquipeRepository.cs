using ESportsTournament.Api.Models;

namespace ESportsTournament.Api.Repositories
{
    public interface IEquipeRepository
    {
        Task<List<Equipe>> ObterTodasAsync();
        Task<Equipe?> ObterPorIdAsync(int id);
        Task<List<Equipe>> ObterPorNomeAsync(string nome);
        
        Task AdicionarAsync(Equipe equipe);
        Task AtualizarAsync(Equipe equipe);
        Task RemoverAsync(Equipe equipe);
        Task SalvarAlteracoesAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
