using Microsoft.AspNetCore.Mvc;
using Shampan.Models.AuditModule;
using Shampan.Models;
using ShampanERP.Models;
using StackExchange.Exceptional;
using ShampanERP.Persistence;

namespace SSLAudit.Controllers
{
	public class AuditResponseController : Controller
	{

		private readonly ApplicationDbContext _applicationDb;

		public AuditResponseController(ApplicationDbContext applicationDb)
		{
			_applicationDb = applicationDb;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult<IList<AuditBranchFeedback>> _indexAuditResponse(int? id)
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


				ResultModel<List<AuditBranchFeedback>> indexData = null;
					//_auditBranchFeedbackService.GetIndexData(index,conditionalFields, conditionalValue);


				ResultModel<int> indexDataCount = null;
					//_auditBranchFeedbackService.GetIndexDataCount(index,conditionalFields, conditionalValue);

				int result = 2; //_auditBranchFeedbackService.GetCount(TableName.A_Audits, "Id", null, null);

				
				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<AuditIssue>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}
		}



	}
}
