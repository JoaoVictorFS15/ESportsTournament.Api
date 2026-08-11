using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESportsTournament.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EquipeController : Controller
    {
        private readonly ILogger<EquipeController> _logger;
        private readonly IEquipeService _equipeService;
        public EquipeController(ILogger<EquipeController> logger, IEquipeService equipeService)
        {
            _logger = logger;
            _equipeService = equipeService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            var equipes = await _equipeService.ObterTodasAsync();
            return Ok(equipes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var equipe = await _equipeService.ObterPorIdAsync(id);
            if (equipe == null)
            {
                return NotFound(new { Mensagem = "Equipe não encontrada." });
            }
            return Ok(equipe);
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> ObterPorNome([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return BadRequest(new { Mensagem = "O nome para busca não pode ser vazio." });
            }

            var equipes = await _equipeService.ObterPorNomeAsync(nome);
            return Ok(equipes);
        }

        [HttpPost]
        public async Task<IActionResult> CriarEquipe([FromBody] EquipeCriacaoDto dto)
        {
            try
            {
                var equipeCriada = await _equipeService.CriarEquipeAsync(dto);

                if (equipeCriada == null)
                {
                    return BadRequest(new { Mensagem = "O torneio informado não existe. Verifique o TorneioId." });
                }

                return Created(string.Empty, equipeCriada);
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensagem = "Erro interno ao cadastrar a equipe.",
                    Detalhe = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirEquipe(int id)
        {
            try
            {
                var resultado = await _equipeService.ExcluirEquipeAsync(id);

                if (!resultado)
                {
                    return NotFound(new { Mensagem = "Equipe não encontrada." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensagem = "Erro interno ao excluir a equipe.",
                    Detalhe = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEquipe(int id, [FromBody] EquipeAtualizacaoDto dto)
        {
            try
            {
                var equipeAtualizada = await _equipeService.AtualizarEquipeAsync(id, dto);

                if (equipeAtualizada == null)
                {
                    return NotFound(new { Mensagem = "Equipe não encontrada para atualização." });
                }

                return Ok(equipeAtualizada);
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensagem = "Erro interno ao atualizar a equipe.",
                    Detalhe = ex.Message
                });
            }
        }
    }
}
