using Module.Dto.Config;
using Module.Factory.Interface;
using Module.Factory.Interface.Conexao;
using Module.Service.Interface.Base;
using System;

namespace Module.Service.Base
{
    /// <summary>
    /// Classe base para serviços
    /// </summary>
    public abstract class BaseService : IBaseService
    {
        private bool disposedValue;

        public BaseService()
        {
        }

        /// <summary>
        /// Objeto de configurações (registrado no início da aplicação)
        /// </summary>
        public SettingsDto Settings { get; set; }

        /// <summary>
        /// Factory para conversão de objetos automatica utilizando factory
        /// </summary>
        public IObjectConverterFactory ObjectConverterFactory { get; set; }

        /// <summary>
        /// Factory para construção de transação
        /// </summary>
        public IDbTransactionFactory DbTransactionFactory { get; set; }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Método para abrir transação utilizando o TransacaoDbFactory
        /// </summary>
        protected void OpenTransaction()
        {
            this.DbTransactionFactory.Open();
        }

        /// <summary>
        /// Método para atualizar transação
        /// </summary>
        protected void Commit()
        {
            this.DbTransactionFactory.Commit();
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
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Método para rollback da transação
        /// </summary>
        protected void Rollback()
        {
            this.DbTransactionFactory.Rollback();
        }
    }
}