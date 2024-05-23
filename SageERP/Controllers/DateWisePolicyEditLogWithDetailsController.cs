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
    public class DateWisePolicyEditLogWithDetailsController : Controller
    {

        private readonly ApplicationDbContext _applicationDb;
        private readonly IDateWisePolicyEditLogService _dateWisePolicyEditLogService;
        private readonly IDateWisePolicyEditLogWithDetailsService _dateWisePolicyEditLogWithDetailsService;



        public DateWisePolicyEditLogWithDetailsController(ApplicationDbContext applicationDb, IDateWisePolicyEditLogService dateWisePolicyEditLogService,IDateWisePolicyEditLogWithDetailsService dateWisePolicyEditLogWithDetailsService)
        {

            _applicationDb = applicationDb;
            _dateWisePolicyEditLogService = dateWisePolicyEditLogService;
			_dateWisePolicyEditLogWithDetailsService = dateWisePolicyEditLogWithDetailsService;

        }


        
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {

            ModelState.Clear();
			DateWisePolicyEditLogWithDetails vm = new DateWisePolicyEditLogWithDetails();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }

        public ActionResult CreateEdit(DateWisePolicyEditLogWithDetails master)
        {
            ResultModel<DateWisePolicyEditLogWithDetails> result = new ResultModel<DateWisePolicyEditLogWithDetails>();
            try
            {

                if (master.Operation == "update")
                {
                    foreach (var item in master.DateWisePolicyEditLogDetailsData)
                    {
                        item.Id = master.Id;
                        string userName = User.Identity.Name;
                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
						item.Audit.LastUpdateBy = user.UserName;
						item.Audit.LastUpdateOn = DateTime.Now;
						item.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                        result = _dateWisePolicyEditLogWithDetailsService.Update(item);
                    }
                    return Ok(result);
                }
                else
                {

                    foreach (var item in master.DateWisePolicyEditLogDetailsData)
                    {
                        string userName = User.Identity.Name;

                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
						item.Audit.CreatedBy = user.UserName;
						item.Audit.CreatedOn = DateTime.Now;
						item.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();


                        result = _dateWisePolicyEditLogWithDetailsService.Insert(item);
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
                ResultModel<List<DateWisePolicyEditLogWithDetails>> result =
					_dateWisePolicyEditLogWithDetailsService.GetAll(new[] { "Id" }, new[] { id.ToString() });

				DateWisePolicyEditLogWithDetails pre = result.Data.FirstOrDefault();
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

                ResultModel<List<DateWisePolicyEditLogWithDetails>> indexData =
					_dateWisePolicyEditLogWithDetailsService.GetIndexData(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
				_dateWisePolicyEditLogWithDetailsService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _dateWisePolicyEditLogWithDetailsService.GetCount(TableName.DateWisePolicyEditLog, "Id", new[] { "DateWisePolicyEditLogWithDetails.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<DateWisePolicyEditLogWithDetails>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }



        

    }
}
