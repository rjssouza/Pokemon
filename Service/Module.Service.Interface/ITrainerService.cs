using Module.Dto.Pokemon;
using Module.Dto.Trainer;
using Module.Service.Interface.Base;
using System;

namespace Module.Service.Interface
{
    public interface ITrainerService : IBaseEntityService<PokemonTrainerDto, Guid>
    {
        PokemonDto Capture(Guid trainerId, string pokemonName);
    }
}