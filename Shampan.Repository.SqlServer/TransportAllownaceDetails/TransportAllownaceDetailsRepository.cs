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
using Shampan.Core.Interfaces.Repository.TransportAllownaceDetails;

namespace Shampan.Repository.SqlServer.TransportAllownaceDetails
{
    public class TransportAllownaceDetailRepository : Repository, ITransportAllownaceDetailRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;

        public TransportAllownaceDetailRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

        }
        public List<TransportAllownaceDetail> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			try
			{
				string sqlText = @" 

                 SELECT
                 Id
                ,Date
                ,Particulars
                ,Amount 
                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
     
                FROM TransportAllownaceDetails


                where 1=1";


				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


				SqlCommand objComm = CreateCommand(sqlText);

				objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

				SqlDataAdapter adapter = new SqlDataAdapter(objComm);
				DataTable dtResult = new DataTable();
				adapter.Fill(dtResult);

				List<TransportAllownaceDetail> vms = dtResult.ToList<TransportAllownaceDetail>();
				return vms;


			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

        public List<TransportFoodAndOthers> GetAllFoodAndOthers(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @" 

                 SELECT
                 Id
                ,Date
                ,Details
                ,Amount 
     
                FROM TransportAllownaceOthers 


                where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<TransportFoodAndOthers> vms = dtResult.ToList<TransportFoodAndOthers>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<LessAdvance> GetAllLessAdvance(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @" 

                 SELECT
                 Id
                ,Date
                ,Details
                ,Amount 
     
                FROM TransportAllownaceLessAdvance 


                where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<LessAdvance> vms = dtResult.ToList<LessAdvance>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TransportAllownaceDetail> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
			string sqlText = "";
			List<TransportAllownaceDetail> VMs = new List<TransportAllownaceDetail>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"

                 SELECT
                 Id
                ,Date
                ,Particulars
                ,Amount                
                ,CreatedBy
                ,CreatedOn
                ,CreatedFrom
     
                FROM TransportAllownaceDetails 

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
					.ToList<TransportAllownaceDetail>();
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
			List<TransportAllownaceDetail> VMs = new List<TransportAllownaceDetail>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"
                 select count(Id)FilteredCount
                from TransportAllownaceDetails  where 1=1 ";


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

        public TransportAllownaceDetail Insert(TransportAllownaceDetail model)
        {
			try
			{
				string sqlText = "";
				int count = 0;


				var command = CreateCommand(@" INSERT INTO TransportAllownaceDetails(

                 TransportAllowanceId
                ,Date
                ,Particulars
                ,Amount

                --,CreatedBy
                --,CreatedOn
                --,CreatedFrom
                            
                ) VALUES (

                 @TransportAllowanceId
                ,@Date
                ,@Particulars
                ,@Amount

                --,@CreatedBy
                --,@CreatedOn
                --,@CreatedFrom
                              

                )SELECT SCOPE_IDENTITY()");





				command.Parameters.Add("@TransportAllowanceId", SqlDbType.Int).Value = model.TransportAllowanceId;
				command.Parameters.Add("@Date", SqlDbType.DateTime).Value = model.Date;
				command.Parameters.Add("@Particulars", SqlDbType.NVarChar).Value = model.Particulars;
				command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = model.Amount;


				//command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = model.Audit.CreatedBy;
				//command.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = model.Audit.CreatedOn;
				//command.Parameters.Add("@CreatedFrom", SqlDbType.NVarChar).Value = model.Audit.CreatedFrom;

				

				model.Id = Convert.ToInt32(command.ExecuteScalar());


				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public TransportFoodAndOthers InsertFoodAndOthers(TransportFoodAndOthers model)
        {
            try
            {
                string sqlText = "";
                int count = 0;


                var command = CreateCommand(@" INSERT INTO TransportAllownaceOthers(

                 TransportAllowanceId
                ,Date
                ,Details
                ,Amount


                ) VALUES (

                 @TransportAllowanceId
                ,@Date
                ,@Details
                ,@Amount         
                        

                )SELECT SCOPE_IDENTITY()");





                command.Parameters.Add("@TransportAllowanceId", SqlDbType.Int).Value = model.TransportAllowanceId;
                command.Parameters.Add("@Date", SqlDbType.DateTime).Value = model.Date;
                command.Parameters.Add("@Details", SqlDbType.NVarChar).Value = model.Details;
                command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = model.Amount;


               

                model.Id = Convert.ToInt32(command.ExecuteScalar());


                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LessAdvance InsertLessAdvance(LessAdvance model)
        {
            try
            {
                string sqlText = "";
                int count = 0;


                var command = CreateCommand(@" INSERT INTO TransportAllownaceLessAdvance(

                 TransportAllowanceId
                ,Date
                ,Details
                ,Amount


                ) VALUES (

                 @TransportAllowanceId
                ,@Date
                ,@Details
                ,@Amount         
                        

                )SELECT SCOPE_IDENTITY()");





                command.Parameters.Add("@TransportAllowanceId", SqlDbType.Int).Value = model.TransportAllowanceId;
                command.Parameters.Add("@Date", SqlDbType.DateTime).Value = model.Date;
                command.Parameters.Add("@Details", SqlDbType.NVarChar).Value = model.Details;
                command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = model.Amount;




                model.Id = Convert.ToInt32(command.ExecuteScalar());


                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TransportAllownaceDetail Update(TransportAllownaceDetail model)
        {
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update TransportAllownaceDetails set

 Date                          =@Date  
,Particulars                   =@Particulars  
,Amount                        =@Amount    
,LastUpdateBy                  =@LastUpdateBy  
,LastUpdateOn                  =@LastUpdateOn  
,LastUpdateFrom                =@LastUpdateFrom  
   
                   
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;


				command.Parameters.Add("@Date", SqlDbType.DateTime).Value = model.Date;
				command.Parameters.Add("@Particulars", SqlDbType.NVarChar).Value = model.Particulars;
				command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = model.Amount;

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
