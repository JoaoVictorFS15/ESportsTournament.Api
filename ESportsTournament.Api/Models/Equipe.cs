namespace ESportsTournament.Api.Models
{
    public class Equipe
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public int TorneioId { get; set; }
        public Torneio? Torneio { get; set; }
    }
}
