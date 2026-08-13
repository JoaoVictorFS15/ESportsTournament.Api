using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Models;

namespace ESportsTournament.Api.Services
{
    public interface IEquipeService
    {
        Task<PaginacaoResponseDto<EquipeResponseDto>> ObterTodasAsync(int pagina, int tamanhoPagina, string? nome = null);
        Task<EquipeResponseDto?> ObterPorIdAsync(int id);

        Task<EquipeResponseDto> CriarEquipeAsync(EquipeCriacaoDto dto, int usuarioId, string perfil);
        Task<bool> ExcluirEquipeAsync(int id, int usuarioId, string perfil);
        Task<EquipeResponseDto> AtualizarEquipeAsync(int id, EquipeAtualizacaoDto dto, int usuarioId, string perfil);
    }
}
