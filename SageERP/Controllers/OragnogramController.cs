using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Oragnogram;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using ShampanERP.Persistence;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]

	public class OragnogramController : Controller
    {

		private readonly ApplicationDbContext _applicationDb;
		private readonly IOragnogramService _oragnogramService;

		public OragnogramController(ApplicationDbContext applicationDb, IOragnogramService oragnogramService)
		{

			_applicationDb = applicationDb;
			_oragnogramService = oragnogramService;

		}

		public IActionResult Index()
        {
            return View();
        }


		public ActionResult GetEmployees()
		{

			ResultModel<List<object>> chart = _oragnogramService.GetEmployeesData();

			List<object> chartData = chart.Data;


			//List<object> chartData = new List<object>();


			//string query = "SELECT EmployeeId, Name, Designation, isnull(ReportingManager,'')ReportingManager ";
			//query += " FROM EmployeesHierarchy";

			//string constr = "Data Source=192.168.15.100\\MSSQLSERVER2019;Initial Catalog=AjaxSamples;User id=sa;Password=S123456_;";
			//using (SqlConnection con = new SqlConnection(constr))
			//{
			//	using (SqlCommand cmd = new SqlCommand(query, con))
			//	{
			//		con.Open();

			//		using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
			//		{
			//			DataTable dt = new DataTable();
			//			sda.Fill(dt);

			//			foreach (DataRow row in dt.Rows)
			//			{
			//				chartData.Add(new object[]
			//				{
			//					row["EmployeeId"], row["Name"], row["Designation"], row["ReportingManager"]
			//				});
			//			}
			//		}



			//		con.Close();
			//	}
			//}

			return Ok(chartData);

		}


	}
}
