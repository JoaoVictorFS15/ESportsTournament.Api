namespace ESportsTournament.Api.DTOs
{
    public class EquipeResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Abreviacao { get; set; } = string.Empty;
        public int TorneioId { get; set; }
        public string CapitaoNome { get; set; } = string.Empty;
        public string CapitaoNick { get; set; } = string.Empty;
    }
}
