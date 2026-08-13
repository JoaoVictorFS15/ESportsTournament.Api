using ESportsTournament.Api.Data;
using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Models;
using ESportsTournament.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ESportsTournament.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, TokenService tokenService, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<string> RegistrarAsycn(RegistroDto dto)
        {
            var emailJaExiste = await _usuarioRepository.ObterPorEmailAsync(dto.Email);
            if (emailJaExiste != null) return "Erro: Email já cadastrado.";

            var nickJaExiste = await _usuarioRepository.ObterPorNickAsync(dto.Nick);
            if (nickJaExiste != null) return "Erro: Nick já está em uso.";


            var novoUsuario = new Usuario
            {
                Nome = dto.Nome,
                Nick = dto.Nick,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role = "Jogador"
            };

            await _usuarioRepository.AdicionarAsync(novoUsuario);
            await _usuarioRepository.SalvarAlteracoesAsync();

            return string.Empty;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);
            if (usuario == null) return null;

            bool senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash);

            if (!senhaValida) return null;

            var jwtKey = _configuration["Jwt:Key"];

            var token = _tokenService.GenerateToken(usuario.Id, usuario.Email, usuario.Role, jwtKey!);

            return token;
        }
    }
}
