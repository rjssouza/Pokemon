using Module.Factory.Interface.Base;

namespace Module.Factory.Interface.Conexao
{
    public interface IDbTransactionFactory : IBaseFactory
    {
        /// <summary>
        /// Abrir transação
        /// </summary>
        void Open();

        /// <summary>
        /// Enviar dados ao banco
        /// </summary>
        void Commit();

        /// <summary>
        /// Desfazer alterações
        /// </summary>
        void Rollback();
    }
}