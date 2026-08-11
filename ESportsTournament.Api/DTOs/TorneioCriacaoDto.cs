namespace ESportsTournament.Api.DTOs
{
    public class TorneioCriacaoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Jogo { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set;}
        public decimal Premiacao { get; set; }
    }
}
