namespace ESportsTournament.Api.DTOs
{
    public class EquipeResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int TorneioId { get; set; }
        public int CapitaoId { get; set; }
    }
}
