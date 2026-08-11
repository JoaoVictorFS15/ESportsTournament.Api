using ESportsTournament.Api.DTOs;

namespace ESportsTournament.Api.Services
{
    public interface IAuthService
    {
        Task<string> RegistrarAsycn(RegistroDto dto);
        Task<string?> LoginAsync(LoginDto dto);
    }
}
