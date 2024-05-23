using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Advance;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Core.Interfaces.Repository.Tour;
using Shampan.Core.Interfaces.Repository.UserRoll;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Repository.SqlServer.UserRoll
{
	public class UserRollsRepository : Repository, IUserRollsRepository
	{
		public UserRollsRepository(SqlConnection context, SqlTransaction transaction)
		{
			this._context = context;
			this._transaction = transaction;
		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public bool CheckExists(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public List<UserManuInfo> GetUserManu(string Username)
		{
			string sqlText = "";
			List<UserManuInfo> VMs = new List<UserManuInfo>();
			DataTable dt = new DataTable();

			


			try
			{
				sqlText =
@"
select
ModuleID Id,
isnull(IsModuleActive,0)IsModuleActive,
TBLModul.Modul Modul
from UsersPermission
left outer join TBLModul on TBLModul.Id=ModuleID
where IsModuleActive=1 and UserName=@Username
";

		
				SqlDataAdapter objComm = CreateAdapter(sqlText);
				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				objComm.SelectCommand.Parameters.AddWithValue("@Username", Username);

				objComm.Fill(dt);

				VMs = dt
					.ToList<UserManuInfo>();
				return VMs;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public bool CheckPostStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
            try
            {
                bool ÌsPost = false;
                string Post = "";

                DataTable dt = new DataTable();

                // ToDo sql injection
                string sqlText = "select IsPost  from " + tableName + " where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Post = dt.Rows[0]["IsPost"].ToString();
                    return (Post == "Y");
                }


                return ÌsPost;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public bool CheckPushStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public string CodeGeneration(string CodeGroup, string CodeName)
		{
			try
			{

				string codeGeneration = GenerateCode(CodeGroup, CodeName);
				return codeGeneration;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public int Delete(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
            throw new NotImplementedException();

        }

        public List<UserRolls> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id

 ,UserId
,IsAudit
,AuditApproval1
,AuditApproval2
,AuditApproval3
,AuditApproval4

,IsTour
,TourApproval1
,TourApproval2
,TourApproval3
,TourApproval4

,IsAdvance
,AdvanceApproval1
,AdvanceApproval2
,AdvanceApproval3
,AdvanceApproval4

,IsTa
,IsTaApproval1
,IsTaApproval2
,IsTaApproval3
,IsTaApproval4

,IsTourCompletionReport
,TourCompletionReportApproval1
,TourCompletionReportApproval2
,TourCompletionReportApproval3
,TourCompletionReportApproval4

,IsAuditIssue
,AuditIssueApproval1
,AuditIssueApproval2
,AuditIssueApproval3
,AuditIssueApproval4

,IsAuditFeedback
,AuditFeedbackApproval1
,AuditFeedbackApproval2
,AuditFeedbackApproval3
,AuditFeedbackApproval4


,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom


from UserRolls
where 1=1";


//	,Code
//,Description
//,AuditId
//,TeamId
//,TourDate
//,IsPost


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<UserRolls> vms = dtResult.ToList<UserRolls>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
		}

		public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue)
		{
            string sqlText = "";
            List<UserRolls> VMs = new List<UserRolls>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(UserRolls.Id)FilteredCount
                from UserRolls  where 1=1 ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);


                return Convert.ToInt32(dt.Rows[0][0]);


            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public List<UserRolls> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<UserRolls> VMs = new List<UserRolls>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"select 
                  UserRolls.Id
                 ,UserRolls.IsAudit
                 ,UserRolls.IsTour
                 ,UserRolls.IsAdvance
                 ,UserRolls.IsTa
                 ,UserRolls.IsTourCompletionReport

                 ,UserRolls.AuditApproval1
                 ,UserRolls.AuditApproval2
                 ,UserRolls.AuditApproval3
                 ,UserRolls.AuditApproval4

                 ,UserRolls.CreatedBy
                               
                 ,ur.UserName

                 from UserRolls 

                --left outer join  AuthDB.dbo.AspNetUsers  ur on UserRolls.UserId=ur.Id
                left outer join  GDICAuditAuthDB.dbo.AspNetUsers  ur on UserRolls.UserId=ur.Id


                 where 1=1 ";



                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection

                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                if (conditionalFields != null)
                {
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                }
                

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new UserRolls();

                VMs.Add(req);


                VMs = dt.ToList<UserRolls>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public int GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<UserRolls> VMs = new List<UserRolls>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                from UserRolls  where 1=1 ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);


                return Convert.ToInt32(dt.Rows[0][0]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public string GetSettingsValue(string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public UserRolls Insert(UserRolls model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO UserRolls(


 UserId
,IsAudit
,AuditApproval1
,AuditApproval2
,AuditApproval3
,AuditApproval4

,IsTour
,TourApproval1
,TourApproval2
,TourApproval3
,TourApproval4

,IsAdvance
,AdvanceApproval1
,AdvanceApproval2
,AdvanceApproval3
,AdvanceApproval4

,IsTa
,IsTaApproval1
,IsTaApproval2
,IsTaApproval3
,IsTaApproval4

,IsTourCompletionReport
,TourCompletionReportApproval1
,TourCompletionReportApproval2
,TourCompletionReportApproval3
,TourCompletionReportApproval4

,IsAuditIssue
,AuditIssueApproval1
,AuditIssueApproval2
,AuditIssueApproval3
,AuditIssueApproval4

,IsAuditFeedback
,AuditFeedbackApproval1
,AuditFeedbackApproval2
,AuditFeedbackApproval3
,AuditFeedbackApproval4




,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @UserId
,@IsAudit
,@AuditApproval1
,@AuditApproval2
,@AuditApproval3
,@AuditApproval4

,@IsTour
,@TourApproval1
,@TourApproval2
,@TourApproval3
,@TourApproval4

,@IsAdvance
,@AdvanceApproval1
,@AdvanceApproval2
,@AdvanceApproval3
,@AdvanceApproval4

,@IsTa
,@IsTaApproval1
,@IsTaApproval2
,@IsTaApproval3
,@IsTaApproval4

,@IsTourCompletionReport
,@TourCompletionReportApproval1
,@TourCompletionReportApproval2
,@TourCompletionReportApproval3
,@TourCompletionReportApproval4

,@IsAuditIssue
,@AuditIssueApproval1
,@AuditIssueApproval2
,@AuditIssueApproval3
,@AuditIssueApproval4

,@IsAuditFeedback
,@AuditFeedbackApproval1
,@AuditFeedbackApproval2
,@AuditFeedbackApproval3
,@AuditFeedbackApproval4




,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");

				//command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;
				

				//command.Parameters.Add("@Description", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Description) ? (object)DBNull.Value : model.Description;

				//command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
				//command.Parameters.Add("@TeamId", SqlDbType.Int).Value = model.TeamId;

				command.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;

				command.Parameters.Add("@IsAudit", SqlDbType.Bit).Value = model.IsAudit;
				command.Parameters.Add("@AuditApproval1", SqlDbType.Bit).Value = model.AuditApproval1;
				command.Parameters.Add("@AuditApproval2", SqlDbType.Bit).Value = model.AuditApproval2;
				command.Parameters.Add("@AuditApproval3", SqlDbType.Bit).Value = model.AuditApproval3;
				command.Parameters.Add("@AuditApproval4", SqlDbType.Bit).Value = model.AuditApproval4;

				command.Parameters.Add("@IsTour", SqlDbType.Bit).Value = model.IsTour;
				command.Parameters.Add("@TourApproval1", SqlDbType.Bit).Value = model.TourApproval1;
				command.Parameters.Add("@TourApproval2", SqlDbType.Bit).Value = model.TourApproval2;
				command.Parameters.Add("@TourApproval3", SqlDbType.Bit).Value = model.TourApproval3;
				command.Parameters.Add("@TourApproval4", SqlDbType.Bit).Value = model.TourApproval4;

				command.Parameters.Add("@IsAdvance", SqlDbType.Bit).Value = model.IsAdvance;
				command.Parameters.Add("@AdvanceApproval1", SqlDbType.Bit).Value = model.AdvanceApproval1;
				command.Parameters.Add("@AdvanceApproval2", SqlDbType.Bit).Value = model.AdvanceApproval2;
				command.Parameters.Add("@AdvanceApproval3", SqlDbType.Bit).Value = model.AdvanceApproval3;
				command.Parameters.Add("@AdvanceApproval4", SqlDbType.Bit).Value = model.AdvanceApproval4;

				command.Parameters.Add("@IsTa", SqlDbType.Bit).Value = model.IsTa;
				command.Parameters.Add("@IsTaApproval1", SqlDbType.Bit).Value = model.IsTaApproval1;
				command.Parameters.Add("@IsTaApproval2", SqlDbType.Bit).Value = model.IsTaApproval2;
				command.Parameters.Add("@IsTaApproval3", SqlDbType.Bit).Value = model.IsTaApproval3;
				command.Parameters.Add("@IsTaApproval4", SqlDbType.Bit).Value = model.IsTaApproval3;

				command.Parameters.Add("@IsTourCompletionReport", SqlDbType.Bit).Value = model.IsTourCompletionReport;
				command.Parameters.Add("@TourCompletionReportApproval1", SqlDbType.Bit).Value = model.TourCompletionReportApproval1;
				command.Parameters.Add("@TourCompletionReportApproval2", SqlDbType.Bit).Value = model.TourCompletionReportApproval2;
				command.Parameters.Add("@TourCompletionReportApproval3", SqlDbType.Bit).Value = model.TourCompletionReportApproval3;
				command.Parameters.Add("@TourCompletionReportApproval4", SqlDbType.Bit).Value = model.TourCompletionReportApproval4;

				command.Parameters.Add("@IsAuditIssue", SqlDbType.Bit).Value = model.IsAuditIssue;
				command.Parameters.Add("@AuditIssueApproval1", SqlDbType.Bit).Value = model.AuditIssueApproval1;
				command.Parameters.Add("@AuditIssueApproval2", SqlDbType.Bit).Value = model.AuditIssueApproval2;
				command.Parameters.Add("@AuditIssueApproval3", SqlDbType.Bit).Value = model.AuditIssueApproval3;
				command.Parameters.Add("@AuditIssueApproval4", SqlDbType.Bit).Value = model.AuditIssueApproval4;

				command.Parameters.Add("@IsAuditFeedback", SqlDbType.Bit).Value = model.IsAuditFeedback;
				command.Parameters.Add("@AuditFeedbackApproval1", SqlDbType.Bit).Value = model.AuditFeedbackApproval1;
				command.Parameters.Add("@AuditFeedbackApproval2", SqlDbType.Bit).Value = model.AuditFeedbackApproval2;
				command.Parameters.Add("@AuditFeedbackApproval3", SqlDbType.Bit).Value = model.AuditFeedbackApproval3;
				command.Parameters.Add("@AuditFeedbackApproval4", SqlDbType.Bit).Value = model.AuditFeedbackApproval4;


				command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
				command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
				command.Parameters.Add("@CreatedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : model.Audit.CreatedFrom.ToString();

				//command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "N";


				model.Id = Convert.ToInt32(command.ExecuteScalar());


				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		

		public UserRolls Update(UserRolls model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update UserRolls set




 IsAudit  =@IsAudit
,AuditApproval1=@AuditApproval1
,AuditApproval2=@AuditApproval2
,AuditApproval3=@AuditApproval3
,AuditApproval4=@AuditApproval4


,IsTour = @IsTour
,TourApproval1 = @TourApproval1
,TourApproval2 = @TourApproval2
,TourApproval3 = @TourApproval3
,TourApproval4 = @TourApproval4


,IsAdvance = @IsAdvance 
,AdvanceApproval1 = @AdvanceApproval1
,AdvanceApproval2 = @AdvanceApproval2
,AdvanceApproval3 = @AdvanceApproval3
,AdvanceApproval4 = @AdvanceApproval4


,IsTa=@IsTa
,IsTaApproval1=@IsTaApproval1
,IsTaApproval2=@IsTaApproval2
,IsTaApproval3=@IsTaApproval3
,IsTaApproval4=@IsTaApproval4


,IsTourCompletionReport=@IsTourCompletionReport
,TourCompletionReportApproval1=@TourCompletionReportApproval1
,TourCompletionReportApproval2=@TourCompletionReportApproval2
,TourCompletionReportApproval3=@TourCompletionReportApproval3
,TourCompletionReportApproval4=@TourCompletionReportApproval4


,IsAuditIssue = @IsAuditIssue
,AuditIssueApproval1=@AuditIssueApproval1
,AuditIssueApproval2=@AuditIssueApproval2
,AuditIssueApproval3=@AuditIssueApproval3
,AuditIssueApproval4=@AuditIssueApproval4


,IsAuditFeedback = @IsAuditFeedback
,AuditFeedbackApproval1 =@AuditFeedbackApproval1
,AuditFeedbackApproval2 =@AuditFeedbackApproval2
,AuditFeedbackApproval3 =@AuditFeedbackApproval3
,AuditFeedbackApproval4 =@AuditFeedbackApproval4

  

,LastUpdateBy              =@LastUpdateBy  
,LastUpdateOn              =@LastUpdateOn  
,LastUpdateFrom            =@LastUpdateFrom 
                       
where  Id= @Id ";

//Code = @Code
//,Description = @Description
//,AuditId = @AuditId
//,TeamId = @TeamId
//,TourDate = @TourDate


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                //UserId = @UserId
                //command.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;

                command.Parameters.Add("@IsAudit", SqlDbType.Bit).Value = model.IsAudit;
				command.Parameters.Add("@AuditApproval1", SqlDbType.Bit).Value = model.AuditApproval1;
				command.Parameters.Add("@AuditApproval2", SqlDbType.Bit).Value = model.AuditApproval2;
				command.Parameters.Add("@AuditApproval3", SqlDbType.Bit).Value = model.AuditApproval3;
				command.Parameters.Add("@AuditApproval4", SqlDbType.Bit).Value = model.AuditApproval4;

				command.Parameters.Add("@IsTour", SqlDbType.Bit).Value = model.IsTour;
				command.Parameters.Add("@TourApproval1", SqlDbType.Bit).Value = model.TourApproval1;
				command.Parameters.Add("@TourApproval2", SqlDbType.Bit).Value = model.TourApproval2;
				command.Parameters.Add("@TourApproval3", SqlDbType.Bit).Value = model.TourApproval3;
				command.Parameters.Add("@TourApproval4", SqlDbType.Bit).Value = model.TourApproval4;

				command.Parameters.Add("@IsAdvance", SqlDbType.Bit).Value = model.IsAdvance;
				command.Parameters.Add("@AdvanceApproval1", SqlDbType.Bit).Value = model.AdvanceApproval1;
				command.Parameters.Add("@AdvanceApproval2", SqlDbType.Bit).Value = model.AdvanceApproval2;
				command.Parameters.Add("@AdvanceApproval3", SqlDbType.Bit).Value = model.AdvanceApproval3;
				command.Parameters.Add("@AdvanceApproval4", SqlDbType.Bit).Value = model.AdvanceApproval4;

				command.Parameters.Add("@IsTa", SqlDbType.Bit).Value = model.IsTa;
				command.Parameters.Add("@IsTaApproval1", SqlDbType.Bit).Value = model.IsTaApproval1;
				command.Parameters.Add("@IsTaApproval2", SqlDbType.Bit).Value = model.IsTaApproval2;
				command.Parameters.Add("@IsTaApproval3", SqlDbType.Bit).Value = model.IsTaApproval3;
				command.Parameters.Add("@IsTaApproval4", SqlDbType.Bit).Value = model.IsTaApproval3;

				command.Parameters.Add("@IsTourCompletionReport", SqlDbType.Bit).Value = model.IsTourCompletionReport;
				command.Parameters.Add("@TourCompletionReportApproval1", SqlDbType.Bit).Value = model.TourCompletionReportApproval1;
				command.Parameters.Add("@TourCompletionReportApproval2", SqlDbType.Bit).Value = model.TourCompletionReportApproval2;
				command.Parameters.Add("@TourCompletionReportApproval3", SqlDbType.Bit).Value = model.TourCompletionReportApproval3;
				command.Parameters.Add("@TourCompletionReportApproval4", SqlDbType.Bit).Value = model.TourCompletionReportApproval4;

				command.Parameters.Add("@IsAuditIssue", SqlDbType.Bit).Value = model.IsAuditIssue;
				command.Parameters.Add("@AuditIssueApproval1", SqlDbType.Bit).Value = model.AuditIssueApproval1;
				command.Parameters.Add("@AuditIssueApproval2", SqlDbType.Bit).Value = model.AuditIssueApproval2;
				command.Parameters.Add("@AuditIssueApproval3", SqlDbType.Bit).Value = model.AuditIssueApproval3;
				command.Parameters.Add("@AuditIssueApproval4", SqlDbType.Bit).Value = model.AuditIssueApproval4;

				command.Parameters.Add("@IsAuditFeedback", SqlDbType.Bit).Value = model.IsAuditFeedback;
				command.Parameters.Add("@AuditFeedbackApproval1", SqlDbType.Bit).Value = model.AuditFeedbackApproval1;
				command.Parameters.Add("@AuditFeedbackApproval2", SqlDbType.Bit).Value = model.AuditFeedbackApproval2;
				command.Parameters.Add("@AuditFeedbackApproval3", SqlDbType.Bit).Value = model.AuditFeedbackApproval3;
				command.Parameters.Add("@AuditFeedbackApproval4", SqlDbType.Bit).Value = model.AuditFeedbackApproval4;

				command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
				command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn.ToString();
				command.Parameters.Add("@LastUpdateFrom ", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateFrom.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateFrom.ToString();


				int rowcount = command.ExecuteNonQuery();

				if (rowcount <= 0)
				{
					throw new Exception(MessageModel.UpdateFail);
				}

				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<SubmanuList> GetUserSubManu(string Username)
		{
			string sqlText = "";
			List<SubmanuList> VMs = new List<SubmanuList>();
			DataTable dt = new DataTable();




			try
			{
				sqlText =
@"
SELECT 
mo.Id,
mp.Modul,
np.Node,
np.Url,
isnull(np.IsActive,0)IsActive
FROM [NodePermission] np

left join TBLModul as mo on np.ModulId=mo.Id
left Join ModulPermission as mp on mp.Id=np.ModulId
where IsAllowByUser=1 and np.UserId=@Username 

--where np.IsActive=1 and np.UserId=@Username 

";


				SqlDataAdapter objComm = CreateAdapter(sqlText);
				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                sqlText = sqlText + "Orderby np.Id asc";

				objComm.SelectCommand.Parameters.AddWithValue("@Username", Username);

				objComm.Fill(dt);

				VMs = dt
					.ToList<SubmanuList>();
				return VMs;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

        public List<AuditComponent> GetAuditComponents()
        {

            string sqlText = "";
            List<AuditComponent> VMs = new List<AuditComponent>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"



SELECT
    AT.AuditType AS [Name],
    COUNT(CASE WHEN a.IsPlaned = 1 THEN 1 ELSE NULL END) AS BranchPlan,
    COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) AS BranchOngoin,
    COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END) AS BranchCompleted,

    (COUNT(CASE WHEN a.IsPlaned = 1 THEN 1 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)) AS BranchRemaining,
    (CASE
        WHEN (COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) + COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)) = 0 THEN 0
        ELSE (100 * COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END) / (COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) + COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)))
    END) AS [% (Completed + Ongoing)]
FROM
    --[SSLAuditDB].[dbo].[AuditTypes] AS AT
    AuditTypes AS AT
LEFT JOIN
    --[SSLAuditDB].[dbo].[A_Audits] AS a ON AT.Id = a.AuditTypeId
    A_Audits AS a ON AT.Id = a.AuditTypeId
	where a.IsPlaned=1
GROUP BY
    AT.AuditType
 
                ";

                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName.Replace("AuditName", "ad.Name") + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                //objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);

                objComm.Fill(dt);

                VMs = dt.ToList<AuditComponent>();

                //VMs[0].BranchCompletedOngoing = VMs[0].BranchCompleted + VMs[0].BranchOngoin;
                //VMs[0].BranchCompletedOngoing = 60;
                foreach(AuditComponent audit in VMs)
                {
                    audit.BranchCompletedOngoing = ((audit.BranchCompleted + audit.BranchOngoin) / audit.BranchPlan) * 100;
                }

                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }





   //         List<AuditComponent> AuditComponentList = new List<AuditComponent>()
   //             {
   //                 new AuditComponent(){Name = "Branches",BranchPlan=55,BranchOngoin=67,BranchCompleted=8,BranchRemaining=8,BranchCompletedOngoing=69 },
   //                 new AuditComponent(){Name="Departments", BranchPlan=66,BranchOngoin=67,BranchCompleted=0,BranchRemaining=67,BranchCompletedOngoing=90 },
   //                 new AuditComponent(){Name="Subsideary", BranchPlan=77,BranchOngoin=57,BranchCompleted=56,BranchRemaining=8,BranchCompletedOngoing=78 },
   //                 new AuditComponent(){Name="Follow-Up", BranchPlan=88,BranchOngoin=89,BranchCompleted=8,BranchRemaining=66,BranchCompletedOngoing=84 },
   //                 new AuditComponent(){Name="UnderWriting", BranchPlan=99,BranchOngoin=90,BranchCompleted=0,BranchRemaining=77,BranchCompletedOngoing=90 },
   //                 new AuditComponent(){Name="Patte Cash HO", BranchPlan=11,BranchOngoin=89,BranchCompleted=7,BranchRemaining=88,BranchCompletedOngoing=100 },
   //                 new AuditComponent(){Name="Patte Cash Branch", BranchPlan=11,BranchOngoin=23,BranchCompleted=0,BranchRemaining=99,BranchCompletedOngoing=89 },
   //                 new AuditComponent(){Name="Monthly BCO Reporting", BranchPlan=33,BranchOngoin=45,BranchCompleted=0,BranchRemaining=00,BranchCompletedOngoing=67 }
   //             };

			//return AuditComponentList;
        }

        public List<BranchCoverage> GetBranchCoverages()
        {


            string sqlText = "";
            List<BranchCoverage> VMs = new List<BranchCoverage>();
            List<BranchCoverage> VMsFinal = new List<BranchCoverage>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"


  SELECT  bp.BranchName Name,count(a.Id) First
  FROM  BranchProfiles bp
  left join A_Audits a on a.BranchID=bp.BranchID
  group by bp.BranchName

   
                ";

                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName.Replace("AuditName", "ad.Name") + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                //objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);


                objComm.Fill(dt);

                VMs = dt
                    .ToList<BranchCoverage>();

                //VMs[0].BranchCompletedOngoing = VMs[0].BranchCompleted + VMs[0].BranchOngoin;
                //VMs[0].BranchCompletedOngoing = 60;
                foreach (BranchCoverage audit in VMs)
                {
                    if (audit.First != 0)
                    {
                        VMsFinal.Add(audit);
                    }
                }

                return VMsFinal;
            }
            catch (Exception e)
            {
                throw e;
            }


   //         List<BranchCoverage> branchCoverages = new List<BranchCoverage>()
   //             {
   //                 new BranchCoverage(){Name="Dhaka",First=17,Second=3},
   //                 new BranchCoverage(){Name="Gazipur",First=2,Second=3},
   //                 new BranchCoverage(){Name="Moymonsing",First=14,Second=3},
   //                 new BranchCoverage(){Name="Khulna",First=16,Second=5},
   //                 new BranchCoverage(){Name="Comilla",First=4,Second=3},
   //                 new BranchCoverage(){Name="Borisal",First=19,Second=6},
   //                 new BranchCoverage(){Name="Sylet",First=37,Second=0},
   //                 new BranchCoverage(){Name="Dinajpur",First=14,Second=1}
   //             };
			//return branchCoverages;
        }

        public List<BasicData> GetBasicData()
        {

            List<BasicData> basicData = new List<BasicData>()
                {
                    new BasicData(){Data="2,026",Name="Prepament Reviewed"},
                    new BasicData(){Data="18,889",Name="Uncertifies Communicated"},
                    new BasicData(){Data="BDT:6.6Million",Name="Financial Impact"},
                    new BasicData(){Data="70%",Name="IAC Budget"},
                    new BasicData(){Data="Next Business BDT 1.5 Million",Name="IAC Contribution to Reveniew"}
                };
			return basicData;
        }

        public List<SpecialEngagements> GetSpecialEngagements()
        {
            string sqlText = "";
            List<SpecialEngagements> VMs = new List<SpecialEngagements>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

          select top(2) 
          Id
         ,Code
         ,Name
         ,IsPlaned 
          from A_Audits 
          where IsPlaned=0 ";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<SpecialEngagements>();


                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }

            //List<SpecialEngagements> specialEngagements = new List<SpecialEngagements>()
            //    {
            //        new SpecialEngagements(){Details="Variour investication is issued to management for change"},
            //        new SpecialEngagements(){Details="Involvemennt in multiple engagemen as per management requirments"},
            //        new SpecialEngagements(){Details="Automation of Audit Operntaion (Audit Softwate)"},
            //        new SpecialEngagements(){Details="Variour investication is issued to management"},
            //        new SpecialEngagements(){Details="IAC Induction Session"}
                
            //    };
            //return specialEngagements;

        }

        public List<AddHocEngagements> GetAddHocEngagements()
        {
            List<AddHocEngagements> addHocEngagements = new List<AddHocEngagements>()
                {
                    new AddHocEngagements(){Details="IAC Induction Session for change"},
                    new AddHocEngagements(){Details="Strengic Session"},
                    new AddHocEngagements(){Details="Automation of Audit Operntaion (Audit Softwate)"},
                    new AddHocEngagements(){Details="Involvemennt in multiple engagemen as per management requirments"},
                    new AddHocEngagements(){Details="IAC Induction Session"}

                };
            return addHocEngagements;
        }

		public int GetIssues(string UserName)
		{

            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {

				string query = " select Id,UserName from [GDICAuditAuthDB].[dbo].[AspNetUsers] authuser where authuser.UserName=@userName ";
                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter obj = CreateAdapter(query);
                obj.SelectCommand = ApplyParameters(obj.SelectCommand, null, null);
				obj.SelectCommand.Parameters.AddWithValue("@userName", UserName);

                obj.Fill(authdt);
				string id = null;
                foreach (DataRow row in authdt.Rows)
                {
                     id = row["Id"].ToString();   

                }


                sqlText = @"

                SELECT Count(A_AuditIssues.Id)Id

                 FROM A_AuditIssues

                --where A_AuditIssues.CreatedBy=@UserName

                ";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
				//objComm.SelectCommand.Parameters.AddWithValue("@UserId", id);
				//objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);


                objComm.Fill(dt);


                //return Convert.ToInt32(dt.Rows[0][0]);
                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }


        }

    }
}
