using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Module.Dto.Validation.Api;
using Module.Util.Log;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Filter
{
    /// <summary>
    /// Filtro de exceção
    /// </summary>
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly Logger _customLogger;

        /// <summary>
        /// Construtor de exceção utilizando logger registrado no modulo ioc
        /// </summary>
        /// <param name="customLogger">Logger</param>
        public ExceptionFilter(Logger customLogger)
        {
            this._customLogger = customLogger;
        }

        /// <summary>
        /// Obtem código http da exceção de acordo com o tipo retornado da exceção
        /// </summary>
        /// <param name="excecao">Exceção</param>
        /// <returns>Http Status Code</returns>
        private static HttpStatusCode ObterCodigoHttp(Exception excecao)
        {
            var codigoErro = HttpStatusCode.InternalServerError;

            if (excecao is ApiException excecao1)
            {
                codigoErro = excecao1.StatusCode;
            }

            return codigoErro;
        }

        /// <summary>
        /// Obtém mensagem da exeção da mensagem de forma legível na resposta http
        /// </summary>
        /// <param name="excecao">Eceção retornada</param>
        /// <returns>Texto da mensagem</returns>
        private static string ObterMensagem(Exception excecao)
        {
            var mensagem = excecao.Message;
            while (excecao.InnerException != null)
            {
                excecao = excecao.InnerException;
                mensagem += Environment.NewLine + excecao.Message;
            }

            if (excecao is ValidationException exception)
            {
                mensagem = exception.Validation.ErrorMessage;
            }

            return mensagem;
        }

        /// <summary>
        /// Método disparado quando a api estoura uma exceção
        /// </summary>
        /// <param name="context">Contexto da exceção</param>
        /// <returns>Resposta http</returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var excecao = context.Exception;
            var excecaoMensagem = ObterMensagem(excecao);
            var codigoErro = ObterCodigoHttp(excecao);

            var error = new
            {
                StatusCode = codigoErro,
                Message = excecaoMensagem,
                excecao.StackTrace
            };
            context.HttpContext.Response.StatusCode = codigoErro.GetHashCode();
            context.Result = new JsonResult(error);
            this.WriteLog(excecao);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Método para escrever log utilizando o logger customizado
        /// </summary>
        /// <param name="ex">Exceção</param>
        private void WriteLog(Exception ex)
        {
            this._customLogger.Write(ex);

            NotifyDev();
        }

        /// <summary>
        /// Efetua notificação do time de desenvolvimento
        /// </summary>
        private static void NotifyDev()
        {
            // TODO implementar metodo para envio de erro ao desenvolvedor
        }
    }
}