using ESportsTournament.Api.Data;
using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Models;

namespace ESportsTournament.Api.Services
{
    public class EquipeService : IEquipeService
    {
        private readonly AppDbContext _context;
        public EquipeService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<EquipeResponseDto> CriarEquipeAsync(EquipeCriacaoDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var torneio = await _context.Torneios.FindAsync(dto.TorneioId);

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

                _context.Equipes.Add(novaEquipe);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new EquipeResponseDto
                {
                    Id = novaEquipe.Id,
                    Nome = novaEquipe.Nome,
                    TorneioId = novaEquipe.TorneioId
                };
            }
            catch (InvalidOperationException)
            {
                await transaction.RollbackAsync();
                throw;
            }

            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                throw new Exception("Ocorreu um erro ao criar equipe no banco de dados.", ex);
            }
        }
        public async Task<bool> ExcluirEquipeAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var equipe = await _context.Equipes.FindAsync(id);
                if (equipe == null)
                {
                    return false;
                }

                _context.Equipes.Remove(equipe);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Ocorreu um erro ao excluir a equipe no banco de dados.", ex);
            }
        }

        public async Task<EquipeResponseDto> AtualizarEquipeAsync(int id, EquipeAtualizacaoDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var equipe = await _context.Equipes.FindAsync(id);
                if (equipe == null) return null;

                if (equipe.TorneioId != dto.TorneioId)
                {
                    var novoTorneio = await _context.Torneios.FindAsync(dto.TorneioId);

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

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new EquipeResponseDto
                {
                    Id = equipe.Id,
                    Nome = equipe.Nome,
                    TorneioId = equipe.TorneioId
                };
            }
            catch (InvalidOperationException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Ocorreu um erro ao atualizar a equipe no banco de dados.", ex);
            }
        }

    }
}
