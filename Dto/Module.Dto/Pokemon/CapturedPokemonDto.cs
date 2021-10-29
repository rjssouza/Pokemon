namespace Module.Dto.Pokemon
{
    /// <summary>
    /// Dados pokemon capturado
    /// </summary>
    public class CapturedPokemonDto : PokemonBasicInfoDto
    {
        /// <summary>
        /// Nome treinador
        /// </summary>
        public string TrainerName { get; set; }
    }
}