using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Module.Repository.Model.Base
{
    /// <summary>
    /// Classe base para modelos de entidade com identificador padrão
    /// </summary>
    public abstract class BaseIdentityModel<IdType> : BaseModel
    {
        /// <summary>
        /// Identificador da tabela
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [Column("id")]
        [ExplicitKey]
        public IdType Id { get; set; }
    }
}