using System.Collections.Generic;

namespace Module.Dto.Pokemon
{
    /// <summary>
    /// Dados do pokemon
    /// </summary>
    public class PokemonDto : PokemonBasicInfoDto
    {
        public List<AbilityDto> AbilityList { get; set; }
        public int ExperienceBase { get; set; }
        public List<FormsDto> Forms { get; set; }
        public decimal Height { get; set; }
        public int Id { get; set; }
        public SpriteDto SpriteDefault { get; set; }
        public decimal Weight { get; set; }
    }
}