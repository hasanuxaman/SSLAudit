using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.User;
using Shampan.Core.Interfaces.Services;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;
using Shampan.Core.Interfaces.Services.TeamMember;
using Shampan.Services.User;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]

	public class TeamMembersController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly ICommonService _commonService;
        private readonly ITeamMembersService _teamMembersService;

        public TeamMembersController(ApplicationDbContext applicationDb,ICommonService commonService, ITeamMembersService teamMembersService)
        {

            _applicationDb = applicationDb;
            _commonService = commonService;
            _teamMembersService = teamMembersService;

        }
        public IActionResult Index(string id)
        {
            ViewBag.MemberId = id;
            TeamMembers team = new TeamMembers();
            team.memberId = id;
            return View(team);
        }

        public ActionResult Create(string? id)
        {
            ModelState.Clear();
            TeamMembers vm = new TeamMembers();
            vm.Operation = "add";
            vm.TeamId = id;
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(TeamMembers master)
        {
            ResultModel<TeamMembers> result = new ResultModel<TeamMembers>();
            try
            {



                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();


                    result = _teamMembersService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _teamMembersService.Insert(master);

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


                string users = Request.Form["UserName"].ToString();
                string teamname = Request.Form["teamname"].ToString();




                string branch = Request.Form["BranchName"].ToString();


                string userId = Request.Form["userId"].ToString();
                string memberId = Request.Form["memberId"].ToString();

                //change
                index.TeamId = memberId;



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


                            "UserName like",
                            "TeamName like",
                            "TeamId like"

                            

                        };
                string?[] conditionalValue = new[] { users, teamname, memberId };

                ResultModel<List<TeamMembers>> indexData =
                    _teamMembersService.GetIndexData(index,conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                    _teamMembersService.GetIndexDataCount(index,
                       conditionalFields, conditionalValue);

                int result = _teamMembersService.GetCount(TableName.TeamMember, "Id", new[]{ "A_TeamMembers.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<TeamMembers>(), draw = "", recordsTotal = 0, recordsFiltered = "" });
            }


        }

        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<TeamMembers>> result =
                    _teamMembersService.GetAll(new[] { "u.Id" }, new[] { id.ToString() });

                TeamMembers teamMember = result.Data.FirstOrDefault();
                teamMember.Operation = "update";
                teamMember.Id = id;
                teamMember.UserId = teamMember.UserId;
                teamMember.TeamId = teamMember.TeamId;

                return View("CreateEdit", teamMember);

                
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }


    }
}
