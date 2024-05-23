using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.Settings
{
    public interface ISettingsRepository : IBaseRepository<SettingsModel>
    {

        DbUpdateModel NewTableAdd(string TableName, string createQuery);
        DbUpdateModel DBTableFieldAdd(string TableName, string FieldName, string DataType, bool NullType);
        DbUpdateModel DBTableFieldAlter(string TableName, string FieldName, string DataType);
        DbUpdateModel DBTableFieldRemove(string TableName, string FieldName);

    }
}
