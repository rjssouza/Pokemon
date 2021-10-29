using Module.Dto.Pokemon;
using Module.Integration.Interface;
using Module.Repository.Interface;
using Module.Service.Base;
using Module.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Service
{
    public class PokemonService : BaseService, IPokemonService
    {
        public IPokemonIntegration PokemonIntegration { get; set; }
        public IPokemonCaptureRepository PokemonCaptureRepository { get; set; }
        public ITrainerService TrainerService { get; set; }

        public List<CapturedPokemonDto> GetCapturedPokemonList()
        {
            var allCaptured = this.PokemonCaptureRepository.GetAll();

            return allCaptured.Select(t => new CapturedPokemonDto()
            {
                Name = t.PokemonName,
                TrainerName = t.TrainerName
            }).ToList();
        }

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