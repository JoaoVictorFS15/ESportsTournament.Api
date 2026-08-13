using ESportsTournament.Api.Models;

namespace ESportsTournament.Api.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorIdAsync(int id);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<Usuario?> ObterPorNickAsync(string nick);
        Task AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task SalvarAlteracoesAsync();
    }
}
