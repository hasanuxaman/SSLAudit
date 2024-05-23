using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Advance;
using Shampan.Core.Interfaces.Repository.Calender;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.Calender
{
	public class CalenderRepository : Repository, ICalendersRepository
	{
		public CalenderRepository(SqlConnection context, SqlTransaction transaction)
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

		public bool CheckPostStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();

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

        public List<Calenders> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			try
			{
				string sqlText = @"select 
 Calenders.Id
,Calenders.Code
,Calenders.Title
,Calenders.Color
,Calenders.Description
,Calenders.StartDate [Start]
,Calenders.EndDate [End]
,Calenders.AllDay
,Calenders.IsActive


,Calenders.CreatedBy
,Calenders.CreatedOn
,Calenders.CreatedFrom
,Calenders.LastUpdateBy
,Calenders.LastUpdateOn
,Calenders.LastUpdateFrom


from  Calenders
where 1=1";


				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


				SqlCommand objComm = CreateCommand(sqlText);

				objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

				SqlDataAdapter adapter = new SqlDataAdapter(objComm);
				DataTable dtResult = new DataTable();
				adapter.Fill(dtResult);

				List<Calenders> vms = dtResult.ToList<Calenders>();
				return vms;


			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public List<Calenders> GetCalenderData()
		{
			string sqlText = "";
			List<Calenders> VMs = new List<Calenders>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"
select * from (	
	select 
	 Calenders.Id
	,Calenders.Title
	,Calenders.Description
	,Calenders.StartDate Start
	,Calenders.EndDate [End]
	,Calenders.Color
	,Calenders.AllDay          
	
	
	from Calenders 
	
	union all
	
	select 
	 Id
	,[Name] Title
	,[Name] Description
	,StartDate Start
	,EndDate [End]
	,'#2EACD5' Color
	,1 AllDay          
	

	from A_Audits 
	
	union all
	
	select 
	 Id
	,isnull([Description],'-') Title
	,isnull([Description],'-') Description
	--,TourDate Start
	,FromDate Start
	--,TourDate [End]
	,ToDate [End]
	,'#90EE90' Color
	,1 AllDay          
	
	
	from Tours 


) as [CalenderEvents]


where 1=1 


";



				sqlText = ApplyConditions(sqlText, null, null, true);

				// ToDo Escape Sql Injection

				//sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
				//sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

				SqlDataAdapter objComm = CreateAdapter(sqlText);

				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				objComm.Fill(dt);
				var req = new Calenders();

				VMs.Add(req);


				VMs = dt.ToList<Calenders>();

				return VMs;


			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue)
		{
			string sqlText = "";
			List<Calenders> VMs = new List<Calenders>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"
                 select count(Calenders.Id)FilteredCount
                from Calenders  where 1=1 ";


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

		public List<Calenders> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			string sqlText = "";
			List<Calenders> VMs = new List<Calenders>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"select 
                  Id
                 ,Code
                 ,Title
                 ,Color
                

                 from Calenders 
                 where 1=1 ";



				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

				// ToDo Escape Sql Injection
				sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
				sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

				SqlDataAdapter objComm = CreateAdapter(sqlText);

				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

				objComm.Fill(dt);
				var req = new Calenders();

				VMs.Add(req);


				VMs = dt.ToList<Calenders>();

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
			List<Calenders> VMs = new List<Calenders>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"
                 select count(Id)FilteredCount
                from Calenders  where 1=1 ";


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

		public Calenders Insert(Calenders model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO Calenders(



Code
,Title
,Color
,Description
,StartDate
,EndDate
,AllDay
,IsActive

,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@Title
,@Color
,@Description
,@StartDate
,@EndDate
,@AllDay
,@IsActive

,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");

				command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;
				

				command.Parameters.Add("@Description", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Description) ? (object)DBNull.Value : model.Description;

				command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = model.Title;
				command.Parameters.Add("@Color", SqlDbType.NVarChar).Value = model.Color;
				command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = model.Start;
				command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = model.End;

				command.Parameters.Add("@AllDay", SqlDbType.Bit).Value = 1;
				command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = model.IsActive;


				command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
				command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
				command.Parameters.Add("@CreatedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : model.Audit.CreatedFrom.ToString();



				model.Id = Convert.ToInt32(command.ExecuteScalar());


				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		

		public Calenders Update(Calenders model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update Calenders set

 Code                         =@Code  
,Description                  =@Description  
,Title                        =@Title  
,Color                        =@Color  
,StartDate                    =@StartDate  
,EndDate                      =@EndDate  
,AllDay                       =@AllDay  
,IsActive                     =@IsActive  

,LastUpdateBy              =@LastUpdateBy  
,LastUpdateOn              =@LastUpdateOn  
,LastUpdateFrom            =@LastUpdateFrom 
                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
				command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;

				command.Parameters.Add("@Description", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Description) ? (object)DBNull.Value : model.Description;

				command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = model.Title;
				command.Parameters.Add("@Color", SqlDbType.NVarChar).Value = model.Color;
				command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = model.Start;
				command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = model.End;

				command.Parameters.Add("@AllDay", SqlDbType.Bit).Value = model.AllDay;
				command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = model.IsActive;



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
	}
}
