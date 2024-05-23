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
using Shampan.Core.Interfaces.Repository.Oragnogram;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.Oragnogram
{
	public class OragnogramRepository : Repository, IOragnogramRepository
	{
		public OragnogramRepository(SqlConnection context, SqlTransaction transaction)
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

        public List<EmployeesHierarchy> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 EmployeesHierarchy.EmployeeId
,EmployeesHierarchy.Code
,EmployeesHierarchy.Name
,EmployeesHierarchy.Designation
,EmployeesHierarchy.ReportingManager



,EmployeesHierarchy.CreatedBy
,EmployeesHierarchy.CreatedOn
,EmployeesHierarchy.CreatedFrom
,EmployeesHierarchy.LastUpdateBy
,EmployeesHierarchy.LastUpdateOn
,EmployeesHierarchy.LastUpdateFrom


from  EmployeesHierarchy
where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<EmployeesHierarchy> vms = dtResult.ToList<EmployeesHierarchy>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

		public List<object> GetEmployeesData()
		{
			string sqlText = "";
			List<EmployeesHierarchy> VMs = new List<EmployeesHierarchy>();
			DataTable dt = new DataTable();

			//,ReportingManager

			try
			{
				sqlText = @"select 
                  EmployeeId
                 ,Name
                 ,Designation
                 ,isnull(ReportingManager,'')ReportingManager
                
                        

                 from EmployeesHierarchy 
                 where 1=1 ";



				sqlText = ApplyConditions(sqlText, null, null, true);

				// ToDo Escape Sql Injection

				//sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
				//sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

				SqlDataAdapter objComm = CreateAdapter(sqlText);

				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				objComm.Fill(dt);
				var req = new EmployeesHierarchy();

				VMs.Add(req);

				List<object> chartData = new List<object>();

				//List<EmployeesHierarchy> chartData = new List<EmployeesHierarchy>();

				foreach (DataRow row in dt.Rows)
				{
					chartData.Add(new object[]
					{
								row["EmployeeId"], row["Name"], row["Designation"], row["ReportingManager"]
					});
				}


				VMs = dt.ToList<EmployeesHierarchy>();

				//return VMs;
				return chartData;


			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue)
		{
            string sqlText = "";
            List<EmployeesHierarchy> VMs = new List<EmployeesHierarchy>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(EmployeesHierarchy.EmployeeId)FilteredCount
                from EmployeesHierarchy  where 1=1 ";


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

		public List<EmployeesHierarchy> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{

            string sqlText = "";
            List<EmployeesHierarchy> VMs = new List<EmployeesHierarchy>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"select 
                  EmployeeId
                 ,Code
                 ,Name
                 ,Designation
                

                 from EmployeesHierarchy 
                 where 1=1 ";



                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection

                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new EmployeesHierarchy();

                VMs.Add(req);


                VMs = dt.ToList<EmployeesHierarchy>();

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
            List<EmployeesHierarchy> VMs = new List<EmployeesHierarchy>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(EmployeeId)FilteredCount
                from EmployeesHierarchy  where 1=1 ";


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

		public EmployeesHierarchy Insert(EmployeesHierarchy model)
		{
            try
            {
                string sqlText = "";

                var command = CreateCommand(@" INSERT INTO EmployeesHierarchy(



 Code
,Name
,Designation
,ReportingManager


,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@Name
,@Designation
,@ReportingManager


,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");

                command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;
                command.Parameters.Add("@Name", SqlDbType.NChar).Value = model.Name;
                command.Parameters.Add("@Designation", SqlDbType.NChar).Value = model.Designation;
                command.Parameters.Add("@ReportingManager", SqlDbType.Int).Value = model.ReportingManager;




                command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
                command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
                command.Parameters.Add("@CreatedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : model.Audit.CreatedFrom.ToString();



                model.EmployeeId = Convert.ToInt32(command.ExecuteScalar());


                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        public EmployeesHierarchy Update(EmployeesHierarchy model)
		{

            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"  update EmployeesHierarchy set

 Code                               =@Code  
,Name                                =@Name  
,Designation                        =@Designation  
,ReportingManager                    =@ReportingManager  


,LastUpdateBy              =@LastUpdateBy  
,LastUpdateOn              =@LastUpdateOn  
,LastUpdateFrom            =@LastUpdateFrom 
                       
where  EmployeeId= @EmployeeId ";


                SqlCommand command = CreateCommand(query);

                command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = model.EmployeeId;
                command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;
                command.Parameters.Add("@Name", SqlDbType.NChar).Value = model.Name;
                command.Parameters.Add("@Designation", SqlDbType.NChar).Value = model.Designation;
                command.Parameters.Add("@ReportingManager", SqlDbType.Int).Value = model.ReportingManager;




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
