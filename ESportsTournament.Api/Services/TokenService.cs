using ESportsTournament.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ESportsTournament.Api.Services
{
    public class TokenService
    {
        // Método que vai gerar a string do Token JWT
        public string GenerateToken(int usuarioId, string userEmail, string role, string jwtKey)
        {
            // 1. Configura a chave de segurança transformando nossa string em bytes
            var key = Encoding.ASCII.GetBytes(jwtKey);

            // 2. Define o que vai dentro do token (Payload)
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                new Claim(ClaimTypes.Email, userEmail),
                new Claim(ClaimTypes.Role, role)
            };

            // 3. Monta a configuração do Token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),// O token expira em 2 horas
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature // Algoritmo de criptografia
                )
            };

            // 4. Cria e retorna o Token em formato de string
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
