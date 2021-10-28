using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Module.Dto.Config;
using Module.IoC.Interface;
using Module.IoC.Register;
using System;
using System.Linq;

namespace UnitTest.Base
{
    /// <summary>
    /// Classe base para elaboração de testes unitarios utilizando automock
    /// </summary>
    public class BaseTest : ITestRegister
    {
        public const string TEXT_LENGTH_100 = "Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio";
        public const string TEXT_LENGTH_255 = "Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio urna, tempus molestie, porttitor ut, iaculis quis, sem. Phasellus rhoncus. Aenean id metus id velit ullamcorper pulvinar. Vestibulum fermentum tortor id m";
        public const string TEXT_LENGTH_50 = "Nam quis nulla. Integer malesuada. In in enim a ar";
        public const string TEXT_LENGTH_500 = "Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio urna, tempus molestie, porttitor ut, iaculis quis, sem. Phasellus rhoncus. Aenean id metus id velit ullamcorper pulvinar. Vestibulum fermentum tortor id mi. Pellentesque ipsum. Nulla non arcu lacinia neque faucibus fringilla. Nulla non lectus sed nisl molestie malesuada. Proin in tellus sit amet nibh dignissim sagittis. Vivamus luctus egestas leo. Maecenas sollicitudin. Nullam rhoncus aliquam met";
        public const string TEXT_LENGTH_501 = "Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio urna, tempus molestie, porttitor ut, iaculis quis, sem. Phasellus rhoncus. Aenean id metus id velit ullamcorper pulvinar. Vestibulum fermentum tortor id mi. Pellentesque ipsum. Nulla non arcu lacinia neque faucibus fringilla. Nulla non lectus sed nisl molestie malesuada. Proin in tellus sit amet nibh dignissim sagittis. Vivamus luctus egestas leo. Maecenas sollicitudin. Nullam rhoncus aliquam mety";

        /// <summary>
        /// Container autofac
        /// </summary>
        protected IContainer Container;

        /// <summary>
        /// Container automock
        /// </summary>
        protected AutoMock TestMock;

        private bool disposedValue;

        public BaseTest()
        {
            var builder = new ContainerBuilder();

            Register.RegisterDependencyInjection(builder, this);
        }

        /// <summary>
        /// Serviço auxiliar para virtualização de objetos utilizados em chamadas e conversão de tipo utilizando automapper
        /// </summary>
        public IObjectMoq ObjectMoqComparer { get; set; }

        /// <summary>
        /// Objeto de configurações (registrado no início da aplicação)
        /// </summary>
        public SettingsDto Settings => new()
        {
            DbConnection = new DbSettingsDto()
            {
            },
            ApiServicesUrl = new ExternalApiSettingsDto()
            {
            }
        };

        /// <summary>
        /// Objeto de configurações
        /// </summary>
        public HttpContextAccessor HttpContextAccessor => new();

        /// <summary>
        /// Inicia o ciclo de vida autofac para a camada de teste e inicializa o automoq
        /// </summary>
        /// <param name="container">Autofac container</param>
        public void AtribuirCicloVida(IContainer container)
        {
            this.Container = container;
            this.Container.BeginLifetimeScope();
            this.TestMock = AutoMock.GetLoose();

            this.ObjectMoqComparer = container.Resolve<IObjectMoq>();
        }

        /// <summary>
        /// Implementação container builder para configuração do ambiente de teste
        /// </summary>
        /// <param name="builder">Container builder autofac</param>
        public void ConfigurarTeste(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>()
                   .AsSelf()
                   .As<IHttpContextAccessor>();

            builder.Register<TesteEnvironment>((context) => new TesteEnvironment()
            {
                ApplicationName = "TesteApp",
                EnvironmentName = "Teste",
                ContentRootPath = $"{Environment.CurrentDirectory}\\Root",
                WebRootPath = $"{Environment.CurrentDirectory}"
            })
            .AsSelf()
            .As<IHostingEnvironment>();
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Método para ações customizadas ao ativar um tipo já cadastrado na injeção de dependencia (caso necessário)
        /// </summary>
        /// <typeparam name="TypeOf">Tipo</typeparam>
        /// <param name="e">Tipo que está sendo instanciado</param>
        public void OnActivatingInstance<TypeOf>(IActivatingEventArgs<TypeOf> e)
        {
            if (this.TestMock == null)
                return;

            foreach (TypedService service in e.Component.Services)
            {
                var isRegistered = this.TestMock.Container
                                                .ComponentRegistry
                                                .Registrations
                                                .Where(t => t.Services.Cast<TypedService>().Any(z => z.ServiceType == service.ServiceType))
                                                .Any();

                if (isRegistered)
                {
                    var newInstance = this.TestMock.Container.Resolve(service.ServiceType);
                    e.ReplaceInstance(newInstance);
                }
            }
        }

        /// <summary>
        /// Registro de classes de teste caso necessario
        /// </summary>
        /// <param name="builder"></param>
        public void RegisterInternalComponents(ContainerBuilder builder)
        {
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
                    this.TestMock.Dispose();
                    this.Container.Dispose();
                    this.Container = null;
                    this.TestMock = null;
                }

                disposedValue = true;
            }
        }
    }

    /// <summary>
    /// Objeto de ambiente herdado para teste
    /// </summary>
    public class TesteEnvironment : IHostingEnvironment
    {
        public string ApplicationName { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
        public string ContentRootPath { get; set; }
        public string EnvironmentName { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string WebRootPath { get; set; }
    }
}