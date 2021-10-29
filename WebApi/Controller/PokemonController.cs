using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module.Dto.Pokemon;
using Module.Service.Interface;
using System.Collections.Generic;
using WebApi.Base;

namespace WebApi.Controller
{
    /// <summary>
    /// Classe para obter pokemon
    /// </summary>
    [Route("api/pokemon")]
    [ApiController]
    [AllowAnonymous]
    public class PokemonController : ServiceController
    {
        /// <summary>
        /// Serviço para obter pokemons e captura-los
        /// </summary>
        public IPokemonService PokemonService { get; set; }

        /// <summary>
        /// Obtem uma lista com 10 pokemons aleatorios
        /// </summary>
        /// <returns>Informações do pokemon</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<PokemonBasicInfoDto>))]
        public IActionResult Get()
        {
            var result = this.PokemonService.GetPokemonList();

            return Ok(result);
        }

        /// <summary>
        /// Obtem uma lista com 10 pokemons aleatorios
        /// </summary>
        /// <returns>Informações do pokemon</returns>
        [HttpGet("pokemonName")]
        [ProducesResponseType(200, Type = typeof(PokemonDto))]
        public IActionResult Get(string pokemonName)
        {
            var result = this.PokemonService.GetPokemon(pokemonName);

            return Ok(result);
        }

        /// <summary>
        /// Obtem uma lista de todos os pokemons capturados
        /// </summary>
        /// <returns>Informações do pokemon</returns>
        [HttpGet("captured")]
        [ProducesResponseType(200, Type = typeof(List<CapturedPokemonDto>))]
        public IActionResult GetCaptured()
        {
            var result = this.PokemonService.GetCapturedPokemonList();

            return Ok(result);
        }
    }
}