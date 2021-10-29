namespace Module.Dto.Pokemon
{
    /// <summary>
    /// Informações de sprite default do pokemon
    /// </summary>
    public class SpriteDto
    {
        /// <summary>
        /// Foto back
        /// </summary>
        public byte[] BackDefault { get; set; }
        
        /// <summary>
        /// Foto front
        /// </summary>
        public byte[] FrontDefault { get; set; }
    }
}