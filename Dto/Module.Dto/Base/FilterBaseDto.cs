using System;
using System.Text.Json.Serialization;

namespace Module.Dto.Base
{
    /// <summary>
    /// Classe base para filtros
    /// </summary>
    public abstract class FilterBaseDto 
    {
        /// <summary>
        /// Identificador da entidade
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Chave De texto para consulta geral, varia de acordo com a entidade em questão
        /// </summary>
        public virtual string Keyword { get; set; }

        /// <summary>
        /// Quantidade de itens por página
        /// </summary>
        public int? LimitMax { get; set; }

        /// <summary>
        /// Define se os limites devem ser utilizados ou não
        /// </summary>
        [JsonIgnore()]
        public bool LimitMustBeUsed
        {
            get
            {
                bool _limitMustBeUsed = this.LimitMax.HasValue && this.LimitStart.HasValue;

                return _limitMustBeUsed;
            }
        }

        /// <summary>
        /// Pagina inicial
        /// </summary>
        public int? LimitStart { get; set; }
    }
}