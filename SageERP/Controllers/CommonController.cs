using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services.Audit;
using ShampanERP.Persistence;

namespace SageERP.Controllers
{
    [Authorize]
    public class CommonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly ICommonService _commonService;
        private readonly IAuditMasterService _auditMasterService;



        private readonly ApplicationDbContext _applicationDb;

        private readonly IBranchProfileService _branchProfileService;

        public CommonController(ICommonService commonService, ApplicationDbContext applicationDb,

             IBranchProfileService branchProfileService, IAuditMasterService auditMasterService
            )
        {
            _commonService = commonService;
            _applicationDb = applicationDb;
            _branchProfileService = branchProfileService;
            _auditMasterService = auditMasterService;
        }


		//SSLAudit
		

		public ActionResult<IList<CommonDropDown>> ReportingManagers()
		{
			var result = _commonService.ReportingManagers();
			return Ok(result);
		}
		public ActionResult<IList<CommonDropDown>> ColorName()
        {
            var result = _commonService.ColorName();
            return Ok(result);
        }
        public ActionResult<IList<CommonDropDown>> TeamName()
        {
            var result = _commonService.TeamName();
            return Ok(result);
        }
        public ActionResult<IList<CommonDropDown>> AuditName()
        {
            var result = _commonService.AuditName();
            return Ok(result);
        }

        public ActionResult<IList<CommonDropDown>> UserId()
        {
            var result = _commonService.UserId();
            return Ok(result);
        }
        public ActionResult<IList<CommonDropDown>> ModulId()
        {
            var result = _commonService.ModulId();
            return Ok(result);
        }
        public ActionResult<IList<CommonDropDown>> NodeName()
        {
            var result = _commonService.NodeName();
            return Ok(result);
        }
        public ActionResult<IList<CommonDropDown>> GetModuleName()
		{
			var result = _commonService.GetModuleName();
			return Ok(result);
		}
		

		//End SSLAudit

		public ActionResult<IList<CommonDropDown>> GetAllHeaders()
        {
            var result = _commonService.GetAllHeaders();
            return Ok(result);
        }


        public ActionResult<IList<CommonDropDown>> ApplyMethod()
        {

            var result = _commonService.ApplyMethod();
            return Ok(result);
        }
        public ActionResult<IList<CommonDropDown>> OrderBy()
        {
            var result = _commonService.OrderBy();
            return Ok(result);
        }
        public ActionResult<IList<CommonDropDown>> TransactionType()
        {
            var result = _commonService.TransactionType();
            return Ok(result);
        }


        public ActionResult<IList<CommonDropDown>> Branch()
        {

            string UserId = User.GetUserId();

            var result = _commonService.Branchs(UserId);
            return Ok(result);
        }

        public ActionResult<IList<CommonDropDown>> BranchName(string UserId)
        {

            //string UserId = User.GetUserId();
            var result = _commonService.BranchName(UserId);
            return Ok(result);
        }


        public ActionResult<IList<CommonDropDown>> EntryTypes()
        {
            var result = _commonService.EntryTypes();
            return Ok(result);
        }

        public ActionResult<IList<CommonDropDown>> GetAuditType(bool isPlanned = true)
        {
            var result = _commonService.GetAuditType(isPlanned);
            return Ok(result);
        }

        public ActionResult<IList<CommonDropDown>> GetTeams()
        {
            var result = _commonService.GetTeams();
            return Ok(result);
        }

        public ActionResult<IList<CommonDropDown>> GetReportStatus()
        {
            var result = _commonService.GetReportStatus();
            return Ok(result);
        }
		public ActionResult<IList<CommonDropDown>> GetCircularType()
		{
			var result = _commonService.GetCircularType();
			return Ok(result);
		}
		

		public ActionResult<IList<CommonDropDown>> GetAuditStatus()
        {
            var result = _commonService.GetAuditStatus();
            return Ok(result);
        }

        public ActionResult<IList<CommonDropDown>> ProrationMethod()
        {
            var result = _commonService.ProrationMethod();
            return Ok(result);
        }

        public ActionResult<IList<CommonDropDown>> GetAuditName()
        {
            var result = _commonService.GetAuditName();
            return Ok(result);
        }
        public ActionResult<IList<CommonDropDown>> GetAllUserName()
        {
            var result = _commonService.GetAllUserName();
            return Ok(result);
        }

        public ActionResult<IList<CommonDropDown>> GetIssuePriority()
        {
            var result = _commonService.GetIssuePriority();
            return Ok(result);
        }
		public ActionResult<IList<CommonDropDown>> GetIssueStatus()
		{
			var result = _commonService.GetIssueStatus();
			return Ok(result);
		}
        //Status
        public ActionResult<IList<CommonDropDown>> GetStatus()
        {
            var result = _commonService.GetStatus();
            return Ok(result);
        }
        public ActionResult<IList<CommonDropDown>> GetAuditTypes()
		{
			var result = _commonService.GetAuditTypes();
			return Ok(result);
		}


		public ActionResult<IList<CommonDropDown>> GetIssues(string auditId)
        {
            var result = _commonService.GetIssues(auditId);
            return Ok(result);
        }

        public ActionResult<IList<CommonDropDown>> GetBranchFeedbackIssues(string auditId)
        {

            //CheckTeamUser

            string userName = User.Identity.Name;
            ResultModel<List<AuditMaster?>> getresult =_auditMasterService.GetAll(new[] { "Id" }, new[] { auditId.ToString() });
            AuditMaster auditMaster = new AuditMaster();

            if (getresult != null)
            {
                auditMaster = getresult.Data.FirstOrDefault();

                ResultModel<List<AuditUser>> UserData = _auditMasterService.GetAuditUserTeamId(auditMaster.TeamId);
                if (UserData.Data != null)
                {
                    foreach (AuditUser audit in UserData.Data)
                    {
                        if (userName == audit.UserName)
                        {                          
                            auditMaster.IsTeam = true;                           
                        }
                    }
                }
            }
            
            //End Of Checking
            //string userName = User.Identity.Name;

            var result = _commonService.GetBranchFeedbackIssues(auditId,userName, auditMaster.IsTeam);
            return Ok(result);
        }


        public ActionResult SourceCodeModal(string SourceLedger)
        {
            return PartialView("_SourceCodeModal", SourceLedger);
        }

        public ActionResult priceListModal()
        {
            return PartialView("_priceListModal");

        }
        [HttpPost]
        public ActionResult sageTemplateModal()
        {
            return PartialView("_sageTemplateModal");

        }
        [HttpPost]

        public ActionResult EntryCodeModal()
        {
            return PartialView("_EntryCodeModal");
        }
        

    }



}