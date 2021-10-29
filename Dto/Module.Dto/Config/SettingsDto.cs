using Module.Dto.Base;
using System.Collections.Generic;

namespace Module.Dto.Config
{
    /// <summary>
    /// Objeto de configurações do sistema (acessado por todas as camadas)
    /// </summary>
    public class SettingsDto : BaseDto
    {
        /// <summary>
        /// Endereços de api de integração externa
        /// </summary>
        public List<ExternalApiSettingsDto> ApiServicesUrl { get; set; }

        /// <summary>
        /// Conexões com banco de dados
        /// </summary>
        public DbSettingsDto DbConnection { get; set; }

        /// <summary>
        /// Chave de segurança autenticação
        /// </summary>
        public string SecureKey { get; set; }
        public string WebRootPath { get; set; }
    }
}