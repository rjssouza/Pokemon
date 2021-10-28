using Module.Factory.Base;
using Module.Factory.Interface.Conexao;
using System.Data;
using System.Data.SqlClient;

namespace Module.Factory.Conexao
{
    /// <summary>
    /// Classe factory de conexão com banco de dados
    /// </summary>
    public class DbConnectionFactory : BaseFactory, IDbConnectionFactory, IDbTransactionFactory
    {
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        private int _contadorTransacao;

        /// <summary>
        /// Conexão do banco de dados
        /// </summary>
        public IDbConnection DbConnection
        {
            get
            {
                if (this._dbConnection != null && this._dbConnection.State == ConnectionState.Closed)
                    this._dbConnection.Open();

                return this._dbConnection;
            }
        }

        /// <summary>
        /// Transação atual aberta (utilizada tanto nos services como no repository)
        /// </summary>
        public IDbTransaction Transaction
        {
            get
            {
                return this._dbTransaction;
            }
        }

        /// <summary>
        /// Inicializa a conexão e abre a mesma
        /// </summary>
        /// <param name="connectionString">String de conexão informada pelo construtor da injeção de dependencia</param>
        public DbConnectionFactory(string connectionString)
        {
            this._dbTransaction = null;
            this._dbConnection = new SqlConnection(connectionString);
            this._dbConnection.Open();
        }

        /// <summary>
        /// Metodo para abrir transação
        /// </summary>
        public void Open()
        {
            if (this._dbTransaction == null || this._dbTransaction.Connection.State == ConnectionState.Closed)
                this._dbTransaction = this.DbConnection.BeginTransaction();

            this._contadorTransacao++;
        }

        /// <summary>
        /// Método para atualizar transação
        /// </summary>
        public void Commit()
        {
            this._contadorTransacao--;

            if (_contadorTransacao <= 0)
                this._dbTransaction.Commit();
        }

        /// <summary>
        /// Método para rollback da transação
        /// </summary>
        public void Rollback()
        {
            if (this._dbTransaction != null && this._dbTransaction.Connection != null)
                this._dbTransaction.Rollback();
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            this.Rollback();
            this._dbConnection.Close();
            this._dbConnection = null;
        }
    }
}