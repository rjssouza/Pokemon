using System;

namespace Module.IoC.Interface
{
    public interface IObjectMoq
    {
        /// <summary>
        /// Adiciona um objeto para ser virtualizado assim a proxima chamada para a sua conversão o mesmo será substituido pelo objeto ja informado
        /// </summary>
        /// <param name="src">Objeto a ser informado</param>
        /// <param name="validacao">Metodo para validar se o objeto deve ser ou não substituido pelo que foi informado</param>
        void AdicionarObjetoMoq(object src, Func<object, bool> validacao);
    }
}