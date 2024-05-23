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
using Microsoft.VisualBasic;
using System.Xml.Linq;

namespace Shampan.Repository.SqlServer.CISReport
{
    public class CollectionEditLogRepository : Repository, ICollectionEditLogRepository
	{
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;

        public CollectionEditLogRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

        }
        public List<CollectionEditLog> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {

                string sqlText = @" 

                 SELECT
                 Id
                ,MR
                ,PCName
                ,UserId
                ,EditDate 
                ,Status
                ,SlNo
                ,DepositDate
                ,Deposit
                ,Record

                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
     

                FROM CollectionEditLog


                where 1=1";


				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


				SqlCommand objComm = CreateCommand(sqlText);

				objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

				SqlDataAdapter adapter = new SqlDataAdapter(objComm);
				DataTable dtResult = new DataTable();
				adapter.Fill(dtResult);

				List<CollectionEditLog> vms = dtResult.ToList<CollectionEditLog>();
				return vms;


			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

        public List<CollectionEditLog> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			string sqlText = "";
			List<CollectionEditLog> VMs = new List<CollectionEditLog>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"

                 SELECT
                 Id
                ,MR
                ,PCName
                ,UserId
                ,EditDate 
                ,Status
                ,SlNo
                ,DepositDate
                ,Deposit
                ,Record

                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
     
                FROM CollectionEditLog 

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
					.ToList<CollectionEditLog>();
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
			List<CollectionEditLog> VMs = new List<CollectionEditLog>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"
                 select count(Id)FilteredCount
                from CollectionEditLog  where 1=1 ";


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

        public CollectionEditLog Insert(CollectionEditLog model)
        {
			try
			{
				string sqlText = "";
				int count = 0;


				var command = CreateCommand(@" INSERT INTO CollectionEditLog(

                 MR
                ,PCName
                ,UserId
                ,EditDate 
                ,Status
                ,SlNo
                ,DepositDate
                ,Deposit
                ,Record
                
                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
                
                

                ) VALUES (


                 @MR
                ,@PCName
                ,@UserId
                ,@EditDate 
                ,@Status
                ,@SlNo
                ,@DepositDate
                ,@Deposit
                ,@Record
                 
                ,@CreatedBy
                ,@CreatedOn
                ,@CreatedFrom
               
                

                )SELECT SCOPE_IDENTITY()");




				command.Parameters.Add("@MR", SqlDbType.VarChar).Value = model.MR;
				command.Parameters.Add("@PCName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.PCName) ? (object)DBNull.Value : model.PCName;
				command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.UserId) ? (object)DBNull.Value : model.UserId;
				command.Parameters.Add("@EditDate", SqlDbType.DateTime).Value = string.IsNullOrEmpty(model.EditDate) ? (object)DBNull.Value : model.EditDate;
		
				command.Parameters.Add("@Status", SqlDbType.VarChar).Value = model.Status;
				command.Parameters.Add("@SlNo", SqlDbType.VarChar).Value = model.SlNo;
				command.Parameters.Add("@DepositDate", SqlDbType.DateTime).Value = model.DepositDate;
				command.Parameters.Add("@Deposit", SqlDbType.Decimal).Value = model.Deposit;
				command.Parameters.Add("@Record", SqlDbType.VarChar).Value = model.Record;

				command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = model.Audit.CreatedBy;
				command.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = model.Audit.CreatedOn;
				command.Parameters.Add("@CreatedFrom", SqlDbType.NVarChar).Value = model.Audit.CreatedFrom;

				
				model.Id = Convert.ToInt32(command.ExecuteScalar());


				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public CollectionEditLog Update(CollectionEditLog model)
        {
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"update CollectionEditLog set

 MR                          =@MR  
,PCName                      =@PCName  
,UserId                      =@UserId   
,EditDate                    =@EditDate  
,Status                      =@Status  
,SlNo                        =@SlNo  
,DepositDate                 =@DepositDate  
,Deposit                     =@Deposit  
,Record                      =@Record 

,LastUpdateBy                =@LastUpdateBy  
,LastUpdateOn                =@LastUpdateOn  
,LastUpdateFrom              =@LastUpdateFrom


   
                   
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

				var item = model.CollectionEditLogDetails.FirstOrDefault();

                command.Parameters.Add("@MR", SqlDbType.VarChar).Value = model.MR;
                command.Parameters.Add("@PCName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.PCName) ? (object)DBNull.Value : model.PCName;
                command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.UserId) ? (object)DBNull.Value : model.UserId;
                command.Parameters.Add("@EditDate", SqlDbType.DateTime).Value = string.IsNullOrEmpty(model.EditDate) ? (object)DBNull.Value : model.EditDate;

                command.Parameters.Add("@Status", SqlDbType.VarChar).Value = model.Status;
                command.Parameters.Add("@SlNo", SqlDbType.VarChar).Value = model.SlNo;
                command.Parameters.Add("@DepositDate", SqlDbType.DateTime).Value = model.DepositDate;
                command.Parameters.Add("@Deposit", SqlDbType.Decimal).Value = model.Deposit;
                command.Parameters.Add("@Record", SqlDbType.VarChar).Value = model.Record;


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
