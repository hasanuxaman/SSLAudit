using Shampan.Models;

namespace Shampan.Core.Interfaces.Services;

public interface IBaseService<TModel> where TModel : class, new()
{
    ResultModel<List<TModel>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    ResultModel<List<TModel>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    ResultModel<TModel> Insert(TModel model);
    ResultModel<TModel> Update(TModel model);
    ResultModel<TModel> Delete(int id);
    //ResultModel<TModel> MultiplePost(TModel model);
    int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

}