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
            var torneioCriado = await _torneioService.CriaTorneioAsync(dto);
            return Created(string.Empty, torneioCriado);
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10, [FromQuery] string? nome = null)
        {

            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1) tamanhoPagina = 10;
            if (tamanhoPagina > 50) tamanhoPagina = 50;

            var torneiosPaginados = await _torneioService.ObterTodosAsync(pagina, tamanhoPagina, nome);

            return Ok(torneiosPaginados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {

            var torneio = await _torneioService.ObterPorIdAsync(id);
            if (torneio == null)
            {
                return NotFound(new { Mensagem = "Torneio não encontrado." });
            }
            return Ok(torneio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarTorneio(int id, [FromBody] TorneioAtualizacaoDto dto)
        {

            var torneioAtualizado = await _torneioService.AtualizarTorneioAsync(id, dto);

            if (torneioAtualizado == null)
            {
                return NotFound(new { Mensagem = "Torneio não encontrado para atualização." });
            }

            return Ok(torneioAtualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirTorneio(int id)
        {
            var sucesso = await _torneioService.ExcluirTorneioAsync(id);

            if (!sucesso)
            {
                return NotFound(new { Mensagem = "Torneio não encontrado para exclusão." });
            }

            return NoContent();
        }
    }
}
