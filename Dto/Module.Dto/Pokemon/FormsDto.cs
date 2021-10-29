using Module.Dto.Base;

namespace Module.Dto.Pokemon
{
    /// <summary>
    /// Transformação do pokemon
    /// </summary>
    public class FormsDto 
    {
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