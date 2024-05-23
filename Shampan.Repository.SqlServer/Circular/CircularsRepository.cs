using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Advance;
using Shampan.Core.Interfaces.Repository.Circular;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Core.Interfaces.Repository.Tour;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.Tour
{
	public class CircularsRepository : Repository, ICircularsRepository
    {
		public CircularsRepository(SqlConnection context, SqlTransaction transaction)
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

        public List<Circulars> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,Code
,CircularSummary
,CircularType
,CircularDate
,Attachment
,IsPublished
,CircularDetails


,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom


from Circulars
where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<Circulars> vms = dtResult.ToList<Circulars>();
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
            List<Circulars> VMs = new List<Circulars>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Circulars.Id)FilteredCount
                from Circulars  where 1=1 ";


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

        public List<Circulars> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<Circulars> VMs = new List<Circulars>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"select 
                  Circulars.Id
                 ,Circulars.Code
                 ,Circulars.CircularDate
                 ,Circulars.CircularDetails
                 --,Circulars.CircularType
                 ,CircularType.Name Name    

                 from Circulars 
                 
                 left outer join circulartype on Circulars.CircularType = CircularType.id

                 where 1=1 ";



                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new Circulars();

                VMs.Add(req);


                VMs = dt.ToList<Circulars>();

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
            List<Circulars> VMs = new List<Circulars>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                from Circulars  where 1=1 ";


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

		public Circulars Insert(Circulars model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO Circulars(



 Code
,CircularSummary
,CircularType
,CircularDate
,Attachment
,IsPublished
,CircularDetails


,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@CircularSummary
,@CircularType
,@CircularDate
,@Attachment
,@IsPublished
,@CircularDetails


,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");



				//foreach (AttachmentModel item in model.AttachmentDetailsModel)
				//{
				

				//	cmdDetails.Parameters.AddWithValue("@InvoiceId", model.Id);
				//	cmdDetails.Parameters.AddWithValue("@InvoiceNo", model.InvoiceNo);
				//	cmdDetails.Parameters.AddWithValue("@FileName", model.Id + "_" + item.FileName ?? Convert.DBNull);
				//	cmdDetails.ExecuteNonQuery();
				//}




				command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;
                command.Parameters.Add("@CircularSummary", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CircularSummary) ? (object)DBNull.Value : model.CircularSummary;       
                command.Parameters.Add("@CircularDetails", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CircularDetails) ? (object)DBNull.Value : model.CircularDetails;       
				command.Parameters.Add("@CircularType", SqlDbType.Int).Value = model.CircularType;
				command.Parameters.Add("@CircularDate", SqlDbType.DateTime).Value = model.CircularDate;
                command.Parameters.Add("@Attachment", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Attachment) ? (object)DBNull.Value : model.Attachment;
                command.Parameters.Add("@IsPublished", SqlDbType.Bit).Value = model.IsPublished;
		
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

		

		public Circulars Update(Circulars model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update Circulars set

 Code               =@Code  
,CircularSummary    =@CircularSummary
,CircularType       =@CircularType
,CircularDate       =@CircularDate
,Attachment         =@Attachment
,IsPublished        =@IsPublished
,CircularDetails        =@CircularDetails
  

,LastUpdateBy              =@LastUpdateBy  
,LastUpdateOn              =@LastUpdateOn  
,LastUpdateFrom            =@LastUpdateFrom 
                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
				command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;

                command.Parameters.Add("@CircularSummary", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CircularSummary) ? (object)DBNull.Value : model.CircularSummary;
                command.Parameters.Add("@CircularDetails", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CircularDetails) ? (object)DBNull.Value : model.CircularDetails;


                //command.Parameters.Add("@CircularSummary", SqlDbType.NChar).Value = model.CircularSummary;

                command.Parameters.Add("@CircularType", SqlDbType.Int).Value = model.CircularType;
                command.Parameters.Add("@CircularDate", SqlDbType.DateTime).Value = model.CircularDate;

                //command.Parameters.Add("@Attachment", SqlDbType.NChar).Value = model.Attachment;

                command.Parameters.Add("@Attachment", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Attachment) ? (object)DBNull.Value : model.Attachment;


                command.Parameters.Add("@IsPublished", SqlDbType.Bit).Value = model.IsPublished;



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
