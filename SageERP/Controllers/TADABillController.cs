using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.CISReport;
using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.TransportAllownaceDetails;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;
using System;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class TADABillController : Controller
    {

        private readonly ApplicationDbContext _applicationDb;
        private readonly IDateWisePolicyEditLogService _dateWisePolicyEditLogService;
        private readonly ICISReportService _cisReportService;
        private readonly ITransportAllownaceDetailService _transportAllownaceDetailService;




        public TADABillController(ApplicationDbContext applicationDb, IDateWisePolicyEditLogService dateWisePolicyEditLogService, ICISReportService cisReportService,
            ITransportAllownaceDetailService transportAllownaceDetailService
            )
        {

            _applicationDb = applicationDb;
            _dateWisePolicyEditLogService = dateWisePolicyEditLogService;
            _cisReportService = cisReportService;
            _transportAllownaceDetailService = transportAllownaceDetailService;

        }

        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {

            ModelState.Clear();
            TransportAllownaceDetail vm = new TransportAllownaceDetail();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }

        public ActionResult CreateEdit(TransportAllownaceDetail master)
        {
            ResultModel<TransportAllownaceDetail> result = new ResultModel<TransportAllownaceDetail>();
            try
            {

                if (master.Operation == "update")
                {
                    foreach (var item in master.TADABillDetails)
                    {
                        item.Id = master.Id;
                        string userName = User.Identity.Name;
                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
						item.Audit.LastUpdateBy = user.UserName;
						item.Audit.LastUpdateOn = DateTime.Now;
						item.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
						result = _transportAllownaceDetailService.Update(item);
					}
					return Ok(result);
                }
                else
                {

                    foreach (var item in master.TADABillDetails)
                    {
                        string userName = User.Identity.Name;

                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
						item.Audit.CreatedBy = user.UserName;
						item.Audit.CreatedOn = DateTime.Now;
						item.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();


                        result = _transportAllownaceDetailService.Insert(item);
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
                ResultModel<List<TransportAllownaceDetail>> result =
                    _transportAllownaceDetailService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                TransportAllownaceDetail tada = result.Data.FirstOrDefault();
                tada.Operation = "update";
                tada.Id = id;
                tada.Edit = "Close";

                return View("CreateEdit", tada);
               
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
                            "MRNo like"
                            
                };

                string?[] conditionalValue = new[] { mrNo };

                ResultModel<List<TransportAllownaceDetail>> indexData =
                    _transportAllownaceDetailService.GetIndexData(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _transportAllownaceDetailService.GetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _transportAllownaceDetailService.GetCount(TableName.TADABill, "Id", new[] { "TADABill.createdBy", }, new[] { userName });


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
