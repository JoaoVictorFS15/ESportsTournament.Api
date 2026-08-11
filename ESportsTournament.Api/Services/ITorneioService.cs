using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Models;

namespace ESportsTournament.Api.Services
{
    public interface ITorneioService
    {
        Task<Torneio> CriaTorneioAsync(TorneioCriacaoDto dto);

        Task<IEnumerable<TorneioResponseDto>> ObterTodosAsync();

        Task<TorneioResponseDto> ObterPorIdAsync(int id);

        Task<TorneioResponseDto> AtualizarTorneioAsync(int id, TorneioAtualizacaoDto dto);

        Task<bool> ExcluirTorneioAsync(int id);
    }
}
