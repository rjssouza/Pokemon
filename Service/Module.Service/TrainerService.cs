using Module.Dto.Pokemon;
using Module.Dto.Trainer;
using Module.Repository.Interface;
using Module.Repository.Model;
using Module.Service.Base;
using Module.Service.Interface;
using Module.Service.Validation.Interface;
using System;
using System.Linq;

namespace Module.Service
{
    public class TrainerService : BaseEntityService<PokemonTrainer, PokemonTrainerDto, string, ITrainerRepository, ITrainerValidation>, ITrainerService
    {
        public override ITrainerRepository CrudRepository { get; set; }
        public override ITrainerValidation CrudValidation { get; set; }

        public IPokemonCaptureRepository PokemonCaptureRepository { get; set; }
        public IPokemonService PokemonService { get; set; }

        public PokemonDto Capture(string cpf, string pokemonName)
        {
            this.CrudValidation.CanCapture(cpf, pokemonName);

            var pokemon = this.PokemonService.GetPokemon(pokemonName);

            var pokemonCapture = new PokemonCapture()
            {
                PokemonId = pokemon.Id,
                PokemonName = pokemonName,
                TrainerCpf = cpf
            };

            this.PokemonCaptureRepository.Insert<string>(pokemonCapture);

            return pokemon;
        }

        public PokemonTrainerDto GetByCpf(string cpf)
        {
            var trainer = this.CrudRepository.GetAll().Where( t => t.Cpf == cpf);

            return this.ObjectConverterFactory.ConvertTo<PokemonTrainerDto>( trainer.FirstOrDefault());
        }
    }
}