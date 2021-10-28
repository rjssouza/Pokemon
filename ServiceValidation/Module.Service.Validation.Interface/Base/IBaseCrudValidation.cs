using Module.Repository.Model.Base;

namespace Module.Service.Validation.Interface.Base
{
    public interface IBaseCrudValidation<TModel> : IBaseValidation
        where TModel : BaseModel
    {
        void ValidateUpdate(TModel model);

        void ValidateDeletion(TModel model);

        void ValidateInsert(TModel model);
    }
}