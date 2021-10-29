using Module.Repository.Model.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Repository.Model
{
    [Table("PokemonTrainer")]
    public class PokemonTrainer : BaseIdentityModel<Guid>
    {
        [Column("age")]
        public int Age { get; set; }

        [Column("cpf")]
        public string Cpf { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}