using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services.Audit;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Models;
using StackExchange.Exceptional;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Shampan.Services;
using System.Data.SqlClient;

namespace SSLAudit.Controllers.Audit
{
    [ServiceFilter(typeof(UserMenuActionFilter))]

    public class AuditIssueController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAuditIssueService _auditIssueService;
        private readonly IAuditMasterService _auditMasterService;
        private readonly IAuditIssueAttachmentsService _auditIssueAttachmentsService;
        private readonly IAuditIssueUserService _auditIssueUserService;
        private readonly IConfiguration _configuration;


        public AuditIssueController(ApplicationDbContext applicationDb,
            IAuditIssueUserService auditIssueUserService,
            IAuditIssueService auditIssueService, 
            IAuditIssueAttachmentsService auditIssueAttachmentsService,
            IAuditMasterService auditMasterService,
             IConfiguration configuration

            )
        {
            _applicationDb = applicationDb;
            _auditIssueService = auditIssueService;
            _auditIssueAttachmentsService = auditIssueAttachmentsService;
            _auditMasterService = auditMasterService;
            _auditIssueUserService = auditIssueUserService;
            _configuration = configuration;


            AuthDbConfig.AuthDB = AuthDbName();
        }

        public IActionResult Index(int? id)
        {
            if (id is null || id == 0)
            {
                return RedirectToAction("Index", "Audit");
            }

            AuditIssue auditIssue = new AuditIssue()
            {
                AuditId = id.Value
            };

            var auditMaster = _auditMasterService.GetAll(new[] { "Id" }, new[] { id.Value.ToString() }).Data;

            if (auditMaster != null && auditMaster.Count > 0)
            {
                auditIssue.AuditMaster = auditMaster.FirstOrDefault();
            }

            return View(auditIssue);
        }



        public IActionResult Create(int? id)
        {

            if (id is null || id == 0)
            {
                return RedirectToAction("Index", "Audit");
            }

            AuditIssue auditIssue = new AuditIssue()
            {
                Operation = "add",
                AuditId = id.Value
            };
            return View(auditIssue);
        }



        [HttpPost]
        public ActionResult CreateEdit(AuditIssue master)
        {
            ResultModel<AuditIssue> result = new ResultModel<AuditIssue>();
            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

                if (master.Operation == "update")
                {

                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = "";

                    result = _auditIssueService.Update(master);

                    //if (result.Status == Status.Fail)
                    //{
                    //    throw result.Exception;
                    //}


                    return Ok(result);
                }
                else
                {

                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = "";
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = "";
                    result = _auditIssueService.Insert(master);

                    //if (result.Status == Status.Fail)
                    //{
                    //    throw result.Exception;
                    //}

                    //save link of audit address

                    //https://localhost:7031/Audit/Edit/28?edit=issue

                    //Uri returnurl = new Uri(Request.Host.ToString());
                    //string urlString = returnurl.ToString();
                    //string Url = "https://" + urlString + "/Audit/Edit/" + result.Data.Id + "?edit=issueApprove";


                    //MailSetting ms = new MailSetting();
                    //ms.ApprovedUrl = Url;
                    //ms.AuditIssueId = result.Data.Id;
                    //_auditMasterService.SaveUrl(ms);


                    //end




                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<IList<AuditIssue>> _index(int? id)
        {
            try
            {
               
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string issuename = Request.Form["issuename"].ToString();
                string issuepriority = Request.Form["issuepriority"].ToString();
                string dateofsubmission = Request.Form["dateofsubmission"].ToString();

                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();

                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();

                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                //index.OrderName = orderName;
                //index.orderDir = orderDir;
                index.OrderName = "Id";
                //index.orderDir = "desc";
                index.orderDir = "asc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);

                index.createdBy = userName;
                index.AuditId = id.Value;



                string[] conditionalFields = new[]
                {
                    "IssueName like",
                    "Enums.EnumValue like",
                    "DateOfSubmission like"
                };
                string?[] conditionalValue = new[] { issuename, issuepriority, dateofsubmission };


                ResultModel<List<AuditIssue>> indexData =
                    _auditIssueService.GetIndexData(index,
                        conditionalFields, conditionalValue);
                // rony ///
                //List<AuditIssue> auditIssues = new List<AuditIssue>();
                //foreach (AuditIssue issue in indexData.Data)
                //{
                //    if (issue.CreatedBy != userName)
                //    {
                //        if (issue.IsPost == "Y")
                //        {

                //            auditIssues.Add(issue);


                //        }

                //    }
                //    else
                //    {
                //        auditIssues.Add(issue);
                //    }

                //}

                ///rony ///

                ResultModel<int> indexDataCount =
                    _auditIssueService.GetIndexDataCount(index,
                        conditionalFields, conditionalValue);

                int result = _auditIssueService.GetCount(TableName.A_AuditIssues, "Id", new[]
                        {
                            "createdBy",
                        }, new[] { userName });

                // data, draw, recordsTotal = TotalReocrd, recordsFiltered = data.Count
                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
                //return Ok(new { data = auditIssues, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }




        public ActionResult<IList<AuditIssue>> Edit(int id)
        {
            try
            {
                ResultModel<List<AuditIssue?>> result =
                    _auditIssueService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                AuditIssue? auditMaster = result.Data.FirstOrDefault();

                auditMaster.AttachmentsList = _auditIssueAttachmentsService.GetAll(new[] { "AuditIssueId" }, new[] { id.ToString() }).Data;

                auditMaster.Operation = "update";

                return View("Create", auditMaster);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

        //IssueIndex 
        public async Task<IActionResult> AuditIssueIndex(string edit)
        {
            return View();
        }

        [HttpPost]
        public ActionResult<IList<AuditIssue>> _auditIssueIndex(string edit)
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();

                string auditCode = Request.Form["auditCode"].ToString();
                string auditName = Request.Form["auditName"].ToString();
                string auditStatus = Request.Form["auditStatus"].ToString();
                string issuename = Request.Form["issuename"].ToString();
                string issuepriority = Request.Form["issuepriority"].ToString();
                string dateofsubmission = Request.Form["dateofsubmission"].ToString();
                string operational = Request.Form["operational"].ToString();
                string financial = Request.Form["Financial"].ToString();
                string compliance = Request.Form["compliance"].ToString();


                if (operational == "")
                {
                    operational = "Select";
                }
                if (operational != "Select")
                {
                    operational = (operational == "True") ? "1" : "0";

                }

                if (financial == "")
                {
                    financial = "Select";
                }
                if (financial != "Select")
                {
                    financial = (financial == "True") ? "1" : "0";

                }

                if (compliance == "")
                {
                    compliance = "Select";
                }
                if (compliance != "Select")
                {
                    compliance = (compliance == "True") ? "1" : "0";

                }

                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = "asc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.Status = edit;
                index.UserName = userName;

                string[] conFields = new string[10];
                string[] conditionalFields = new[]
                {
                    "A.Code like",
                    "A.Name like",
                    "A.AuditStatus like",

                    "AI.IssueName like",
                    "Enums.EnumValue like",
                    "AI.DateOfSubmission like",
                    "AI.Operational like",
                    "AI.Financial like",
                    "AI.Compliance like"
                };
                string?[] conditionalValue = new[] { auditCode, auditName, auditStatus, issuename, issuepriority, dateofsubmission, operational, financial, compliance };


                if (operational == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Operational like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                if (financial == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Financial like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                if (compliance == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Compliance like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }


                ResultModel<List<AuditIssue>> indexData =
                    _auditIssueService.GetAuditIssueIndexData(index,
                        conditionalFields, conditionalValue);

                foreach (var item in indexData.Data)
                {
                    if (item.IssueDetails != null)
                    {
                        item.IssueDetails = Encoding.UTF8.GetString(Convert.FromBase64String(item.IssueDetails));

                    }                   
                }
                
                ResultModel<int> indexDataCount =
                    _auditIssueService.GetAuditIssueIndexCount(index, conditionalFields, conditionalValue);

                int result = _auditIssueService.GetCount(TableName.A_AuditIssues, "Id", new[]
                        {
                            "createdBy",
                        }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }



        //ForExcelIndex
        public async Task<IActionResult> ExcelIndex()
        {
            return View();
        }
        [HttpPost]
        public ActionResult<IList<AuditIssue>> _excelIndex()
        {
            try
            {

                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string auditCode = Request.Form["auditCode"].ToString();
                string auditName = Request.Form["auditName"].ToString();
                string auditStatus = Request.Form["auditStatus"].ToString();
                string issuename = Request.Form["issuename"].ToString();
                string issueHeading = Request.Form["issueHeading"].ToString();
                string issuepriority = Request.Form["issuepriority"].ToString();
                string issueStatus = Request.Form["issueStatus"].ToString();
                string dateofsubmission = Request.Form["dateofsubmission"].ToString();
                string investigationOrforensis = Request.Form["investigationOrforensis"].ToString();
                string stratigicMeeting = Request.Form["stratigicMeeting"].ToString();
                string managementReviewMeeting = Request.Form["managementReviewMeeting"].ToString();
                string otherMeeting = Request.Form["otherMeeting"].ToString();
                string training = Request.Form["training"].ToString();
                string operational = Request.Form["operational"].ToString();
                string financial = Request.Form["Financial"].ToString();
                string compliance = Request.Form["compliance"].ToString();

                //investigationOrforensis
                if (investigationOrforensis == "")
                {
                    investigationOrforensis = "Select";
                }
                if (investigationOrforensis != "Select")
                {
                    investigationOrforensis = (investigationOrforensis == "True") ? "1" : "0";

                }
                //managementReviewMeeting
                if (managementReviewMeeting == "")
                {
                    managementReviewMeeting = "Select";
                }
                if (managementReviewMeeting != "Select")
                {
                    managementReviewMeeting = (managementReviewMeeting == "True") ? "1" : "0";
                }
                //otherMeeting
                if (otherMeeting == "")
                {
                    otherMeeting = "Select";
                }
                if (otherMeeting != "Select")
                {
                    otherMeeting = (otherMeeting == "True") ? "1" : "0";
                }
                //training
                if (training == "")
                {
                    training = "Select";
                }
                if (training != "Select")
                {
                    training = (training == "True") ? "1" : "0";
                }
                //stratigicMeeting
                if (stratigicMeeting == "")
                {
                    stratigicMeeting = "Select";
                }
                if (stratigicMeeting != "Select")
                {
                    stratigicMeeting = (stratigicMeeting == "True") ? "1" : "0";
                }
                if (operational == "")
                {
                    operational = "Select";
                }
                if (operational != "Select")
                {
                    operational = (operational == "True") ? "1" : "0";

                }
                if (financial == "")
                {
                    financial = "Select";
                }
                if (financial != "Select")
                {
                    financial = (financial == "True") ? "1" : "0";

                }
                if (compliance == "")
                {
                    compliance = "Select";
                }
                if (compliance != "Select")
                {
                    compliance = (compliance == "True") ? "1" : "0";

                }

                //operational = (operational == "True") ? "1" : "0";
                //financial = (financial == "True") ? "1" : "0";
                //compliance = (compliance == "True") ? "1" : "0";

                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = "asc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                //index.Status = edit;
                index.Status = "TotalIssues";
                index.UserName = userName;

                string[] conFields = new string[10];

                string[] conditionalFields = new[]
                {
                    "A.Code like",
                    "A.Name like",
                    "A.AuditStatus like",

                    "AI.IssueName like",
                    "Enums.EnumValue like",

                    "anm.EnumValue like",
                    "AI.DateOfSubmission like",


                    "AI.InvestigationOrForensis like",
                    "AI.StratigicMeeting like",
                    "AI.ManagementReviewMeeting like",
                    "AI.OtherMeeting like",
                    "AI.Training like",

                    "AI.Operational like",
                    "AI.Financial like",
                    "AI.Compliance like"
                };
                string?[] conditionalValue = new[] { auditCode, auditName, auditStatus, issueHeading, issuepriority, issueStatus, dateofsubmission, investigationOrforensis, stratigicMeeting, managementReviewMeeting, otherMeeting, training, operational, financial, compliance };

                //InvestigationOrForensis
                if (investigationOrforensis == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.InvestigationOrForensis like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                //StratigicMeeting
                if (stratigicMeeting == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.StratigicMeeting like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                //ManagementReviewMeeting
                if (managementReviewMeeting == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.ManagementReviewMeeting like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }

                //OtherMeeting
                if (otherMeeting == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.OtherMeeting like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                //Training
                if (training == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Training like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                //Operational
                if (operational == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Operational like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                if (financial == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Financial like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }
                if (compliance == "Select")
                {
                    int indexdata = Array.IndexOf(conditionalFields, "AI.Compliance like");

                    if (indexdata != -1)
                    {
                        conditionalFields = conditionalFields.Take(indexdata).Concat(conditionalFields.Skip(indexdata + 1)).ToArray();
                        conditionalValue = conditionalValue.Take(indexdata).Concat(conditionalValue.Skip(indexdata + 1)).ToArray();
                    }
                }

                ResultModel<List<AuditIssue>> indexData =_auditIssueService.GetExcelIndexData(index,conditionalFields, conditionalValue);

                //AddintAllDetailsFromEncryptTextToNormalText
                foreach (var item in indexData.Data)
                {
                    if (item.IssueDetails != null)
                    {
                        item.IssueDetails = Encoding.UTF8.GetString(Convert.FromBase64String(item.IssueDetails));

                    }
                }
                foreach (var item in indexData.Data)
                {
                    if (item.FeedbackDetails != null)
                    {
                        item.FeedbackDetails = Encoding.UTF8.GetString(Convert.FromBase64String(item.FeedbackDetails));

                    }
                }
                foreach (var item in indexData.Data)
                {
                    if (item.BranchFeedBackDetails != null)
                    {
                        item.BranchFeedBackDetails = Encoding.UTF8.GetString(Convert.FromBase64String(item.BranchFeedBackDetails));

                    }
                }

                ResultModel<int> indexDataCount =
                    _auditIssueService.GetExcelIndexCount(index, conditionalFields, conditionalValue);

                int result = _auditIssueService.GetCount(TableName.A_AuditIssues, "Id", new[]
                        {
                            "createdBy",
                        }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }


        //EndOfExcelIndex



        //FollowUpAuditIssueEamil
        [HttpPost]
        public ActionResult FollowUpAuditIssueEamil(AuditIssueUser master)
        {
            ResultModel<AuditIssueUser> result = new ResultModel<AuditIssueUser>();
            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                UserProfile urp = new UserProfile();
                urp.Id = master.AuditId;
                urp.UserName = user.UserName;
                MailSetting mailSetting = new MailSetting();
                mailSetting.Id = master.AuditId;

                var notifiEmai = _auditMasterService.GetEamil(urp);
                var email = notifiEmai.Data.FirstOrDefault();
                var currentUrl = HttpContext.Request.GetDisplayUrl();
                string[] parts = new string[] { "", "" };
                parts = currentUrl.TrimStart('/').Split('/');
                string urlhttp = parts[0];
                string HostUrl = parts[2];
                string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + master.AuditId + "?edit=Branchfeedback";

                //For Getting Audit Name
                ResultModel<List<AuditMaster?>> AuditMaster =_auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });
                AuditMaster? auditMaster = AuditMaster.Data.FirstOrDefault();
                //End Of Getting
                List<AuditIssueUser> userlist = _auditIssueUserService.GetAuditIssueUsersById(master.AuditIssueId);

                foreach (var item in userlist)
                {
                    MailService.FollowUpAuditIssueEamil(item.EmailAddress, AuditPreviewUrl, auditMaster.Name);
                    master.Status = "200";
                }

                return Ok(master);



            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }


        [HttpPost]
        public ActionResult IssuedeadLineLapsed(AuditIssueUser master)
        {
            ResultModel<AuditIssueUser> result = new ResultModel<AuditIssueUser>();
            try
            {

                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

                UserProfile urp = new UserProfile();
                urp.Id = master.AuditId;
                urp.UserName = user.UserName;
                MailSetting mailSetting = new MailSetting();
                mailSetting.Id = master.AuditId;

                var notifiEmai = _auditMasterService.GetEamil(urp);

                var email = notifiEmai.Data.FirstOrDefault();


                var currentUrl = HttpContext.Request.GetDisplayUrl();
                string[] parts = new string[] { "", "" };
                parts = currentUrl.TrimStart('/').Split('/');
                string urlhttp = parts[0];
                string HostUrl = parts[2];
                string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + master.AuditId + "?edit=Branchfeedback";

                //For Getting Audit Name
                ResultModel<List<AuditMaster?>> AuditMaster =
                _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });

                AuditMaster? auditMaster = AuditMaster.Data.FirstOrDefault();
                //End Of Getting


                List<AuditIssueUser> userlist = _auditIssueUserService.GetAuditIssueUsersById(master.AuditIssueId);

                foreach (var item in userlist)
                {
                    MailService.IssuedeadLineLapsedEamil(item.EmailAddress, AuditPreviewUrl, auditMaster.Name);

                    master.Status = "200";

                }

                return Ok(master);



            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }
        //TotalPendingIssuesReview
        public ActionResult TotalPendingIssuesReview(AuditIssueUser master)
        {
            ResultModel<AuditIssueUser> result = new ResultModel<AuditIssueUser>();
            try
            {

                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                UserProfile urp = new UserProfile();
                urp.Id = master.AuditId;
                urp.UserName = user.UserName;
                MailSetting mailSetting = new MailSetting();
                mailSetting.Id = master.AuditId;

                var notifiEmai = _auditMasterService.GetEamil(urp);

                var email = notifiEmai.Data.FirstOrDefault();


                var currentUrl = HttpContext.Request.GetDisplayUrl();
                string[] parts = new string[] { "", "" };
                parts = currentUrl.TrimStart('/').Split('/');
                string urlhttp = parts[0];
                string HostUrl = parts[2];
                string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + master.AuditId + "?edit=Branchfeedback";

                //For Getting Audit Name
                ResultModel<List<AuditMaster?>> AuditMaster =
                _auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });

                AuditMaster? auditMaster = AuditMaster.Data.FirstOrDefault();
                //End Of Getting


                List<AuditIssueUser> userlist = _auditIssueUserService.GetAuditIssueUsersById(master.AuditIssueId);

                foreach (var item in userlist)
                {
                    MailService.TotalPendingIssuesReviewEamil(item.EmailAddress, AuditPreviewUrl, auditMaster.Name);

                    master.Status = "200";

                }

                return Ok(master);



            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }



        //End Of ALlEmail Save


        public ActionResult<IList<AuditIssueUser>> Delete(AuditIssueUser master)
        {
            try
            {

                ResultModel<AuditIssueUser> result = _auditIssueUserService.Delete(master.Id);
                //ResultModel<AuditMaster> result = new ResultModel<AuditMaster>();

                return Ok(result);

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

        //ReportDelete
        public ActionResult<IList<AuditReportUsers>> ReportDelete(AuditReportUsers master)
        {
            try
            {

                ResultModel<AuditReportUsers> result = _auditIssueUserService.ReportDelete(master.Id);             
                return Ok(result);

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

        public ActionResult MultiplePost(AuditIssue master)
        {
            ResultModel<AuditIssue> result = new ResultModel<AuditIssue>();

            try
            {

                foreach (string ID in master.IDs)
                {


                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    master.Operation = "post";
                    result = _auditIssueService.MultiplePost(master);


                }




                return Ok(result);


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        public ActionResult MultipleUnPost(AuditIssue master)
        {
            ResultModel<AuditIssue> result = new ResultModel<AuditIssue>();

            try
            {

                foreach (string ID in master.IDs)
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    master.Operation = "unpost";
                    result = _auditIssueService.MultipleUnPost(master);
                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }







        [HttpPost]
        public IActionResult DeleteFile(string filePath, string id)
        {
            string saveDirectory = "wwwroot\\files";

            ResultModel<AuditIssueAttachments> result = new ResultModel<AuditIssueAttachments>
            {
                Message = "File could not be deleted"
            };

            try
            {
                var path = Path.Combine(saveDirectory, filePath);
                if (!System.IO.File.Exists(path)) return Ok(result);

                result = _auditIssueAttachmentsService.Delete(Convert.ToInt32(id.Replace("file-", "")));

                if (result.Status == Status.Success)
                {
                    System.IO.File.Delete(path);
                }


                return Ok(result);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);

                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public async Task<IActionResult> DownloadFile(string filePath)
        {
            string saveDirectory = "wwwroot\\files";

            try
            {
                var path = Path.Combine(saveDirectory, filePath);
                var memory = new MemoryStream();
                await using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var ext = Path.GetExtension(path).ToLowerInvariant();
                return File(memory, GetMimeType(ext), Path.GetFileName(path));
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);

                return RedirectToAction("Index");
            }
        }


        private string GetMimeType(string ext)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(ext, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
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
