using ESportsTournament.Api.Data;
using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Models;
using ESportsTournament.Api.Repositories;

namespace ESportsTournament.Api.Services
{
    public class EquipeService : IEquipeService
    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly ITorneioRepository _torneioRepository;

        public EquipeService(IEquipeRepository equipeRepository, ITorneioRepository torneioRepository)
        {
            _equipeRepository = equipeRepository;
            _torneioRepository = torneioRepository;
        }

        public async Task<IEnumerable<EquipeResponseDto>> ObterTodasAsync()
        {
            var equipes = await _equipeRepository.ObterTodasAsync();
            return equipes.Select(e => new EquipeResponseDto
            {
                Id = e.Id,
                Nome = e.Nome,
                TorneioId = e.TorneioId
            });
        }

        public async Task<EquipeResponseDto?> ObterPorIdAsync(int id)
        {
            var equipe = await _equipeRepository.ObterPorIdAsync(id);
            if (equipe == null) return null;

            return new EquipeResponseDto
            {
                Id = equipe.Id,
                Nome = equipe.Nome,
                TorneioId = equipe.TorneioId
            };
        }

        public async Task<IEnumerable<EquipeResponseDto>> ObterPorNomeAsync(string nome)
        {
            var equipes = await _equipeRepository.ObterPorNomeAsync(nome);
            return equipes.Select(e => new EquipeResponseDto
            {
                Id = e.Id,
                Nome = e.Nome,
                TorneioId = e.TorneioId
            });
        }

        public async Task<EquipeResponseDto> CriarEquipeAsync(EquipeCriacaoDto dto)
        {
            await _equipeRepository.BeginTransactionAsync();
            try
            {
                var torneio = await _torneioRepository.ObterPorIdAsync(dto.TorneioId);

                if (torneio == null)
                {
                    return null;
                }

                if (torneio.Status != "Aberto")
                {
                    throw new InvalidOperationException($"Não é possível inscrever a equipe. O torneio selecionado está com status: '{torneio.Status}'.");
                }

                var novaEquipe = new Equipe
                {
                    Nome = dto.Nome,
                    TorneioId = dto.TorneioId
                };

                await _equipeRepository.AdicionarAsync(novaEquipe);
                await _equipeRepository.SalvarAlteracoesAsync();
                await _equipeRepository.CommitTransactionAsync();

                return new EquipeResponseDto
                {
                    Id = novaEquipe.Id,
                    Nome = novaEquipe.Nome,
                    TorneioId = novaEquipe.TorneioId
                };
            }
            catch (InvalidOperationException)
            {
                await _equipeRepository.RollbackTransactionAsync();
                throw;
            }
            catch (Exception ex)
            {
                await _equipeRepository.RollbackTransactionAsync();
                throw new Exception("Ocorreu um erro ao criar equipe no banco de dados.", ex);
            }
        }

        public async Task<bool> ExcluirEquipeAsync(int id)
        {
            await _equipeRepository.BeginTransactionAsync();
            try
            {
                var equipe = await _equipeRepository.ObterPorIdAsync(id);
                if (equipe == null)
                {
                    return false;
                }

                await _equipeRepository.RemoverAsync(equipe);
                await _equipeRepository.SalvarAlteracoesAsync();
                await _equipeRepository.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _equipeRepository.RollbackTransactionAsync();
                throw new Exception("Ocorreu um erro ao excluir a equipe no banco de dados.", ex);
            }
        }

        public async Task<EquipeResponseDto> AtualizarEquipeAsync(int id, EquipeAtualizacaoDto dto)
        {
            await _equipeRepository.BeginTransactionAsync();

            try
            {
                var equipe = await _equipeRepository.ObterPorIdAsync(id);
                if (equipe == null) return null;

                if (equipe.TorneioId != dto.TorneioId)
                {
                    var novoTorneio = await _torneioRepository.ObterPorIdAsync(dto.TorneioId);

                    if (novoTorneio == null)
                    {
                        throw new InvalidOperationException("O novo torneio informado não existe.");
                    }

                    if (novoTorneio.Status != "Aberto")
                    {
                        throw new InvalidOperationException($"Não é possível transferir a equipe. O novo torneio está com status: '{novoTorneio.Status}'.");
                    }
                }

                equipe.Nome = dto.Nome;
                equipe.TorneioId = dto.TorneioId;

                await _equipeRepository.AtualizarAsync(equipe);
                await _equipeRepository.SalvarAlteracoesAsync();
                await _equipeRepository.CommitTransactionAsync();

                return new EquipeResponseDto
                {
                    Id = equipe.Id,
                    Nome = equipe.Nome,
                    TorneioId = equipe.TorneioId
                };
            }
            catch (InvalidOperationException)
            {
                await _equipeRepository.RollbackTransactionAsync();
                throw;
            }
            catch (Exception ex)
            {
                await _equipeRepository.RollbackTransactionAsync();
                throw new Exception("Ocorreu um erro ao atualizar a equipe no banco de dados.", ex);
            }
        }
    }
}
