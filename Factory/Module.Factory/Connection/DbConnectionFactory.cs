using Microsoft.AspNetCore.Hosting;
using Module.Factory.Base;
using Module.Factory.Interface.Conexao;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace Module.Factory.Conexao
{
    /// <summary>
    /// Classe factory de conexão com banco de dados
    /// </summary>
    public class DbConnectionFactory : BaseFactory, IDbConnectionFactory, IDbTransactionFactory
    {
        private int _contadorTransacao;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;

        /// <summary>
        /// Inicializa a conexão e abre a mesma
        /// </summary>
        /// <param name="connectionString">String de conexão informada pelo construtor da injeção de dependencia</param>
        public DbConnectionFactory(string sqliteDirectory, string rootPath)
        {
            var connectionString = this.TryBoostrapDataBase(sqliteDirectory, rootPath);

            this._dbTransaction = null;
            this._dbConnection = new SQLiteConnection(connectionString);
            this._dbConnection.Open();
        }

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
        /// Método para atualizar transação
        /// </summary>
        public void Commit()
        {
            this._contadorTransacao--;

            if (_contadorTransacao <= 0)
                this._dbTransaction.Commit();
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
            if (this._dbConnection == null)
                return;

            this.Rollback();
            this._dbConnection.Close();
            this._dbConnection = null;
        }

        private static void CreateCaptureTable(SQLiteConnection tempConnection)
        {
            using var createCaptureTable = tempConnection.CreateCommand();
            createCaptureTable.CommandText = "CREATE TABLE IF NOT EXISTS PokemonCapture(id text primary key, pokemon_id int, pokemon_name varchar(50), treinador_id text)";
            createCaptureTable.ExecuteNonQuery();
        }

        private static void CreateTrainerTable(SQLiteConnection tempConnection)
        {
            using var createTableCmd = tempConnection.CreateCommand();
            createTableCmd.CommandText = "CREATE TABLE IF NOT EXISTS PokemonTrainer(id text primary key, name Varchar(50), cpf VarChar(11), age int)";
            createTableCmd.ExecuteNonQuery();
        }

        private string TryBoostrapDataBase(string sqliteDirectory, string rootPath)
        {
            rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var sqliteFile = string.Concat(rootPath, sqliteDirectory);
            var connectionString = $"Data Source={sqliteFile}; Version=3;";
            if (!File.Exists(sqliteFile))
            {
                var dirInfo = new DirectoryInfo(sqliteFile).Parent;
                if (!(dirInfo.Exists))
                    dirInfo.Create();

                SQLiteConnection.CreateFile(sqliteFile);

                using var tempConnection = new SQLiteConnection(connectionString);
                tempConnection.Open();
                CreateTrainerTable(tempConnection);
                CreateCaptureTable(tempConnection);
            }

            return connectionString;
        }
    }
}