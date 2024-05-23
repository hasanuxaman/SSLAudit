using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.TeamMember;
using Shampan.Core.Interfaces.Services.TransportAllownace;
using Shampan.Models;
using Shampan.Services.Advance;
using Shampan.Services.TeamMember;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;
using System.Globalization;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]

	[Authorize]
	public class TransportAllownacesController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly ITransportAllownacesService _transportAllownacesService;
        private readonly ITeamMembersService _teamMembersService;

        public TransportAllownacesController(ApplicationDbContext applicationDb, ITeamsService teamsService, ITransportAllownacesService transportAllownacesService, ITeamMembersService teamMembersService)
        {
            _applicationDb = applicationDb;
            _transportAllownacesService = transportAllownacesService;
            _teamMembersService = teamMembersService;
        }

        public IActionResult Index()
        {
            return View();
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
				string teamname = Request.Form["teamname"].ToString();
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

							"TransportAllownaces.Code like",
							"TeamName like",
							"TransportAllownaces.Description like",
							"TransportAllownaces.IsPost like"

				};

				string?[] conditionalValue = new[] { code, teamname, description, post };

				ResultModel<List<TransportAllownaces>> indexData =
					_transportAllownacesService.GetIndexData(index, conditionalFields, conditionalValue);



				ResultModel<int> indexDataCount =
				_transportAllownacesService.GetIndexDataCount(index, conditionalFields, conditionalValue);


				int result = _transportAllownacesService.GetCount(TableName.TransportAllownaces, "Id", new[] { "TransportAllownaces.createdBy", }, new[] { userName });


				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<TransportAllownaces>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}


		}


		public IActionResult ApproveStatusIndex()
		{
			return View();
		}
       public IActionResult ApproveSelfStatusIndex()
		{
			return View();
		}

        public IActionResult _approveStatusIndex(string status)
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                index.UserName = userName;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["BranchCode"].ToString();
                string branch = Request.Form["BranchName"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();


                string code = Request.Form["code"].ToString();
                string teamname = Request.Form["teamname"].ToString();
                string description = Request.Form["description"].ToString();
                string approveStatus = Request.Form["approveStatus"].ToString();


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



                string[] conditionalFields = null;
                string?[] conditionalValue = null;

                if (status == "self")
                {
                    conditionalFields = new[]

                    {
                            "TransportAllownaces.Code like",
                            "TransportAllownaces.TeamName like",
                            "TransportAllownaces.Description like",

                            "TransportAllownaces.IsPost",
                            "TransportAllownaces.CreatedBy",
                     };

                    conditionalValue = new[] { code, teamname, description, "Y", index.createdBy };
                }

                else
                {
                    conditionalFields = new[]
                    {
                           "TransportAllownaces.Code like",
                            "TransportAllownaces.TeamName like",
                            "TransportAllownaces.Description like",

                            "TransportAllownaces.IsPost"
                    };

                    conditionalValue = new[] { code, teamname, description, "Y" };
                }

                //string[] conditionalFields = new[]
                //{
                //            "TransportAllownaces.Code like",
                //            "TransportAllownaces.TeamName like",
                //            "TransportAllownaces.Description like",

                //            "TransportAllownaces.IsPost"

                //};

                //string?[] conditionalValue = new[] { code, teamname, description, "Y" };

                ResultModel<List<TransportAllownaces>> indexData =
                    _transportAllownacesService.GetIndexDataStatus(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _transportAllownacesService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _transportAllownacesService.GetCount(TableName.TransportAllownaces, "Id", new[] { "TransportAllownaces.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<TransportAllownaces>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }

        public IActionResult _approveSelfStatusIndex()
		{
			try
			{
				IndexModel index = new IndexModel();
				string userName = User.Identity.Name;
				index.UserName = userName;
				ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

				var search = Request.Form["search[value]"].FirstOrDefault();
				string users = Request.Form["BranchCode"].ToString();
				string branch = Request.Form["BranchName"].ToString();
				string Address = Request.Form["Address"].ToString();
				string TelephoneNo = Request.Form["TelephoneNo"].ToString();


				string code = Request.Form["code"].ToString();
				string teamname = Request.Form["teamname"].ToString();
				string description = Request.Form["description"].ToString();
				string approveStatus = Request.Form["approveStatus"].ToString();


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
                            "TransportAllownaces.Code like",
                            "TransportAllownaces.TeamName like",
                            "TransportAllownaces.Description like",

                            "TransportAllownaces.IsPost"

                };

				string?[] conditionalValue = new[] { code, teamname, description, "Y" };

				ResultModel<List<TransportAllownaces>> indexData =
					_transportAllownacesService.GetIndexDataSelfStatus(index, conditionalFields, conditionalValue);



				ResultModel<int> indexDataCount =
				_transportAllownacesService.GetIndexDataCount(index, conditionalFields, conditionalValue);


				int result = _transportAllownacesService.GetCount(TableName.TransportAllownaces, "Id", new[] { "TransportAllownaces.createdBy", }, new[] { userName });


				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<TransportAllownaces>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}


		}

		public ActionResult Create()
        {
            ModelState.Clear();
            TransportAllownaces vm = new TransportAllownaces();
            vm.Operation = "add";
            return View("CreateEdit", vm);
        }

        public ActionResult CreateEdit(TransportAllownaces master)
        {
            ResultModel<TransportAllownaces> result = new ResultModel<TransportAllownaces>();
            try
            {

               if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _transportAllownacesService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _transportAllownacesService.Insert(master);

                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }


        //public ActionResult Edit(int id)
        public ActionResult Edit(int id, string edit = "audit")   
        {
            try
            {
                ResultModel<List<TransportAllownaces>> result =
                    _transportAllownacesService.GetAll(new[] { "ta.Id" }, new[] { id.ToString() });

                TransportAllownaces transportAllownaces = result.Data.FirstOrDefault();
                transportAllownaces.Operation = "update";
				transportAllownaces.Edit = edit;

				transportAllownaces.Id = id;

                return View("CreateEdit", transportAllownaces);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }


        //Report
        public ActionResult TransportAllownaceReport(int id)
        {
            try
            {
                ResultModel<List<TransportAllownaces>> result =
                   _transportAllownacesService.GetAll(new[] { "ta.Id" }, new[] { id.ToString() });

                TransportAllownaces transportAllownaces = result.Data.FirstOrDefault();
                transportAllownaces.Operation = "update";             
                transportAllownaces.Id = id;
				transportAllownaces.Edit = "TransportReport";


                return View("CreateEdit", transportAllownaces);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }
        public IActionResult TransportReport(int id)
        {

            ResultModel<List<TransportAllownaces>> result = _transportAllownacesService.GetAll(new[] { "ta.Id" }, new[] { id.ToString() });
            List<string> str = new List<string>();

            ResultModel<List<UserProfileAttachments>> data = _teamMembersService.GetUserProfileName(result.Data.FirstOrDefault().TeamId);
            string FullName = "";
            string FullDesignation = "";
            int count = data.Data.Count();
            int i = 0;
            foreach (var item in data.Data)
            {
                FullName = FullName + item.ProfileName;
                i++;
                if (i != count)
                {
                    FullName = FullName + "/";
                }
            }
            i = 0;
            foreach (var designation in data.Data)
            {
                FullDesignation = FullDesignation + designation.Designation;
                i++;
                if (i != count)
                {
                    FullDesignation = FullDesignation + "/";
                }
            }


            DateTime fromdate = Convert.ToDateTime(result.Data.FirstOrDefault().ArrivalDate);
            DateTime todate = Convert.ToDateTime(result.Data.FirstOrDefault().DepartureDate);
            TimeSpan difference = fromdate.AddDays(1)-todate;
            int daysCount = difference.Days;


            TransportAllownaces transport = result.Data.FirstOrDefault();
            decimal LessAdvanceTotalVlaue = 0;
            decimal LumpsumTotalVlaue = 0;     
            decimal TotalVlaue = 0;
            transport.Name = FullName;
            transport.Designation = FullDesignation;
            transport.DayCount = daysCount;
           
            foreach(var item in transport.TransportAllownaceDetails)
                                        {
                TotalVlaue = TotalVlaue + item.Amount;
            }
            foreach(var item in transport.TransportFoodAndOthersDetails)
                                        {
                LumpsumTotalVlaue = LumpsumTotalVlaue + item.Amount;
            }                     
            decimal grossTotal = TotalVlaue + LumpsumTotalVlaue;       
            foreach (var item in transport.LessAdvanceDetails)
            {
                LessAdvanceTotalVlaue = LessAdvanceTotalVlaue + item.Amount;
            }
            decimal receiptVisitor = grossTotal - LessAdvanceTotalVlaue;
            if(receiptVisitor > 0)
            {
                string WordValue =NumberToWord.ConvertNumberToString(Convert.ToInt32(receiptVisitor));
                transport.Word = WordValue;

            }
            else
            {

                decimal positiveNumber = Math.Abs(receiptVisitor);
                transport.Word = "Negavite ";
                transport.Word = transport.Word + NumberToWord.ConvertNumberToString(Convert.ToInt32(positiveNumber));
                
            }

            return View(transport);


        }
        public IActionResult TransportLocalConveyanceReport(int id)
        {

            ResultModel<List<TransportAllownaces>> result = _transportAllownacesService.GetAll(new[] { "ta.Id" }, new[] { id.ToString() });

            List<string> str = new List<string>();
            TransportAllownaces transport = result.Data.FirstOrDefault();
            NumberToWord ntw =new NumberToWord();
            string val = NumberToWord.ConvertNumberToString(0012);

            return View(transport);


        }

   
        //End

        public ActionResult TADAReportView(int id)
        {
            try
            {
                ResultModel<List<TransportAllownaces>> result =
                    _transportAllownacesService.GetTADAForReports(new[] { "TA.Id" }, new[] { id.ToString() });

                TransportAllownaces transport = result.Data.FirstOrDefault();
                transport.Operation = "update";
                transport.Id = id;

                return View(transport);
                
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

        public ActionResult MultiplePost(TransportAllownaces master)
		{
			ResultModel<TransportAllownaces> result = new ResultModel<TransportAllownaces>();

			try
			{

				foreach (string ID in master.IDs)
				{


					string userName = User.Identity.Name;
					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.PostedBy = user.UserName;
					master.Audit.PostedOn = DateTime.Now;
					master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

					result = _transportAllownacesService.MultiplePost(master);


				}




				return Ok(result);


			}
			catch (Exception ex)
			{
				ex.LogAsync(ControllerContext.HttpContext);
			}

			return Ok("");
		}

		public ActionResult MultipleUnPost(TransportAllownaces master)
		{
			ResultModel<TransportAllownaces> result = new ResultModel<TransportAllownaces>();

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

					result = _transportAllownacesService.MultipleUnPost(master);
				}


				return Ok(result);
			}
			catch (Exception ex)
			{
				ex.LogAsync(ControllerContext.HttpContext);
			}

			return Ok("");
		}


		public ActionResult MultipleReject(TransportAllownaces master)
		{
			ResultModel<TransportAllownaces> result = new ResultModel<TransportAllownaces>();

			try
			{

				foreach (string ID in master.IDs)
				{

					string userName = User.Identity.Name;
					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Approval.IsRejected = true;
					master.Approval.RejectedBy = user.UserName;
					master.Approval.RejectedDate = DateTime.Now;
					master.Operation = "reject";

					result = _transportAllownacesService.MultipleUnPost(master);

				}


				return Ok(result);
			}
			catch (Exception ex)
			{
				ex.LogAsync(ControllerContext.HttpContext);
			}

			return Ok("");
		}


		public ActionResult MultipleApproved(TransportAllownaces master)
		{
			ResultModel<TransportAllownaces> result = new ResultModel<TransportAllownaces>();

			try
			{

				foreach (string ID in master.IDs)
				{

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    master.Operation = "approved";

					result = _transportAllownacesService.MultipleUnPost(master);
				}


				return Ok(result);
			}
			catch (Exception ex)
			{
				ex.LogAsync(ControllerContext.HttpContext);
			}

			return Ok("");
		}

	}
}
