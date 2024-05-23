using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Core.Interfaces.Services.Company;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers;
using StackExchange.Exceptional;

namespace SageERP.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]

	public class CompanyInfoController : Controller
    {
        
        private readonly ApplicationDbContext _applicationDb;
        private readonly ICompanyInfosService _companyInfosService;

        public CompanyInfoController(ApplicationDbContext applicationDb, ICompanyInfosService companyInfosService)
        {
            
            _applicationDb = applicationDb;
            _companyInfosService = companyInfosService;


        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            ModelState.Clear();
            Companyinfos vm = new Companyinfos();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(Companyinfos master)
        {
            ResultModel<Companyinfos> result = new ResultModel<Companyinfos>();
            try
            {
               


                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastModifiedBy = user.UserName;
                    master.Audit.LastModifiedOn = DateTime.Now;
                    

                    result = _companyInfosService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _companyInfosService.Insert(master);

                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }

        public IActionResult _index()
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["CompanyName"].ToString();
                string City = Request.Form["City"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();

   
              
                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();

                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();

                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();

                index.OrderName = "CompanyID";

                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);


                index.createdBy = userName;


                string[] conditionalFields = new[]
                                                        {

                                "CompanyName like",
                            "City like",

                            "Address like",
                            "TelephoneNo like",

                             "CompanyName like",
                            "City like",

                            "Address like",
                            "TelephoneNo like"


                        };
                string?[] conditionalValue = new[] { search,search,search,search, users, City, Address , TelephoneNo };
                ResultModel<List<Companyinfos>> indexData =
                    _companyInfosService.GetIndexData(index,
                        conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                    _companyInfosService.GetIndexDataCount(index,
                       conditionalFields, conditionalValue);

                int result = _companyInfosService.GetCount(TableName.CompanyInfo, "ComapanyID", new[]
                        {
                            "CompanyInfo.createdBy",
                        }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Companyinfos>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }

        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<Companyinfos>> result =
                    _companyInfosService.GetAll(new[] { "CompanyID" }, new[] { id.ToString() });

                Companyinfos companyinfos = result.Data.FirstOrDefault();
                companyinfos.Operation = "update";
                companyinfos.CompanyID = id;
                
                return View("CreateEdit", companyinfos);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

    }
}
