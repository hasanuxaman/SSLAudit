using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.CISReport;
using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class DateWisePolicyEditLogController : Controller
    {

        private readonly ApplicationDbContext _applicationDb;
        private readonly IDateWisePolicyEditLogService _dateWisePolicyEditLogService;



        public DateWisePolicyEditLogController(ApplicationDbContext applicationDb, IDateWisePolicyEditLogService dateWisePolicyEditLogService)
        {

            _applicationDb = applicationDb;
            _dateWisePolicyEditLogService = dateWisePolicyEditLogService;

        }


        //DateWisePolicyEditLog
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {

            ModelState.Clear();
            DateWisePolicyEditLog vm = new DateWisePolicyEditLog();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }

        public ActionResult CreateEdit(DateWisePolicyEditLog master)
        {
            ResultModel<DateWisePolicyEditLog> result = new ResultModel<DateWisePolicyEditLog>();
            try
            {

                if (master.Operation == "update")
                {
                    foreach (var item in master.DateWisePolicyEditLogDetails)
                    {
                        item.Id = master.Id;
                        string userName = User.Identity.Name;
                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
						item.Audit.LastUpdateBy = user.UserName;
						item.Audit.LastUpdateOn = DateTime.Now;
						item.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                        result = _dateWisePolicyEditLogService.Update(item);
                    }
                    return Ok(result);
                }
                else
                {

                    foreach (var item in master.DateWisePolicyEditLogDetails)
                    {
                        string userName = User.Identity.Name;

                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
						item.Audit.CreatedBy = user.UserName;
						item.Audit.CreatedOn = DateTime.Now;
						item.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();


                        result = _dateWisePolicyEditLogService.Insert(item);
                    }

                    result.Data.Operation = "add";


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
                ResultModel<List<DateWisePolicyEditLog>> result =
					_dateWisePolicyEditLogService.GetAll(new[] { "Id" }, new[] { id.ToString() });

				DateWisePolicyEditLog pre = result.Data.FirstOrDefault();
                pre.Operation = "update";


                pre.Id = id;

                return View("CreateEdit", pre);
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

                ResultModel<List<DateWisePolicyEditLog>> indexData =
					_dateWisePolicyEditLogService.GetIndexData(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
				_dateWisePolicyEditLogService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _dateWisePolicyEditLogService.GetCount(TableName.DateWisePolicyEditLog, "Id", new[] { "DateWisePolicyEditLog.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<DateWisePolicyEditLog>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }

        //End DateWisePolicyEditLog

        

    }
}
