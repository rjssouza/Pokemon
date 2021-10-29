using Module.Dto.Pokemon;
using Module.Integration.Base;
using Module.Integration.Interface;
using Module.Util;
using Module.Util.Extensao;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Module.Integration
{
    public class PokemonIntegration : BaseIntegration, IPokemonIntegration
    {
        public const string POKEMON = "Pokemon";

        private const int PAGE_LIMIT = 10;

        public PokemonIntegration(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
        }

        protected override string Name => POKEMON;

        public PokemonDto GetPokemon(string pokemonName)
        {
            try
            {
                var result = this.Get<JObject>($"pokemon/{pokemonName}");

                var spriteBackDefault = this._httpClient
                    .GetAsync(result["sprites"]["back_default"].Value<string>())
                    .Result;
                var spriteBack = spriteBackDefault.Content.ReadAsStream();

                var spriteFrontDefault = this._httpClient
                    .GetAsync(result["sprites"]["front_default"].Value<string>())
                    .Result;

                var spriteFront = spriteFrontDefault.Content.ReadAsStream();

                var pokemon = new PokemonDto()
                {
                    Name = result["name"].Value<string>(),
                    ExperienceBase = result["base_experience"].Value<int>(),
                    Height = result["height"].Value<decimal>(),
                    Id = result["id"].Value<int>(),
                    Weight = result["weight"].Value<decimal>(),
                    AbilityList = result["abilities"].Select(t => new AbilityDto()
                    {
                        IsHidden = t["is_hidden"].Value<bool>(),
                        Name = t["ability"]["name"].Value<string>(),
                        Url = t["ability"]["url"].Value<string>()
                    }).ToList(),
                    Forms = result["forms"].Select(t => new FormsDto()
                    {
                        Name = t["name"].Value<string>(),
                        Url = t["url"].Value<string>()
                    }).ToList(),
                    SpriteDefault = new SpriteDto()
                    {
                        FrontDefault = spriteFront.ToArray(),
                        BackDefault = spriteBack.ToArray()
                    }
                };

                return pokemon;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<PokemonBasicInfoDto> GetPokemonList()
        {
            var offset = NumberHelper.GetRandomNumber(100);
            var result = this.Get<JObject>($"pokemon?limit={PAGE_LIMIT}&offset={offset}");
            var pokemonList = result["results"].Select(t => new PokemonBasicInfoDto()
            {
                Name = t["name"].Value<string>()
            }).ToList();

            return pokemonList;
        }
    }
}