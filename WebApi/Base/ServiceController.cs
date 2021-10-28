using Microsoft.AspNetCore.Mvc;
using Module.Dto.Config;

namespace WebApi.Base
{
    /// <summary>
    /// Classe base para Controller
    /// </summary>
    public abstract class ServiceController : ControllerBase
    {
        /// <summary>
        /// Objeto de configurações (registrado no início da aplicação)
        /// </summary>
        public SettingsDto Settings { get; set; }
    }
}