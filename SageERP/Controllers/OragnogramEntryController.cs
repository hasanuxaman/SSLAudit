using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Data.SqlClient;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Oragnogram;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]

	public class OragnogramEntryController : Controller
    {

		private readonly ApplicationDbContext _applicationDb;
		private readonly IOragnogramService _oragnogramService;
		private readonly IEmployeesHiAttachmentsService _employeesHiAttachmentsService;

		public OragnogramEntryController(ApplicationDbContext applicationDb, IOragnogramService oragnogramService, IEmployeesHiAttachmentsService employeesHiAttachmentsService)
		{

			_applicationDb = applicationDb;
			_oragnogramService = oragnogramService;
			_employeesHiAttachmentsService = employeesHiAttachmentsService;

		}

		public IActionResult Index()
        {
            return View();
        }

		public ActionResult Create()
		{

			ModelState.Clear();
			EmployeesHierarchy vm = new EmployeesHierarchy();
			vm.Operation = "add";
			return View("CreateEdit", vm);


		}
		public ActionResult CreateEdit(EmployeesHierarchy master)
		{
			ResultModel<EmployeesHierarchy> result = new ResultModel<EmployeesHierarchy>();
			try
			{

				//throw new NotImplementedException();


				if (master.Operation == "update")
				{

					string userName = User.Identity.Name;
					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.LastUpdateBy = user.UserName;
					master.Audit.LastUpdateOn = DateTime.Now;
					master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

					result = _oragnogramService.Update(master);

					return Ok(result);
				}
				else
				{

					string userName = User.Identity.Name;

					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.CreatedBy = user.UserName;
					master.Audit.CreatedOn = DateTime.Now;
					master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
					result = _oragnogramService.Insert(master);

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
		public IActionResult DeleteFile(string filePath, string id)
		{
			string saveDirectory = "wwwroot\\Images";

			ResultModel<EmployeesHierarchyAttachments> result = new ResultModel<EmployeesHierarchyAttachments>
			{
				Message = "File could not be deleted"
			};

			try
			{
				var path = Path.Combine(saveDirectory, filePath);
				if (!System.IO.File.Exists(path)) return Ok(result);

				result = _employeesHiAttachmentsService.Delete(Convert.ToInt32(id.Replace("file-", "")));

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
			string saveDirectory = "wwwroot\\Images";

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




		public IActionResult _index()
		{
			try
			{
				IndexModel index = new IndexModel();
				string userName = User.Identity.Name;
				ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

				var search = Request.Form["search[value]"].FirstOrDefault();
				string users = Request.Form["BranchCode"].ToString();
				string branch = Request.Form["BranchName"].ToString();
				string Address = Request.Form["Address"].ToString();
				string TelephoneNo = Request.Form["TelephoneNo"].ToString();



				string code = Request.Form["code"].ToString();
				string name = Request.Form["name"].ToString();
				string designation = Request.Form["designation"].ToString();



				string post = Request.Form["ispost"].ToString();
				if (post == "Select")
				{
					post = "";

				}





				string draw = Request.Form["draw"].ToString();
				var startRec = Request.Form["start"].FirstOrDefault();
				var pageSize = Request.Form["length"].FirstOrDefault();
				var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();

				var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();

				index.SearchValue = Request.Form["search[value]"].FirstOrDefault();

				index.OrderName = "EmployeeId";

				index.orderDir = orderDir;
				index.startRec = Convert.ToInt32(startRec);
				index.pageSize = Convert.ToInt32(pageSize);


				index.createdBy = userName;


				string[] conditionalFields = new[]
				{
							"Code like",
                            "Name like",
							"Designation like"

				};

				string?[] conditionalValue = new[] { code, name, designation };

				ResultModel<List<EmployeesHierarchy>> indexData =
					_oragnogramService.GetIndexData(index, conditionalFields, conditionalValue);



				ResultModel<int> indexDataCount =
				_oragnogramService.GetIndexDataCount(index, conditionalFields, conditionalValue);


				int result = _oragnogramService.GetCount(TableName.EmployeesHierarchy, "EmployeeId", new[] { "EmployeesHierarchy.createdBy", }, new[] { userName });


				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<EmployeesHierarchy>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}


		}


		public ActionResult Edit(int id)
		{
			try
			{
				ResultModel<List<EmployeesHierarchy>> result =
					_oragnogramService.GetAll(new[] { "EmployeeId" }, new[] { id.ToString() });

				EmployeesHierarchy employeesHierarchy = result.Data.FirstOrDefault();
				employeesHierarchy.Operation = "update";
				employeesHierarchy.EmployeeId = id;


				employeesHierarchy.AttachmentsList = _employeesHiAttachmentsService.GetAll(new[] { "EmployeeId" }, new[] { id.ToString() }).Data;


				return View("CreateEdit", employeesHierarchy);
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return RedirectToAction("Index");
			}
		}



	}
}
