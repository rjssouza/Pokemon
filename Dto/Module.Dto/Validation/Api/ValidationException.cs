namespace Module.Dto.Validation.Api
{
    /// <summary>
    /// Exceção disparada após validação de regras de negócio
    /// </summary>
    public class ValidationException : ApiException
    {
        private readonly ValidationSummary validation;

        /// <summary>
        /// Construtor exceção de validação
        /// </summary>
        public ValidationException()
            : base(System.Net.HttpStatusCode.PreconditionFailed)
        {
        }

        /// <summary>
        /// Construtor exceção de validação
        /// </summary>
        /// <param name="validation">Sumário de validação</param>
        public ValidationException(ValidationSummary validation)
            : base(System.Net.HttpStatusCode.PreconditionFailed)
        {
            this.validation = validation;
        }

        /// <summary>
        /// Construtor exceção de validação
        /// </summary>
        /// <param name="subject">Assunto da validação</param>
        /// <param name="message">Mensagem da validação</param>
        public ValidationException(string subject, string message)
            : base(System.Net.HttpStatusCode.PreconditionFailed)
        {
            this.validation = new ValidationSummary();
            this.validation.AddError(subject, message);
        }

        /// <summary>
        /// Sumário da validação
        /// </summary>
        public ValidationSummary Validation
        {
            get
            {
                return this.validation;
            }
        }
    }
}