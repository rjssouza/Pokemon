using Module.Util;
using System;

namespace Module.Dto.Base
{
    /// <summary>
    /// Classe base para data transfer object
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        public BaseDto()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Identificador da entidade
        /// </summary>
        public Guid Id { get; set; }
    }
}