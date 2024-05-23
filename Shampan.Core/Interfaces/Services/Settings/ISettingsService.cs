using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Settings
{
    public interface ISettingsService : IBaseService<SettingsModel>
    {
        public ResultModel<DbUpdateModel> DbUpdate(DbUpdateModel model);

    }
}
