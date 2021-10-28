using Autofac;
using Module.IoC.Interface.Base;

namespace Module.IoC.Register
{
    public static class Register
    {
        /// <summary>
        /// Método estático para chamada de registro (efetuado em camada diferente para evitar que a camada de api ou aplicões enxerguem as camadas que não deveriam
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="containerRegister">Interface de registro herdada por pontos de entrada das aplicações</param>
        public static void RegisterDependencyInjection(ContainerBuilder builder, IContainerRegister containerRegister)
        {
            RegisterIoC.RegisterDependencyInjection(builder, containerRegister);
        }
    }
}