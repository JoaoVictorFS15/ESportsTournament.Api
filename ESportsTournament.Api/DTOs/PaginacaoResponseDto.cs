using System;
using System.Collections.Generic;

namespace ESportsTournament.Api.DTOs
{
    /// 
    /// Envelope padronizado para respostas paginadas.
    /// 

    public class PaginacaoResponseDto<T>
    {
        public int PaginaAtual { get; set; }
        public int TamanhoDaPagina { get; set; }
        public int TotalDeItens { get; set; }
        public int TotalDePaginas { get; set; }

        // Aqui é onde os torneios (ou outra entidade) vão ficar
        public IEnumerable<T> Itens { get; set; } = new List<T>();
    }
}
