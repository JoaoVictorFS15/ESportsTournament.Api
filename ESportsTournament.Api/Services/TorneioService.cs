using ESportsTournament.Api.Data;
using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Models;
using ESportsTournament.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ESportsTournament.Api.Services
{
    public class TorneioService : ITorneioService
    {
        private readonly ITorneioRepository _repository;
        public TorneioService(ITorneioRepository repository)
        {
            _repository = repository;
        }

        public async Task<Torneio> CriaTorneioAsync(TorneioCriacaoDto dto)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                var novoTorneio = new Torneio
                {
                    Nome = dto.Nome,
                    Jogo = dto.Jogo,
                    DataInicio = dto.DataInicio,
                    DataFim = dto.DataFim,
                    Premiacao = dto.Premiacao
                };

                // O Repositório adiciona e salva
                await _repository.AdicionarAsync(novoTorneio);
                await _repository.SalvarAlteracoesAsync();
                await _repository.CommitTransactionAsync();

                return novoTorneio;
            }
            catch (Exception ex)
            {
                await _repository.RollbackTransactionAsync();
                throw new Exception("Ocorreu um erro ao cria o torneio no banco de dados.", ex);
            }

        }


        public async Task<IEnumerable<TorneioResponseDto>> ObterTodosAsync()
        {
            var torneios = await _repository.ObterTodosAsync();

            var torneiosDto = torneios.Select(t => new TorneioResponseDto
            {
                Id = t.Id,
                Nome = t.Nome,
                Jogo = t.Jogo,
                DataInicio = t.DataInicio,
                DataFim = t.DataFim,
                Premiacao = t.Premiacao,
                Status = t.Status,
                Equipes = t.Equipes.Select(e => new EquipeResponseDto
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    TorneioId = e.TorneioId
                }).ToList()

            });

            return torneiosDto;
        }

        public async Task<TorneioResponseDto> ObterPorIdAsync(int id)
        {
            try
            {
                var torneio = await _repository.ObterPorIdAsync(id);

                if (torneio == null) return null;

                return new TorneioResponseDto
                {
                    Id = torneio.Id,
                    Nome = torneio.Nome,
                    Jogo = torneio.Jogo,
                    DataInicio = torneio.DataInicio,
                    DataFim = torneio.DataFim,
                    Premiacao = torneio.Premiacao,
                    Status = torneio.Status,
                    Equipes = torneio.Equipes.Select(e => new EquipeResponseDto
                    {
                        Id = e.Id,
                        Nome = e.Nome,
                        TorneioId = e.TorneioId
                    }).ToList()
                };
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public async Task<TorneioResponseDto> AtualizarTorneioAsync(int id, TorneioAtualizacaoDto dto)
        {
            await _repository.BeginTransactionAsync();

            try
            {
                var torneio = await _repository.ObterPorIdAsync(id);
                if (torneio == null) return null;

                torneio.Nome = dto.Nome;
                torneio.Jogo = dto.Jogo;
                torneio.DataInicio = dto.DataInicio;
                torneio.DataFim = dto.DataFim;
                torneio.Premiacao = dto.Premiacao;
                torneio.Status = dto.Status;

                await _repository.AtualizarAsync(torneio);
                await _repository.SalvarAlteracoesAsync();

                // Confirma a transação
                await _repository.CommitTransactionAsync();

                return new TorneioResponseDto
                {
                    Id = torneio.Id,
                    Nome = torneio.Nome,
                    Jogo = torneio.Jogo,
                    DataInicio = torneio.DataInicio,
                    DataFim = torneio.DataFim,
                    Premiacao = torneio.Premiacao,
                    Status = torneio.Status
                };
            }
            catch (Exception ex)
            {
                await _repository.RollbackTransactionAsync();
                throw new Exception("Ocorreu um erro ao atualizar o torneio no banco de dados.", ex);
            }
        }
        
        public async Task<bool> ExcluirTorneioAsync(int id)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                var torneio = await _repository.ObterPorIdAsync(id);
                if (torneio == null) return false;

                await _repository.RemoverAsync(torneio);
                await _repository.SalvarAlteracoesAsync();
                await _repository.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _repository.RollbackTransactionAsync();
                throw new Exception("Ocorreu um erro ao excluir o torneio no banco de dados.", ex);
            }
        }
        
    }
}
