using Autofac;
using Autofac.Core;
using Microsoft.AspNetCore.Http;
using Module.Dto.Config;

namespace Module.IoC.Interface.Base
{
    public interface IContainerRegister
    {
        SettingsDto Settings { get; }
        HttpContextAccessor HttpContextAccessor { get; }

        void OnActivatingInstance<TypeOf>(IActivatingEventArgs<TypeOf> e);

        void RegisterInternalComponents(ContainerBuilder builder);
    }
}