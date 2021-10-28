using AutoMapper;
using Module.Factory.Interface;

namespace Module.IoC.Mapper
{
    /// <summary>
    /// Classe que implementa o factory para conversão de objetos
    /// </summary>
    public partial class ObjectConverter : IObjectConverterFactory
    {
        private readonly bool _ehAmbienteTeste;
        private readonly IMapper _mapper;
        private bool disposedValue;

        public ObjectConverter(bool ehAmbienteTeste)
        {
            var mapperConfiguration = new MapperConfiguration(ConfigureMapper);

            this._mapper = mapperConfiguration.CreateMapper();
            this._ehAmbienteTeste = ehAmbienteTeste;
        }

        /// <summary>
        /// Serviço auxiliar para virtualização de objetos utilizados em chamadas e conversão de tipo utilizando automapper
        /// </summary>
        public ObjectMoq ComparadorMoq { get; set; }

        /// <summary>
        /// Efetua a conversão de um objeto utilizando automapper
        /// </summary>
        /// <typeparam name="T">Tipo esperado do retorno</typeparam>
        /// <param name="src">Objeto a ser convertido</param>
        /// <returns>Objeto convertido</returns>
        public TResult ConvertTo<TResult>(object src)
        {
            var resultado = this._mapper.Map<TResult>(src);
            if (_ehAmbienteTeste && this.ComparadorMoq != null)
                this.ComparadorMoq.TentarObterMoq<TResult>(ref resultado);

            return resultado;
        }

        /// <summary>
        /// Método chamada na inicialização para configurar as conversões
        /// </summary>
        /// <param name="mapperConfigExpression"></param>
        private void ConfigureMapper(IMapperConfigurationExpression mapperConfigExpression)
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
                disposedValue = true;
            }
        }

        ~ObjectConverter()
        {
            Dispose(disposing: false);
        }

        /// <summary>
        /// Dispose pattern
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}