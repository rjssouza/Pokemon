namespace Module.Dto.Config
{
    /// <summary>
    /// Endereços de api de integração externa
    /// </summary>
    public class ExternalApiSettingsDto
    {
        /// <summary>
        /// Endereço de api dos serviços de via cep https://viacep.com.br/
        /// </summary>
        public string ViaCepApiUrl { get; set; }
    }
}