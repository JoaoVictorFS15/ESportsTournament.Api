using ESportsTournament.Api.Data;
using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ESportsTournament.Api.Services
{
    public class TorneioService : ITorneioService
    {
        private readonly AppDbContext _context;
        public TorneioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Torneio> CriaTorneioAsync(TorneioCriacaoDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var novoTorneio = new Torneio
                {
                    Nome = dto.Nome,
                    Jogo = dto.Jogo,
                    DataInicio = dto.DataInicio,
                    DataFim = dto.DataFim,
                    Premiacao = dto.Premiacao,
                    Status = "Aberto"
                };
                _context.Torneios.Add(novoTorneio);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return novoTorneio;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Ocorreu um erro ao cria o torneio no banco de dados.", ex);
            }

        }


        public async Task<IEnumerable<TorneioResponseDto>> ObterTodosAsync()
        {
            var torneios = await _context.Torneios.Include(x=> x.Equipes).ToListAsync();

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
                var torneio = await _context.Torneios
                    .Include(x => x.Equipes)
                    .FirstOrDefaultAsync(x=> x.Id == id);

                if (torneio == null)
                {
                    return null;
                }

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
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var torneioExistente = await _context.Torneios.FindAsync(id);
                if (torneioExistente == null)
                {
                    return null;
                }

                torneioExistente.Nome = dto.Nome;
                torneioExistente.Jogo = dto.Jogo;
                torneioExistente.DataInicio = dto.DataInicio;
                torneioExistente.DataFim = dto.DataFim;
                torneioExistente.Premiacao = dto.Premiacao;
                torneioExistente.Status = dto.Status;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new TorneioResponseDto
                {
                    Id = torneioExistente.Id,
                    Nome = torneioExistente.Nome,
                    Jogo = torneioExistente.Jogo,
                    DataInicio = torneioExistente.DataInicio,
                    DataFim = torneioExistente.DataFim,
                    Premiacao = torneioExistente.Premiacao,
                    Status = torneioExistente.Status
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Ocorreu um erro ao atualizar o torneio no banco de dados.", ex);
            }
        }
        
        public async Task<bool> ExcluirTorneioAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var torneioExistente = await _context.Torneios.FindAsync(id);
                if (torneioExistente == null)
                {
                    return false;
                }

                _context.Torneios.Remove(torneioExistente);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Ocorreu um erro ao excluir o torneio no banco de dados.", ex);
            }
        }
        
    }
}
