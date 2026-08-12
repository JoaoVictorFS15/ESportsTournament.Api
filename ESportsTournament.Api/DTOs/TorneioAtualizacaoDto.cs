using System.ComponentModel.DataAnnotations;

namespace ESportsTournament.Api.DTOs
{
    public class TorneioAtualizacaoDto
    {
        [Required(ErrorMessage = "O nome do torneio é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome do jogo é obrigatório.")]
        public string Jogo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime DataFim { get; set; }

        [Range(0, 9999999.99, ErrorMessage = "A premiação não pode ser negativa.")]
        public decimal Premiacao { get; set; }

        [Required(ErrorMessage = "O status do torneio é obrigatório.")]
        public string Status { get; set; } = string.Empty;
    }
}
