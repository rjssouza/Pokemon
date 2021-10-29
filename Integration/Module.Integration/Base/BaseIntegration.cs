using Module.Dto.Config;
using Module.Dto.Validation.Api;
using Module.Integration.Interface.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Module.Integration.Base
{
    /// <summary>
    /// Base para integrações externas do sistema
    /// </summary>
    public abstract class BaseIntegration : IBaseIntegration
    {
        protected HttpClient _httpClient;
        private bool disposedValue;
        public List<ExternalApiSettingsDto> TEst { get; set; }
        ~BaseIntegration()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public BaseIntegration(IHttpClientFactory httpClientFactory)
        {
            this._httpClient = httpClientFactory.CreateClient(this.Name);
        }

        /// <summary>
        /// Objeto de cabeçalho de autenticação
        /// </summary>
        public AuthenticationHeaderValue Authentication { get; }

        /// <summary>
        /// Objeto de configurações (registrado no início da aplicação)
        /// </summary>
        public SettingsDto Settings { get; set; }

        /// <summary>
        /// Endereço api da integração
        /// </summary>
        protected virtual string ApiAddress
        {
            get
            {
                var apiAddresss = this.Settings.ApiServicesUrl.Where(t => t.Name == this.Name)
                    .FirstOrDefault();

                return apiAddresss.Url;
            } 
        }

        /// <summary>
        /// Nome da integração
        /// </summary>
        protected abstract string Name { get; }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Chama um metodo sincrono com verbo http DELETE
        /// </summary>
        /// <typeparam name="TRetorno">Tipo do retorno esperado pela api</typeparam>
        /// <param name="urlRequisicao">Url da requisição</param>
        /// <param name="dados">Dados que irão ao corpo da requisição</param>
        /// <returns>Retorno esperado pela api</returns>
        protected TRetorno Delete<TRetorno>(string urlRequisicao, object dados = null)
        {
            var resultado = ExecuteSync(async () =>
            {
                var url = this.FormatUrl(urlRequisicao);
                var respostaHttp = await this.SendRequest(HttpMethod.Delete, url, dados);
                var objetoRetorno = await this.ProcessHttpResponse<TRetorno>(respostaHttp);

                return objetoRetorno;
            });

            return resultado;
        }

        /// <summary>
        /// Chama um metodo assincrono com verbo http DELETE
        /// </summary>
        /// <typeparam name="TRetorno">Tipo do retorno esperado pela api</typeparam>
        /// <param name="urlRequisicao">Url da requisição</param>
        /// <param name="dados">Dados que irão ao corpo da requisição</param>
        /// <returns>Retorno esperado pela api async</returns>
        protected async Task<HttpResponseMessage> Delete(string urlRequisicao, object dados = null)
        {
            var url = this.FormatUrl(urlRequisicao);
            var respostaHttp = await this.SendRequest(HttpMethod.Delete, url, dados);

            return respostaHttp;
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                this._httpClient.Dispose();
                this._httpClient = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// Método auxiliar para executar chamada assincrona de forma sincrona e traduzir seu retorno
        /// </summary>
        /// <typeparam name="TRetorno">Tipo do objeto de retorno</typeparam>
        /// <param name="taskAssincrona">Task assincrona para efetuar chamada</param>
        /// <returns>Objeto de retorno</returns>
        protected static TRetorno ExecuteSync<TRetorno>(Func<Task<TRetorno>> taskAssincrona)
        {
            try
            {
                TRetorno retorno = default;
                Task.Run<TRetorno>(async () => await taskAssincrona.Invoke().ConfigureAwait(false))
                    .ContinueWith<TRetorno>((retornoData) => retorno = retornoData.Result)
                    .Wait();

                return retorno;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                // Tratamento de erro generico de thread assíncrona, retorna o erro "real" para o sistema.
                throw ex;
            }
        }

        /// <summary>
        /// Método auxiliar para executar chamada assincrona de forma sincrona e traduzir seu retorno
        /// </summary>
        /// <typeparam name="TRetorno">Tipo do objeto de retorno</typeparam>
        /// <typeparam name="TEntrada">Tipo da entrada</typeparam>
        /// <param name="taskAssincrona">Task assincrona para efetuar chamada</param>
        /// <param name="entrada">Entrada de dados</param>
        /// <returns>Objeto de retorno</returns>
        protected static TRetorno ExecuteSync<TRetorno, TEntrada>(Func<TEntrada, Task<TRetorno>> taskAssincrona, TEntrada entrada)
        {
            try
            {
                TRetorno retorno = default;
                Task.Run<TRetorno>(async () => await taskAssincrona.Invoke(entrada).ConfigureAwait(false))
                    .ContinueWith<TRetorno>((retornoData) => retorno = retornoData.Result)
                    .Wait();

                return retorno;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                // Tratamento de erro generico de thread assíncrona, retorna o erro "real" para o sistema.
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TRetorno">Tipo do objeto de retorno</typeparam>
        /// <typeparam name="TEntrada">Tipo da entrada</typeparam>
        /// <typeparam name="TEntrada2">Tipo da segunda entrada</typeparam>
        /// <param name="taskAssincrona">Task assincrona para efetuar chamada</param>
        /// <param name="entrada">Entrada de dados</param>
        /// <param name="entrada2">Segunda entrada de dados</param>
        /// <returns>Objeto de retorno</returns>
        protected static TRetorno ExecuteSync<TRetorno, TEntrada1, TEntrada2>(Func<TEntrada1, TEntrada2, Task<TRetorno>> taskAssincrona, TEntrada1 entrada, TEntrada2 entrada2)
        {
            TRetorno retorno = default;
            Task.Run<TRetorno>(async () => await taskAssincrona.Invoke(entrada, entrada2))
                .ContinueWith<TRetorno>((retornoData) => retorno = retornoData.Result);

            return retorno;
        }

        /// <summary>
        /// Efetua chamada verbo GET de forma sincrona e retorna o tipo informado
        /// </summary>
        /// <typeparam name="TRetorno">Tipo do retorno desejado</typeparam>
        /// <param name="urlRequisicao">Url da requisição</param>
        /// <returns>Retorna o tipo esperado</returns>
        protected TRetorno Get<TRetorno>(string urlRequisicao)
        {
            var resultado = ExecuteSync(async () =>
            {
                var url = this.FormatUrl(urlRequisicao);
                var respostaHttp = await this.SendRequest(HttpMethod.Get, url);
                var objetoRetorno = await this.ProcessHttpResponse<TRetorno>(respostaHttp);

                return objetoRetorno;
            });

            return resultado;
        }

        /// <summary>
        /// Efetua chamada verbo GET de forma assincrona e retorna o tipo informado
        /// </summary>
        /// <param name="urlRequisicao">Url da requisição</param>
        /// <returns>Retorna o tipo esperado</returns>
        protected async Task<HttpResponseMessage> Get(string urlRequisicao)
        {
            var url = this.FormatUrl(urlRequisicao);
            var respostaHttp = await this.SendRequest(HttpMethod.Get, url);

            return respostaHttp;
        }

        /// <summary>
        /// Efetua post de forma sincrona e retorna o tipo informado
        /// </summary>
        /// <typeparam name="TRetorno">Tipo do retorno desejado</typeparam>
        /// <param name="urlRequisicao">Url da requisição</param>
        /// <param name="dados">Dados que serão enviados no corpo da requisição</param>
        /// <returns>Retorna o tipo esperado</returns>
        protected TRetorno Post<TRetorno>(string urlRequisicao, object dados = null)
        {
            var resultado = ExecuteSync(async () =>
            {
                var url = this.FormatUrl(urlRequisicao);
                var respostaHttp = await this.SendRequest(HttpMethod.Post, url, dados);
                var objetoRetorno = await this.ProcessHttpResponse<TRetorno>(respostaHttp);

                return objetoRetorno;
            });

            return resultado;
        }

        /// <summary>
        /// Efetua post de forma assincrona e retorna o tipo informado
        /// </summary>
        /// <param name="urlRequisicao">Url da requisição</param>
        /// <param name="dados">Dados que serão enviados no corpo da requisição</param>
        /// <returns>Retorna o tipo esperado async</returns>
        protected async Task<HttpResponseMessage> Post(string urlRequisicao, object dados = null)
        {
            var url = this.FormatUrl(urlRequisicao);
            var respostaHttp = await this.SendRequest(HttpMethod.Post, url, dados);

            return respostaHttp;
        }

        /// <summary>
        /// Efetua put de forma sincrona
        /// </summary>
        /// <typeparam name="TRetorno">Tipo do retorno desejado</typeparam>
        /// <param name="urlRequisicao">Url da requisição</param>
        /// <param name="dados">Dados que serão enviados no corpo da requisição</param>
        /// <returns>Retorna o tipo esperado</returns>
        protected TRetorno Put<TRetorno>(string urlRequisicao, object dados = null)
        {
            var resultado = ExecuteSync(async () =>
            {
                var url = this.FormatUrl(urlRequisicao);
                var respostaHttp = await this.SendRequest(HttpMethod.Put, url, dados);
                var objetoRetorno = await this.ProcessHttpResponse<TRetorno>(respostaHttp);

                return objetoRetorno;
            });

            return resultado;
        }

        /// <summary>
        /// Efetua put de forma assincrona
        /// </summary>
        /// <param name="urlRequisicao">Url da requisição</param>
        /// <param name="dados">Dados que serão enviados no corpo da requisição</param>
        /// <returns>Retorna o tipo esperado</returns>
        protected async Task<HttpResponseMessage> Put(string urlRequisicao, object dados = null)
        {
            var url = this.FormatUrl(urlRequisicao);
            var respostaHttp = await this.SendRequest(HttpMethod.Put, url, dados);

            return respostaHttp;
        }

        /// <summary>
        /// Preenche o cabeçalho de autenticação para serviço externo, caso tenha
        /// </summary>
        /// <param name="requisicao">Objeto de requisi~çao</param>
        private void SetAuthHeader(HttpRequestMessage requisicao)
        {
            requisicao.Headers.Authorization = this.Authentication;
        }

        /// <summary>
        /// Método que envia requisição previamente formatada
        /// </summary>
        /// <param name="metodo">verbo http</param>
        /// <param name="url">url da requisição</param>
        /// <param name="data">objeto que devera ser enviado no corpo da requisicao</param>
        /// <returns>Task assincrona contendo o resultado externo</returns>
        private async Task<HttpResponseMessage> SendRequest(HttpMethod metodo, string url, object data = null)
        {
            using var requisicao = new HttpRequestMessage(metodo, url);
            this.SetAuthHeader(requisicao);

            await FillRequest(data, requisicao);

            var response = await this._httpClient.SendAsync(requisicao).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Efetua tratamento da url para chamada limpa
        /// </summary>
        /// <param name="urlRequisicao">Url de requisição</param>
        /// <returns>Url formatada</returns>
        private string FormatUrl(string urlRequisicao)
        {
            var urlFormatada = string.Join("/", new string[] { this.ApiAddress, urlRequisicao });
            var url = new Uri(urlFormatada);

            return url.AbsoluteUri;
        }

        /// <summary>
        /// Efetua preenchimento do conteudo no corpo da requisicao
        /// </summary>
        /// <param name="dados">Dados a serem enviados</param>
        /// <param name="requisicao">Objeto de requisição</param>
        /// <returns></returns>
        private static async Task FillRequest(object dados, HttpRequestMessage requisicao)
        {
            if (dados != null)
            {
                var content = await Task.FromResult(JsonConvert.SerializeObject(dados)).ConfigureAwait(false);

                requisicao.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }
        }

        /// <summary>
        /// Processa a resposta do serviço externo convertendo para o tipo desejado ou tratando a exceção
        /// </summary>
        /// <typeparam name="T">Tipo que se deseja converter</typeparam>
        /// <param name="respostaHttp">Resposta http da api externa</param>
        /// <returns>Tipo convertido de forma assincrona</returns>
        private async Task<T> ProcessHttpResponse<T>(HttpResponseMessage respostaHttp)
        {
            var data = await respostaHttp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (respostaHttp.IsSuccessStatusCode)
                return await Task.FromResult(JsonConvert.DeserializeObject<T>(data)).ConfigureAwait(false);

            var ex = await this.ThrowException(respostaHttp);
            throw ex;
        }

        /// <summary>
        /// Trata a exceção caso o serviço externo tenha retornado uma resposta diferente de 200, faz uma conversão para o tipo de exceção listado na arquitetura de acordo com a resposta do servidor
        /// </summary>
        /// <param name="respostaHttp">Resposta http do serviço externo</param>
        /// <returns>Exceção de forma assincrona</returns>
        private async Task<Exception> ThrowException(HttpResponseMessage respostaHttp)
        {
            var data = await respostaHttp.Content.ReadAsStringAsync().ConfigureAwait(false);
            var statusCode = respostaHttp.StatusCode;

            throw statusCode switch
            {
                HttpStatusCode.Conflict => new ConfirmationException(data),
                HttpStatusCode.MethodNotAllowed => new DeniedAcessException(),
                HttpStatusCode.PreconditionFailed => new ValidationException(this.Name, data),
                _ => new Exception(data),
            };
        }
    }
}