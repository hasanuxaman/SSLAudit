using Shampan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Core.Interfaces.Repository
{
    public interface IBaseRepository<TModel> where TModel: class, new()
    {
        string GetSettingsValue(string[] conditionalFields, string[] conditionalValue);

        List<TModel> GetAll(string[] conditionalFields , string[] conditionalValue, PeramModel vm = null);

        List<TModel> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

        int GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

        int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue);

        TModel Insert(TModel model);
        TModel Update(TModel model);

        int Delete(string tableName,  string[] conditionalFields, string[] conditionalValue);

        int Archive(string tableName,  string[] conditionalFields, string[] conditionalValue);

        bool CheckExists(string tableName, string[] conditionalFields,string[] conditionalValue);
        string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue);

        bool CheckPostStatus(string tableName, string[] conditionalFields, string[] conditionalValue);
        bool CheckPushStatus(string tableName, string[] conditionalFields, string[] conditionalValue);
        string GenerateCode(string CodeGroup, string CodeName, int branchId = 1);

        int DetailsDelete(string tableName, string[] conditionalFields, string[] conditionalValue);
    }
}
