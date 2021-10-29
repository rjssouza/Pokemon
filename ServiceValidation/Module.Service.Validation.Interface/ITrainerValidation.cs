using Module.Repository.Model;
using Module.Service.Validation.Interface.Base;
using System;

namespace Module.Service.Validation.Interface
{
    public interface ITrainerValidation : IBaseCrudValidation<PokemonTrainer>
    {
        void CanCapture(string trainerId, string pokemonName);
    }
}