using System.Collections.Generic;

namespace ESportsTournament.Api.Models
{
    public class Torneio
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Jogo { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal Premiacao { get; set; }
        public string Status { get; set; } = "Aberto";

        public List<Equipe> Equipes { get; set; } = new List<Equipe>();
    }
}
