using Module.Dto.Pokemon;
using Module.Dto.Trainer;
using Module.Repository.Interface;
using Module.Repository.Model;
using Module.Service.Base;
using Module.Service.Interface;
using Module.Service.Validation.Interface;
using System;

namespace Module.Service
{
    public class TrainerService : BaseEntityService<PokemonTrainer, PokemonTrainerDto, Guid, ITrainerRepository, ITrainerValidation>, ITrainerService
    {
        public override ITrainerRepository CrudRepository { get; set; }
        public override ITrainerValidation CrudValidation { get; set; }

        public IPokemonCaptureRepository PokemonCaptureRepository { get; set; }
        public IPokemonService PokemonService { get; set; }

        public PokemonDto Capture(Guid trainerId, string pokemonName)
        {
            this.CrudValidation.CanCapture(trainerId, pokemonName);

            var pokemon = this.PokemonService.GetPokemon(pokemonName);

            var pokemonCapture = new PokemonCapture()
            {
                PokemonId = pokemon.Id,
                PokemonName = pokemonName,
                TrainerId = trainerId
            };

            this.PokemonCaptureRepository.Insert<Guid>(pokemonCapture);

            return pokemon;
        }
    }
}