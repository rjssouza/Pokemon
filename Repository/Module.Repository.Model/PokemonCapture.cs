using Module.Repository.Model.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Repository.Model
{
    [Table("PokemonCapture")]
    public class PokemonCapture : BaseIdentityModel<string>
    {
        [Column("pokemon_id")]
        public int PokemonId { get; set; }

        [Column("pokemon_name")]
        public string PokemonName { get; set; }

        [Column("trainer_cpf")]
        public string TrainerCpf { get; set; }
    }
}