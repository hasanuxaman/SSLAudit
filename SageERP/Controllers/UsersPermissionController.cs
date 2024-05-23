using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.ModulePermissions;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.UsersPermission;
using Shampan.Models;
using Shampan.Models.AuditModule;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class UsersPermissionController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IUsersPermissionService _usersPermissionService;
        private readonly IModulePermissionService _modulePermissionService;

        public UsersPermissionController(ApplicationDbContext applicationDb, ITeamsService teamsService,
            IAdvancesService advancesService, IUsersPermissionService usersPermissionService, IModulePermissionService modulePermissionService)
        {

            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _usersPermissionService = usersPermissionService;
            _modulePermissionService = modulePermissionService;

        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            ModelState.Clear();
            Users vm = new Users();

            ResultModel<List<ModulePermission>> indexData = _modulePermissionService.GetModulListData(null, null, null);
            vm.ModuleList = indexData.Data;
            vm.Operation = "add";
            return View("CreateEdit", vm);

        }



        [HttpPost]
        public ActionResult CreateEdit(Users master)
        {

            ResultModel<Users> result = new ResultModel<Users>();
            DataTable dt5 = new DataTable();
            List<Users> VMs = new List<Users>();

            try
            {
                if (master.Operation == "update")
                {
                    foreach (var module in master.ModuleList)
                    {
                        string userName = User.Identity.Name;
                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                        master.Audit.LastUpdateBy = user.UserName;
                        master.Audit.LastUpdateOn = DateTime.Now;
                        master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                        master.IsModuleActive = module.IsActive;
                        master.Id = module.UserPermissionId;

                        result = _usersPermissionService.Update(master);

                    }
                    foreach (var sumManu in master.NodeList)
                    {
                        string userName = User.Identity.Name;
                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);                     
                        //master.IsModuleActive = module.IsActive;
                        //master.Id = module.UserPermissionId;
                        SubmanuList sub = new SubmanuList();
                        sub.Id = sumManu.Id;
                        sub.IsAllowByUser = sumManu.IsAllowByUser;

                        sub.Audit.LastUpdateBy = user.UserName;
                        sub.Audit.LastUpdateOn = DateTime.Now;
                        sub.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();


                        _usersPermissionService.UpdateNodes(sub);
                    }


                    return Ok(result);
                }
                else
                {

                    foreach (var module in master.ModuleList)
                    {

                        string userName = User.Identity.Name;
                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                        master.Audit.CreatedBy = user.UserName;
                        master.Audit.CreatedOn = DateTime.Now;
                        master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                        master.Module = module;
                        result = _usersPermissionService.Insert(master);

                        if(result.Status == Status.Fail)
                        {
                            break;
                        }

                    }

                    //master.RoleValue = master.R1Value;
                    //string userName = User.Identity.Name;
                    //ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    //master.Audit.CreatedBy = user.UserName;
                    //master.Audit.CreatedOn = DateTime.Now;
                    //master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    //result = _usersPermissionService.Insert(master);

                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<Users>> result =_usersPermissionService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                //Users UserPermission = result.Data.FirstOrDefault();
                Users UserPermission = new Users();
                UserPermission.UserId = result.Data.FirstOrDefault().UserId;

                foreach (var ur in result.Data)
                {
                    ModulePermission Mp = new ModulePermission();
                    Mp.Id = ur.ModuleID;
                    Mp.IsActive = ur.IsModuleActive;
                    Mp.UserName = ur.UserName;
                    Mp.UserPermissionId = ur.Id;
                    Mp.Modul = ur.Modul;

                    UserPermission.ModuleList.Add(Mp);
                }


                UserPermission.Operation = "update";
                UserPermission.Id = id;

                return View("CreateEdit", UserPermission);
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
                string code = Request.Form["userName"].ToString();
                string advanceAmount = Request.Form["advanceAmount"].ToString();
                string description = Request.Form["description"].ToString();
                string post = Request.Form["ispost"].ToString();
                string un = Request.Form["username"].ToString();

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
                            "UserName like"

                };

                string?[] conditionalValue = new[] { un };

                ResultModel<List<Users>> indexData =
                    _usersPermissionService.GetIndexData(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _usersPermissionService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _usersPermissionService.GetCount(TableName.Users, "Id", new[] { "Users.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }

        }

        public IActionResult _indexNodes(string edit,string UserName)
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
                string code = Request.Form["userName"].ToString();
                string advanceAmount = Request.Form["advanceAmount"].ToString();
                string description = Request.Form["description"].ToString();
                string post = Request.Form["ispost"].ToString();
                string un = Request.Form["username"].ToString();
                index.ModuleId = edit;
                index.UserName = UserName;

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
                            "UserName like"

                };

                string?[] conditionalValue = new[] { un };

                ResultModel<List<SubmanuList>> indexData =
                    _usersPermissionService.GetNodesIndexData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                _usersPermissionService.GetNodesIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _usersPermissionService.GetCount(TableName.Users, "Id", new[] { "Users.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }

        }



        public IActionResult PermissionTreeIndex()
        {

            ViewBag.Message = "Welcome!";

            //var model = GetTreeData();
            // string jsonModel = new JavaScriptSerializer().Serialize(model);
            //return View("Index", "_Layout", jsonModel);

            return View();
        }

        [HttpPost]
        public IActionResult GetTreeData1()
        {
            // Fetch and return your tree data here
            //var treeData = GetTreeDataFromDatabase(); // You need to implement this method

            //return Json(treeData);
            var tree = new JsTreeModel[] { };
            return Json(tree);
        }

        private JsTreeModel[] GetTreeData()
        {
            var tree = new JsTreeModel[]
            {
    new JsTreeModel { data = "Confirm Application", attr = new JsTreeAttribute { id = "10", selected = true } },


    new JsTreeModel
    {
        data = "Things",
        attr = new JsTreeAttribute { id = "20" },
        children = new JsTreeModel[]
            {
                new JsTreeModel { data = "Thing 1", attr = new JsTreeAttribute { id = "21", selected = true } },
                new JsTreeModel { data = "Thing 2", attr = new JsTreeAttribute { id = "22" } },
                new JsTreeModel { data = "Thing 3", attr = new JsTreeAttribute { id = "23" } },
                new JsTreeModel
                {
                    data = "Thing 4",
                    attr = new JsTreeAttribute { id = "24" },
                    children = new JsTreeModel[]
                    {
                        new JsTreeModel { data = "Thing 4.1", attr = new JsTreeAttribute { id = "241" } },
                        new JsTreeModel { data = "Thing 4.2", attr = new JsTreeAttribute { id = "242" } },
                        new JsTreeModel { data = "Thing 4.3", attr = new JsTreeAttribute { id = "243" } }
                    },
                },
            }
    },

    new JsTreeModel
    {
        data = "Colors",
        attr = new JsTreeAttribute { id = "40" },
        children = new JsTreeModel[]
            {
                new JsTreeModel { data = "Red", attr = new JsTreeAttribute { id = "41" } },
                new JsTreeModel { data = "Green", attr = new JsTreeAttribute { id = "42" } },
                new JsTreeModel { data = "Blue", attr = new JsTreeAttribute { id = "43" } },
                new JsTreeModel { data = "Yellow", attr = new JsTreeAttribute { id = "44" } },
            }
    }
            };

            return tree;
        }



    }




    public class JsTreeModel
    {
        public string data;
        public JsTreeAttribute attr;
        public JsTreeModel[] children;
    }

    public class JsTreeAttribute
    {
        public string id;
        public bool selected;
    }




}
