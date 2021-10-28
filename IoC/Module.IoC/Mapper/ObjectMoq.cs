using Module.IoC.Interface;
using System;
using System.Collections.Generic;

namespace Module.IoC.Mapper
{
    /// <summary>
    /// Classe que implementa o serviço auxiliar para virtualização de objetos utilizados em chamadas e conversão de tipo utilizando automapper
    /// </summary>
    /// </summary>
    public class ObjectMoq : IDisposable, IObjectMoq
    {
        private readonly Dictionary<Type, KeyValuePair<object, Func<object, bool>>> _mockedPredicates;
        private bool disposedValue;

        public ObjectMoq()
        {
            this._mockedPredicates = new Dictionary<Type, KeyValuePair<object, Func<object, bool>>>();
        }

        /// <summary>
        /// Adiciona um objeto para ser virtualizado assim a proxima chamada para a sua conversão o mesmo será substituido pelo objeto ja informado
        /// </summary>
        /// <param name="src">Objeto a ser informado</param>
        /// <param name="validacao">Metodo para validar se o objeto deve ser ou não substituido pelo que foi informado</param>
        public void AdicionarObjetoMoq(object src, Func<object, bool> validacao)
        {
            var type = src.GetType();
            var keyValuePair = new KeyValuePair<object, Func<object, bool>>(src, validacao);

            this._mockedPredicates.Add(type, keyValuePair);
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
        /// Tenta obter objeto de predicado de acordo com o que foi registrado para teste
        /// </summary>
        /// <typeparam name="TResult">Tipo a ser retornado</typeparam>
        /// <param name="result">Resultado esperado</param>
        public void TentarObterMoq<TResult>(ref TResult result)
        {
            if (result == null)
                return;

            var type = result.GetType();
            if (!_mockedPredicates.ContainsKey(type))
                return;

            var predicateDictionary = _mockedPredicates[type];
            var predicateFunc = predicateDictionary.Value;
            var predicateObject = predicateDictionary.Key;

            if (predicateFunc.Invoke(result))
                result = (TResult)predicateObject;
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
                    this.Clear();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Limpar objeto de predicados do mocker
        /// </summary>
        private void Clear()
        {
            _mockedPredicates.Clear();
        }
    }
}