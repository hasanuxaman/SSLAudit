using System;
using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.AuditFeedbackRepo;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Repository.SqlServer.AuditFeedbackRepo
{
    public class AuditFeedbackRepository : Repository, IAuditFeedbackRepository
    {

        private DbConfig _dbConfig;
        public AuditFeedbackRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;
        }

        public List<AuditFeedback> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditFeedback> VMs = new List<AuditFeedback>();
            DataTable dt = new DataTable();
            try
            {
                sqlText = @"


      SELECT af.[Id]
      ,af.[AuditId]
      ,af.[AuditIssueId]
      ,af.[IssueDetails]
      ,af.[Heading]
      ,af.[IsPost]
      ,af.[CreatedBy]
      ,af.[CreatedOn]
      ,af.[CreatedFrom]
      ,af.[LastUpdateBy]
      ,af.[LastUpdateOn]
      ,af.[LastUpdateFrom]
      ,isnull(af.[IsPosted],0)IsPosted
      ,af.[PostedBy]
      ,af.[PostedOn]
      ,af.[PostedFrom]

	  ,afba.FileName

	  ,ad.TeamId


  FROM [A_AuditFeedbacks] as af

  left outer join A_AuditFeedbackAttachments  afba on af.Id=afba.AuditFeedbackId
  left outer join A_Audits ad on ad.Id = af.AuditId

  where 1=1  




      --SELECT af.[Id]
      --,af.[AuditId]
      --,af.[AuditIssueId]
      --,[IssueDetails]
      --,[Heading]
      --,[IsPost]
      --,[CreatedBy]
      --,[CreatedOn]
      --,[CreatedFrom]
      --,[LastUpdateBy]
      --,[LastUpdateOn]
      --,[LastUpdateFrom]
      --,isnull([IsPosted],0)IsPosted
      --,[PostedBy]
      --,[PostedOn]
      --,[PostedFrom]
	  --,afba.FileName
      -- FROM [A_AuditFeedbacks] as af
      -- left outer join A_AuditFeedbackAttachments  afba on af.Id=afba.AuditFeedbackId
       --where 1=1  

";
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);

                VMs = dt.ToList<AuditFeedback>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditFeedback> GetAuditFeedbackByAuditId(AuditMaster model)
        {
            string sqlText = "";
            List<AuditFeedback> VMs = new List<AuditFeedback>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

       SELECT

      DISTINCT [AuditIssueId]
      
      FROM [A_AuditFeedbacks] 
  
      WHERE 1=1 AND AuditId = @AuditId  


";

                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                //sqlText += @"  order by  " + index.OrderName.Replace("AuditName", "ad.Name") + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", model.Id);


                objComm.Fill(dt);

                VMs = dt.ToList<AuditFeedback>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditIssue> GetAuditLastFeedback(AuditIssue model)
        {
            try
            {
                string sqlText = @"


        SELECT  
        A_AuditIssues.Id
       ,AuditId
      ,IssueName
      ,IssueDetails
      ,DateOfSubmission
      --,IssuePriority
      --,AuditType
      ,IssueStatus
      ,ReportStatus
      ,Risk


      ,isnull([InvestigationOrForensis] ,'0')InvestigationOrForensis
      ,isnull([StratigicMeeting] ,'0')StratigicMeeting
      ,isnull([ManagementReviewMeeting] ,'0')ManagementReviewMeeting
      ,isnull([OtherMeeting] ,'0')OtherMeeting
      ,isnull([Training] ,'0')Training

      ,isnull([Operational] ,'0')Operational
      ,isnull([Financial] ,'0')Financial
      ,isnull([Compliance] ,'0')Compliance

      --,InvestigationOrForensis
      --,StratigicMeeting
      --,ManagementReviewMeeting
      --,OtherMeeting
      --,Training


      ,CreatedBy
      ,CreatedOn
      ,CreatedFrom
      ,LastUpdateBy
      ,LastUpdateOn
      ,LastUpdateFrom
      ,isnull(IsPosted,0)IsPosted
      ,PostedBy
      ,PostedOn
      ,PostedFrom
      ,IsPost

      ,e.EnumValue

  FROM A_AuditIssues

  left outer join Enums e on e.Id = A_AuditIssues.IssuePriority

where 1=1  and AuditId =@AuditId and  A_AuditIssues.Id=@Id 


--select ID Value, EnumValue Name  from Enums where EnumType = 'IssuePriority'

      --select top(1)
       --A_AuditFeedbacks.[Id]
      --,A_AuditFeedbacks.[AuditId]
      --,A_AuditFeedbacks.[AuditIssueId]
      --,A_AuditFeedbacks.[IssueDetails]
       --,A_AuditFeedbacks.[Heading]
	   --,ai.IssueName
--from A_AuditFeedbacks  
--left outer join A_AuditIssues ai on  ai.Id = A_AuditFeedbacks.AuditIssueId
--WHERE 1=1 and
--A_AuditFeedbacks.AuditId =@AuditId and  AuditIssueId=@AuditIssueId order by Id desc




";


                sqlText = ApplyConditions(sqlText, null, null);

                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, null, null);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);


                adapter.SelectCommand.Parameters.AddWithValue("@AuditId", model.AuditId);
                //adapter.SelectCommand.Parameters.AddWithValue("@AuditIssueId", model.Id);
                adapter.SelectCommand.Parameters.AddWithValue("@Id", model.Id);



                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<AuditIssue> vms = dtResult.ToList<AuditIssue>();
                return vms;

            }
            catch (Exception)
            {

                throw;
            };
        }

        public List<AuditFeedback> GetAuditUsers(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            try
            {
                string sqlText = @"

       Select 
       usr.UserName
       From AuditUsers AU

       left outer join GDICAuditAuthDB.dbo.AspNetUsers usr on usr.Id = AU.UserId

       WHERE 1=1

            ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<AuditFeedback> vms = dtResult.ToList<AuditFeedback>();
                return vms;

            }
            catch (Exception)
            {

                throw;
            };
        }

        public List<AuditFeedback> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditFeedback> VMs = new List<AuditFeedback>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT af.[Id]
      ,af.[AuditId]
      ,af.[AuditIssueId]
      ,af.[IssueDetails]
      ,af.[CreatedBy]
      --,af.[CreatedOn]
       ,Format(af.CreatedOn , 'dd-MM-yyyy   -   hh:mm:ss tt') CreatedOn
      ,af.[CreatedFrom]
      ,af.[LastUpdateBy]
      ,af.[LastUpdateOn]
      ,af.[LastUpdateFrom]
      ,af.[Heading]
--     ,af.[IsPosted]
--     ,af.[PostedBy]
--     ,af.[PostedOn]
--     ,af.[PostedFrom]
	  ,ad.Name AuditName
	  ,ai.IssueName
	  ,af.IsPost


  FROM [A_AuditFeedbacks] af 
  left outer join A_Audits ad on af.AuditId = ad.Id
  left outer join A_AuditIssues ai on ai.Id = af.AuditIssueId
   
where 1=1
and af.AuditId = @AuditId  


";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName.Replace("AuditName", "ad.Name") + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);


                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditFeedback>();
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

SELECT count(af.Id)FilteredCount
  FROM [A_AuditFeedbacks] af 
  left outer join A_Audits ad on af.AuditId = ad.Id
   
where 1=1
and af.AuditId = @AuditId  

";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);

                objComm.Fill(dt);
                //return Convert.ToInt32(dt.Rows);
                return Convert.ToInt32(dt.Rows[0]["FilteredCount"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AuditFeedback Insert(AuditFeedback model)
        {
            try
            {
                string sqlText = "";
                int Id = 0;


                sqlText = @"

INSERT INTO [dbo].[A_AuditFeedbacks]
           (
 [AuditId]
,[AuditIssueId]
,[IssueDetails]
,[Heading]

,[IsPost]

,[CreatedBy]
,[CreatedOn]
,[CreatedFrom]

		   )
VALUES
(
 @AuditId
,@AuditIssueId
,@IssueDetails
,@Heading


,@IsPost


,@CreatedBy
,@CreatedOn
,@CreatedFrom

)   

SELECT SCOPE_IDENTITY() ";

                var command = CreateCommand(sqlText);

                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@AuditIssueId", SqlDbType.NVarChar).Value = model.AuditIssueId;
                //command.Parameters.Add("@IssueDetails", SqlDbType.NVarChar).Value = model.IssueDetails;
                command.Parameters.Add("@IssueDetails", SqlDbType.NVarChar).Value = model.FeedbackDetails is null ? DBNull.Value : model.FeedbackDetails;
                ///command.Parameters.Add("@IssueDetails", SqlDbType.NVarChar).Value = model.FeedbackDetails is null ? DBNull.Value : model.FeedbackDetails;

                command.Parameters.Add("@Heading", SqlDbType.NVarChar).Value = model.Heading;

                command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = model.Audit.CreatedBy;
                command.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = model.Audit.CreatedOn;
                command.Parameters.Add("@CreatedFrom", SqlDbType.NVarChar).Value = model.Audit.CreatedFrom;



                command.Parameters.Add("@IsPost", SqlDbType.NVarChar).Value = "N";


                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public AuditFeedback MultiplePost(AuditFeedback vm)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  ";
                SqlCommand command = CreateCommand(query);

                foreach (string ID in vm.IDs)
                {

                    query = @"  update A_AuditFeedbacks set 
      IsPost=@IsPost
     ,PostedBy=@PostedBy
     ,PostedOn=@PostedOn
     ,PostedFrom=@PostedFrom
      where  Id= @Id ";
                    command = CreateCommand(query);

                    command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "Y";
                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = ID;
                    command.Parameters.Add("@PostedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedBy.ToString()) ? (object)DBNull.Value : vm.Audit.PostedBy.ToString();
                    command.Parameters.Add("@PostedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedOn.ToString()) ? (object)DBNull.Value : vm.Audit.PostedOn.ToString();
                    command.Parameters.Add("@PostedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedFrom.ToString()) ? (object)DBNull.Value : vm.Audit.PostedFrom.ToString();

                    rowcount = command.ExecuteNonQuery();




                }
                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.UpdateFail);
                }
                return vm;
            }


            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AuditFeedback MultipleUnPost(AuditFeedback vm)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  ";
                SqlCommand command = CreateCommand(query);

                foreach (string ID in vm.IDs)
                {

                    query = @"  update A_AuditFeedbacks set 
     IsPost=@Post                   
    ,ReasonOfUnPost=@ReasonOfUnPost
    ,LastUpdateBy=@LastUpdateBy
    ,LastUpdateOn=@LastUpdateOn
    ,LastUpdateFrom=@LastUpdateFrom

      where  Id= @Id ";
                    command = CreateCommand(query);

                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
                    command.Parameters.Add("@Post", SqlDbType.NChar).Value = "N";
                    command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedBy.ToString()) ? (object)DBNull.Value : vm.Audit.PostedBy.ToString();
                    command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedOn.ToString()) ? (object)DBNull.Value : vm.Audit.PostedOn.ToString();
                    command.Parameters.Add("@LastUpdateFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedFrom.ToString()) ? (object)DBNull.Value : vm.Audit.PostedFrom.ToString();
                    command.Parameters.Add("@ReasonOfUnPost", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.UnPostReasonOfFeedback.ToString()) ? (object)DBNull.Value : vm.UnPostReasonOfFeedback.ToString();
                    rowcount = command.ExecuteNonQuery();








                }
                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.UpdateFail);
                }
                return vm;
            }


            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AuditFeedback Update(AuditFeedback model)
        {
            try
            {
                string sql = @"

    UPDATE A_AuditFeedbacks
    SET
    AuditId = @AuditId,
    AuditIssueId = @AuditIssueId,
    IssueDetails = @IssueDetails,
    Heading = @Heading,
    LastUpdateBy = @LastUpdateBy,
    LastUpdateOn = @LastUpdateOn,
    LastUpdateFrom = @LastUpdateFrom
    WHERE Id = @Id
    ";

                // Create a SQL command using the SQL statement
                var command = CreateCommand(sql);

                // Set the parameter values for the command
                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@AuditIssueId", SqlDbType.Int).Value = model.AuditIssueId;
                //command.Parameters.Add("@IssueDetails", SqlDbType.NVarChar).Value = model.FeedbackDetails;

                command.Parameters.Add("@IssueDetails", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.FeedbackDetails) ? (object)DBNull.Value : model.FeedbackDetails;


                command.Parameters.Add("@Heading", SqlDbType.NVarChar).Value = model.Heading;
                command.Parameters.Add("@LastUpdateBy", SqlDbType.NVarChar).Value = model.Audit.LastUpdateBy;
                command.Parameters.Add("@LastUpdateOn", SqlDbType.DateTime).Value = model.Audit.LastUpdateOn;
                command.Parameters.Add("@LastUpdateFrom", SqlDbType.NVarChar).Value = model.Audit.LastUpdateFrom;

                // Execute the update query and retrieve the number of affected rows
                int rowCount = Convert.ToInt32(command.ExecuteNonQuery());

                if (rowCount <= 0)
                {
                    // If no rows were updated, throw an exception indicating failure
                    throw new Exception(MessageModel.UpdateFail);
                }

                // Return the updated model object
                return model;
            }
            catch
            {
                // Rethrow any exception that occurred during the update process
                throw;
            }
        }
    }
}
