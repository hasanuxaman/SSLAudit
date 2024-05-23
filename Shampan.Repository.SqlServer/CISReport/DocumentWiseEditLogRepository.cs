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
    public class DocumentWiseEditLogRepository : Repository, IDocumentWiseEditLogRepository
	{
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;

        public DocumentWiseEditLogRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

        }
        public List<DocumentWiseEditLog> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {

                string sqlText = @" 

                 SELECT
                 Id
                ,DocNo
                ,PCName
                ,UserId
                ,EntryDate 
                ,ClassCode
                ,Status
                ,DocDate
                ,CustomerId
                ,NetPremium

                ,SumInsured
                ,VatAmount
                ,StampAmount
                ,ProducerCode
                ,BusinessStatus

                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
     

                FROM DocumentWiseEditLog


                where 1=1";


				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


				SqlCommand objComm = CreateCommand(sqlText);

				objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

				SqlDataAdapter adapter = new SqlDataAdapter(objComm);
				DataTable dtResult = new DataTable();
				adapter.Fill(dtResult);

				List<DocumentWiseEditLog> vms = dtResult.ToList<DocumentWiseEditLog>();
				return vms;


			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

        public List<DocumentWiseEditLog> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			string sqlText = "";
			List<DocumentWiseEditLog> VMs = new List<DocumentWiseEditLog>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"

                 SELECT
                 Id
                ,DocNo
                ,PCName
                ,UserId
                ,EntryDate 
                ,ClassCode
                ,Status
                ,DocDate
                ,CustomerId
                ,NetPremium

                ,SumInsured
                ,VatAmount
                ,StampAmount
                ,ProducerCode
                ,BusinessStatus

                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
     
                FROM DocumentWiseEditLog 

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
					.ToList<DocumentWiseEditLog>();
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
			List<DocumentWiseEditLog> VMs = new List<DocumentWiseEditLog>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"
                 select count(Id)FilteredCount
                from DocumentWiseEditLog  where 1=1 ";


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

        public DocumentWiseEditLog Insert(DocumentWiseEditLog model)
        {
			try
			{
				string sqlText = "";
				int count = 0;


				var command = CreateCommand(@" INSERT INTO DocumentWiseEditLog(


                 DocNo
                ,PCName
                ,UserId
                ,EntryDate 
                ,ClassCode
                ,Status
                ,DocDate
                ,CustomerId
                ,NetPremium

                ,SumInsured
                ,VatAmount
                ,StampAmount
                ,ProducerCode
                ,BusinessStatus

                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
                
                

                ) VALUES (



                 @DocNo
                ,@PCName
                ,@UserId
                ,@EntryDate 
                ,@ClassCode
                ,@Status
                ,@DocDate
                ,@CustomerId
                ,@NetPremium
                 
                ,@SumInsured
                ,@VatAmount
                ,@StampAmount
                ,@ProducerCode
                ,@BusinessStatus
				 
                ,@CreatedBy
                ,@CreatedOn
                ,@CreatedFrom
               
                

                )SELECT SCOPE_IDENTITY()");




				command.Parameters.Add("@DocNo", SqlDbType.VarChar).Value = model.DocNo;
				command.Parameters.Add("@PCName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.PCName) ? (object)DBNull.Value : model.PCName;
				command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.UserId) ? (object)DBNull.Value : model.UserId;
				command.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = string.IsNullOrEmpty(model.EntryDate) ? (object)DBNull.Value : model.EntryDate;
				command.Parameters.Add("@ClassCode", SqlDbType.VarChar).Value = model.ClassCode;
				command.Parameters.Add("@Status", SqlDbType.VarChar).Value = model.Status;
                command.Parameters.Add("@DocDate", SqlDbType.DateTime).Value = model.DocDate;

                command.Parameters.Add("@CustomerId", SqlDbType.VarChar).Value = model.CustomerId;
				command.Parameters.Add("@NetPremium", SqlDbType.Decimal).Value = model.NetPremium;
				command.Parameters.Add("@SumInsured", SqlDbType.Decimal).Value = model.SumInsured;
				command.Parameters.Add("@VatAmount", SqlDbType.Decimal).Value = model.VatAmount;
				command.Parameters.Add("@StampAmount", SqlDbType.Decimal).Value = model.StampAmount;
				command.Parameters.Add("@ProducerCode", SqlDbType.Decimal).Value = model.ProducerCode;
				command.Parameters.Add("@BusinessStatus", SqlDbType.VarChar).Value = model.BusinessStatus;

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

        public DocumentWiseEditLog Update(DocumentWiseEditLog model)
        {
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"update DocumentWiseEditLog set

 DocNo                         =@DocNo  
,PCName                        =@PCName  
,UserId                        =@UserId   
,EntryDate                     =@EntryDate  
,ClassCode                     =@ClassCode  
,Status                        =@Status  
,DocDate                       =@DocDate  
,CustomerId                    =@CustomerId  
,NetPremium                    =@NetPremium  
,SumInsured                     =@SumInsured 
,VatAmount                     =@VatAmount 
,StampAmount                   =@StampAmount 
,ProducerCode                  =@ProducerCode 
,BusinessStatus                =@BusinessStatus 

,LastUpdateBy                  =@LastUpdateBy  
,LastUpdateOn                  =@LastUpdateOn  
,LastUpdateFrom                =@LastUpdateFrom



   
                   
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

				var item = model.DocumentWiseEditLogDetails.FirstOrDefault();

                command.Parameters.Add("@DocNo", SqlDbType.VarChar).Value = model.DocNo;
                command.Parameters.Add("@PCName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.PCName) ? (object)DBNull.Value : model.PCName;
                command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = string.IsNullOrEmpty(model.UserId) ? (object)DBNull.Value : model.UserId;
                command.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = string.IsNullOrEmpty(model.EntryDate) ? (object)DBNull.Value : model.EntryDate;
                command.Parameters.Add("@ClassCode", SqlDbType.VarChar).Value = model.ClassCode;
                command.Parameters.Add("@Status", SqlDbType.VarChar).Value = model.Status;
                command.Parameters.Add("@DocDate", SqlDbType.DateTime).Value = model.DocDate;

                command.Parameters.Add("@CustomerId", SqlDbType.VarChar).Value = model.CustomerId;
                command.Parameters.Add("@NetPremium", SqlDbType.Decimal).Value = model.NetPremium;
                command.Parameters.Add("@SumInsured", SqlDbType.Decimal).Value = model.SumInsured;
                command.Parameters.Add("@VatAmount", SqlDbType.Decimal).Value = model.VatAmount;
                command.Parameters.Add("@StampAmount", SqlDbType.Decimal).Value = model.StampAmount;
                command.Parameters.Add("@ProducerCode", SqlDbType.Decimal).Value = model.ProducerCode;
                command.Parameters.Add("@BusinessStatus", SqlDbType.VarChar).Value = model.BusinessStatus;


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
