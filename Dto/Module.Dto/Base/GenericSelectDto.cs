using System;
using System.ComponentModel;

namespace Module.Dto.Base
{
    /// <summary>
    /// Classe abstrata para objetos de seleção combo, listview etc.
    /// </summary>
    /// <typeparam name="T">Tipo genérico do valor</typeparam>
    public abstract class BaseGenericSelectDto<T>
    {
        /// <summary>
        /// Texto para ser exibido na caixa de seleção
        /// </summary>
        [Description("Texto para ser exibido na caixa de seleção")]
        public virtual string Text { get; set; }

        /// <summary>
        /// Valor da entidade na seleção
        /// </summary>
        [Description("Valor da entidade na seleção")]
        public virtual T Value { get; set; }
    }

    /// <summary>
    /// Classe para objetos de seleção combo com identificador do tipo inteiro
    /// </summary>
    public class GenericIntSelectDto : BaseGenericSelectDto<int>
    {
    }

    /// <summary>
    /// Classe abstrata para de seleção combo com identificador do tipo guid
    /// </summary>
    public class GenericGuidSelectDto : BaseGenericSelectDto<Guid>
    {
    }
}