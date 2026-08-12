using ESportsTournament.Api.DTOs;

namespace ESportsTournament.Api.Services
{
    public interface IEquipeService
    {
        Task<PaginacaoResponseDto<EquipeResponseDto>> ObterTodasAsync(int pagina, int tamanhoPagina, string? nome = null);
        Task<EquipeResponseDto?> ObterPorIdAsync(int id);

        Task<EquipeResponseDto> CriarEquipeAsync(EquipeCriacaoDto dto);
        Task<bool> ExcluirEquipeAsync(int id);
        Task<EquipeResponseDto> AtualizarEquipeAsync(int id, EquipeAtualizacaoDto dto);
    }
}
