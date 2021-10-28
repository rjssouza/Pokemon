using System;
using System.Collections.Generic;
using System.Linq;

namespace Module.Dto.Validation
{
    /// <summary>
    /// Sumário de validação
    /// </summary>
    public class ValidationSummary
    {
        private readonly Dictionary<string, ValidationError> errors;

        /// <summary>
        /// Construtor padrão sumário de validação
        /// </summary>
        public ValidationSummary()
        {
            this.errors = new Dictionary<string, ValidationError>();
        }

        /// <summary>
        /// Flag que indica se sumário possui erros
        /// </summary>
        public bool ContainsErrors
        {
            get
            {
                return errors.Count > 0;
            }
        }

        /// <summary>
        /// Mensagem de erro da validação formatada
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return this.GetErrorMessage();
            }
        }

        /// <summary>
        /// Dicionário de erros
        /// </summary>
        public Dictionary<string, ValidationError> Errors
        {
            get
            {
                return this.errors;
            }
        }

        /// <summary>
        /// Método que adiciona erro ao sumário
        /// </summary>
        /// <param name="subject">Tema do erro</param>
        /// <param name="message">Mensagem de erro</param>
        public void AddError(string subject, string message)
        {
            if (!errors.ContainsKey(subject))
            {
                ValidationError validationError = new(subject, message);

                errors.Add(subject, validationError);

                return;
            }

            errors[subject].Errors.Add(message);
        }

        /// <summary>
        /// Método que adiciona um objeto de validação ao dicionário
        /// </summary>
        /// <param name="validationError">Objeto que representa o erro de validação</param>
        public void AddValidationError(ValidationError validationError)
        {
            if (!errors.ContainsKey(validationError.Subject))
            {
                errors.Add(validationError.Subject, validationError);

                return;
            }

            errors[validationError.Subject].Errors.AddRange(validationError.Errors);
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

            List<string> errorList = this.Errors
                .Select((dictError) =>
                {
                    string errorsFromSubject = dictError.Value.GetErrorMessage(errorIndicator, _errorSeparator);

                    return errorsFromSubject;
                }).ToList();

            string errorMessage = string.Join(_errorSeparator, errorList);

            return errorMessage;
        }
    }
}