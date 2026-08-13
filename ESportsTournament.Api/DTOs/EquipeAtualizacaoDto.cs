using System.ComponentModel.DataAnnotations;

namespace ESportsTournament.Api.DTOs
{
    public class EquipeAtualizacaoDto
    {
        [Required(ErrorMessage = "O nome da equipe é obrigatório.")]
        [MinLength(2, ErrorMessage = "O nome da equipe deve ter pelo menos 2 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A abreviação (Tag) é obrigatória.")]
        [StringLength(5, ErrorMessage = "A abreviação pode ter no máximo 5 letras.")]
        public string Abreviacao { get; set; } = string.Empty;

        public int? TorneioId { get; set; }
    }
}
