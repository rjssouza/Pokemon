using Autofac;
using Module.IoC.Interface.Base;

namespace Module.IoC.Interface
{
    public interface ITestRegister : ILiveCycleRegister
    {
        void ConfigurarTeste(ContainerBuilder builder);
    }
}