using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESportsTournament.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TorneioController : ControllerBase
    {
        private readonly ILogger<TorneioController> _logger;
        private readonly ITorneioService _torneioService;

        public TorneioController(ITorneioService torneioService, ILogger<TorneioController> logger)
        {
            _logger = logger;
            _torneioService = torneioService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarTorneio([FromBody] TorneioCriacaoDto dto)
        {
            try
            {
                var torneioCriado = await _torneioService.CriaTorneioAsync(dto);
                return Created(string.Empty, torneioCriado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar torneio");
                return StatusCode(500, new { Mensagem = ex });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            try
            {
                var torneios = await _torneioService.ObterTodosAsync();
                return Ok(torneios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensagem = "Ocorreu um erro interno no servidor.",
                    Detalhe = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var torneio = await _torneioService.ObterPorIdAsync(id);
                if (torneio == null)
                {
                    return NotFound(new { Mensagem = "Torneio não encontrado." });
                }
                return Ok(torneio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensagem = "Ocorreu um erro interno no servidor.",
                    Detalhe = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarTorneio(int id, [FromBody] TorneioAtualizacaoDto dto)
        {
            try
            {
                var torneioAtualizado = await _torneioService.AtualizarTorneioAsync(id, dto);

                if (torneioAtualizado == null)
                {
                    return NotFound(new { Mensagem = "Torneio não encontrado para atualização." });
                }

                return Ok(torneioAtualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensagem = "Ocorreu um erro interno no servidor.",
                    Detalhe = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirTorneio(int id)
        {
            try
            {
                var sucesso = await _torneioService.ExcluirTorneioAsync(id);

                if (!sucesso)
                {
                    return NotFound(new { Mensagem = "Torneio não encontrado para exclusão." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Mensagem = "Ocorreu um erro interno ao tentar excluir o torneio.",
                    Detalhe = ex.Message
                });
            }
        }
    }
}
