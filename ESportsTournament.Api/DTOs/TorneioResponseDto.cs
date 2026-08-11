using System.Collections.Generic;

namespace ESportsTournament.Api.DTOs
{
    public class TorneioResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Jogo { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal Premiacao { get; set; }
        public string Status { get; set; } = string.Empty;

        public List<EquipeResponseDto> Equipes { get; set; } = new List<EquipeResponseDto>();
    }
}
