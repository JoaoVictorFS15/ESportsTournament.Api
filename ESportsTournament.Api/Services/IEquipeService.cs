using ESportsTournament.Api.DTOs;

namespace ESportsTournament.Api.Services
{
    public interface IEquipeService
    {
        Task<IEnumerable<EquipeResponseDto>> ObterTodasAsync();
        Task<EquipeResponseDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<EquipeResponseDto>> ObterPorNomeAsync(string nome);

        Task<EquipeResponseDto> CriarEquipeAsync(EquipeCriacaoDto dto);
        Task<bool> ExcluirEquipeAsync(int id);
        Task<EquipeResponseDto> AtualizarEquipeAsync(int id, EquipeAtualizacaoDto dto);
    }
}
