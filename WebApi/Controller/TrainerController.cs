using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Dto.Pokemon;
using Module.Dto.Trainer;
using Module.Service.Interface;
using System;
using WebApi.Base;

namespace WebApi.Controller
{
    /// <summary>
    /// Classe para obter pokemon
    /// </summary>
    [Route("api/trainer")]
    [ApiController]
    [AllowAnonymous]
    public class TrainerController : ServiceController
    {
        /// <summary>
        /// Serviço para obter pokemons e captura-los
        /// </summary>
        public ITrainerService TrainerService { get; set; }

        /// <summary>
        /// Captura um pokemon
        /// </summary>
        /// <param name="cpf">Identificador do treinador </param>
        /// <param name="pokemonName">Nome do pokemon</param>
        /// <returns>Dados do pokemon capturado</returns>
        [HttpGet("capture/{cpf}/{pokemonName}")]
        [ProducesResponseType(200, Type = typeof(PokemonDto))]
        public IActionResult Capture(string cpf, string pokemonName)
        {
            var result = this.TrainerService.Capture(cpf, pokemonName);

            return Ok(result);
        }

        /// <summary>
        /// Registra um treinador na base
        /// </summary>
        /// <param name="pokemonTrainer">Treinador</param>
        /// <returns>Identificador do treinador</returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult Post(PokemonTrainerDto pokemonTrainer)
        {
            var result = this.TrainerService.Insert(pokemonTrainer);

            return Ok(result);
        }
    }
}