using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Repository.Audit
{
    public interface IAuditIssueUserRepository : IBaseRepository<AuditIssueUser>
    {
        List<AuditIssueUser> GetAuditIssueUsersById(int id);

        AuditReportUsers AuditReportUsersInsert(AuditReportUsers model);
        AuditReportUsers AuditReportUsersUpdate(AuditReportUsers model);
        public List<AuditReportUsers> AuditReportUsersGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

        List<AuditReportUsers> AuditReportUsersGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    }
}
