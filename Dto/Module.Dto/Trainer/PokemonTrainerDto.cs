using Module.Dto.Base;

namespace Module.Dto.Trainer
{
    /// <summary>
    /// Dados do treinador
    /// </summary>
    public class PokemonTrainerDto : BaseDto
    {
        /// <summary>
        /// Idade
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Cpf
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Name { get; set; }
    }
}