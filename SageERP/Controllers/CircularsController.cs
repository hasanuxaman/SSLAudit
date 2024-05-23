using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Circular;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class CircularsController : Controller
	{
		private readonly ApplicationDbContext _applicationDb;
		private readonly ICircularsService _circularsService;
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly ICircularAttachmentsService _circularAttachmentsService;
		public CircularsController(ApplicationDbContext applicationDb, ITeamsService teamsService, ICircularsService circularsService,
			IWebHostEnvironment hostingEnvironment, ICircularAttachmentsService circularAttachmentsService)
		{

			_applicationDb = applicationDb;
			_circularsService = circularsService;
			_hostingEnvironment = hostingEnvironment;
			_circularAttachmentsService = circularAttachmentsService;
		}


		public IActionResult Index()
		{
			return View();
		}
		public ActionResult Create()
		{

			ModelState.Clear();
			Circulars vm = new Circulars();
			vm.Operation = "add";
			return View("CreateEdit", vm);



		}

		//, IFormFile file
		//, List<IFormFile> Attachment

		[HttpPost]
		public ActionResult CreateEdit(Circulars master)
		{
			ResultModel<Circulars> result = new ResultModel<Circulars>();


			try
			{

				//using (var stream = new MemoryStream())
				//{
				//	Attachments.CopyTo(stream);

				//	using (var package = new ExcelPackage(stream))
				//	{
				//		var worksheet = package.Workbook.Worksheets[0]; 
				
				//		for (int row = 2; row <= worksheet.Dimension.Rows; row++)
				//		{
				//			var cellValue1 = worksheet.Cells[row, 1].Value?.ToString(); 
				//			var cellValue2 = worksheet.Cells[row, 2].Value?.ToString(); 
				//			var cellValue3 = worksheet.Cells[row, 3].Value?.ToString(); 
				//			var cellValue4 = worksheet.Cells[row, 4].Value?.ToString(); 
																					   
				//		}
				//	}

				//}




				if (master.Operation == "update")
				{

					string userName = User.Identity.Name;
					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.LastUpdateBy = user.UserName;
					master.Audit.LastUpdateOn = DateTime.Now;
					master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

					result = _circularsService.Update(master);

					return Ok(result);
				}
				else
				{

					string userName = User.Identity.Name;

					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.CreatedBy = user.UserName;
					master.Audit.CreatedOn = DateTime.Now;
					master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
					result = _circularsService.Insert(master);

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
			string saveDirectory = "wwwroot\\files";

			ResultModel<CircularAttachments> result = new ResultModel<CircularAttachments>
			{
				Message = "File could not be deleted"
			};

			try
			{
				var path = Path.Combine(saveDirectory, filePath);
				if (!System.IO.File.Exists(path)) return Ok(result);

				result = _circularAttachmentsService.Delete(Convert.ToInt32(id.Replace("file-", "")));

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
				string circulartype = Request.Form["circulartype"].ToString();
				string circulardate = Request.Form["circulardate"].ToString();
				string circulardetails = Request.Form["circulardetails"].ToString();




				string description = Request.Form["description"].ToString();
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

				index.OrderName = "Id";

				index.orderDir = orderDir;
				index.startRec = Convert.ToInt32(startRec);
				index.pageSize = Convert.ToInt32(pageSize);


				index.createdBy = userName;


				string[] conditionalFields = new[]
				{
							"Code like",
							"CircularType.Name like",
							"CircularDate like",
							"CircularDetails like"

				};

				string?[] conditionalValue = new[] { code, circulartype, circulardate, circulardetails };

				ResultModel<List<Circulars>> indexData =
					_circularsService.GetIndexData(index, conditionalFields, conditionalValue);



				ResultModel<int> indexDataCount =
				_circularsService.GetIndexDataCount(index, conditionalFields, conditionalValue);


				int result = _circularsService.GetCount(TableName.Circulars, "Id", new[] { "Circulars.createdBy", }, new[] { userName });


				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}


		}


		public ActionResult Edit(int id, string isFromDeshBoard = "index")
		{
			try
			{
				ResultModel<List<Circulars>> result =
					_circularsService.GetAll(new[] { "Id" }, new[] { id.ToString() });

				Circulars circular = result.Data.FirstOrDefault();
				circular.Operation = "update";
				circular.Id = id;
				circular.Edit = isFromDeshBoard;

				//AuditIssue? auditMaster = result.Data.FirstOrDefault();

				circular.AttachmentsList = _circularAttachmentsService.GetAll(new[] { "CircularId" }, new[] { id.ToString() }).Data;


				return View("CreateEdit", circular);
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return RedirectToAction("Index");
			}
		}





	}
}
