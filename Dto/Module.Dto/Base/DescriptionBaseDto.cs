namespace Module.Dto.Base
{
    /// <summary>
    /// Base objetos com nome e descrição
    /// </summary>
    public class DescriptionBaseDto : NameBaseDto
    {
        /// <summary>
        /// Descrição
        /// </summary>
        public string Description { get; set; }
    }
}