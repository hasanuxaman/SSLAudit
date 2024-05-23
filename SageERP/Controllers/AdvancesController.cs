using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class AdvancesController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;

        public AdvancesController(ApplicationDbContext applicationDb, ITeamsService teamsService, IAdvancesService advancesService)
        {

            _applicationDb = applicationDb;
            _advancesService = advancesService;

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
				string advanceAmount = Request.Form["advanceAmount"].ToString();
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
							"AdvanceAmount like",
							"Description like",
							"IsPost like"
				};

				string?[] conditionalValue = new[] { code, advanceAmount, description, post };

				ResultModel<List<Advances>> indexData =
					_advancesService.GetIndexData(index, conditionalFields, conditionalValue);



				ResultModel<int> indexDataCount =
				_advancesService.GetIndexDataCount(index, conditionalFields, conditionalValue);


				int result = _advancesService.GetCount(TableName.Advances, "Id", new[] { "Advances.createdBy", }, new[] { userName });


				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}


		}


		public IActionResult ApproveStatusIndex()
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
				string advanceAmount = Request.Form["advanceAmount"].ToString();
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
                            "Advances.Code like",
                            "Advances.AdvanceAmount like",
                            "Advances.Description like",
                            "Advances.IsPost",
                            "Advances.CreatedBy",
                     };

                    conditionalValue = new[] { code, advanceAmount, description, "Y", index.createdBy };
                }

                else
                {
                    conditionalFields = new[]
                    {
                            "Advances.Code like",
                            "Advances.AdvanceAmount like",
                            "Advances.Description like",
                            "Advances.IsPost"
                    };

                    conditionalValue = new[] { code, advanceAmount, description, "Y" };
                }


    //            string[] conditionalFields = new[]
				//{
    //                        "Advances.Code like",
    //                        "Advances.AdvanceAmount like",
    //                        "Advances.Description like",
				//			   "Advances.IsPost"
				//};

				//string?[] conditionalValue = new[] { code, advanceAmount, description, "Y" };

				ResultModel<List<Advances>> indexData =
					_advancesService.GetIndexDataStatus(index, conditionalFields, conditionalValue);



				ResultModel<int> indexDataCount =
				_advancesService.GetIndexDataCount(index, conditionalFields, conditionalValue);


				int result = _advancesService.GetCount(TableName.Advances, "Id", new[] { "Advances.createdBy", }, new[] { userName });


				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}


		}

        public IActionResult ApproveSelfStatusIndex()
        {
            return View();
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
                string advanceAmount = Request.Form["advanceAmount"].ToString();
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
                            "Advances.Code like",
                            "Advances.AdvanceAmount like",
                            "Advances.Description like",
                            "Advances.IsPost"
                };

                string?[] conditionalValue = new[] { code, advanceAmount, description, "Y" };

                ResultModel<List<Advances>> indexData =
                    _advancesService.GetIndexDataSelfStatus(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _advancesService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _advancesService.GetCount(TableName.Advances, "Id", new[] { "Advances.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }

        public ActionResult Create()
        {

            ModelState.Clear();
            Advances vm = new Advances();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(Advances master)
        {
            ResultModel<Advances> result = new ResultModel<Advances>();
            try
            {



                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _advancesService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _advancesService.Insert(master);

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
                ResultModel<List<Advances>> result =
                    _advancesService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                Advances advances = result.Data.FirstOrDefault();
                advances.Operation = "update";
				advances.Edit = edit;

				advances.Id = id;

                return View("CreateEdit", advances);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }



        public ActionResult MultiplePost(Advances master)
        {
            ResultModel<Advances> result = new ResultModel<Advances>();

            try
            {

                foreach (string ID in master.IDs)
                {

                    //result = _receiptMaserService.GetBranchIds(ID);
                    //master.BranchId = result.Data.BranchId;
                    //#region  BranchCheckWithCurrentBranch 
                    //string CurentBranchid = User.GetCurrentBranchId();
                    //if (string.IsNullOrEmpty(CurentBranchid))
                    //{
                    //    result.Success = false;
                    //    result.Status = Status.Warning;
                    //    result.Message = "Branch Not Assign for this user,Please assign branch first";
                    //    return Ok(result);
                    //}
                    //if (CurentBranchid != master.BranchId.ToString())
                    //{
                    //    result.Success = false;
                    //    result.Status = Status.Warning;
                    //    result.Message = "This transaction doesn't belong to current branch";
                    //    return Ok(result);
                    //}
                    //#endregion




                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _advancesService.MultiplePost(master);


                }




                return Ok(result);


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

		public ActionResult MultipleUnPost(Advances master)
		{
			ResultModel<Advances> result = new ResultModel<Advances>();

			try
			{

				foreach (string ID in master.IDs)
				{

					//result = _receiptMaserService.GetBranchIds(ID);

					//master.BranchId = result.Data.BranchId;


					//#region  BranchCheckWithCurrentBranch 
					//string CurentBranchid = User.GetCurrentBranchId();


					//if (string.IsNullOrEmpty(CurentBranchid))
					//{
					//	result.Success = false;
					//	result.Status = Status.Warning;
					//	result.Message = "Branch Not Assign for this user,Please assign branch first";
					//	return Ok(result);
					//}


					//if (CurentBranchid != master.BranchId.ToString())
					//{
					//	result.Success = false;
					//	result.Status = Status.Warning;
					//	result.Message = "This transaction doesn't belong to current branch";
					//	return Ok(result);
					//}
					//#endregion

					string userName = User.Identity.Name;
					ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
					master.Audit.PostedBy = user.UserName;
					master.Audit.PostedOn = DateTime.Now;
					master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
					master.Operation = "unpost";


					result = _advancesService.MultipleUnPost(master);
				}
				

				return Ok(result);
			}
			catch (Exception ex)
			{
				ex.LogAsync(ControllerContext.HttpContext);
			}

			return Ok("");
		}


		public ActionResult MultipleReject(Advances master)
		{
			ResultModel<Advances> result = new ResultModel<Advances>();

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

					result = _advancesService.MultipleUnPost(master);

				}


				return Ok(result);
			}
			catch (Exception ex)
			{
				ex.LogAsync(ControllerContext.HttpContext);
			}

			return Ok("");
		}


		public ActionResult MultipleApproved(Advances master)
		{
			ResultModel<Advances> result = new ResultModel<Advances>();

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

					result = _advancesService.MultipleUnPost(master);
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
