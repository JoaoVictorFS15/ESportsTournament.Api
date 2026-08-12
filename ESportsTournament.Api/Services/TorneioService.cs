using AutoMapper;
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
        private readonly IMapper _mapper;
        public TorneioService(ITorneioRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Torneio> CriaTorneioAsync(TorneioCriacaoDto dto)
        {
            await _repository.BeginTransactionAsync();
            try
            {
                var novoTorneio = _mapper.Map<Torneio>(dto);

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


        public async Task<PaginacaoResponseDto<TorneioResponseDto>> ObterTodosAsync(int pagina, int tamanhoPagina, string? nome = null)
        {
            var resultado = await _repository.ObterTodosAsync(pagina, tamanhoPagina, nome);
            var itensDto = _mapper.Map<IEnumerable<TorneioResponseDto>>(resultado.Itens);
            var totalPaginas = (int)Math.Ceiling(resultado.Total / (double)tamanhoPagina);
            
            return new PaginacaoResponseDto<TorneioResponseDto>
            {
                PaginaAtual = pagina,
                TamanhoDaPagina = tamanhoPagina,
                TotalDeItens = resultado.Total,
                TotalDePaginas = totalPaginas,
                Itens = itensDto
            };
        }

        public async Task<TorneioResponseDto> ObterPorIdAsync(int id)
        {
            var torneio = await _repository.ObterPorIdAsync(id);

            if (torneio == null) return null;

            return _mapper.Map<TorneioResponseDto>(torneio);

        }

        public async Task<TorneioResponseDto> AtualizarTorneioAsync(int id, TorneioAtualizacaoDto dto)
        {
            await _repository.BeginTransactionAsync();

            try
            {
                var torneio = await _repository.ObterPorIdAsync(id);
                if (torneio == null) return null;

                _mapper.Map(dto, torneio);

                await _repository.AtualizarAsync(torneio);
                await _repository.SalvarAlteracoesAsync();

                // Confirma a transação
                await _repository.CommitTransactionAsync();

                return _mapper.Map<TorneioResponseDto>(torneio);
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
