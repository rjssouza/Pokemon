using Castle.DynamicProxy;
using Module.Factory.Interface.Conexao;
using System;

namespace Module.IoC.Middleware
{
    /// <summary>
    /// Interceptador para fechar transação de forma automatica
    /// </summary>
    public class TransactionInterceptor : IInterceptor
    {
        private readonly IDbTransactionFactory _transacaoFactory;

        public TransactionInterceptor(IDbTransactionFactory transacaoFactory)
        {
            this._transacaoFactory = transacaoFactory;
        }

        /// <summary>
        /// Método autofac para interceptar a chamada
        /// </summary>
        /// <param name="invocation">Chamada</param>
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception)
            {
                this.TentarRollback();

                throw;
            }
        }

        /// <summary>
        /// Tenta efetuar um rollback caso de falha de forma automatica
        /// </summary>
        private void TentarRollback()
        {
            try
            {
                this._transacaoFactory.Rollback();
            }
            finally
            {
            }
        }
    }
}