using AutoMapper;
using Module.Dto.Trainer;
using Module.Repository.Model;

namespace Module.IoC.Mapper.Configuracoes
{
    public class PokemonProfile : Profile
    {
        public PokemonProfile()
        {
            CreateMap<PokemonTrainerDto, PokemonTrainer>()
                .ReverseMap();
        }
    }
}