using AutoMapper;
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
        private readonly IMapper _mapper;

        public EquipeService(IEquipeRepository equipeRepository, ITorneioRepository torneioRepository, IMapper mapper)
        {
            _equipeRepository = equipeRepository;
            _torneioRepository = torneioRepository;
            _mapper = mapper;
        }

        public async Task<PaginacaoResponseDto<EquipeResponseDto>> ObterTodasAsync(int pagina, int tamanhoPagina, string? nome = null)
        {
            var resultado = await _equipeRepository.ObterTodasAsync(pagina, tamanhoPagina, nome);
            var itensDto = _mapper.Map<IEnumerable<EquipeResponseDto>>(resultado.Itens);
            var totalPaginas = (int)Math.Ceiling(resultado.Total / (double)tamanhoPagina);

            return new PaginacaoResponseDto<EquipeResponseDto>
            {
                PaginaAtual = pagina,
                TamanhoDaPagina = tamanhoPagina,
                TotalDeItens = resultado.Total,
                TotalDePaginas = totalPaginas,
                Itens = itensDto
            };
        }

        public async Task<EquipeResponseDto?> ObterPorIdAsync(int id)
        {
            var equipe = await _equipeRepository.ObterPorIdAsync(id);
            if (equipe == null) return null;

            return _mapper.Map<EquipeResponseDto>(equipe);
        }

        public async Task<EquipeResponseDto> CriarEquipeAsync(EquipeCriacaoDto dto, int usuarioId, string perfil)
        {
            await _equipeRepository.BeginTransactionAsync();
            try
            {
                var equipeExistenteNome = await _equipeRepository.ObterEquipePorNomeCandidatoAsync(dto.Nome);
                if (equipeExistenteNome != null)
                {
                    throw new InvalidOperationException("Já existe uma equipe com este nome.");
                }

                var equipeExistenteCapitao = await _equipeRepository.ObterEquipePorCapitaoIdAsync(usuarioId);
                if (equipeExistenteCapitao != null)
                {
                    throw new InvalidOperationException("Você já é capitão de uma equipe. É permitido gerenciar apenas uma equipe por vez.");
                }

                if (dto.TorneioId.HasValue)
                {
                    var torneio = await _torneioRepository.ObterPorIdAsync(dto.TorneioId.Value);
                    if (torneio == null)
                        throw new InvalidOperationException("O torneio informado não existe.");

                    if (torneio.Status != "Aberto")
                        throw new InvalidOperationException($"O torneio selecionado está com status: '{torneio.Status}'.");
                }

                var novaEquipe = _mapper.Map<Equipe>(dto);

                novaEquipe.CapitaoId = usuarioId;

                // =========================================================================
                // ATENÇÃO: Mudança de Role do Usuário
                // Como o cara acabou de criar um time, ele vira 'Capitao'. 
                // Você vai precisar atualizar a tabela Usuario aqui no futuro!
                // Algo como: 
                // var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
                // usuario.Role = "Capitao";
                // await _usuarioRepository.AtualizarAsync(usuario);
                // =========================================================================

                await _equipeRepository.AdicionarAsync(novaEquipe);
                await _equipeRepository.SalvarAlteracoesAsync();
                await _equipeRepository.CommitTransactionAsync();

                return _mapper.Map<EquipeResponseDto>(novaEquipe);
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

        public async Task<bool> ExcluirEquipeAsync(int id, int usuarioId, string perfil)
        {
            await _equipeRepository.BeginTransactionAsync();
            try
            {
                var equipe = await _equipeRepository.ObterPorIdAsync(id);
                if (equipe == null)
                {
                    return false;
                }

                if (perfil != "Organizador" && equipe.CapitaoId != usuarioId)
                {
                    throw new UnauthorizedAccessException("Acesso negado: Você só pode excluir a sua própria equipe.");
                }

                await _equipeRepository.RemoverAsync(equipe);

                // =========================================================================
                // ATENÇÃO: Mudança de Role do Usuário
                // Se a equipe for apagada, o usuário volta a ser um "Jogador" comum.
                // Futuramente adicione a lógica com o UsuarioRepository aqui!
                // var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
                // usuario.Role = "Jogador";
                // await _usuarioRepository.AtualizarAsync(usuario);
                // =========================================================================

                await _equipeRepository.SalvarAlteracoesAsync();
                await _equipeRepository.CommitTransactionAsync();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                await _equipeRepository.RollbackTransactionAsync();
                throw; // Repassa o erro 403 para o Controller
            }

            catch (Exception ex)
            {
                await _equipeRepository.RollbackTransactionAsync();
                throw new Exception("Ocorreu um erro ao excluir a equipe no banco de dados.", ex);
            }
        }

        public async Task<EquipeResponseDto> AtualizarEquipeAsync(int id, EquipeAtualizacaoDto dto, int usuarioId, string perfil)
        {
            await _equipeRepository.BeginTransactionAsync();

            try
            {

                var equipe = await _equipeRepository.ObterPorIdAsync(id);
                if (equipe == null) return null;

                if (equipe.CapitaoId != usuarioId && perfil != "Organizador") 
                {
                    throw new UnauthorizedAccessException("Acesso negado. Apenas o capitão da equipe ou um Organizador podem editá-la.");
                }

                if (equipe.Nome.ToLower() != dto.Nome.ToLower())
                {
                    var equipeExistenteNome = await _equipeRepository.ObterEquipePorNomeCandidatoAsync(dto.Nome);
                    if (equipeExistenteNome != null)
                    {
                        throw new InvalidOperationException("Já existe uma equipe com este nome.");
                    }
                }

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

                _mapper.Map(dto, equipe);

                await _equipeRepository.AtualizarAsync(equipe);
                await _equipeRepository.SalvarAlteracoesAsync();
                await _equipeRepository.CommitTransactionAsync();

                return _mapper.Map<EquipeResponseDto>(equipe);
            }
            catch (UnauthorizedAccessException)
            {
                await _equipeRepository.RollbackTransactionAsync();
                throw;
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
