namespace Module.Dto.Validation.Api
{
    /// <summary>
    /// Exceção disparada quando necessário a confirmação de um usuário 
    /// </summary>
    public class ConfirmationException : ApiException
    {
        private readonly string _message;

        /// <summary>
        /// Construtor da confirmação de usuário
        /// </summary>
        public ConfirmationException()
            : base(System.Net.HttpStatusCode.Conflict)
        {
        }

        /// <summary>
        /// Construtor da convirmação de usuário
        /// </summary>
        /// <param name="message">Mensagem de erro</param>
        public ConfirmationException(string message) : base(System.Net.HttpStatusCode.Conflict)
        {
            this._message = message;
        }

        /// <summary>
        /// Mensagem da exceção
        /// </summary>
        public override string Message => this._message;
    }
}