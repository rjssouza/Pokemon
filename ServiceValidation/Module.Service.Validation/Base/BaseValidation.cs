using Module.Dto.Config;
using Module.Dto.Validation;
using Module.Dto.Validation.Api;
using Module.Factory.Interface;
using System;

namespace Module.Service.Validation.Base
{
    /// <summary>
    /// Classe base para serviço de validação de regras de negócio
    /// </summary>
    public abstract class BaseValidation : IDisposable
    {
        protected ValidationSummary summary;
        private bool disposedValue;

        public BaseValidation()
        {
            summary = new ValidationSummary();
        }

        /// <summary>
        /// Objeto de configurações (registrado no início da aplicação)
        /// </summary>
        public SettingsDto Settings { get; set; }

        /// <summary>
        /// Factory para conversão de objetos
        /// </summary>
        public IObjectConverterFactory ObjectConverterFactory { get; set; }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    summary = null;
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Método para disparar gatilho da validação
        /// </summary>
        protected virtual void OnValidated()
        {
            if (summary.ContainsErrors)
            {
                throw new ValidationException(summary);
            }
        }
    }
}