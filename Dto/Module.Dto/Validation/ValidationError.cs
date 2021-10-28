using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Dto.Validation
{
    /// <summary>
    /// Objeto que representa o erro de validação
    /// </summary>
    public class ValidationError
    {
        private readonly List<string> errors;
        private readonly string subject;

        /// <summary>
        /// Construtor do erro de validação
        /// </summary>
        /// <param name="subject">Assunto</param>
        /// <param name="message">Mensagem de erro</param>
        public ValidationError(string subject, string message)
        {
            this.errors = new List<string>() { message };
            this.subject = subject;
        }

        /// <summary>
        /// Construtor do erro de validação
        /// </summary>
        /// <param name="subject">Assunto</param>
        /// <param name="messages">Lista de mensagens</param>
        public ValidationError(string subject, List<string> messages)
        {
            this.errors = messages;
            this.subject = subject;
        }

        /// <summary>
        /// Flag que indica contém erro
        /// </summary>
        public bool ContainsErrors
        {
            get
            {
                return errors.Count > 0;
            }
        }

        /// <summary>
        /// Lista de erros
        /// </summary>
        public List<string> Errors
        {
            get
            {
                return this.errors;
            }
        }

        /// <summary>
        /// Assunto da validação
        /// </summary>
        public string Subject
        {
            get
            {
                return this.subject;
            }
        }

        /// <summary>
        /// Obtém a mensagens de erro
        /// </summary>
        /// <param name="errorIndicator">Caractere que indica a pontuação do erro</param>
        /// <param name="errorSeparator">Separador </param>
        /// <returns>Mensagem formatada</returns>
        public string GetErrorMessage(string errorIndicator = null, string errorSeparator = null)
        {
            if (!this.ContainsErrors)
            {
                return null;
            }

            string _errorSeparator = errorSeparator ?? Environment.NewLine;

            List<string> errorsList = this.Errors
                .Select((message) =>
                {
                    return $"{errorIndicator}{message}";
                }).ToList();

            string errorMessage = string.Join(_errorSeparator, errorsList);

            return errorMessage;
        }
    }
}