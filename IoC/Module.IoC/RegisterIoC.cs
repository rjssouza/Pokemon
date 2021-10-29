using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Module.Dto.Config;
using Module.Factory.Base;
using Module.Factory.Conexao;
using Module.Factory.Interface;
using Module.Factory.Interface.Conexao;
using Module.Integration.Base;
using Module.IoC.Interface;
using Module.IoC.Interface.Base;
using Module.IoC.Mapper;
using Module.IoC.Middleware;
using Module.Repository.Base;
using Module.Service.Base;
using Module.Service.Validation.Base;
using Module.Util.Log;
using System;
using System.Net.Http.Headers;
using System.Reflection;

namespace Module.IoC
{
    /// <summary>
    /// Classe para registro das injeções de dependencia
    /// </summary>
    public class RegisterIoC
    {
        private static IContainer _container;
        private static IContainerRegister _containerRegister;

        /// <summary>
        /// Chamada estatica para registrar containers
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="registro">Interface de registro herdada por pontos de entrada das aplicações</param>
        public static void RegisterDependencyInjection(ContainerBuilder builder, IContainerRegister registro)
        {
            _containerRegister = registro;
            RegisterModules(builder, registro);

            if (registro is ITestRegister testeRegisrto)
                OpenTestLiveCycle(builder, testeRegisrto);
            else if (registro is ILiveCycleRegister cicloVidaRegistro)
                OpenLiveCycle(builder, cicloVidaRegistro);
        }

        /// <summary>
        /// Efetua chamada no registro para abrir ciclo de vida do container apos build (nao necessario para api)
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="registro">Interface de registro herdada por pontos de entrada das aplicações</param>
        private static void OpenLiveCycle(ContainerBuilder builder, ILiveCycleRegister registro)
        {
            _container = builder.Build();

            registro.AtribuirCicloVida(_container);
        }

        /// <summary>
        /// Abre o ciclo de vida para classe de teste e configura parametros para teste
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="registro">Interface de registro herdada por pontos de entrada das aplicações</param>
        private static void OpenTestLiveCycle(ContainerBuilder builder, ITestRegister registro)
        {
            registro.ConfigurarTeste(builder);
            _container = builder.Build();

            registro.AtribuirCicloVida(_container);
        }

        /// <summary>
        /// Configuração dos serviços de conversão e mock de objetos utilizando automapper
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="ehAmbienteTeste">Indica se é um ambiente de teste para considerar mock de objetos</param>
        private static void ConfigureAutoMapper(ContainerBuilder builder, bool ehAmbienteTeste = false)
        {
            builder.RegisterType<ObjectMoq>()
                .AsSelf()
                .As<IObjectMoq>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ObjectConverter>()
                   .As<IObjectConverterFactory>()
                   .SingleInstance()
                   .UsingConstructor(typeof(bool))
                   .WithParameters(new[] { new NamedParameter("ehAmbienteTeste", ehAmbienteTeste) })
                   .PropertiesAutowired();
        }

        /// <summary>
        /// Configura interceptadores de chamada para efetuar ações de tratamento como retorno automatico da transação
        /// </summary>
        /// <param name="builder">Container builder</param>
        private static void ConfigureHandler(ContainerBuilder builder)
        {
            // Registrar interceptador caso de erro, o rollback ou commit é automático
            builder.Register(c => new TransactionInterceptor(c.Resolve<IDbTransactionFactory>()))
                   .AsSelf()
                   .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
        }

        /// <summary>
        /// Registra classes uteis na injeção de dependencia como o Logger
        /// </summary>
        /// <param name="builder">Container builder</param>
        private static void RegisterUtils(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(Logger))
                .AsSelf()
                .SingleInstance();
        }

        /// <summary>
        /// Registra o metodo de chamada quando uma instancia for ativada
        /// </summary>
        /// <typeparam name="TypeOf">Tipo da instancia</typeparam>
        /// <param name="e">Tipo da instancia</param>
        private static void OnActivatingInstanceForTesting<TypeOf>(IActivatingEventArgs<TypeOf> e)
        {
            if (_containerRegister is ITestRegister testeRegistro)
                testeRegistro.OnActivatingInstance<TypeOf>(e);
        }

        /// <summary>
        /// Efetua o registro dos modulos nas camadas do sistema
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="registro">Interface de registro herdada por pontos de entrada das aplicações</param>
        private static void RegisterModules(ContainerBuilder builder, IContainerRegister registro)
        {
            var conexaoDto = registro.Settings.DbConnection;

            builder.Register<SettingsDto>((s) => registro.Settings)
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.Load(typeof(BaseFactory).Assembly.GetName()))
                .Where(t => t.Name.EndsWith("Factory"))
                .OnActivating(OnActivatingInstanceForTesting)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<DbConnectionFactory>()
                .As<IDbTransactionFactory>()
                .As<IDbConnectionFactory>()
                .UsingConstructor(typeof(string), typeof(string))
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .WithParameters(new[] {
                    new NamedParameter("sqliteDirectory", conexaoDto.Default),
                    new NamedParameter("rootPath", registro.Settings.WebRootPath)
                });

            builder.RegisterAssemblyTypes(Assembly.Load(typeof(BaseRepository).Assembly.GetName()))
                .Where(t => t.Name.EndsWith("Repository"))
                .OnActivating(OnActivatingInstanceForTesting)
                .AsImplementedInterfaces()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(TransactionInterceptor))
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterAssemblyTypes(Assembly.Load(typeof(BaseService).Assembly.GetName()))
                .Where(t => t.Name.EndsWith("Service"))
                .OnActivating(OnActivatingInstanceForTesting)
                .AsImplementedInterfaces()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(TransactionInterceptor))
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterAssemblyTypes(Assembly.Load(typeof(BaseIntegration).Assembly.GetName()))
                .Where(t => t.Name.EndsWith("Integration"))
                .OnActivating(OnActivatingInstanceForTesting)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterAssemblyTypes(Assembly.Load(typeof(BaseValidation).Assembly.GetName()))
                .Where(t => t.Name.EndsWith("Validation"))
                .AsImplementedInterfaces()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(TransactionInterceptor))
                .InstancePerDependency()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            RegisterUtils(builder);
            ConfigureAutoMapper(builder, registro is ITestRegister);
            ConfigureHandler(builder);

            registro.RegisterInternalComponents(builder);
        }
    }
}