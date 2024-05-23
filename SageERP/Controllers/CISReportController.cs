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
using System;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class CISReportController : Controller
    {

        private readonly ApplicationDbContext _applicationDb;
        private readonly IDateWisePolicyEditLogService _dateWisePolicyEditLogService;
        private readonly ICISReportService _cisReportService;




        public CISReportController(ApplicationDbContext applicationDb, IDateWisePolicyEditLogService dateWisePolicyEditLogService, ICISReportService cisReportService)
        {

            _applicationDb = applicationDb;
            _dateWisePolicyEditLogService = dateWisePolicyEditLogService;
            _cisReportService = cisReportService;

        }


        //MRWiseChangeLog
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {

            ModelState.Clear();
            MRWiseChangeLog vm = new MRWiseChangeLog();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }

        public ActionResult CreateEdit(MRWiseChangeLog master)
        {
            ResultModel<MRWiseChangeLog> result = new ResultModel<MRWiseChangeLog>();
            try
            {

                if (master.Operation == "update")
                {
                    foreach (var item in master.MRWiseChangeLogDetails)
                    {
                        item.Id = master.Id;
                        string userName = User.Identity.Name;
                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
						item.Audit.LastUpdateBy = user.UserName;
						item.Audit.LastUpdateOn = DateTime.Now;
						item.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                        result = _cisReportService.Update(item);
                    }
                    return Ok(result);
                }
                else
                {

                    foreach (var item in master.MRWiseChangeLogDetails)
                    {
                        string userName = User.Identity.Name;

                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
						item.Audit.CreatedBy = user.UserName;
						item.Audit.CreatedOn = DateTime.Now;
						item.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();


                        result = _cisReportService.Insert(item);
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
                ResultModel<List<MRWiseChangeLog>> result =
					_cisReportService.GetAll(new[] { "Id" }, new[] { id.ToString() });

				MRWiseChangeLog mrWiseChangeLog = result.Data.FirstOrDefault();
                mrWiseChangeLog.Operation = "update";
                mrWiseChangeLog.Id = id;
                mrWiseChangeLog.Edit = "Close";

                return View("CreateEdit", mrWiseChangeLog);
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

                string mrNo = Request.Form["mrNo"].ToString();
                string pcNo = Request.Form["pcNo"].ToString();
                string userId = Request.Form["userId"].ToString();
                string editDate = Request.Form["editDate"].ToString();
                string status = Request.Form["status"].ToString();
                string mrNet = Request.Form["mrNet"].ToString();
                string mrVat = Request.Form["mrVat"].ToString();
                string mrStamp = Request.Form["mrStamp"].ToString();
                string mrCoinsPayable = Request.Form["mrCoinsPayable"].ToString();
                string mrDateTime = Request.Form["mrDateTime"].ToString();


                //if (post == "Select")
                //{
                //    post = "";
                //}





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
                            "MRNo like",
                            "PCNo like",
                            "UserId like",
                            "EditDate like",
                            "Status like",
                            "MRNet like",
                            "MRVat like",
                            "MRStamp like",
                            "MRCoinsPayable like",
                            "MRDateTime like"
                };

                string?[] conditionalValue = new[] { mrNo, pcNo, userId, editDate, status, mrNet, mrVat, mrStamp , mrCoinsPayable, mrDateTime };

                ResultModel<List<MRWiseChangeLog>> indexData =
					_cisReportService.GetIndexData(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
				_cisReportService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _cisReportService.GetCount(TableName.MRWiseChangeLog, "Id", new[] { "MRWiseChangeLog.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }

        //End MRWiseChangeLog

        

    }
}
