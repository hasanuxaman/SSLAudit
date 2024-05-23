using DocumentFormat.OpenXml.Office2021.PowerPoint.Comment;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NuGet.Packaging;
using OpenQA.Selenium.DevTools.V114.Page;
using Shampan.Core.Interfaces.Repository.AuditFeedbackRepo;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.AuditFeedbackService;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services;
using Shampan.Services.Audit;
using Shampan.Services.AuditFeedbackService;
using Shampan.Services.AuditIssues;
using Shampan.Services.Deshboard;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;
using System.Data.SqlClient;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]

    public class AuditBranchFeedbackController : Controller
    {

        private readonly ApplicationDbContext _applicationDb;
        private readonly IAuditFeedbackService _auditFeedbackService;
        private readonly IAuditBranchFeedbackService _auditBranchFeedbackService;
        private readonly IAuditFeedbackAttachmentsService _auditFeedbackAttachmentsService;
        private readonly IAuditBranchFeedbackAttachmentsService _auditBranchFeedbackAttachmentsService;
        private readonly IAuditMasterService _auditMasterService;
        private readonly IConfiguration _configuration;
		private readonly IDeshboardService _deshboardService;


		public AuditBranchFeedbackController(
            ApplicationDbContext applicationDb,
            IAuditBranchFeedbackService auditBranchFeedbackService,
            IAuditFeedbackAttachmentsService auditFeedbackAttachmentsService,
            IAuditBranchFeedbackAttachmentsService auditBranchFeedbackAttachmentsService,
            IAuditMasterService auditMasterService,
            IConfiguration configuration,
			IDeshboardService deshboardService

	   )
        {

            _applicationDb = applicationDb;
            _auditBranchFeedbackService = auditBranchFeedbackService;
            _auditFeedbackAttachmentsService = auditFeedbackAttachmentsService;
            _auditBranchFeedbackAttachmentsService = auditBranchFeedbackAttachmentsService;
            _auditMasterService = auditMasterService;
            _configuration = configuration;
			_deshboardService = deshboardService;


			AuthDbConfig.AuthDB = AuthDbName();
        }
        public IActionResult Index(int? id)
        {
            if (id is null || id == 0)
            {
                return RedirectToAction("Index", "Audit");
            }
            AuditBranchFeedback auditIssue = new AuditBranchFeedback()
            {
                AuditId = id.Value
            };
            return View(auditIssue);
        }

        public IActionResult Create(int? id)
        {

            if (id is null || id == 0)
            {
                return RedirectToAction("Index", "Audit");
            }
            AuditBranchFeedback auditIssue = new AuditBranchFeedback()
            {
                Operation = "add",
                AuditId = id.Value
            };
            return View(auditIssue);
        }

        [HttpPost]
        public ActionResult CreateEdit(AuditBranchFeedback master)
        {
            ResultModel<AuditBranchFeedback> result = new ResultModel<AuditBranchFeedback>();
            try
            {
                master.AuditIssueId = master.AuditBranchIssueId;
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

                if (master.Operation == "update")
                {

                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = "";
                    result = _auditBranchFeedbackService.Update(master);

                    //if (result.Status == Status.Fail)
                    //{
                    //    throw result.Exception;
                    //}

                    return Ok(result);
                }

                master.Audit.CreatedBy = user.UserName;
                master.Audit.CreatedOn = DateTime.Now;
                master.Audit.CreatedFrom = "";
                master.Audit.LastUpdateBy = user.UserName;
                master.Audit.LastUpdateOn = DateTime.Now;
                master.Audit.LastUpdateFrom = "";

                result = _auditBranchFeedbackService.Insert(master);

                //AuditName
                ResultModel<List<AuditMaster?>> AuditMaster =_auditMasterService.GetAll(new[] { "Id" }, new[] { master.AuditId.ToString() });
                AuditMaster? auditMaster = AuditMaster.Data.FirstOrDefault();


				bool isBranch = _deshboardService.AuditBranchUserGetAll(userName);

				//if (master.TeamCheck == "Team")
				if (!isBranch)
                {
                    ResultModel<List<AuditUser>> UserData = _auditMasterService.GetAuditIssueUsers(master.BranchEmailIssueId);
                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];
                    string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + master.AuditId + "?edit=Branchfeedback";

                    foreach (var User in UserData.Data)
                    {
                        User.Audit.CreatedBy = user.UserName;
                        User.AuditId = result.Data.Id;
                        User.Audit.CreatedOn = DateTime.Now;
                        User.Audit.CreatedFrom = "";
                        User.Audit.LastUpdateBy = user.UserName;
                        User.Audit.LastUpdateOn = DateTime.Now;
                        //_auditUserService.Insert(User);                      
                        MailService.SendAuditTeamUserToBranchFeedbackMail(User.EmailAddress, AuditPreviewUrl, auditMaster.Name);
                    }

                }
                else
                {

                    ResultModel<List<AuditUser>> UserData = _auditMasterService.GetAuditUsers(master.AuditId);
                    ResultModel<List<AuditUser>> UserDataForTeam = _auditMasterService.GetAuditUserTeamId(UserData.Data.FirstOrDefault().TeamId);
                    var currentUrl = HttpContext.Request.GetDisplayUrl();
                    string[] parts = new string[] { "", "" };
                    parts = currentUrl.TrimStart('/').Split('/');
                    string urlhttp = parts[0];
                    string HostUrl = parts[2];           
                    string AuditPreviewUrl = urlhttp + "//" + HostUrl + "/Audit/Edit/" + master.AuditId + "?edit=Branchfeedback";

                    foreach (var User in UserDataForTeam.Data)
                    {
                        User.Audit.CreatedBy = user.UserName;
                        User.AuditId = result.Data.Id;
                        User.Audit.CreatedOn = DateTime.Now;
                        User.Audit.CreatedFrom = "";
                        User.Audit.LastUpdateBy = user.UserName;
                        User.Audit.LastUpdateOn = DateTime.Now;
                        //_auditUserService.Insert(User);
                        MailService.SendAuditBranchFeedbackTeamUserMail(User.EmailAddress, AuditPreviewUrl, auditMaster.Name);
                    }

                }

                //if (result.Status == Status.Fail)
                //{
                //    throw result.Exception;
                //}

                Uri returnurl = new Uri(Request.Host.ToString());
                string urlString = returnurl.ToString();
                string Url = "https://" + urlString + "/Audit/Edit/" + result.Data.Id + "?edit=branchFeedbackApprove";
                MailSetting ms = new MailSetting();
                ms.ApprovedUrl = Url;
                ms.BranchFeedbackId = result.Data.Id;
                //_auditMasterService.SaveUrl(ms);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }

        public ActionResult<IList<AuditFeedback>> Edit(int id)
        {
            try
            {
                ResultModel<List<AuditBranchFeedback?>> result =_auditBranchFeedbackService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                if (result.Status == Status.Fail)
                {
                    throw result.Exception;
                }

                AuditBranchFeedback? auditMaster = result.Data.FirstOrDefault();

                //ChangeOfAuditAndIdName
                auditMaster.AttachmentsList = _auditBranchFeedbackAttachmentsService.GetAll(new[] { "AuditBranchFeedbackId" }, new[] { id.ToString() }).Data;
                auditMaster.Operation = "update";

                return View("Create", auditMaster);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult<IList<AuditBranchFeedback>> _index(int? id)
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

                string? search = Request.Form["search[value]"].FirstOrDefault();
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                //index.OrderName = orderName;
                //index.orderDir = orderDir;
                index.OrderName = "Id";
                index.orderDir = "desc";
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.AuditId = id.Value;

                string[] conditionalFields = new[]
                {
                    "ai.IssueName like",
                    "ad.Name like",
                    "af.Heading like",
                    "af.IsPost like"
                };
                string?[] conditionalValue = new[] { search, search, search, search };


                ResultModel<List<AuditBranchFeedback>> indexData =
                    _auditBranchFeedbackService.GetIndexData(index,
                conditionalFields, conditionalValue);

                #region IssueCommentsForApproveAndReject And Adding New Row on BranchFeedback Table
                             
                AuditBranchFeedback bf = new AuditBranchFeedback();
                bf.Id = Convert.ToInt32(id);
                List<AuditBranchFeedback> IssueIDs = _auditBranchFeedbackService.GetAuditIssuesFromBranch(bf);
                List<int> issueId = new List<int>();
                //top from branchfeedback
                List<AuditBranchFeedback> branchfeedbackIds = new List<AuditBranchFeedback>();
                foreach (var issue in IssueIDs)
                {
                    var item = _auditBranchFeedbackService.GetTopOneBranchId(issue.AuditIssueId);
                    branchfeedbackIds.Add(item);
                }
                List<int> branckId = new List<int>();            
                var RejectItems = new List<AuditBranchFeedback>();     
                foreach (var branchfeedback in branchfeedbackIds)
                {
                    foreach (var item in indexData.Data)
                    {                       
                        if (item.Id == branchfeedback.Id)
                        {

                            List<IssueRejectComments> commentsList = _auditBranchFeedbackService.GetIssueRejectComments(item.AuditIssueId);
                            List<IssueRejectComments> approveData = _auditBranchFeedbackService.GetIssueApproveComments(item.AuditIssueId);
                            string comm = "";
                            int i = 1;

                            foreach (var comment in commentsList)
                            {
                                CommonFields CommonFields = new CommonFields();                               
                                AuditBranchFeedback abf = new AuditBranchFeedback();

                                abf.IssueRejectComments = comment.IssuesRejectComments;
                                abf.Id = item.Id;
                                abf.ImplementationDate = "NA";
                                abf.Heading = item.Heading;
                                abf.ImplementationStatus = "NA";
                                abf.IssueApproveComments = "Rejected";

                                abf.CommonFields.IssueName = item.IssueName;
                                abf.CommonFields.CreatedOn = comment.CreatedOn;
                                abf.CommonFields.CreatedBy = "HOD";

                                RejectItems.Add(abf);

                                //RejectItems.Add(new AuditBranchFeedback()
                                //{
                                //    IssueRejectComments = comment.IssuesRejectComments,
                                //    Id = item.Id,
                                //    ImplementationDate = "NA",
                                //    CreatedOn = comment.CreatedOn,
                                //    IssueName = item.IssueName,
                                //    Heading = item.Heading,
                                //    IsPost = item.IsPost,
                                //    ImplementationStatus = "NA",
                                //    IssueApproveComments = "Rejected",
                                //    CreatedBy = "HOD"
                                //}); 
                            }

                            //ForApproveStatusset
                            foreach (var itemdata in approveData)
                            {
                                if (itemdata.IsIssueApprove == true)
                                {
                                    item.IssueApproveComments = "Approved By HOD";
                                }
                            }
                        }
                    }
                }

                var NewList = new List<AuditBranchFeedback>();
                foreach (var item in indexData.Data)
                {
                    NewList.Add(item);
                    foreach (var data in RejectItems)
                    {
                        if (data.Id == item.Id)
                        {
                            NewList.Add(data);
                        }
                    }

                }

                #endregion

                //AuditBranchFeedback br = indexData.Data.Where(x => x.Id == 2456).FirstOrDefault();
                //br.Heading = "New Heading";

                ResultModel<int> indexDataCount =
                    _auditBranchFeedbackService.GetIndexDataCount(index,
                conditionalFields, conditionalValue);

                int result = _auditBranchFeedbackService.GetCount(TableName.A_AuditBranchFeedbacks, "Id", null, null);

                // data, draw, recordsTotal = TotalReocrd, recordsFiltered = data.Count
                //return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
                return Ok(new { data = NewList, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data});
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        [HttpPost]
        public ActionResult<IList<AuditBranchFeedback>> _indexBranchFeedback(int? id)
        {
            try
            {
                //if (id is null || id == 0)
                //{
                //    return Ok(new { Data = new List<AuditFeedback>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
                //}

                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                string? search = Request.Form["search[value]"].FirstOrDefault();
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = orderName;
                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);
                index.createdBy = userName;
                index.AuditId = id.Value;

                string[] conditionalFields = new[]
                {
                    "ai.IssueName like",
                    "ad.Name like"
                };
                string?[] conditionalValue = new[] { search, search };


                ResultModel<List<AuditBranchFeedback>> indexData =
                    _auditBranchFeedbackService.GetIndexData(index,
                conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                    _auditFeedbackService.GetIndexDataCount(index,
                conditionalFields, conditionalValue);

                int result = _auditFeedbackService.GetCount(TableName.A_Audits, "Id", null, null);

                // data, draw, recordsTotal = TotalReocrd, recordsFiltered = data.Count
                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }
        }

        public ActionResult MultiplePost(AuditBranchFeedback master)
        {
            ResultModel<AuditBranchFeedback> result = new ResultModel<AuditBranchFeedback>();

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
                    result = _auditBranchFeedbackService.MultiplePost(master);

                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }


        public ActionResult MultipleUnPost(AuditBranchFeedback master)
        {
            ResultModel<AuditBranchFeedback> result = new ResultModel<AuditBranchFeedback>();

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
                    result = _auditBranchFeedbackService.MultipleUnPost(master);
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

            ResultModel<AuditBranchFeedbackAttachments> result = new ResultModel<AuditBranchFeedbackAttachments>
            {
                Message = "File could not be deleted"
            };

            try
            {
                var path = Path.Combine(saveDirectory, filePath);
                if (!System.IO.File.Exists(path)) return Ok(result);

                //result = _auditFeedbackAttachmentsService.Delete(Convert.ToInt32(id.Replace("file-", "")));
                result = _auditBranchFeedbackAttachmentsService.Delete(Convert.ToInt32(id.Replace("file-", "")));

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
