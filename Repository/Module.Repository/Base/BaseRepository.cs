using Dapper;
using Module.Factory.Interface.Conexao;
using Module.Repository.Interface.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Module.Repository.Base
{
    /// <summary>
    /// Classe base para repositorios implementa metodos de interação com o banco de dados
    /// </summary>
    public abstract class BaseRepository : IBaseRepository
    {
        /// <summary>
        /// Factory de conexão com banco de dados
        /// </summary>
        protected readonly IDbConnectionFactory _dbService;

        private bool disposedValue = false;

        public BaseRepository(IDbConnectionFactory dbService)
        {
            lock (this)
            {
                this._dbService = dbService;
                ConfigureDapper();
            }
        }

        /// <summary>
        /// Executa um comando sql no banco de dados
        /// </summary>
        /// <param name="sql">Código sql</param>
        /// <param name="parameterList">Lista de parâmetros</param>
        /// <returns>Resultado 1 = Sucesso, 0 = Falha</returns>
        protected virtual int Execute(string sql, DynamicParameters parameters = null)
        {
            return ExecuteFuncWithLog(sql, parameters, () => this._dbService.DbConnection.Execute(sql, parameters));
        }

        /// <summary>
        /// Obtém o resultado de uma contagem no banco
        /// </summary>
        /// <typeparam name="T">Tipo do retorno</typeparam>
        /// <param name="sql">Código sql</param>
        /// <param name="parameterList">Lista de parâmetros</param>
        /// <returns>Resultado da contagem no banco</returns>
        protected virtual T ExecuteScalar<T>(string sql, DynamicParameters parameters = null)
        {
            return ExecuteFuncWithLog(sql, parameters, () => this._dbService.DbConnection.ExecuteScalar<T>(sql, parameters, transaction: this._dbService.Transaction));
        }

        /// <summary>
        /// Obtém uma lista de retorno da consulta com base no tipo de retorno informado
        /// </summary>
        /// <typeparam name="T">Tipo do retorno</typeparam>
        /// <param name="sql">Código sql</param>
        /// <param name="parameterList">Lista de parâmetros</param>
        /// <returns>Lista de retorno da consulta</returns>
        protected virtual List<T> Select<T>(string sql, DynamicParameters parameters = null)
        {
            return ExecuteFuncWithLog(sql, parameters, () =>
            {
                var resultadoEnumerado = this._dbService.DbConnection.Query<T>(sql, parameters, transaction: this._dbService.Transaction);

                return resultadoEnumerado.ToList();
            });
        }

        /// <summary>
        /// Obtém o primeiro retorno da consulta
        /// </summary>
        /// <typeparam name="T">Tipo do retorno</typeparam>
        /// <param name="sql">Código sql</param>
        /// <param name="parameterList">Lista de parâmetros</param>
        /// <returns>Primeiro retorno da consulta ou null</returns>
        protected virtual T SelectFirstOrDefault<T>(string sql, DynamicParameters parameters = null)
        {
            return ExecuteFuncWithLog(sql, parameters, () => this._dbService.DbConnection.QueryFirstOrDefault<T>(sql, parameters, transaction: this._dbService.Transaction));
        }

        /// <summary>
        /// Configurações globais do dapper
        /// </summary>
        private static void ConfigureDapper()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        /// <summary>
        /// Método intercepta e efetua log das funcões sql solicitadas com tipo genérico
        /// </summary>
        /// <typeparam name="T">Retorno do método</typeparam>
        /// <param name="sql">Código sql</param>
        /// <param name="parameterList">Lista de parâmetros</param>
        /// <param name="returnFunc">Função que deverá receber log</param>
        /// <returns>Resultado da função</returns>
        private static T ExecuteFuncWithLog<T>(string sql, DynamicParameters parameterList, Func<T> returnFunc)
        {
            T result = default;
#if (DEBUG)
            var queryWatch = System.Diagnostics.Stopwatch.StartNew();
#endif
            result = returnFunc();

#if (DEBUG)
            queryWatch.Stop();
            LogSql(sql, parameterList, queryWatch.ElapsedMilliseconds);
#endif

            return result;
        }

        /// <summary>
        /// Obtém o nome do método
        /// </summary>
        /// <returns>Nome do método</returns>
        private static string GetMethodName()
        {
            try
            {
                var stackTrace = new StackTrace();
                var method = stackTrace?.GetFrames().Where(t => t.GetMethod().DeclaringType != null && t.GetMethod().DeclaringType.FullName.Contains("Module.Repository")).Select(t => t.GetMethod()).LastOrDefault();
                if (method == null || method.DeclaringType == null)
                    return string.Empty;

                var methodName = method.DeclaringType.FullName + "." + method.Name;
                return methodName;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Obtém o log sql com os parâmetros traduzidos
        /// </summary>
        /// <param name="sql">Codigo sql </param>
        /// <param name="parameterList">Lista de parametros</param>
        /// <param name="currentElapsedMs">Tempo que os métodos tiveram na execução em banco (em milisegundos)</param>
        private static void LogSql(string sql, DynamicParameters parameterList, long currentElapsedMs)
        {
            try
            {
                var method = GetMethodName();

                if (parameterList != null)
                {
                    string parametroMarcador = "@";

                    foreach (var key in parameterList.ParameterNames)
                    {
                        var consulta = string.Format("{0}{1}\\b", parametroMarcador, key);
                        var regex = new Regex(consulta, RegexOptions.IgnoreCase);

                        sql = regex.Replace(sql, ConverterParametro(parameterList.Get<dynamic>(key)));
                    }
                }

                Debug.WriteLine($"Log SQL - (QUERY TIME: {currentElapsedMs}ms): ({method}) - {sql}");
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Ocorreu um erro ao tentar logar a consulta:{0}{0}{1}", Environment.NewLine, exception.ToString());
            }

            // Método da função
            static string ConverterParametro(dynamic valor)
            {
                if (valor != null)
                {
                    Type type = valor.GetType();

                    if (type == typeof(DateTime))
                    {
                        return string.Format("CONVERT(DATETIME, '{0}', 103)", ((DateTime)valor).ToString("dd/MM/yyyy"));
                    }

                    if (type == typeof(bool))
                    {
                        return string.Format("{0}", valor ? "1" : "0");
                    }

                    if (type == typeof(string))
                    {
                        return string.Format("'{0}'", valor.ToString());
                    }

                    if (type == typeof(Guid))
                    {
                        return string.Format("'{0}'", valor.ToString());
                    }

                    if (valor is IEnumerable<object>)
                    {
                        List<string> sqlIn = new();

                        foreach (dynamic valorIn in valor)
                        {
                            string itemLista = ConverterParametro(valorIn);
                            sqlIn.Add(itemLista);
                        }

                        return string.Format("({0})", string.Join(", ", sqlIn));
                    }

                    return valor.ToString();
                }

                return "null";
            }
        }

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
                    if (!disposedValue)
                    {
                        if (disposing)
                        {
                        }

                        disposedValue = true;
                    }
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        ~BaseRepository()
        {
            Dispose(disposing: false);
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}