namespace Module.Dto.Pokemon
{
    /// <summary>
    /// Dados habilidade do pokemon
    /// </summary>
    public class AbilityDto
    {
        /// <summary>
        /// Indica se e oculta
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
    }
}