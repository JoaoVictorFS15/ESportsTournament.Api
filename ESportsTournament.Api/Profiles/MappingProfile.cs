using AutoMapper;
using ESportsTournament.Api.DTOs;
using ESportsTournament.Api.Models;

namespace ESportsTournament.Api.Profiles
{
    /// 

    /// Classe responsável por ensinar ao AutoMapper como converter as nossas entidades e DTOs.
    /// 

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --------------------------------------------------
            // MAPEAMENTOS DE TORNEIO
            // --------------------------------------------------

            // Ensina a converter os dados de entrada (DTO) para a entidade do banco (Model)
            CreateMap<TorneioCriacaoDto, Torneio>();
            CreateMap<TorneioAtualizacaoDto, Torneio>();

            // Ensina a converter a entidade do banco (Model) para os dados de saída (DTO)
            CreateMap<Torneio, TorneioResponseDto>();

            // --------------------------------------------------
            // MAPEAMENTOS DE EQUIPE
            // --------------------------------------------------
            CreateMap<EquipeCriacaoDto, Equipe>();
            CreateMap<EquipeAtualizacaoDto, Equipe>();

            // Ensina a converter a entidade do banco (Model) para os dados de saída (DTO)
            CreateMap<Equipe, EquipeResponseDto>();
        }
    }
}
