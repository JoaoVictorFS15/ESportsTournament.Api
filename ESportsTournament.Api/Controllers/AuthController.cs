using ESportsTournament.Api.Data;
using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESportsTournament.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroDto dto)
        {
            var resultadoErro = await _authService.RegistrarAsycn(dto);
            if (!string.IsNullOrEmpty(resultadoErro))
            {
                _logger.LogError(resultadoErro);
                return BadRequest(new { Mensagem = resultadoErro });
            }

            return Ok(new { Mensagem = "Usuário cadastrado com sucesso!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);

            // Se o token for nulo, significa que o email ou a senha estão incorretos
            if (string.IsNullOrEmpty(token))
            {
                // Retornamos 401 Unauthorized (Não Autorizado)
                return Unauthorized(new { Mensagem = "Email ou senha incorretos." });
            }

            // Se deu tudo certo, devolvemos o Token com status 200 OK
            return Ok(new { Token = token });
        }
    }
}
