using ESportsTournament.Api.Data;
using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ESportsTournament.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, TokenService tokenService, IConfiguration configuration)
        {
            _context = context;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<string> RegistrarAsycn(RegistroDto dto)
        {
            var emailJaExiste = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (emailJaExiste) return "Erro: Email já cadastrado.";

            var novoUsuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role = "Jogador"
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return string.Empty;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usuario == null) return null;

            bool senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash);

            if (!senhaValida) return null;

            var jwtKey = _configuration["Jwt:Key"];

            var token = _tokenService.GenerateToken(usuario.Email, usuario.Role, jwtKey!);

            return token;
        }
    }
}
