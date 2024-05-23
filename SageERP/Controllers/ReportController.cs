using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.AuditFeedbackService;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services;
using Shampan.Services.Audit;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers.Audit;
using StackExchange.Exceptional;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SSLAudit.Controllers
{

    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IAuditMasterService _auditMasterService;
        private readonly IConfiguration _configuration;


        public ReportController(ApplicationDbContext applicationDb, IAuditMasterService auditMasterService, IConfiguration configuration)
        {
            _auditMasterService = auditMasterService;
            _configuration = configuration;


        }

        public IActionResult AuditeeResponse(int id)
        {
            AuditMaster master = new AuditMaster();

            try
            {
                string data = AuthDbName();
                PeramModel pm = new PeramModel();
                pm.AuthDb = data;

                AuditBranchFeedback branchFeedback = new AuditBranchFeedback();
                AuditBranchFeedback branchFeedbackDepartment = new AuditBranchFeedback();
                AuditBranchFeedback auditResponseFollowUp = new AuditBranchFeedback();
                AuditFeedback auditFeedback = new AuditFeedback();
                AuditReportHeading auditReportHeading = new AuditReportHeading();

                ResultModel<List<AuditReportHeading?>> auditReportResult =_auditMasterService.GetReportHeadingData(new[] { "AuditId" }, new[] { id.ToString() });
                AuditReportHeading reportheading = auditReportResult.Data.FirstOrDefault();

                #region First and Second Heading And Annexery

                if (auditReportResult.Data.Count() != 0)
                {
                    if (reportheading.AuditReportDetails != null)
                    {
                        master.AuditReportDetails = Base64Decode(reportheading.AuditReportDetails);
                    }
                    if (reportheading.AuditSecondReportDetails != null)
                    {
                        master.AuditSecondReportDetails = Base64Decode(reportheading.AuditSecondReportDetails);
                    }
                    if (reportheading.AuditAnnexureDetails != null)
                    {
                        master.AuditAnnexureDetails = Base64Decode(reportheading.AuditAnnexureDetails);
                    }
                    if (reportheading.AuditSecondReportDetails == null)
                    {
                        master.AuditSecondReportDetails = "";
                    }
                    if (reportheading.AuditReportDetails == null)
                    {
                        master.AuditReportDetails = "";
                    }
                    if (reportheading.AuditAnnexureDetails == null)
                    {
                        master.AuditAnnexureDetails = "";
                    }
                }
                else
                {
                    master.AuditReportDetails = "";
                    master.AuditSecondReportDetails = "";
                    master.AuditAnnexureDetails = "";
                }

                #endregion


                #region Getting Audit Issues And Team Name

                ResultModel<List<AuditIssue?>> result =_auditMasterService.GetReportData(new[] { "ad.Id" }, new[] { id.ToString() });

                var auditValue = result.Data.FirstOrDefault();
                string userName = User.Identity.Name;
                if (auditValue != null)
                {
                    ResultModel<List<AuditUser>> UserData = _auditMasterService.GetAuditUserTeamId(auditValue.TeamId.ToString(), pm);

                }

                #endregion


                #region DepartmentResponseAllowPart FromBranchUsers

                //ForBranchFeedback or DepartmentalResponse Allow Part -->FromBranchUsers

                string Name = User.Identity.Name;
                PeramModel pvm = new PeramModel();
                pvm.AuditId = id;
                pvm.UserName = Name;

                if (result.Data != null)
                {
                    if (result.Data.Count() != 0)
                    {
                        foreach (var issue in result.Data)
                        {

                            ResultModel<List<AuditBranchFeedback?>> resultABF =
                                  _auditMasterService.GetReportBranchFeedbackData(new[] { "ABF.AuditIssueId" }, new[] { issue.Id.ToString() }, pvm);

                            //branchFeedback = resultABF.Data.FirstOrDefault();
                            //if (branchFeedback != null)
                            //{
                            //    issue.AuditBranchFeedback = branchFeedback;
                            //}
                            foreach (var auditBranch in resultABF.Data)
                            {
                                issue.AuditBranchFeedbackList.Add(auditBranch);
                            }


                        }

                    }
                }
                if (result.Data != null)
                {
                    if (result.Data.Count() != 0)
                    {
                        int i = 0;
                        foreach (var issue in result.Data)
                        {

                            //if (issue.AuditBranchFeedback == null)
                            //{
                            //    issue.AuditBranchFeedback = new AuditBranchFeedback();
                            //    issue.AuditBranchFeedback.ImplementationDate = "";
                            //    issue.AuditBranchFeedback.IssueDetails = "";
                            //    i++;
                            //}
                            //else
                            //{


                            if (issue.AuditBranchFeedbackList.Count() != 0)
                            {
                                foreach (var auditBranch in issue.AuditBranchFeedbackList)
                                {
                                    if (auditBranch.IssueDetails == null)
                                    {
                                        auditBranch.IssueDetails = "";
                                    }
                                    else
                                    {
                                        auditBranch.IssueDetails = Base64Decode(auditBranch.IssueDetails);

                                    }

                                }
                            }


                            //if (issue.AuditBranchFeedback.IssueDetails != null)
                            //{
                            //    issue.AuditBranchFeedback.IssueDetails = Base64Decode(issue.AuditBranchFeedback.IssueDetails);

                            //}
                            //else
                            //{
                            //    issue.AuditBranchFeedback.IssueDetails = "";
                            //}
                            //i++;

                            //}


                        }
                    }
                }

                //EndForBranchFeedback or DepartmentalResponse Allow Part

                #endregion


                #region AuditResponseAllowPart FromTeamUsers

                //This is previous code . i have to change class name AuditFeedback to AuditBranchFeedback
                //ForTeamBranchFeedbackOrAuditResponseAllowPart -->FromTeamUsers

                if (result.Data != null)
                {
                    if (result.Data.Count() != 0)
                    {
                        foreach (var issue in result.Data)
                        {
                            //BranchFeedbackFromTeamUsers
                            ResultModel<List<AuditFeedback?>> resultAF =
                                  _auditMasterService.GetReportAuditFeedbackData(new[] { "ABF.AuditIssueId" }, new[] { issue.Id.ToString() }, pvm);
                            //_auditMasterService.GetReportAuditFeedbackData(new[] { "AF.AuditIssueId" }, new[] { issue.Id.ToString() },pvm);


                            //auditFeedback = resultAF.Data.FirstOrDefault();
                            //if (auditFeedback != null)
                            //{
                            //    issue.AuditFeedback = auditFeedback;
                            //}
                            foreach (var auditFB in resultAF.Data)
                            {
                                issue.AuditFeedbackList.Add(auditFB);
                            }



                        }

                    }
                }

                if (result.Data != null)
                {
                    if (result.Data.Count() != 0)
                    {
                        int i = 0;
                        foreach (var issue in result.Data)
                        {

                            if (issue.AuditFeedbackList.Count() != 0)
                            {
                                foreach (var auditFB in issue.AuditFeedbackList)
                                {
                                    if (auditFB.IssueDetails == null)
                                    {
                                        auditFB.IssueDetails = "";
                                    }
                                    else
                                    {
                                        auditFB.IssueDetails = Base64Decode(auditFB.IssueDetails);

                                    }

                                }
                            }




                            //if (issue.AuditFeedback == null)
                            //{
                            //    issue.AuditFeedback = new AuditFeedback();
                            //    issue.AuditFeedback.IssueDetails = "";
                            //    i++;
                            //}
                            //else
                            //{
                            //    if (issue.AuditFeedback.IssueDetails != null)
                            //    {
                            //        issue.AuditFeedback.IssueDetails = Base64Decode(issue.AuditFeedback.IssueDetails);

                            //    }
                            //    else
                            //    {
                            //        issue.AuditFeedback.IssueDetails = "";
                            //    }
                            //    i++;
                            //}

                        }
                    }
                }

                //End Of TeamBranchFeedback or Audit Reponse Allow Part

                #endregion


                #region DepartmentFollowUpPart FromBranchUsers

                //ForBranchFeedbackDepartmentFollowUp -->FromBranchUsers
                if (result.Data != null)
                {
                    if (result.Data.Count() != 0)
                    {
                        foreach (var issue in result.Data)
                        {

                            ResultModel<List<AuditBranchFeedback?>> DepartmentFollowUp =
                                  _auditMasterService.GetBranchFeedbackDeprtemnetFollowUpData(new[] { "ABF.AuditIssueId" }, new[] { issue.Id.ToString() }, pvm);

                            //branchFeedbackDepartment = DepartmentFollowUp.Data.FirstOrDefault();
                            //if (branchFeedbackDepartment != null)
                            //{
                            //    issue.AuditBranchFeedbackDepartmentFollowUp = branchFeedbackDepartment;
                            //}

                            foreach (var auditBranch in DepartmentFollowUp.Data)
                            {
                                issue.AuditBranchFeedbackDepartmentFollowUpList.Add(auditBranch);
                            }


                        }

                    }
                }
                if (result.Data != null)
                {
                    if (result.Data.Count() != 0)
                    {
                        int i = 0;
                        foreach (var issue in result.Data)
                        {

                            if (issue.AuditBranchFeedbackDepartmentFollowUpList.Count() != 0)
                            {
                                foreach (var auditBranch in issue.AuditBranchFeedbackDepartmentFollowUpList)
                                {
                                    if (auditBranch.IssueDetails == null)
                                    {
                                        auditBranch.IssueDetails = "";
                                    }
                                    else
                                    {
                                        auditBranch.IssueDetails = Base64Decode(auditBranch.IssueDetails);

                                    }

                                }
                            }


                            //if (issue.AuditBranchFeedbackDepartmentFollowUp == null)
                            //{
                            //    issue.AuditBranchFeedbackDepartmentFollowUp = new AuditBranchFeedback();
                            //    issue.AuditBranchFeedbackDepartmentFollowUp.IssueDetails = "";
                            //    i++;
                            //}
                            //else
                            //{
                            //    if (issue.AuditBranchFeedbackDepartmentFollowUp.IssueDetails != null)
                            //    {
                            //        issue.AuditBranchFeedbackDepartmentFollowUp.IssueDetails = Base64Decode(issue.AuditBranchFeedbackDepartmentFollowUp.IssueDetails);
                            //    }
                            //    else
                            //    {
                            //        issue.AuditBranchFeedbackDepartmentFollowUp.IssueDetails = "";
                            //    }
                            //    i++;
                            //}

                        }
                    }

                }
                //EndDepartmentFollowUp

                #endregion


                #region AuditResponseFollowUpPart FromTeamUsers

                //ForBranchFeedbackAuditResponseFollowUp
                if (result.Data != null)
                {
                    if (result.Data.Count() != 0)
                    {
                        foreach (var issue in result.Data)
                        {

                            ResultModel<List<AuditBranchFeedback?>> AuditResponseFollowUp =
                                  _auditMasterService.GetBranchFeedbackAuditResponseFollowUpData(new[] { "ABF.AuditIssueId" }, new[] { issue.Id.ToString() }, pvm);


                            foreach (var auditBranch in AuditResponseFollowUp.Data)
                            {
                                issue.AuditBranchFeedbackAuditResponserFollwoUpList.Add(auditBranch);
                            }

                            //auditResponseFollowUp = AuditResponseFollowUp.Data.FirstOrDefault();
                            //if (auditResponseFollowUp != null)
                            //{
                            //    issue.AuditBranchFeedbackAuditResponserFollwoUp = auditResponseFollowUp;
                            //}


                        }

                    }
                }
                if (result.Data != null)
                {
                    if (result.Data.Count() != 0)
                    {
                        int i = 0;
                        foreach (var issue in result.Data)
                        {


                            if (issue.AuditBranchFeedbackAuditResponserFollwoUpList.Count() != 0)
                            {
                                foreach (var auditBranch in issue.AuditBranchFeedbackAuditResponserFollwoUpList)
                                {
                                    if (auditBranch.IssueDetails == null)
                                    {
                                        auditBranch.IssueDetails = "";
                                    }
                                    else
                                    {
                                        auditBranch.IssueDetails = Base64Decode(auditBranch.IssueDetails);

                                    }

                                }
                            }



                            //if (issue.AuditBranchFeedbackAuditResponserFollwoUp == null)
                            //{
                            //    issue.AuditBranchFeedbackAuditResponserFollwoUp = new AuditBranchFeedback();
                            //    issue.AuditBranchFeedbackAuditResponserFollwoUp.IssueDetails = "";
                            //    i++;
                            //}
                            //else
                            //{
                            //    if (issue.AuditBranchFeedbackAuditResponserFollwoUp.IssueDetails != null)
                            //    {
                            //        issue.AuditBranchFeedbackAuditResponserFollwoUp.IssueDetails = Base64Decode(issue.AuditBranchFeedbackAuditResponserFollwoUp.IssueDetails);
                            //    }
                            //    else
                            //    {
                            //        issue.AuditBranchFeedbackAuditResponserFollwoUp.IssueDetails = "";
                            //    }
                            //    i++;
                            //}

                        }
                    }

                }
                //EndAuditResponseFollowUp

                #endregion


                //ForIssues
                if (result.Data != null)
                {
                    if (result.Data.FirstOrDefault() != null)
                    {
                        master.Name = result.Data.FirstOrDefault().Name;
                    }
                    else
                    {
                        master.Name = "";
                    }
                }

                master.auditIssue = result.Data;

                if (result.Data != null)
                {
                    if (result.Data.Count() != 0)
                    {
                        int i = 0;
                        foreach (var issue in result.Data)
                        {
                            if (issue.IssueDetails == null)
                            {
                                master.auditIssue[i].IssueDetails = "";
                                master.auditIssue[i].Risk = "";
                                i++;
                            }
                            else
                            {
                                master.auditIssue[i].IssueDetails = Base64Decode(issue.IssueDetails);
                                master.auditIssue[i].Risk = issue.Risk;
                                i++;
                            }

                        }
                    }
                }

                //EndOfIssues

                return View(master);


            }

            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return View(master);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            byte[] bytes = Convert.FromBase64String(base64EncodedData);

            string decodedIssueDetails = Encoding.UTF8.GetString(bytes);
            return decodedIssueDetails;
        }

        public static List<string> Base64DecodeAndSplit(string base64EncodedData, int chunkSize)
        {
            byte[] bytes = Convert.FromBase64String(base64EncodedData);
            List<string> dataChunks = new List<string>();

            for (int i = 0; i < bytes.Length; i += chunkSize)
            {
                int remainingLength = Math.Min(chunkSize, bytes.Length - i);
                byte[] chunk = new byte[remainingLength];
                Array.Copy(bytes, i, chunk, 0, remainingLength);
                string decodedChunk = Encoding.UTF8.GetString(chunk);
                dataChunks.Add(decodedChunk);
            }

            return dataChunks;
        }

        public string AuthDbName()
        {
            string connectionString = _configuration.GetConnectionString("AuthContext");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            string databaseName = builder.InitialCatalog;
            return databaseName;
        }

    }
}
