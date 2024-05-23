using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Node;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Models.AuditModule;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class NodeController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly INodeService _nodeService;

        public NodeController(ApplicationDbContext applicationDb, ITeamsService teamsService, IAdvancesService advancesService
            , INodeService nodeService
            
            )
        {

            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _nodeService = nodeService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            ModelState.Clear();
            SubmanuList vm = new SubmanuList();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(SubmanuList master)
        {
            ResultModel<SubmanuList> result = new ResultModel<SubmanuList>();
            try
            {
                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _nodeService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _nodeService.Insert(master);

                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }


        [HttpGet]
        public ActionResult GetNodeInfo(string nodeId)
        {

            SubmanuList? node = _nodeService.GetNodeById(nodeId);
            node.Url = node.Url;
            node.Node = node.Node;
            node.ActionName = node.ActionName;
            node.ControllerName = node.ControllerName;       
            return Ok(new { node });

        }

        public ActionResult Edit(int id, string edit = "audit")
		{
			try
            {
                ResultModel<List<SubmanuList>> result =_nodeService.GetAll(new[] { "np.Id" }, new[] { id.ToString() });
                SubmanuList sub = result.Data.FirstOrDefault();
                sub.Operation = "update";
                sub.Id = id;
                sub.NodeName = sub.Node;
                sub.UserId = sub.UserId;
                sub.NodeName = sub.NodeId;

                return View("CreateEdit", sub);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }
        public ActionResult<IList<SubmanuList>> Delete(SubmanuList master)
        {
            try
            {
                ResultModel<SubmanuList> result = _nodeService.Delete(master.Id);
                return Ok(result);

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
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

                string Name = Request.Form["userName"].ToString();
                string node = Request.Form["node"].ToString();


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
                //var orderName = "desc";

                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();

                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();

                index.OrderName = "Id";

                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);


                index.createdBy = userName;


                string[] conditionalFields = new[]
                {
                            "UserId like",
                            "Node like",
                            "Description like",
                            "IsPost like"
                };

                string?[] conditionalValue = new[] { Name, node, description, post };

                ResultModel<List<SubmanuList>> indexData =
                    _nodeService.GetIndexData(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _nodeService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _nodeService.GetCount(TableName.NodePermission, "Id", new[] { "NodePermission.Id", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }





    }
}
