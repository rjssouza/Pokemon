using Module.Dto.Pokemon;
using Module.Service.Interface.Base;
using System.Collections.Generic;

namespace Module.Service.Interface
{
    public interface IPokemonService : IBaseService
    {
        PokemonDto GetPokemon(string pokemonName);

        List<PokemonBasicInfoDto> GetPokemonList();
    }
}