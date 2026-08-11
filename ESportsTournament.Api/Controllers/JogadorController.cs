using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ESportsTournament.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JogadorController : ControllerBase
    {
        [HttpGet("perfil")]
        public IActionResult VerPerfil()
        {
            var emailLogado = User.FindFirst(ClaimTypes.Email)?.Value;
            var nivelAcesso = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Mensagem = "Bem-vindo à área restrita do torneio!",
                Email = emailLogado,
                Permissao = nivelAcesso
            });
        }
    }
}
