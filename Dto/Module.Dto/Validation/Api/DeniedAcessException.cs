using System.Net;

namespace Module.Dto.Validation.Api
{
    /// <summary>
    /// Exceção de acesso negado
    /// </summary>
    public class DeniedAcessException : ApiException
    {
        /// <summary>
        /// Construtor 
        /// </summary>
        public DeniedAcessException() : base(HttpStatusCode.MethodNotAllowed)
        {
        }
    }
}