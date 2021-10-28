using System;

namespace Module.Util.Log
{
    /// <summary>
    /// Classe para log do sistema
    /// </summary>
    public class Logger : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Dispose pattern
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        ~Logger()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

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
        /// Método para escrever log a partir da exceção
        /// </summary>
        /// <param name="ex">Exceção disparada pelo sistema</param>
        public void Write(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}