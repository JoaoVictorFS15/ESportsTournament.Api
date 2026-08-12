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
        public async Task<IActionResult> ObterTodas([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10, [FromQuery] string? nome = null)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1) tamanhoPagina = 10;
            if (tamanhoPagina > 50) tamanhoPagina = 50;

            var equipes = await _equipeService.ObterTodasAsync(pagina, tamanhoPagina, nome);
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



        [HttpPost]
        public async Task<IActionResult> CriarEquipe([FromBody] EquipeCriacaoDto dto)
        {
                var equipeCriada = await _equipeService.CriarEquipeAsync(dto);

                if (equipeCriada == null)
                {
                    return BadRequest(new { Mensagem = "O torneio informado não existe. Verifique o TorneioId." });
                }

                return Created(string.Empty, equipeCriada);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirEquipe(int id)
        {
           
                var resultado = await _equipeService.ExcluirEquipeAsync(id);

                if (!resultado)
                {
                    return NotFound(new { Mensagem = "Equipe não encontrada." });
                }

                return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEquipe(int id, [FromBody] EquipeAtualizacaoDto dto)
        {
          
                var equipeAtualizada = await _equipeService.AtualizarEquipeAsync(id, dto);

                if (equipeAtualizada == null)
                {
                    return NotFound(new { Mensagem = "Equipe não encontrada para atualização." });
                }

                return Ok(equipeAtualizada);
        }
    }
}
