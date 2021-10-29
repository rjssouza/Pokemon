using Module.Repository.Model;
using Module.Service.Interface;
using Module.Service.Validation.Base;
using Module.Service.Validation.Interface;
using System;

namespace Module.Service.Validation
{
    public class TrainerValidation : BaseCrudValidation<PokemonTrainer>, ITrainerValidation
    {
        public IPokemonService PokemonService { get; set; }
        public ITrainerService TrainerService { get; set; }

        public void CanCapture(string cpf, string pokemonName)
        {
            var pokemon = this.PokemonService.GetPokemon(pokemonName);
            if (pokemon == null)
                this.summary.AddError("Pokemon", "Este pokemon não existe");

            var trainer = this.TrainerService.GetByCpf(cpf);
            if (trainer == null)
                this.summary.AddError("Treinador", "Este treinador não existe");

            this.OnValidated();
        }
    }
}