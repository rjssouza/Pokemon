using Module.Dto.Pokemon;
using Module.Integration.Interface.Base;
using System.Collections.Generic;

namespace Module.Integration.Interface
{
    public interface IPokemonIntegration : IBaseIntegration
    {
        PokemonDto GetPokemon(string pokemonName);

        List<PokemonBasicInfoDto> GetPokemonList();
    }
}