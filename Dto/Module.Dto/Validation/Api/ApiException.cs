using System.Net;

namespace Module.Dto.Validation.Api
{
    /// <summary>
    /// Exceção api
    /// </summary>
    public class ApiException : System.Exception
    {
        private readonly HttpStatusCode statusCode;

        /// <summary>
        /// Construtor da classe de exceção da api
        /// </summary>
        /// <param name="statusCode">Codigo http</param>
        /// <param name="message">Mensagem</param>
        /// <param name="ex">Exceção</param>
        public ApiException(HttpStatusCode statusCode, string message, System.Exception ex)
            : base(message, ex)
        {
            this.statusCode = statusCode;
        }

        /// <summary>
        /// Construtor da classe de exceção da api
        /// </summary>
        /// <param name="statusCode">Codigo http</param>
        /// <param name="message">Mensagem</param>
        public ApiException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.statusCode = statusCode;
        }

        /// <summary>
        /// Construtor da classe de exceção da api
        /// </summary>
        /// <param name="statusCode">Codigo http</param>
        public ApiException(HttpStatusCode statusCode)
        {
            this.statusCode = statusCode;
        }

        /// <summary>
        /// Código http da exceção
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                return this.statusCode;
            }
        }
    }
}