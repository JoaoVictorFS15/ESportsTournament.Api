using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var perfil = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

            var equipeCriada = await _equipeService.CriarEquipeAsync(dto, usuarioId, perfil);

            if (equipeCriada == null)
            {
                return BadRequest(new { Mensagem = "Não foi possível criar a equipe. Verifique as regras de negócio." });

            }

            return Created(string.Empty, equipeCriada);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Organizador,Capitao")]
        public async Task<IActionResult> ExcluirEquipe(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var perfil = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

            var resultado = await _equipeService.ExcluirEquipeAsync(id, usuarioId, perfil);

            if (!resultado)
            {
                return NotFound(new { Mensagem = "Equipe não encontrada." });
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Organizador,Capitao")]
        public async Task<IActionResult> AtualizarEquipe(int id, [FromBody] EquipeAtualizacaoDto dto)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var perfil = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

            var equipeAtualizada = await _equipeService.AtualizarEquipeAsync(id, dto, usuarioId, perfil);

            if (equipeAtualizada == null)
            {
                return NotFound(new { Mensagem = "Equipe não encontrada para atualização." });
            }

            return Ok(equipeAtualizada);
        }
    }
}
