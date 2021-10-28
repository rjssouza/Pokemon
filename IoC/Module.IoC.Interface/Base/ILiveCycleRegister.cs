using Autofac;
using System;

namespace Module.IoC.Interface.Base
{
    public interface ILiveCycleRegister : IContainerRegister, IDisposable
    {
        void AtribuirCicloVida(IContainer container);
    }
}