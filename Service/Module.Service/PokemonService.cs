using Module.Dto.Pokemon;
using Module.Integration.Interface;
using Module.Service.Base;
using Module.Service.Interface;
using System;
using System.Collections.Generic;

namespace Module.Service
{
    public class PokemonService : BaseService, IPokemonService
    {
        public IPokemonIntegration PokemonIntegration { get; set; }

        public PokemonDto GetPokemon(string pokemonName)
        {
            return this.PokemonIntegration.GetPokemon(pokemonName);
        }

        public List<PokemonBasicInfoDto> GetPokemonList()
        {
            return this.PokemonIntegration.GetPokemonList();
        }
    }
}