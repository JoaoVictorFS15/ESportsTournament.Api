using ESportsTournament.Api.DTOs;

namespace ESportsTournament.Api.Services
{
    public interface IEquipeService
    {
        Task<EquipeResponseDto> CriarEquipeAsync(EquipeCriacaoDto dto);

        Task<bool> ExcluirEquipeAsync(int id);

        Task<EquipeResponseDto> AtualizarEquipeAsync(int id, EquipeAtualizacaoDto dto);
    }
}
