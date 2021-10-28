using Module.Factory.Interface.Base;

namespace Module.Factory.Interface
{
    /// <summary>
    /// Factory para conversão de objetos
    /// </summary>
    public interface IObjectConverterFactory : IBaseFactory
    {
        /// <summary>
        /// Efetua a conversão de um objeto utilizando automapper
        /// </summary>
        /// <typeparam name="T">Tipo esperado do retorno</typeparam>
        /// <param name="src">Objeto a ser convertido</param>
        /// <returns>Objeto convertido</returns>
        T ConvertTo<T>(object src);
    }
}