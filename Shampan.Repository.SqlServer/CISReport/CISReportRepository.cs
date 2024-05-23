using Microsoft.Data.SqlClient;
using Shampan.Core.Interfaces.Repository.User;
using Shampan.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Deshboard;
using Shampan.Models.AuditModule;
using System.Reflection;
using SixLabors.ImageSharp.ColorSpaces;
using Shampan.Core.Interfaces.Repository.CISReport;

namespace Shampan.Repository.SqlServer.CISReport
{
    public class CISReportRepository : Repository, ICISReportRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;

        public CISReportRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

        }
        public List<MRWiseChangeLog> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			try
			{
				string sqlText = @" 

                 SELECT
                 Id
                ,MRNo
                ,PCNo
                ,UserId
                ,EditDate 
                ,Status
                ,MRNet
                ,MRVat
                ,MRStamp
                ,MRCoinsPayable
                ,MRDateTime
                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
     
                FROM MRWiseChangeLog


                where 1=1";


				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


				SqlCommand objComm = CreateCommand(sqlText);

				objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

				SqlDataAdapter adapter = new SqlDataAdapter(objComm);
				DataTable dtResult = new DataTable();
				adapter.Fill(dtResult);

				List<MRWiseChangeLog> vms = dtResult.ToList<MRWiseChangeLog>();
				return vms;


			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

        public List<MRWiseChangeLog> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			string sqlText = "";
			List<MRWiseChangeLog> VMs = new List<MRWiseChangeLog>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"

                 SELECT
                 Id
                ,MRNo
                ,PCNo
                ,UserId
                ,EditDate 
                ,Status
                ,MRNet
                ,MRVat
                ,MRStamp
                ,MRCoinsPayable
                ,MRDateTime
                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
     
                FROM MRWiseChangeLog 

                where 1=1 
    
 
";

				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

				// ToDo Escape Sql Injection
				sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
				sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

				SqlDataAdapter objComm = CreateAdapter(sqlText);
				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

				//objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);


				objComm.Fill(dt);



				VMs = dt
					.ToList<MRWiseChangeLog>();
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
			List<MRWiseChangeLog> VMs = new List<MRWiseChangeLog>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"
                 select count(Id)FilteredCount
                from MRWiseChangeLog  where 1=1 ";


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

        public MRWiseChangeLog Insert(MRWiseChangeLog model)
        {
			try
			{
				string sqlText = "";
				int count = 0;


				var command = CreateCommand(@" INSERT INTO MRWiseChangeLog(

                 MRNo
                ,PCNo
                ,UserId
                ,EditDate 
                ,Status
                ,MRNet
                ,MRVat
                ,MRStamp
                ,MRCoinsPayable
                ,MRDateTime
                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
                
                

                ) VALUES (

                 @MRNo
                ,@PCNo
                ,@UserId
                ,@EditDate 
                ,@Status 
                ,@MRNet
                ,@MRVat
                ,@MRStamp
                ,@MRCoinsPayable
                ,@MRDateTime
                ,@CreatedBy
                ,@CreatedOn
                ,@CreatedFrom
               
                

                )SELECT SCOPE_IDENTITY()");



				
				command.Parameters.Add("@MRNo", SqlDbType.VarChar).Value = model.MRNo;
				command.Parameters.Add("@PCNo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.PCNo) ? (object)DBNull.Value : model.PCNo;
				command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.UserId) ? (object)DBNull.Value : model.UserId;
				command.Parameters.Add("@EditDate", SqlDbType.DateTime).Value = string.IsNullOrEmpty(model.EditDate) ? (object)DBNull.Value : model.EditDate;
				command.Parameters.Add("@Status", SqlDbType.VarChar).Value = model.Status;
				command.Parameters.Add("@MRNet", SqlDbType.Decimal).Value = model.MRNet;
				command.Parameters.Add("@MRVat", SqlDbType.Decimal).Value = model.MRVat;
				command.Parameters.Add("@MRStamp", SqlDbType.Decimal).Value = model.MRStamp;
				command.Parameters.Add("@MRCoinsPayable", SqlDbType.Decimal).Value = model.MRCoinsPayable;
				command.Parameters.Add("@MRDateTime", SqlDbType.DateTime).Value = model.MRDateTime;


				command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = model.Audit.CreatedBy;
				command.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = model.Audit.CreatedOn;
				command.Parameters.Add("@CreatedFrom", SqlDbType.NVarChar).Value = model.Audit.CreatedFrom;

				//command.Parameters.Add("@LastUpdateBy", SqlDbType.NVarChar).Value = model.Audit.LastUpdateBy;
				//command.Parameters.Add("@LastUpdateOn", SqlDbType.NVarChar).Value = model.Audit.LastUpdateOn;
				//command.Parameters.Add("@LastUpdateFrom", SqlDbType.NVarChar).Value = model.Audit.LastUpdateFrom;


				model.Id = Convert.ToInt32(command.ExecuteScalar());


				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public MRWiseChangeLog Update(MRWiseChangeLog model)
        {
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update MRWiseChangeLog set

 MRNo                          =@MRNo  
,PCNo                          =@PCNo  
,UserId                        =@UserId   
,EditDate                      =@EditDate  
,Status                        =@Status  
,MRNet                         =@MRNet  
,MRVat                         =@MRVat  
,MRStamp                       =@MRStamp  
,MRCoinsPayable                =@MRCoinsPayable  
,MRDateTime                    =@MRDateTime  
,LastUpdateBy                  =@LastUpdateBy  
,LastUpdateOn                  =@LastUpdateOn  
,LastUpdateFrom                =@LastUpdateFrom  
   
                   
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

				var item = model.MRWiseChangeLogDetails.FirstOrDefault();

				command.Parameters.Add("@MRNo", SqlDbType.VarChar).Value = model.MRNo;
				command.Parameters.Add("@PCNo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.PCNo) ? (object)DBNull.Value : model.PCNo;
				command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.UserId) ? (object)DBNull.Value : model.UserId;
				command.Parameters.Add("@EditDate", SqlDbType.DateTime).Value = string.IsNullOrEmpty(model.EditDate) ? (object)DBNull.Value : model.EditDate;
				command.Parameters.Add("@Status", SqlDbType.VarChar).Value = model.Status;
				command.Parameters.Add("@MRNet", SqlDbType.Decimal).Value = model.MRNet;
				command.Parameters.Add("@MRVat", SqlDbType.Decimal).Value = model.MRVat;
				command.Parameters.Add("@MRStamp", SqlDbType.Decimal).Value = model.MRStamp;
				command.Parameters.Add("@MRCoinsPayable", SqlDbType.Decimal).Value = model.MRCoinsPayable;
				command.Parameters.Add("@MRDateTime", SqlDbType.DateTime).Value = model.MRDateTime;


				//command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = model.Audit.CreatedBy;
				//command.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = model.Audit.CreatedOn;
				//command.Parameters.Add("@CreatedFrom", SqlDbType.NVarChar).Value = model.Audit.CreatedFrom;

				command.Parameters.Add("@LastUpdateBy", SqlDbType.NVarChar).Value = model.Audit.LastUpdateBy;
				command.Parameters.Add("@LastUpdateOn", SqlDbType.NVarChar).Value = model.Audit.LastUpdateOn;
				command.Parameters.Add("@LastUpdateFrom", SqlDbType.NVarChar).Value = model.Audit.LastUpdateFrom;




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
