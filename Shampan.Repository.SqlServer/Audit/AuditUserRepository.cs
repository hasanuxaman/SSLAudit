using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Audit;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Repository.SqlServer.Audit
{
    public class AuditUserRepository : Repository, IAuditUserRepository
    {

        private DbConfig _dbConfig;
        public AuditUserRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

        }

		

		public List<AuditUser> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @"

SELECT  [Id]
      ,[AuditId]
      ,[UserId]
      ,(select UserName from GDICAuditAuthDB.dbo.AspNetUsers where Id = UserId) UserName
      ,[EmailAddress]
      ,[Remarks]
      ,[IssuePriority]

  FROM [AuditUsers]
WHERE 1=1

            ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<AuditUser> vms = dtResult.ToList<AuditUser>();
                return vms;

            }
            catch (Exception)
            {

                throw;
            };
        }

     

        public List<AuditUser> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditUser> VMs = new List<AuditUser>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

      SELECT  AU.[Id]
      ,AU.[AuditId]
      ,AU.[UserId]
      ,AU.TeamId
      --,(select UserName from GDICAuditAuthDB.dbo.AspNetUsers where Id = AU.UserId) UserName
	  ,usr.UserName
      ,[EmailAddress]
      ,[Remarks]
      ,[IssuePriority]
	  

      FROM [AuditUsers] AU

     left outer join GDICAuditAuthDB.dbo.AspNetUsers usr on usr.Id = AU.UserId
  
WHERE 1=1 
and AU.AuditId=@AuditId
";

                //if (index.self)
                //{
                //    sqlText = sqlText + "and AU.TeamId=@TeamId";
                //}

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

              
                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);
                //if (index.self)
                //{
                //    objComm.SelectCommand.Parameters.AddWithValue("@TeamId", index.TeamId);

                //}

                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditUser>();
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

            DataTable dt = new DataTable();
            try
            {
                sqlText = @"
 select count(ad.ID) FilteredCount
from  AuditUsers ad 

where 1=1 
and AuditId = @AuditId

";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);
                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0][0]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AuditUser Insert(AuditUser model)
        {
            try
            {
               
                string sqlText = "";
                int Id = 0;

                sqlText = @"
insert into AuditUsers(

[AuditId]
,[UserId]
,[EmailAddress]
,[Remarks]

,TeamId

, CreatedBy
, CreatedOn
, CreatedFrom
)
 values( 
 @AuditId
,@UserId
,@EmailAddress
,@Remarks

,@TeamId

,@CreatedBy
,@CreatedOn
,@CreatedFrom
     
)     SELECT SCOPE_IDENTITY() ";

                var command = CreateCommand(sqlText);

                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value =
                model.UserId is null ? DBNull.Value : model.UserId;
                command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = model.EmailAddress;
                //command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = model.Remarks;
                command.Parameters.Add("@Remarks", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Remarks) ? (object)DBNull.Value : model.Remarks;
                command.Parameters.Add("@TeamId", SqlDbType.NChar).Value = model.TeamId;

                //command.Parameters.Add("@IssuePriority", SqlDbType.NVarChar).Value = model.IssuePriority;
                command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = model.Audit.CreatedBy;
                command.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = model.Audit.CreatedOn;
                command.Parameters.Add("@CreatedFrom", SqlDbType.NVarChar).Value = model.Audit.CreatedFrom;



                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public AuditUser Update(AuditUser model)
        {
            try
            {

                string sql = "";

                sql = @"

Update [AuditUsers]
set

 AuditId = @AuditId
,UserId = @UserId
,EmailAddress = @EmailAddress
,Remarks = @Remarks
--,IssuePriority = @IssuePriority

,LastUpdateBy =   @LastUpdateBy
,LastUpdateOn =   @LastUpdateOn
,LastUpdateFrom = @LastUpdateFrom

where Id=@Id
";


                var command = CreateCommand(sql);


                command.Parameters.AddWithValue("@Id", model.Id);
                command.Parameters.AddWithValue("@AuditId", model.AuditId);
                command.Parameters.AddWithValue("@UserId", model.UserId);

                command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                command.Parameters.Add("@Remarks", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Remarks) ? (object)DBNull.Value : model.Remarks;

                //command.Parameters.AddWithValue("@Remarks", model.Remarks);
                //command.Parameters.AddWithValue("@IssuePriority", model.IssuePriority);

                command.Parameters.AddWithValue("@LastUpdateBy", model.Audit.LastUpdateBy);
                command.Parameters.AddWithValue("@LastUpdateOn", model.Audit.LastUpdateOn);
                command.Parameters.AddWithValue("@LastUpdateFrom", model.Audit.LastUpdateFrom);

                int rowcount = Convert.ToInt32(command.ExecuteNonQuery());

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
