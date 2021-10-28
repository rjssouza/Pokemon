using System;

namespace Module.Dto.Base
{
    /// <summary>
    /// Classe base para data transfer object
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// Identificador da entidade
        /// </summary>
        public Guid Id { get; set; }
    }
}