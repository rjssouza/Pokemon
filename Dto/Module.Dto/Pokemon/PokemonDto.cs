using System.Collections.Generic;

namespace Module.Dto.Pokemon
{
    /// <summary>
    /// Dados do pokemon
    /// </summary>
    public class PokemonDto : PokemonBasicInfoDto
    {
        /// <summary>
        /// Lista habilidades
        /// </summary>
        public List<AbilityDto> AbilityList { get; set; }

        /// <summary>
        /// Experiencia
        /// </summary>
        public int ExperienceBase { get; set; }

        /// <summary>
        /// Evoluções do pokemon
        /// </summary>
        public List<FormsDto> Forms { get; set; }

        /// <summary>
        /// Peso
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sprite default frente e verso
        /// </summary>
        public SpriteDto SpriteDefault { get; set; }

        /// <summary>
        /// Peso
        /// </summary>
        public decimal Weight { get; set; }
    }
}