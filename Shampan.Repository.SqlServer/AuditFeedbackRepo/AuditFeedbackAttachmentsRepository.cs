using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.AuditFeedbackRepo;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Repository.SqlServer.AuditFeedbackRepo
{
    public class AuditFeedbackAttachmentsRepository : Repository, IAuditFeedbackAttachmentsRepository
    {

        private DbConfig _dbConfig;
        public AuditFeedbackAttachmentsRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;
        }

        public List<AuditFeedbackAttachments> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditFeedbackAttachments> VMs = new List<AuditFeedbackAttachments>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

SELECT [Id]
      ,[AuditId]
      ,[AuditIssueId]
      ,[AuditFeedbackId]
      ,[FileName]
  FROM [A_AuditFeedbackAttachments] 

where 1=1  

";
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);

                VMs = dt.ToList<AuditFeedbackAttachments>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditFeedbackAttachments> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditFeedbackAttachments> VMs = new List<AuditFeedbackAttachments>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT [Id]
      ,[AuditId]
      ,[AuditIssueId]
      ,[AuditFeedbackId]
      ,[FileName]
  FROM [A_AuditFeedbackAttachments]  

where 1=1  



  where 1=1 
";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditFeedbackAttachments>();
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
select 
     count(Id)FilteredCount

    from FROM A_AuditFeedbackAttachments

  where 1=1 ";

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

        public AuditFeedbackAttachments Insert(AuditFeedbackAttachments model)
        {
            try
            {
                string sqlText = "";
                int Id = 0;


                sqlText = @"
insert into A_AuditFeedbackAttachments(
[AuditId]
,[AuditIssueId]
,[AuditFeedbackId]
,[FileName]
)
values( 
 @AuditId
,@AuditIssueId
,@AuditFeedbackId
,@FileName
  
)     SELECT SCOPE_IDENTITY() ";

                var command = CreateCommand(sqlText);


                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@AuditIssueId", SqlDbType.NVarChar).Value = model.AuditIssueId;
                command.Parameters.Add("@AuditFeedbackId", SqlDbType.NVarChar).Value = model.AuditFeedbackId;
                command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = model.FileName;

                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw;
            };
        }

        public AuditFeedbackAttachments Update(AuditFeedbackAttachments model)
        {
            try
            {

                string sql = "";

                sql = @"

Update A_AuditFeedbackAttachments
set
,AuditId=@AuditId
,AuditIssueId=@AuditIssueId
,AuditFeedbackId=@AuditFeedbackId
,FileName=@FileName


where Id=@Id
";


                var command = CreateCommand(sql);
                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@AuditIssueId", SqlDbType.NVarChar).Value = model.AuditIssueId;
                command.Parameters.Add("@AuditFeedbackId", SqlDbType.NVarChar).Value = model.AuditFeedbackId;
                command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = model.FileName;




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
