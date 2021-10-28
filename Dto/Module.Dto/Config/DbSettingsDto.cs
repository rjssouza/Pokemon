using Module.Dto.Base;

namespace Module.Dto.Config
{
    /// <summary>
    /// Conexões de banco de dados
    /// </summary>
    public class DbSettingsDto : BaseDto
    {
        /// <summary>
        /// String de conexão padrão
        /// </summary>
        public string Default { get; set; }
    }
}