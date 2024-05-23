using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Services.Audit
{
    public interface IAuditIssueUserService : IBaseService<AuditIssueUser>
    {

        List<AuditIssueUser> GetAuditIssueUsersById(int id);

        ResultModel<AuditReportUsers> AuditReportUsersInsert(AuditReportUsers model);
        ResultModel<AuditReportUsers> AuditReportUsersUpdate(AuditReportUsers model);
        ResultModel<List<AuditReportUsers>> AuditReportUsersGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<AuditReportUsers>> AuditReportUsersGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


        ResultModel<AuditReportUsers> ReportDelete(int id);


    }
}
