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

namespace Shampan.Repository.SqlServer.Deshboard
{
    public class DeshboardRepository : Repository, IDeshboardRepository
    {

        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;

       
        public DeshboardRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;
            
        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public List<AuditIssueUser> AuditBranchUserGetAll()
        {
            try
            {
                string sqlText = @"select 
                 
                 AIU.Id
                ,AuditId
                ,AuditIssueId
                ,UserId
                ,EmailAddress 
				,ASP.UserName
                
                from  AuditIssueUsers AIU
                Left outer join [GDICAuditAuthDB].[dbo].[AspNetUsers] ASP on ASP.Id = AIU.UserId
                where 1=1";


                sqlText = ApplyConditions(sqlText,null,null);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, null, null);
                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<AuditIssueUser> vms = dtResult.ToList<AuditIssueUser>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int BeforeDeadLineIssue(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"


             SELECT 
             COUNT(A.Id)Id			 
             FROM A_AuditIssues AI

             LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
             --LEFT OUTER JOIN Enums E on E.Id=AI.IssuePriority

             WHERE 1=1 AND AI.IssueStatus != '1046' AND 
			 
			 --CAST(AI.IssueDeadLine AS DATE) <= CAST(GETDATE() AS DATE)
             CAST(AI.IssueDeadLine AS DATE) > CAST(GETDATE() AS DATE)

			 AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks) 





                 --SELECT Count(A_AuditIssues.Id)Id 
                 --FROM A_AuditIssues
                 --WHERE GETDATE() <= A_AuditIssues.IssueDeadLine
                 
                ";

                //IssueDeadLine <= DateOfSubmission

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
        
                //objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CheckExists(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

		public string CodeGeneration(string codeGroup, string codeName)
		{
			try
			{
				
				string codeGeneration = GenerateCode(codeGroup, codeName);
				return codeGeneration;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public int DeadLineForResponse(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {

                //string query = " select Id,UserName from [GDICAuditAuthDB].[dbo].[AspNetUsers] authuser where authuser.UserName=@userName ";
                //sqlText = ApplyConditions(sqlText, null, null);
                //SqlDataAdapter obj = CreateAdapter(query);
                //obj.SelectCommand = ApplyParameters(obj.SelectCommand, null, null);
                //obj.SelectCommand.Parameters.AddWithValue("@userName", UserName);

                //obj.Fill(authdt);
                //string id = null;
                //foreach (DataRow row in authdt.Rows)
                //{
                //    id = row["Id"].ToString();

                //}


                sqlText = @"

      SELECT Count(AI.Id)Id

       from A_AuditIssues AI

       left outer join A_Audits A on AI.AuditId=A.Id
       left outer join Enums E on E.Id=AI.IssuePriority

       WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)

	   --and AI.Id not in(select AuditId from A_AuditBranchFeedbacks)

	   and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks) and AI.createdby = @UserName
                ";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserId", id);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);


                objComm.Fill(dt);


                //return Convert.ToInt32(dt.Rows[0][0]);
                return Convert.ToInt32(dt.Rows[0]["Id"]);


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

        public int FinalAuidtApproved(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {

 
                sqlText = @"

                 SELECT Count(A_Audits.Id)Id
                 FROM A_Audits
                 where A_Audits.IssueIsApprovedL4=1
                 --where A_Audits.CreatedBy='erp' and A_Audits.BFIsApprovedL4=1

                ";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public int FollowUpAuditIssues(string UserName)
		{
			string sqlText = "";
			DataTable dt = new DataTable();
			DataTable authdt = new DataTable();
			try
			{
				sqlText = @"

                 SELECT 
                 COUNT(A.Id)Id
                 FROM A_AuditIssues AI
                 LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
                 WHERE 1=1 AND AI.IssueStatus != '1046' AND			 
			     CAST(AI.ImplementationDate AS DATE) <= CAST(GETDATE() AS DATE)
			     AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks)



                 --(SELECT Count(A_AuditIssues.Id)Id 
                 --FROM A_AuditIssues
                 --where 1=1 AND IssueStatus != '1046' AND A_AuditIssues.CreatedBy=@UserName AND ImplementationDate <  GetDate())

                 ";

				sqlText = ApplyConditions(sqlText, null, null);
				SqlDataAdapter objComm = CreateAdapter(sqlText);
				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				//objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

				objComm.Fill(dt);

				return Convert.ToInt32(dt.Rows[0]["Id"]);


			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<AuditMaster> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @"select 

u.Id,
A.UserName,

U.UserId,
U.BranchId,
B.BranchName





--from [AuthDB].[dbo]. AspNetUsers A
from [GDICAuditAuthDB].[dbo]. AspNetUsers A
left outer join UserBranchMap U on U.UserId=A.Id
left outer join BranchProfiles B on B.BranchId=U.BranchId
WHERE U.UserId IS NOT NULL";

                //SRCELEDGER + '-' + SRCETYPE
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResutl = new DataTable();
                adapter.Fill(dtResutl);

                List<AuditMaster> vms = dtResutl.ToList<AuditMaster>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public UserBranch GetBranchName(string UserId)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            List<UserBranch> vms = new List<UserBranch>();

            try
            {


                sqlText = @"

                 SELECT BranchName
                 from 
                 [GDICAuditAuthDB].[dbo].AspNetUsers 
                  WHERE Id =@Id

                ";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@Id", UserId);
                objComm.Fill(dt);

                vms = dt.ToList<UserBranch>();
          
                return vms.FirstOrDefault();


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(UserBranchMap.UserId)FilteredCount
                from UserBranchMap  where 1=1 ";


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
    

        public List<AuditMaster> GetIndexData(IndexModel Index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"Select
u.Id,
A.UserName,

U.UserId,
U.BranchId,
B.BranchName





from  [GDICAuditAuthDB].[dbo]. AspNetUsers A
left outer join UserBranchMap U on U.UserId=A.Id
left outer join BranchProfiles B on B.BranchId=U.BranchId
WHERE U.UserId IS NOT NULL"; 



                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + Index.OrderName + "  " + Index.orderDir;
                sqlText += @" OFFSET  " + Index.startRec + @" ROWS FETCH NEXT " + Index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new AuditMaster();

                VMs.Add(req);


                VMs = dt
                .ToList<AuditMaster>();

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
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                from UserBranchMap  where 1=1 ";


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

        public PrePaymentMaster GetPrePayment()
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            List<PrePaymentMaster> VMs = new List<PrePaymentMaster>();
            try
            {
                sqlText = @"

    SELECT    
    FORMAT(COALESCE(SUM(0), 0), '0.00') AS PreviousAmount,
    FORMAT(COALESCE(count(Id), 0), '0.00') AS CorrectedAmount,
    FORMAT(COALESCE(SUM(AdditionalPayment), 0), '0.00') AS AdditionalPayment 
    FROM 
    Financial
    ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                //objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                objComm.Fill(dt);                
                VMs = dt.ToList<PrePaymentMaster>();
                return VMs.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public List<AuditComponent> GetUnPlanAuditComponents()
        {

            string sqlText = "";
            List<AuditComponent> VMs = new List<AuditComponent>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"



SELECT
    AT.AuditType AS [Name],
    COUNT(CASE WHEN a.IsPlaned = 0 THEN 1 ELSE NULL END) AS BranchPlan,
    COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) AS BranchOngoin,
    COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END) AS BranchCompleted,
    (COUNT(CASE WHEN a.IsPlaned = 0 THEN 1 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)) AS BranchRemaining,
    (CASE
        WHEN (COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) + COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)) = 0 THEN 0
        ELSE (100 * COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END) / (COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) + COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)))
    END) AS [% (Completed + Ongoing)]
FROM   
    AuditTypes AS AT
LEFT JOIN  
    A_Audits AS a ON AT.Id = a.AuditTypeId
	where a.IsPlaned=0
GROUP BY
    AT.AuditType


                ";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                //objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);


                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditComponent>();

                //VMs[0].BranchCompletedOngoing = VMs[0].BranchCompleted + VMs[0].BranchOngoin;
                //VMs[0].BranchCompletedOngoing = 60;

               

                foreach (AuditComponent audit in VMs)
                {

                    audit.BranchCompletedOngoing = ((audit.BranchCompleted + audit.BranchOngoin) / audit.BranchPlan) * 100;
                }

                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        public AuditMaster Insert(AuditMaster model)
        {
            try
            {
                // Check if user ID has already been assigned to the branch
                string selectQuery = @"SELECT COUNT(*) FROM UserBranchMap WHERE UserId = @UserId AND BranchId = @BranchId";
                var selectCommand = CreateCommand(selectQuery);
                selectCommand.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
                //selectCommand.Parameters.Add("@BranchId", SqlDbType.NChar).Value = model.BranchId;

                int count = Convert.ToInt32(selectCommand.ExecuteScalar());
                if (count > 0)
                {
                    throw new Exception("User ID has already been assigned to the branch");
                }

               
                //  string[] retResults = { "Fail", "Fail", Id.ToString(), sqlText, "ex", "Insert" };

                var command = CreateCommand(@" INSERT INTO UserBranchMap(




 UserId
,BranchId
,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


@UserId
,@BranchId


,@CreatedBy
,@CreatedOn
,@CreatedFrom




)SELECT SCOPE_IDENTITY()");
                command.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
                //command.Parameters.Add("@BranchId", SqlDbType.NChar).Value = model.BranchId;



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

		public int MissDeadLineIssues(string UserName)
		{
			string sqlText = "";
			DataTable dt = new DataTable();
			DataTable authdt = new DataTable();
			try
			{
				sqlText = @"


             SELECT 
             COUNT(A.Id)Id			 
             FROM A_AuditIssues AI

             LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
             --LEFT OUTER JOIN Enums E on E.Id=AI.IssuePriority

             WHERE 1=1 AND AI.IssueStatus != '1046'  
			 			 
			 --AND CAST(AI.IssueDeadLine AS DATE) > CAST(GETDATE() AS DATE)

             AND CAST(GETDATE() AS DATE) >=  CAST(AI.IssueDeadLine AS DATE)
			 AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks) 




                --(SELECT 
				--Count(AI.Id)Id 
                --FROM A_AuditIssues AI
				--left join AuditIssueUsers as AIU On AIU.AuditIssueId=AI.Id
                --where
				--AIU.UserId=(select UserId from GDICAuditAuthDB..AspNetUsers where UserName=@UserName ) 
				--and  IssueDeadLine <= DateOfSubmission)

                ";

				sqlText = ApplyConditions(sqlText, null, null);
				SqlDataAdapter objComm = CreateAdapter(sqlText);
				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				//objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

				objComm.Fill(dt);

				return Convert.ToInt32(dt.Rows[0]["Id"]);


			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<PrePaymentMaster> NonFinancialGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{

			try
			{
				string sqlText = @"select 
                Id
                ,Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,Format(FinalCorrectionDate , 'yyyy-MM-dd') FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks

 from  NonFinancial
 where 1=1";


				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


				SqlCommand objComm = CreateCommand(sqlText);

				objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

				SqlDataAdapter adapter = new SqlDataAdapter(objComm);
				DataTable dtResult = new DataTable();
				adapter.Fill(dtResult);

				List<PrePaymentMaster> vms = dtResult.ToList<PrePaymentMaster>();
				return vms;


			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public List<PrePaymentMaster> NonFinancialGetGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public List<PrePaymentMaster> NonFinancialGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			string sqlText = "";
			List<PrePaymentMaster> VMs = new List<PrePaymentMaster>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"
                 SELECT 
                 Id
                ,Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,Format(FinalCorrectionDate , 'yyyy-MM-dd') FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks
     
      FROM NonFinancial 

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
					.ToList<PrePaymentMaster>();
				return VMs;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public int NonFinancialGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			string sqlText = "";
			List<Teams> VMs = new List<Teams>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"
                 select count(Id)FilteredCount
                from NonFinancial  where 1=1 ";


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

		public PrePaymentMaster NonFinancialInsert(PrePaymentMaster objMaster)
		{
			try
			{
				string sqlText = "";
				int count = 0;


				var command = CreateCommand(@" INSERT INTO NonFinancial(

                 Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks
                

                ) VALUES (

                 @Code
                ,@Auditor
                ,@EmployeeName
                ,@Details 
                ,@FinalCorrectionDate 
                ,@PreviousAmount
                ,@CorrectedAmount
                ,@AdditionalPayment
                ,@PaymentMemoReferenceNo
                ,@Department
                ,@Remarks
                

                )SELECT SCOPE_IDENTITY()");



				//command.Parameters.Add("@Code", SqlDbType.VarChar).Value = objMaster.Code.ToString();
				//command.Parameters.Add("@ReceiptNumber", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.ReceiptNumber) ? (object)DBNull.Value : objMaster.ReceiptNumber;

				command.Parameters.Add("@Code", SqlDbType.VarChar).Value = objMaster.Code;
				command.Parameters.Add("@Auditor", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Auditor) ? (object)DBNull.Value : objMaster.Auditor;
				command.Parameters.Add("@EmployeeName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.EmployeeName) ? (object)DBNull.Value : objMaster.EmployeeName;
				command.Parameters.Add("@Details", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Details) ? (object)DBNull.Value : objMaster.Details;
				//command.Parameters.Add("@Details ", SqlDbType.VarChar).Value = objMaster.Details;
				command.Parameters.Add("@FinalCorrectionDate", SqlDbType.DateTime).Value = objMaster.FinalCorrectionDate;
				command.Parameters.Add("@PreviousAmount", SqlDbType.Decimal).Value = objMaster.PreviousAmount;
				command.Parameters.Add("@CorrectedAmount", SqlDbType.Decimal).Value = objMaster.CorrectedAmount;
				command.Parameters.Add("@AdditionalPayment", SqlDbType.Decimal).Value = objMaster.AdditionalPayment;

				command.Parameters.Add("@PaymentMemoReferenceNo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.PaymentMemoReferenceNo) ? (object)DBNull.Value : objMaster.PaymentMemoReferenceNo;
				command.Parameters.Add("@Department", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Department) ? (object)DBNull.Value : objMaster.Department;
				command.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Remarks) ? (object)DBNull.Value : objMaster.Remarks;

				// command.Parameters.Add("@PaymentMemoReferenceNo", SqlDbType.VarChar).Value = objMaster.PaymentMemoReferenceNo;
				//command.Parameters.Add("@Department", SqlDbType.VarChar).Value = objMaster.Department;
				//command.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = objMaster.Remarks;


				objMaster.Id = Convert.ToInt32(command.ExecuteScalar());


				return objMaster;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public PrePaymentMaster NonFinancialUpdate(PrePaymentMaster objMaster)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update NonFinancial set

 Auditor                        =@Auditor  
,EmployeeName                   =@EmployeeName  
,Details                        =@Details   
,FinalCorrectionDate            =@FinalCorrectionDate  
,PreviousAmount                 =@PreviousAmount  
,CorrectedAmount                =@CorrectedAmount  
,AdditionalPayment              =@AdditionalPayment  
,PaymentMemoReferenceNo         =@PaymentMemoReferenceNo  
,Department                     =@Department  
,Remarks                        =@Remarks  
   
                   
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = objMaster.Id;



				var item = objMaster.NonFinancialDetails.FirstOrDefault();


				//command.Parameters.Add("@Code", SqlDbType.VarChar).Value = objMaster.Code.ToString();
				//command.Parameters.Add("@ReceiptNumber", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.ReceiptNumber) ? (object)DBNull.Value : objMaster.ReceiptNumber;


				command.Parameters.Add("@Auditor", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Auditor) ? (object)DBNull.Value : item.Auditor;
				command.Parameters.Add("@EmployeeName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.EmployeeName) ? (object)DBNull.Value : item.EmployeeName;
				command.Parameters.Add("@Details", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Details) ? (object)DBNull.Value : item.Details;
				//command.Parameters.Add("@Details ", SqlDbType.VarChar).Value = objMaster.Details;
				command.Parameters.Add("@FinalCorrectionDate", SqlDbType.DateTime).Value = item.FinalCorrectionDate;
				command.Parameters.Add("@PreviousAmount", SqlDbType.Decimal).Value = item.PreviousAmount;
				command.Parameters.Add("@CorrectedAmount", SqlDbType.Decimal).Value = item.CorrectedAmount;
				command.Parameters.Add("@AdditionalPayment", SqlDbType.Decimal).Value = item.AdditionalPayment;

				command.Parameters.Add("@PaymentMemoReferenceNo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.PaymentMemoReferenceNo) ? (object)DBNull.Value : item.PaymentMemoReferenceNo;
				command.Parameters.Add("@Department", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Department) ? (object)DBNull.Value : item.Department;
				command.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Remarks) ? (object)DBNull.Value : item.Remarks;

				// command.Parameters.Add("@PaymentMemoReferenceNo", SqlDbType.VarChar).Value = objMaster.PaymentMemoReferenceNo;
				//command.Parameters.Add("@Department", SqlDbType.VarChar).Value = objMaster.Department;
				//command.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = objMaster.Remarks;


				int rowcount = command.ExecuteNonQuery();
				if (rowcount <= 0)
				{
					throw new Exception(MessageModel.UpdateFail);
				}
				return objMaster;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int PendingAuditApproval(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {

                sqlText = @"

                 --SELECT Count(A_Audits.Id)Id
                 --FROM A_Audits
                 --where A_Audits.IsApprovedL4 != 1



DECLARE @A1 VARCHAR(1);
DECLARE @A2 VARCHAR(1);
DECLARE @A3 VARCHAR(1);
DECLARE @A4 VARCHAR(1);
DECLARE @UserId VARCHAR(MAX);

SELECT @UserId = Id FROM GDICAuditAuthDB.dbo.AspNetUsers WHERE UserName = @UserName;

CREATE TABLE #TempId(Id INT);

SELECT @A1 = AuditApproval1, @A2 = AuditApproval2, @A3 = AuditApproval3, @A4 = AuditApproval4  
FROM UserRolls
WHERE IsAudit = 1 AND UserId = @UserId;

IF (@A4 = 1)
BEGIN
    INSERT INTO #TempId(Id)
    SELECT Id FROM A_Audits WHERE IsRejected = 0 AND IsApprovedL1 = 1 AND IsApprovedL2 = 1 AND IsApprovedL3 = 1 AND IsApprovedL4 = 0;
END

IF (@A3 = 1)
BEGIN
    INSERT INTO #TempId(Id)
    SELECT Id FROM A_Audits WHERE IsRejected = 0 AND IsApprovedL1 = 1 AND IsApprovedL2 = 1 AND IsApprovedL3 = 0 AND IsApprovedL4 = 0;
END

IF (@A2 = 1)
BEGIN
    INSERT INTO #TempId(Id)
    SELECT Id FROM A_Audits WHERE IsRejected = 0 AND IsApprovedL1 = 1 AND IsApprovedL2 = 0 AND IsApprovedL3 = 0 AND IsApprovedL4 = 0;
END

IF (@A1 = 1)
BEGIN
    INSERT INTO #TempId(Id)
    SELECT Id FROM A_Audits WHERE IsRejected = 0 AND IsApprovedL1 = 0 AND IsApprovedL2 = 0 AND IsApprovedL3 = 0 AND IsApprovedL4 = 0;
END

SELECT COUNT(*) AS Id
FROM (
    SELECT 
        ad.Id,
        ad.[Code],
        ISNULL(ad.[Name], '') AS Name,
        FORMAT(ad.[StartDate], 'yyyy-MM-dd') AS StartDate,
        FORMAT(ad.[EndDate], 'yyyy-MM-dd') AS EndDate,
        CASE 
            WHEN ISNULL(ad.IsRejected, 0) = 1 THEN 'Reject'
            WHEN ISNULL(ad.IsApprovedL4, 0) = 1 THEN 'Approved' 
            WHEN ISNULL(ad.IsApprovedL3, 0) = 1 THEN 'Waiting For Approval 4' 
            WHEN ISNULL(ad.IsApprovedL2, 0) = 1 THEN 'Waiting For Approval 3' 
            WHEN ISNULL(ad.IsApprovedL1, 0) = 1 THEN 'Waiting For Approval 2' 
            ELSE 'Waiting For Approval 1' 
        END AS ApproveStatus,
        CASE 
            WHEN ISNULL(ad.IsAudited, 0) = 1 THEN 'Audited' 
            ELSE 'Not yet Audited' 
        END AS AuditStatus,
        ad.[IsPost]
    FROM A_Audits ad 
    WHERE ad.Id IN (SELECT Id FROM #TempId)
    AND ad.IsPost = 'Y'
) AS SubQuery;

DROP TABLE #TempId;
                 
                ";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserId", id);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);


                objComm.Fill(dt);


                //return Convert.ToInt32(dt.Rows[0][0]);
                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public int PendingAuditResponse(string UserName)
		{
			string sqlText = "";
			DataTable dt = new DataTable();
			DataTable authdt = new DataTable();
			try
			{
				sqlText = @"

             select 
             count(A.Id)Id

             from A_AuditIssues AI

             left outer join A_Audits A on AI.AuditId=A.Id
             left outer join Enums E on E.Id=AI.IssuePriority

             WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)
	         and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks) 

                ";

				sqlText = ApplyConditions(sqlText, null, null);
				SqlDataAdapter objComm = CreateAdapter(sqlText);
				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

				objComm.Fill(dt);

				return Convert.ToInt32(dt.Rows[0]["Id"]);


			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public int PendingForApproval(string UserName)
		{
			string sqlText = "";
			DataTable dt = new DataTable();
			DataTable authdt = new DataTable();
			try
			{
				sqlText = @"

                 SELECT Count(A_Audits.Id)Id
                 FROM A_Audits
                 where A_Audits.CreatedBy=@UserName and A_Audits.IsApprovedL4 != 1
                ";

				sqlText = ApplyConditions(sqlText, null, null);
				SqlDataAdapter objComm = CreateAdapter(sqlText);
				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

				objComm.Fill(dt);

				return Convert.ToInt32(dt.Rows[0]["Id"]);


			}
			catch (Exception e)
			{
				throw e;
			}
		}

        public int PendingForAuditFeedback(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

             select 
             count(A.Id)Id

             from A_AuditIssues AI

             left outer join A_Audits A on AI.AuditId=A.Id
             left outer join Enums E on E.Id=AI.IssuePriority

             WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)
	         and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks) 

                ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int PendingForIssueApproval(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = $@"

             declare @A1 varchar(1);
DECLARE @A2 varchar(1);
DECLARE @A3 varchar(1);
DECLARE @A4 varchar(1);
DECLARE @UserId varchar(max);

SELECT @UserId=Id FROM {AuthDbConfig.AuthDB}.dbo.AspNetUsers WHERE UserName=@UserName

Create TABLE #TempId(Id int)
SELECT @A1=AuditIssueApproval1,@A2=AuditIssueApproval2,@A3=AuditIssueApproval3,@A4=AuditIssueApproval4 from UserRolls
WHERE IsAuditIssue=1 and UserId=@UserId


if(@A4=1)
begin
    INSERT into  #TempId(Id)
    SELECT Id from A_Audits WHERE IsApprovedL4=1 AND isnull(IssueIsRejected,0)=0 and IssueIsApprovedL1=1 AND IssueIsApprovedL2=1 AND IssueIsApprovedL3=1 AND IssueIsApprovedL4=0
end
if(@A3=1)
begin
    INSERT into  #TempId(Id)
    SELECT id from A_Audits WHERE IsApprovedL4=1 AND IssueIsRejected=0 AND IssueIsApprovedL1=1 AND IssueIsApprovedL2=1 AND IssueIsApprovedL3=0 AND IssueIsApprovedL4=0
end
if(@A2=1)
begin
    INSERT into  #TempId(Id)
    SELECT id from A_Audits WHERE IsApprovedL4=1 AND IssueIsRejected=0 AND IssueIsApprovedL1=1 AND IssueIsApprovedL2=0 AND IssueIsApprovedL3=0 AND IssueIsApprovedL4=0
end
if(@A1=1)
begin
    INSERT into  #TempId(Id)
    SELECT id from A_Audits WHERE IsApprovedL4=1 AND IssueIsRejected=0 AND IssueIsApprovedL1=0 AND IssueIsApprovedL2=0 AND IssueIsApprovedL3=0 AND IssueIsApprovedL4=0
end

SELECT COUNT(*) AS TotalCount FROM (
    SELECT 
        ad.Id,
        ad.[Code],
        ISNULL(ad.[Name], '') AS Name,
        FORMAT(ad.[StartDate], 'yyyy-MM-dd') AS StartDate,
        FORMAT(ad.[EndDate], 'yyyy-MM-dd') AS EndDate,
        CASE 
            WHEN ISNULL(ad.IssueIsRejected, 0) = 1 THEN 'Reject'
            WHEN ISNULL(ad.IssueIsApprovedL4, 0) = 1 THEN 'Approveed' 
            WHEN ISNULL(ad.IssueIsApprovedL3, 0) = 1 THEN 'Waiting For Approval 4' 
            WHEN ISNULL(ad.IssueIsApprovedL2, 0) = 1 THEN 'Waiting For Approval 3' 
            WHEN ISNULL(ad.IssueIsApprovedL1, 0) = 1 THEN 'Waiting For Approval 2' 
            ELSE 'Waiting For Approval 1' 
        END AS ApproveStatus,
        CASE 
            WHEN ISNULL(ad.IsAudited, 0) = 1 THEN 'Audited' 
            ELSE 'Not yet Audited' 
        END AS AuditStatus,
        ad.[IsPost]
    FROM 
        A_Audits ad 
    WHERE 
        IsApprovedL4 = 1 
        AND IsCompleteIssueBranchFeedback = 1
        AND ad.id IN (SELECT id FROM #TempId)
        AND ad.IsPost = 'Y'
    ORDER BY  
        Id ASC
    OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY
) AS subquery

DROP TABLE #TempId

             ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["TotalCount"]);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int PendingForReviewerFeedback(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            try
            {
          sqlText = @"

             select count(A.Id)Id from A_AuditIssues a 
             left join AuditUsers AU on a.AuditId=au.AuditId
             left outer join [GDICAuditAuthDB].[dbo].[AspNetUsers] ANU on ANU.Id = AU.UserId
             where a.Id not  in (select af.AuditIssueId from A_AuditFeedbacks af where af.CreatedBy=@UserName ) and  
             ANU.UserName = @UserName and  TeamId in(0,null)
          ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<PrePaymentMaster> PrePaymentGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @"select 
                Id
                ,Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,Format(FinalCorrectionDate , 'yyyy-MM-dd') FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks

                from  Financial
                 where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);
                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<PrePaymentMaster> vms = dtResult.ToList<PrePaymentMaster>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<PrePaymentMaster> PrePaymentGetGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public List<PrePaymentMaster> PrePaymentGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<PrePaymentMaster> VMs = new List<PrePaymentMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                SELECT 
                 Id
                ,Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,Format(FinalCorrectionDate , 'yyyy-MM-dd') FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks
     
                FROM Financial 

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
                    .ToList<PrePaymentMaster>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int PrePaymentGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<Teams> VMs = new List<Teams>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                from Financial  where 1=1 ";


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

        public PrePaymentMaster PrePaymentInsert(PrePaymentMaster objMaster)
		{
			try
			{
				string sqlText = "";
				int count = 0;
				var command = CreateCommand(@" INSERT INTO Financial(
                 Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks
                
                ) VALUES (

                 @Code
                ,@Auditor
                ,@EmployeeName
                ,@Details 
                ,@FinalCorrectionDate 
                ,@PreviousAmount
                ,@CorrectedAmount
                ,@AdditionalPayment
                ,@PaymentMemoReferenceNo
                ,@Department
                ,@Remarks
                

                )SELECT SCOPE_IDENTITY()");
		
                command.Parameters.Add("@Code", SqlDbType.VarChar).Value = objMaster.Code;
				command.Parameters.Add("@Auditor", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Auditor) ? (object)DBNull.Value : objMaster.Auditor;
				command.Parameters.Add("@EmployeeName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.EmployeeName) ? (object)DBNull.Value : objMaster.EmployeeName;
				command.Parameters.Add("@Details", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Details) ? (object)DBNull.Value : objMaster.Details;				
				command.Parameters.Add("@FinalCorrectionDate", SqlDbType.DateTime).Value = objMaster.FinalCorrectionDate;
				command.Parameters.Add("@PreviousAmount", SqlDbType.Decimal).Value = objMaster.PreviousAmount;
				command.Parameters.Add("@CorrectedAmount", SqlDbType.Decimal).Value = objMaster.CorrectedAmount;
				command.Parameters.Add("@AdditionalPayment", SqlDbType.Decimal).Value = objMaster.AdditionalPayment;
                command.Parameters.Add("@PaymentMemoReferenceNo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.PaymentMemoReferenceNo) ? (object)DBNull.Value : objMaster.PaymentMemoReferenceNo;              
                command.Parameters.Add("@Department", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Department) ? (object)DBNull.Value : objMaster.Department;
                command.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Remarks) ? (object)DBNull.Value : objMaster.Remarks;

				objMaster.Id = Convert.ToInt32(command.ExecuteScalar());
				return objMaster;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public PrepaymentReview PrepaymentReview()
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            List<PrepaymentReview> VMs = new List<PrepaymentReview>();
            try
            {
    sqlText = @"

    SELECT top(1)    
    Id,
    Value
    FROM PrepaymentReview Order By Id Desc
    ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.Fill(dt);
                VMs = dt.ToList<PrepaymentReview>();
                return VMs.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PrepaymentReview PrepaymentReviewInsert(PrepaymentReview objMaster)
        {
            try
            {
               
                var command = CreateCommand(@" 
INSERT INTO PrepaymentReview(

 Value

,CreatedBy
,CreatedOn
,CreatedFrom

) VALUES (

 @Value

,@CreatedBy
,@CreatedOn
,@CreatedFrom

)SELECT SCOPE_IDENTITY()");


                command.Parameters.Add("@Value", SqlDbType.Decimal).Value = objMaster.Value;
                
                command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : objMaster.Audit.CreatedBy.ToString();
                command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : objMaster.Audit.CreatedOn.ToString();
                command.Parameters.Add("@CreatedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : objMaster.Audit.CreatedFrom.ToString();
                objMaster.Id = Convert.ToInt32(command.ExecuteScalar());

                return objMaster;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PrePaymentMaster PrePaymentUpdate(PrePaymentMaster objMaster)
		{
			try
			{
				string sqlText = "";
				int count = 0;
				string query = @"  update Financial set

 Auditor                        =@Auditor  
,EmployeeName                   =@EmployeeName  
,Details                        =@Details   
,FinalCorrectionDate            =@FinalCorrectionDate  
,PreviousAmount                 =@PreviousAmount  
,CorrectedAmount                =@CorrectedAmount  
,AdditionalPayment              =@AdditionalPayment  
,PaymentMemoReferenceNo         =@PaymentMemoReferenceNo  
,Department                     =@Department  
,Remarks                        =@Remarks                    
 where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

                 

				command.Parameters.Add("@Id", SqlDbType.Int).Value = objMaster.Id;	
				var item = objMaster.PrePaymentDetails.FirstOrDefault();
                command.Parameters.Add("@Auditor", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Auditor) ? (object)DBNull.Value : item.Auditor;
                command.Parameters.Add("@EmployeeName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.EmployeeName) ? (object)DBNull.Value : item.EmployeeName;
                command.Parameters.Add("@Details", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Details) ? (object)DBNull.Value : item.Details;               
                command.Parameters.Add("@FinalCorrectionDate", SqlDbType.DateTime).Value = item.FinalCorrectionDate;
                command.Parameters.Add("@PreviousAmount", SqlDbType.Decimal).Value = item.PreviousAmount;
                command.Parameters.Add("@CorrectedAmount", SqlDbType.Decimal).Value = item.CorrectedAmount;
                command.Parameters.Add("@AdditionalPayment", SqlDbType.Decimal).Value = item.AdditionalPayment;
                command.Parameters.Add("@PaymentMemoReferenceNo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.PaymentMemoReferenceNo.Trim()) ? (object)DBNull.Value : item.PaymentMemoReferenceNo.Trim();
                command.Parameters.Add("@Department", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Department) ? (object)DBNull.Value : item.Department;
                command.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Remarks) ? (object)DBNull.Value : item.Remarks;

                int rowcount = command.ExecuteNonQuery();
				if (rowcount <= 0)
				{
					throw new Exception(MessageModel.UpdateFail);
				}
				return objMaster;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public int TotalAdditionalPaymentCount()
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {

                sqlText = @"

                SELECT  SUM(AdditionalPayment)AdditionalPayment
                FROM Financial";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                //objComm.SelectCommand.Parameters.AddWithValue("@UserId", id);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["AdditionalPayment"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int TotalAuditApproved(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {

                //string query = " select Id,UserName from [GDICAuditAuthDB].[dbo].[AspNetUsers] authuser where authuser.UserName=@userName ";
                //sqlText = ApplyConditions(sqlText, null, null);
                //SqlDataAdapter obj = CreateAdapter(query);
                //obj.SelectCommand = ApplyParameters(obj.SelectCommand, null, null);
                //obj.SelectCommand.Parameters.AddWithValue("@userName", UserName);

                //obj.Fill(authdt);
                //string id = null;
                //foreach (DataRow row in authdt.Rows)
                //{
                //    id = row["Id"].ToString();

                //}


                sqlText = @"

                  SELECT Count(A_Audits.Id)Id 

                  FROM A_Audits

                  where A_Audits.ApprovedByL4=@UserName and A_Audits.IsApprovedL4=1

                ";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserId", id);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);


                objComm.Fill(dt);


                //return Convert.ToInt32(dt.Rows[0][0]);
                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int TotalAuditRejected(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {

                //string query = " select Id,UserName from [GDICAuditAuthDB].[dbo].[AspNetUsers] authuser where authuser.UserName=@userName ";
                //sqlText = ApplyConditions(sqlText, null, null);
                //SqlDataAdapter obj = CreateAdapter(query);
                //obj.SelectCommand = ApplyParameters(obj.SelectCommand, null, null);
                //obj.SelectCommand.Parameters.AddWithValue("@userName", UserName);

                //obj.Fill(authdt);
                //string id = null;
                //foreach (DataRow row in authdt.Rows)
                //{
                //    id = row["Id"].ToString();

                //}


                sqlText = @"

                  SELECT Count(A_Audits.Id)Id
                  FROM A_Audits

                  where A_Audits.IsRejected=1


                  --left Join AuditIssueUsers on  AuditIssueUsers.AuditIssueId=A_AuditIssues.Id
                  --where AuditIssueUsers.UserId=@UserId and A_Audits.RejectedBy=@UserName

                ";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserId", id);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);


                objComm.Fill(dt);


                //return Convert.ToInt32(dt.Rows[0][0]);
                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditReports> TotalCompletedOngoingRemaing(string UserName)
        {
            string  sqlText = "";
            List<AuditReports> VMs = new List<AuditReports>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"


SELECT
 
    COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) AS Ongoing,
    COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END) AS Completed,
    (COUNT(CASE WHEN a.IsPlaned = 1 THEN 1 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)) AS Remaining
FROM
    --[SSLAuditDB].[dbo].[AuditTypes] AS AT
      [AuditTypes] AS AT
LEFT JOIN
    --[SSLAuditDB].[dbo].[A_Audits] AS a ON AT.Id = a.AuditTypeId
      [A_Audits] AS a ON AT.Id = a.AuditTypeId
WHERE a.IsPlaned = 1 ;


      
                ";

				//And A_Audits.Createdby=@UserName

				//sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

				// ToDo Escape Sql Injection
				//sqlText += @"  order by  " + index.OrderName.Replace("AuditName", "ad.Name") + "  " + index.orderDir;
				//sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

				SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				 //objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);



				objComm.Fill(dt);

                VMs = dt.ToList<AuditReports>();


                var item = VMs.FirstOrDefault();
                decimal[] arr = new decimal[3];
                arr[0] = item.Completed;
                arr[1] = item.Ongoing;
                arr[2] = item.Remaining;
                decimal total = 0;
                foreach(var i in arr)
                {
                    total = total + i;
                }
                decimal compeleted = item.Completed;
                decimal ongoint = item.Ongoing;
                decimal remaining = item.Remaining;
                if(total != 0)
                {
                    item.Completed = Math.Round((compeleted / total) * 100, 0);
                    item.Ongoing = Math.Round((item.Ongoing / total) * 100, 0);
                    item.Remaining = Math.Round((item.Remaining / total) * 100, 0);
                }
                
                


                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public int TotalFollowUpAudit(string UserName)
		{
			string sqlText = "";
			DataTable dt = new DataTable();
			DataTable authdt = new DataTable();
			try
			{
				sqlText = @"

                 SELECT 
                 COUNT(Distinct AI.AuditId)Id
                 FROM A_AuditIssues AI
                 LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
                 WHERE 1=1 AND			 
			     CAST(AI.ImplementationDate AS DATE) <= CAST(GETDATE() AS DATE)
			     AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks)

                 --SELECT COUNT(A.Id) Id
                 --FROM A_Audits A where A.Id in
                 --(select AI.AuditId
                 --From A_AuditIssues  AI
                 --WHERE AI.CreatedBy = @UserName AND AI.IssueDeadLine <= DateOfSubmission)
                 
                 --select * from A_Audits Where Id In 
			     --(			   
			     --SELECT              
			     --Distinct AI.AuditId
                 --FROM A_AuditIssues AI
                 --LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
                 --WHERE 1=1 AND			 
			     --CAST(AI.ImplementationDate AS DATE) <= CAST(GETDATE() AS DATE)
			     --AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks)			      
			     --)

                ";

				sqlText = ApplyConditions(sqlText, null, null);
				SqlDataAdapter objComm = CreateAdapter(sqlText);
				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				//objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

				objComm.Fill(dt);

				return Convert.ToInt32(dt.Rows[0]["Id"]);


			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public int TotalIssueRejected(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {

                //string query = " select Id,UserName from [GDICAuditAuthDB].[dbo].[AspNetUsers] authuser where authuser.UserName=@userName ";
                //sqlText = ApplyConditions(sqlText, null, null);
                //SqlDataAdapter obj = CreateAdapter(query);
                //obj.SelectCommand = ApplyParameters(obj.SelectCommand, null, null);
                //obj.SelectCommand.Parameters.AddWithValue("@userName", UserName);

                //obj.Fill(authdt);
                //string id = null;
                //foreach (DataRow row in authdt.Rows)
                //{
                //    id = row["Id"].ToString();

                //}


                sqlText = @"

                 SELECT Count(A_Audits.Id)Id
                 FROM A_Audits
                 where  A_Audits.IssueIsRejected=1


                --left Join AuditIssueUsers on  AuditIssueUsers.AuditIssueId=A_AuditIssues.Id
                --where AuditIssueUsers.UserId=@UserId A_Audits.IssueRejectedBy=@UserName and


                ";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserId", id);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);


                objComm.Fill(dt);


                //return Convert.ToInt32(dt.Rows[0][0]);
                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public int TotalPendingIssueReview(string UserName)
		{
			string sqlText = "";
			DataTable dt = new DataTable();
			DataTable authdt = new DataTable();
			try
			{
				sqlText = @"




        select 
        count(AI.ID) Id
        from A_AuditIssues AI

        left outer join A_Audits A on AI.AuditId=A.Id
        left outer join Enums E on E.Id=AI.IssuePriority

        WHERE 1=1 and AI.CreatedBy=@UserName and AI.DateOfSubmission < CAST(GETDATE() AS DATE)
	    and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks)

                ";

				sqlText = ApplyConditions(sqlText, null, null);
				SqlDataAdapter objComm = CreateAdapter(sqlText);
				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

				objComm.Fill(dt);

				return Convert.ToInt32(dt.Rows[0]["Id"]);


			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public int TotalRisk(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {

                sqlText = @"

                SELECT  Count(A_AuditIssues.Id)Id
               FROM A_AuditIssues 
               WHERE risk IS NOT NULL and A_AuditIssues.Createdby=@UserName;


                ";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserId", id);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);


                objComm.Fill(dt);


                //return Convert.ToInt32(dt.Rows[0][0]);
                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public List<AuditReports> UnPlan(string UserName)
		{
			string sqlText = "";
			List<AuditReports> VMs = new List<AuditReports>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"


SELECT
 
    COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 0 ELSE NULL END) AS UnPlanOngoing,
    COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 0 ELSE NULL END) AS UnPlanCompleted,
    (COUNT(CASE WHEN a.IsPlaned = 0 THEN 0 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 0 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 0 ELSE NULL END)) AS UnPlanRemaining
FROM   
      [AuditTypes] AS AT
LEFT JOIN    
      [A_Audits] AS a ON AT.Id = a.AuditTypeId
WHERE a.IsPlaned = 0;
      
                ";


				SqlDataAdapter objComm = CreateAdapter(sqlText);
				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

				//objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);


				objComm.Fill(dt);

				VMs = dt.ToList<AuditReports>();


                var item = VMs.FirstOrDefault();
                decimal[] arr = new decimal[3];
                arr[0] = item.UnPlanCompleted;
                arr[1] = item.UnPlanOngoing;
                arr[2] = item.UnPlanRemaining;
                decimal total = 0;
                foreach (var i in arr)
                {
                    total = total + i;
                }
                if(total != 0)
                {
                    item.UnPlanCompleted = Math.Round((item.UnPlanCompleted / total) * 100, 0);
                    item.UnPlanOngoing = Math.Round((item.UnPlanOngoing / total) * 100, 0);
                    item.UnPlanRemaining = Math.Round((item.UnPlanRemaining / total) * 100, 0);
                }
                



                return VMs;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public AuditMaster Update(AuditMaster model)
        {



            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"  update UserBranchMap set 

 UserId               =@UserId  
,BranchId              =@BranchId  
 
,LastUpdateBy               =@LastUpdateBy  
,LastUpdateOn               =@LastUpdateOn  
,LastUpdateFrom            =@LastUpdateFrom   
                       
where  Id= @Id ";


                SqlCommand command = CreateCommand(query);
                command.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
                command.Parameters.Add("@Id", SqlDbType.NChar).Value = model.Id;
                //command.Parameters.Add("@BranchId", SqlDbType.NChar).Value = model.BranchId;

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
