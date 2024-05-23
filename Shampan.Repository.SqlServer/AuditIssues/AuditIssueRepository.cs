


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository;
using Shampan.Core.Interfaces.Repository.AuditIssues;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Repository.SqlServer.AuditIssues
{
    public class AuditIssueRepository : Repository, IAuditIssueRepository
    {
        private DbConfig _dbConfig;

        public AuditIssueRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;
        }

        public List<AuditIssue> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditIssue> VMs = new List<AuditIssue>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
       SELECT Id
      ,AuditId
      ,IssueName
      ,IssueDetails
      ,format(DateOfSubmission,'yyyy-MM-dd')DateOfSubmission
      ,format(IssueOpenDate,'yyyy-MM-dd')IssueOpenDate
      ,format(IssueDeadLine,'yyyy-MM-dd')IssueDeadLine
      ,format(ImplementationDate,'yyyy-MM-dd')ImplementationDate
      ,IssuePriority
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

      FROM A_AuditIssues

      where 1=1  

";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.Fill(dt);
                VMs = dt.ToList<AuditIssue>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetAuditIssueIndexCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";

            DataTable dt = new DataTable();
            try
            {

                if (index.Status == "TotalIssues")
                {
                    sqlText = @"

                 select 
                 count(AI.Id)FilteredCount
                 FROM A_AuditIssues AI
                 
                left outer join A_Audits A on A.Id = AI.AuditId

                 where 1=1 ";

                    //and AI.CreatedBy = @UserName

                }
                else if (index.Status == "TotalIssueRejected")
                {
                    sqlText = @"

                    SELECT Count(A_Audits.Id)FilteredCount

                    FROM A_Audits

                    where A_Audits.CreatedBy=@UserName and A_Audits.IssueIsRejected=1

                 ";

                }

                //else if (index.Status == "MissDeadLineIssues")
                //{
                // sqlText = @"SELECT Count(AI.Id)FilteredCount
                // FROM A_AuditIssues AI
                // left outer join A_Audits A on A.Id = AI.AuditId
                // where
                // AI.CreatedBy = @UserName and IssueDeadLine <= DateOfSubmission";
                //}


                else if (index.Status == "TotalPendingIssueReview")
                {

                    sqlText = @"

        select 
        count(AI.ID) FilteredCount

        from A_AuditIssues AI

        left outer join A_Audits A on AI.AuditId=A.Id
        left outer join Enums E on E.Id=AI.IssuePriority

        WHERE 1=1 and AI.CreatedBy=@UserName and AI.DateOfSubmission < CAST(GETDATE() AS DATE)
	    and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks)

        ";

                }


                else if (index.Status == "FollowUpAuditIssues")
                {

                    sqlText = @"

                 SELECT 
                 COUNT(A.Id)FilteredCount
                 FROM A_AuditIssues AI
                 LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
                 WHERE 1=1 AND AI.IssueStatus != '1046' AND			 
			     CAST(AI.ImplementationDate AS DATE) <= CAST(GETDATE() AS DATE)
			     AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks)

                 --SELECT Count(AI.Id)FilteredCount 
                 --FROM A_AuditIssues AI
                 --left outer join A_Audits A on A.Id = AI.AuditId
                 --where AI.CreatedBy=@UserName and AI.CreatedOn > DateOfSubmission

                ";

                }

                else if (index.Status == "PendingForReviewerFeedback")
                {

                    sqlText = @"

                     select count(A.Id)FilteredCount from A_AuditIssues a 
                     left join AuditUsers AU on a.AuditId=au.AuditId
                     left outer join [GDICAuditAuthDB].[dbo].[AspNetUsers] ANU on ANU.Id = AU.UserId
                     where a.Id not  in (select af.AuditIssueId from A_AuditFeedbacks af where af.CreatedBy=@UserName ) and  
                     ANU.UserName = @UserName and  TeamId in(0,null)

                    ";

                }

                else if (index.Status == "BeforeDeadLineIssue")
                {

                    sqlText = @"

                     SELECT 
                     COUNT(A.Id) FilteredCount

                     FROM A_AuditIssues AI
                     LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
                     
                     WHERE 1=1 AND AI.IssueStatus != '1046' AND 			 
			         --CAST(AI.IssueDeadLine AS DATE) <= CAST(GETDATE() AS DATE)
                     CAST(AI.IssueDeadLine AS DATE) > CAST(GETDATE() AS DATE)
			         AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks) 

                    ";

                }
                //else if (index.Status == "MissDeadLineIssues")
                else if (index.Status == "Issuedeadlinelapsed")
                {

                    sqlText = @"

                     SELECT 
                     COUNT(A.Id) FilteredCount

                     FROM A_AuditIssues AI
                     LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
                     
                     WHERE 1=1 AND AI.IssueStatus != '1046' AND 			 
			         --CAST(AI.IssueDeadLine AS DATE) > CAST(GETDATE() AS DATE) 
                     CAST(GETDATE() AS DATE) >=  CAST(AI.IssueDeadLine AS DATE)
	                 AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks)  

                    ";

                }
                



                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.UserName);
                objComm.Fill(dt);
                //return Convert.ToInt32(dt.Rows);
                return Convert.ToInt32(dt.Rows[0]["FilteredCount"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditIssue> GetAuditIssueIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditIssue> VMs = new List<AuditIssue>();
            DataTable dt = new DataTable();






            try
            {


                //Issues For Deahboard And Normal Index


                 if (index.Status == "TotalIssues")
                {
                    sqlText = @"

       SELECT 
       AI.Id
      ,AI.AuditId
      ,AI.IssueName
      ,AI.IsPost
      ,AI.Risk
      ,AI.IssueDetails
      ,isnull(AI.InvestigationOrForensis ,'0')InvestigationOrForensis
      ,isnull(AI.StratigicMeeting ,'0')StratigicMeeting
      ,isnull(AI.ManagementReviewMeeting ,'0')ManagementReviewMeeting
      ,isnull(AI.OtherMeeting ,'0')OtherMeeting
      ,isnull(AI.Training ,'0')Training
      ,isnull(AI.Operational ,'0')Operational
      ,isnull(AI.Financial ,'0')Financial
      ,isnull(AI.Compliance ,'0')Compliance
      ,Format(AI.DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission

      ,A.Code
      ,A.Name
      ,A.AuditStatus

      ,Enums.EnumValue IssuePriority
      ,anm.EnumValue IssueStatus

       FROM A_AuditIssues AI

       left outer join A_Audits A on A.Id = AI.AuditId
       left outer join Enums on AI.IssuePriority = Enums.Id
       left outer join Enums anm on ai.IssueStatus = anm.Id

       where 1=1 
 
       ";

                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);


                    //objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.createdBy);


                    objComm.Fill(dt);



                    VMs = dt
                        .ToList<AuditIssue>();




                    //foreach (var issueData in VMs)
                    //{
                    //    foreach (DataRow row in dt.Rows)
                    //    {
                    //        string AuditStatus = row["AuditStatus"].ToString();
                    //        issueData.AuditMaster.AuditStatus = AuditStatus;
    
                    //    }
                    //}




                    return VMs;
                }


                else if (index.Status == "TotalIssueRejected")
                {
                    sqlText = @"
      SELECT 
       Ai.Id
       --A.Id
      ,Ai.AuditId
      ,Ai.IssueName

      ,A.Code
      ,A.Name

      ,Format(AI.DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission
      ,Enums.EnumValue IssuePriority
      ,AI.IsPost
      ,AI.Risk
      ,isnull(AI.Operational ,'0')Operational
      ,isnull(AI.Financial ,'0')Financial
      ,isnull(AI.Compliance ,'0')Compliance

       FROM A_AuditIssues AI
       left outer join A_Audits A on A.Id = AI.AuditId
       left outer join Enums on AI.IssuePriority = Enums.Id

       where 1=1 and AI.CreatedBy=@UserName and A.IssueIsRejected=1 




                ";


                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);


                    objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.createdBy);


                    objComm.Fill(dt);



                    VMs = dt
                        .ToList<AuditIssue>();
                    return VMs;



                }


      //          else if (index.Status == "MissDeadLineIssues")
      //          {
      //              sqlText = @"
      // SELECT 
      // Ai.Id
      //,Ai.AuditId
      //,Ai.IssueName
      //,A.Code
      //,A.Name
      //,Format(AI.DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission
      //,Enums.EnumValue IssuePriority
      //,AI.IsPost
      //,AI.Risk
      //,isnull(AI.Operational ,'0')Operational
      //,isnull(AI.Financial ,'0')Financial
      //,isnull(AI.Compliance ,'0')Compliance
      // FROM A_AuditIssues AI
      // left outer join A_Audits A on A.Id = AI.AuditId
      // left outer join Enums on AI.IssuePriority = Enums.Id
      // left join AuditIssueUsers as AIU On AIU.AuditIssueId=AI.Id   
      // where 1=1 and AIU.UserId=(select UserId from GDICAuditAuthDB..AspNetUsers where UserName=@UserName ) and  IssueDeadLine <= DateOfSubmission
      // ";
      //              sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);               
      //              sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
      //              sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
      //              SqlDataAdapter objComm = CreateAdapter(sqlText);
      //              objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
      //              objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.UserName);
      //              objComm.Fill(dt);
      //              VMs = dt.ToList<AuditIssue>();
      //              return VMs;
      //          }


                else if (index.Status == "TotalPendingIssueReview")
                {
                    sqlText = @"
       SELECT 

       Ai.Id
      ,Ai.AuditId
      ,Ai.IssueName

      ,A.Code
      ,A.Name

      ,Format(AI.DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission
      ,E.EnumValue IssuePriority
      ,AI.IsPost
      ,AI.Risk
      ,isnull(AI.Operational ,'0')Operational
      ,isnull(AI.Financial ,'0')Financial
      ,isnull(AI.Compliance ,'0')Compliance

       FROM A_AuditIssues AI

       left outer join A_Audits A on AI.AuditId=A.Id
       left outer join Enums E on E.Id=AI.IssuePriority

       WHERE 1=1 and AI.CreatedBy=@UserName and AI.DateOfSubmission < CAST(GETDATE() AS DATE)
	   and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks)

                ";


                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);


                    objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.UserName);


                    objComm.Fill(dt);



                    VMs = dt
                        .ToList<AuditIssue>();
                    return VMs;



                }

                else if (index.Status == "PendingForReviewerFeedback")
                {
                    sqlText = @"
 SELECT 
 A.Id
,A.AuditId
,A.IssueName
,AA.Code
,AA.Name
,Format(A.DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission
,E.EnumValue IssuePriority
,A.IsPost
,A.Risk
,isnull(A.Operational ,'0')Operational
,isnull(A.Financial ,'0')Financial
,isnull(A.Compliance ,'0')Compliance

from A_AuditIssues a 

left join AuditUsers AU on a.AuditId=au.AuditId
left outer join [GDICAuditAuthDB].[dbo].[AspNetUsers] ANU on ANU.Id = AU.UserId
left outer join A_Audits AA on A.AuditId=AA.Id
left outer join Enums E on E.Id=A.IssuePriority


where a.Id not  in (select af.AuditIssueId from A_AuditFeedbacks af where af.CreatedBy=@UserName) and  
ANU.UserName =@UserName  and  AU.TeamId in(0,null)
";


                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                    objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.UserName);


                    objComm.Fill(dt);

                    VMs = dt
                        .ToList<AuditIssue>();
                    return VMs;


                }

                else if (index.Status == "FollowUpAuditIssues")
                {
                    sqlText = @"


       SELECT 
       AI.Id
      ,AI.AuditId
      ,AI.IssueName

      ,A.Code
      ,A.Name
      ,E.EnumValue IssuePriority

      ,Format(AI.DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission   
      ,AI.IsPost
      ,AI.Risk
      ,ISNULL(AI.Operational ,'0')Operational
      ,ISNULL(AI.Financial ,'0')Financial
      ,ISNULL(AI.Compliance ,'0')Compliance

       FROM A_AuditIssues AI

       LEFT OUTER JOIN A_Audits A on AI.AuditId = A.Id
       LEFT OUTER JOIN Enums E on E.Id = AI.IssuePriority

       WHERE 1=1 AND AI.IssueStatus != '1046' AND

       CAST(AI.ImplementationDate AS DATE) <= CAST(GETDATE() AS DATE)
	   AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks)



      --SELECT 
      --Ai.Id
      --,Ai.AuditId
      --,Ai.IssueName
      --,A.Code
      --,A.Name
      --,Format(AI.DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission
      --,E.EnumValue IssuePriority
      --,AI.IsPost
      --,AI.Risk
      --,isnull(AI.Operational ,'0')Operational
      --,isnull(AI.Financial ,'0')Financial
      --,isnull(AI.Compliance ,'0')Compliance
      --FROM A_AuditIssues AI
      --left outer join A_Audits A on AI.AuditId=A.Id
      --left outer join Enums E on E.Id=AI.IssuePriority
      --where AI.CreatedBy=@UserName and ImplementationDate <  GetDate()

                ";


                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);


                    objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.UserName);


                    objComm.Fill(dt);



                    VMs = dt
                        .ToList<AuditIssue>();
                    return VMs;



                }

                else if (index.Status == "BeforeDeadLineIssue")               
                {
                    sqlText = @"

       SELECT 
       AI.Id
      ,AI.AuditId
      ,AI.IssueName

      ,A.Code
      ,A.Name
      ,E.EnumValue IssuePriority

      ,Format(AI.DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission   
      ,AI.IsPost
      ,AI.Risk
      ,ISNULL(AI.Operational ,'0')Operational
      ,ISNULL(AI.Financial ,'0')Financial
      ,ISNULL(AI.Compliance ,'0')Compliance

       FROM A_AuditIssues AI

       LEFT OUTER JOIN A_Audits A on AI.AuditId = A.Id
       LEFT OUTER JOIN Enums E on E.Id = AI.IssuePriority

       WHERE 1=1 AND AI.IssueStatus != '1046' AND

       --CAST(AI.IssueDeadLine AS DATE) <= CAST(GETDATE() AS DATE)
       CAST(AI.IssueDeadLine AS DATE) > CAST(GETDATE() AS DATE)
	   AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks) 

       --where AI.CreatedBy=@UserName and ImplementationDate <  GetDate()
       ";


                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);


                    //objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.UserName);


                    objComm.Fill(dt);



                    VMs = dt
                        .ToList<AuditIssue>();
                    return VMs;



                }

                //else if (index.Status == "MissDeadLineIssues")
                else if (index.Status == "Issuedeadlinelapsed")
                {
                    sqlText = @"

       SELECT 
       AI.Id
      ,AI.AuditId
      ,AI.IssueName

      ,A.Code
      ,A.Name
      ,E.EnumValue IssuePriority

      ,Format(AI.DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission   
      ,AI.IsPost
      ,AI.Risk
      ,ISNULL(AI.Operational ,'0')Operational
      ,ISNULL(AI.Financial ,'0')Financial
      ,ISNULL(AI.Compliance ,'0')Compliance

       FROM A_AuditIssues AI

       LEFT OUTER JOIN A_Audits A on AI.AuditId = A.Id
       LEFT OUTER JOIN Enums E on E.Id = AI.IssuePriority

       WHERE 1=1 AND AI.IssueStatus != '1046' AND

       --CAST(AI.IssueDeadLine AS DATE) > CAST(GETDATE() AS DATE) 
       CAST(GETDATE() AS DATE) >=  CAST(AI.IssueDeadLine AS DATE)
	   AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks)  

       
       ";


                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);


                    //objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.UserName);


                    objComm.Fill(dt);



                    VMs = dt
                        .ToList<AuditIssue>();
                    return VMs;



                }


                return VMs;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetExcelIndexCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";

            DataTable dt = new DataTable();
            try
            {

              sqlText = @"

                 select 
                 count(AI.Id)FilteredCount
                 FROM A_AuditIssues AI
                 
                 left outer join A_Audits A on A.Id = AI.AuditId

                 where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                //objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.UserName);

                objComm.Fill(dt);
                //return Convert.ToInt32(dt.Rows);
                return Convert.ToInt32(dt.Rows[0]["FilteredCount"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditIssue> GetExcelIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditIssue> VMs = new List<AuditIssue>();
            DataTable dt = new DataTable();

            try
            {  
 sqlText = @"

SELECT 
    --AI.Id AS AuditIssueId,
    AI.Id,
    AI.AuditId,
    AI.IssueName,
    AI.IsPost,
    AI.Risk,
    AI.IssueDetails,
    ISNULL(AI.InvestigationOrForensis ,'0') AS InvestigationOrForensis,
    ISNULL(AI.StratigicMeeting ,'0') AS StratigicMeeting,
    ISNULL(AI.ManagementReviewMeeting ,'0') AS ManagementReviewMeeting,
    ISNULL(AI.OtherMeeting ,'0') AS OtherMeeting,
    ISNULL(AI.Training ,'0') AS Training,
    ISNULL(AI.Operational ,'0') AS Operational,
    ISNULL(AI.Financial ,'0') AS Financial,
    ISNULL(AI.Compliance ,'0') AS Compliance,
    FORMAT(AI.DateOfSubmission, 'yyyy-MM-dd') AS DateOfSubmission,

    A.Code,
    A.Name,
    A.AuditStatus,

    Enums.EnumValue AS IssuePriority,
    anm.EnumValue AS IssueStatus,

    AF.Heading FeedbackHeading, 
    AF.IssueDetails FeedbackDetails,  

	BF.Heading BranchFeedBackHeading,
	Bf.IssueDetails BranchFeedBackDetails


FROM 
    A_AuditIssues AI
LEFT OUTER JOIN 
    A_Audits A ON A.Id = AI.AuditId
LEFT OUTER JOIN 
    Enums ON AI.IssuePriority = Enums.Id
LEFT OUTER JOIN 
    Enums anm ON AI.IssueStatus = anm.Id


OUTER APPLY (
    SELECT TOP 1 
        Heading,  
        IssueDetails  
    FROM 
        A_AuditFeedbacks AF
    WHERE 
        AF.AuditIssueId = AI.Id  
    ORDER BY 
        AF.Id DESC 
) AS AF


OUTER APPLY (
    SELECT TOP 1 
        Heading,  
        IssueDetails    
    FROM 
        A_AuditBranchFeedbacks BF
    WHERE 
        BF.AuditIssueId = AI.Id  
    ORDER BY 
        BF.Id DESC 
) AS BF

WHERE 1 = 1





      --SELECT 
      --AI.Id
      --AI.AuditId
      --AI.IssueName
      --AI.IsPost
      --AI.Risk
      --AI.IssueDetails
      --isnull(AI.InvestigationOrForensis ,'0')InvestigationOrForensis
      --isnull(AI.StratigicMeeting ,'0')StratigicMeeting
      --isnull(AI.ManagementReviewMeeting ,'0')ManagementReviewMeeting
      --isnull(AI.OtherMeeting ,'0')OtherMeeting
      --isnull(AI.Training ,'0')Training
      --isnull(AI.Operational ,'0')Operational
      --isnull(AI.Financial ,'0')Financial
      --isnull(AI.Compliance ,'0')Compliance
      --Format(AI.DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission
      --A.Code
      --A.Name
      --A.AuditStatus
      --Enums.EnumValue IssuePriority
      --anm.EnumValue IssueStatus
      --FROM A_AuditIssues AI
      --left outer join A_Audits A on A.Id = AI.AuditId
      --left outer join Enums on AI.IssuePriority = Enums.Id
      --left outer join Enums anm on ai.IssueStatus = anm.Id
      --where 1=1 

       ";

                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                    //objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.createdBy);

                    objComm.Fill(dt);

                    VMs = dt
                        .ToList<AuditIssue>();
                    //foreach (var issueData in VMs)
                    //{
                    //    foreach (DataRow row in dt.Rows)
                    //    {
                    //        string AuditStatus = row["AuditStatus"].ToString();
                    //        issueData.AuditMaster.AuditStatus = AuditStatus;

                    //    }
                    //}

                    return VMs;
                
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public List<AuditIssue> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditIssue> VMs = new List<AuditIssue>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT A_AuditIssues.Id
      ,AuditId
      ,IssueName
      ,IssueDetails
      --,DateOfSubmission
      ,Format(DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission
      ,Enums.EnumValue IssuePriority
      ,CreatedBy
      --,CreatedOn
      ,Format(CreatedOn , 'dd-MM-yyyy   -   hh:mm:ss tt') CreatedOn
      ,CreatedFrom
      ,LastUpdateBy
      ,LastUpdateOn
      ,LastUpdateFrom
      ,isnull(IsPosted,0)IsPosted
      ,PostedBy
      ,PostedOn
      ,PostedFrom
      ,IsPost
      ,isnull(IsIssueApprove,0)IsIssueApprove
      --,IssueCheck


  FROM A_AuditIssues left outer join Enums 
  on A_AuditIssues.IssuePriority = Enums.Id
  

  where 1=1 
    
 and AuditId = @AuditId  
";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);


                objComm.Fill(dt);



                VMs = dt
                    .ToList<AuditIssue>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel
            vm = null)

        {
            string sqlText = "";

            DataTable dt = new DataTable();
            try
            {
                sqlText = @"
select 
 count(Id)FilteredCount
FROM A_AuditIssues
where 1=1 
and AuditId = @AuditId

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

        public List<AuditIssue> GetIssuesByAuditId(AuditMaster model)
        {
            string sqlText = "";
            List<AuditIssue> VMs = new List<AuditIssue>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
       SELECT 
       Id
      ,AuditId
      ,IssueName
      ,IssueDetails
      ,Format(DateOfSubmission , 'yyyy-MM-dd') DateOfSubmission
      ,Format(CreatedOn , 'dd-MM-yyyy   -   hh:mm:ss tt') CreatedOn
      ,isnull(IsPosted,0)IsPosted
      ,isnull(IsIssueApprove,0)IsIssueApprove

       FROM A_AuditIssues 

       WHERE 1=1 AND AuditId = @AuditId  
";

                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", model.Id);

                objComm.Fill(dt);

                VMs = dt.ToList<AuditIssue>();

                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AuditIssue Insert(AuditIssue model)
        {
            try
            {
                string sqlText = "";
                int Id = 0;


                sqlText = @"
insert into A_AuditIssues(
 AuditId
,IssueName
,IssueDetails
,DateOfSubmission
,IssueOpenDate
,IssueDeadLine
,ImplementationDate
,IssuePriority
,IssueStatus
,Risk
,InvestigationOrForensis
,StratigicMeeting
,ManagementReviewMeeting
,OtherMeeting
,Training
,Operational
,Financial
,Compliance
,CreatedBy
,CreatedOn
,IsPost
)
values( 
 @AuditId
,@IssueName
,@IssueDetails
,@DateOfSubmission
,@IssueOpenDate
,@IssueDeadLine
,@ImplementationDate
,@IssuePriority
,@IssueStatus
,@Risk
,@InvestigationOrForensis
,@StratigicMeeting
,@ManagementReviewMeeting
,@OtherMeeting
,@Training
,@Operational
,@Financial
,@Compliance
,@CreatedBy
,@CreatedOn
 ,@IsPost

)     SELECT SCOPE_IDENTITY() ";

                var command = CreateCommand(sqlText);

                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@IssueName", SqlDbType.NVarChar).Value = model.IssueName; 
                command.Parameters.Add("@IssueDetails", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.IssueDetails) ? (object)DBNull.Value : model.IssueDetails;
                command.Parameters.Add("@DateOfSubmission", SqlDbType.DateTime).Value = model.DateOfSubmission;
                command.Parameters.Add("@IssueOpenDate", SqlDbType.DateTime).Value = model.IssueOpenDate;
                command.Parameters.Add("@IssueDeadLine", SqlDbType.DateTime).Value = model.IssueDeadLine;
                command.Parameters.Add("@ImplementationDate", SqlDbType.DateTime).Value = model.ImplementationDate;
                command.Parameters.Add("@IssuePriority", SqlDbType.NVarChar).Value = model.IssuePriority;
                command.Parameters.Add("@IssueStatus", SqlDbType.NVarChar).Value = model.IssueStatus;
                command.Parameters.Add("@Risk", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Risk) ? (object)DBNull.Value : model.Risk;
                command.Parameters.Add("@InvestigationOrForensis", SqlDbType.Bit).Value = model.InvestigationOrForensis;
                command.Parameters.Add("@StratigicMeeting", SqlDbType.Bit).Value = model.StratigicMeeting;
                command.Parameters.Add("@ManagementReviewMeeting", SqlDbType.Bit).Value = model.ManagementReviewMeeting;
                command.Parameters.Add("@OtherMeeting", SqlDbType.Bit).Value = model.OtherMeeting;
                command.Parameters.Add("@Training", SqlDbType.Bit).Value = model.Training;
                command.Parameters.Add("@Operational", SqlDbType.Bit).Value = model.Operational;
                command.Parameters.Add("@Financial", SqlDbType.Bit).Value = model.Financial;
                command.Parameters.Add("@Compliance", SqlDbType.Bit).Value = model.Compliance;
                command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = model.Audit.CreatedBy;
                command.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = model.Audit.CreatedOn;
                command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "N";

                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public IssueRejectComments IssueApprove(IssueRejectComments model)
        {
            try
            {
                string sql = @"

insert into IssueRejectComments(
 AuditId
,AuditIssueId
,IsIssueApprove
,IssueApproveComments
,ApprovedBy
,ApprovedOn
)

values( 
 @AuditId
,@AuditIssueId
,@IsIssueApprove
,@IssueApproveComments
,@ApprovedBy
,@ApprovedOn

)SELECT SCOPE_IDENTITY()


        ";

                var command = CreateCommand(sql);
                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@AuditIssueId", SqlDbType.NVarChar).Value = model.AuditIssueId;
                command.Parameters.Add("@IsIssueApprove", SqlDbType.Bit).Value = model.IsIssueApprove;
                command.Parameters.Add("@IssueApproveComments", SqlDbType.NVarChar).Value = model.IssueApproveComments;
                command.Parameters.Add("@ApprovedBy", SqlDbType.NVarChar).Value = model.Audit.CreatedBy;
                command.Parameters.Add("@ApprovedOn", SqlDbType.DateTime).Value = model.Audit.CreatedOn;

                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;



                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public AuditIssue IssueApproveStatusUpdate(AuditIssue model)
        {
            try
            {
                string sql = @"

       UPDATE A_AuditIssues
       SET
       IsIssueApprove = @IsIssueApprove
       ,IssueApproveComments = @IssueApproveComments     
       WHERE Id = @Id


        ";


                var command = CreateCommand(sql);
                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@IsIssueApprove", SqlDbType.Bit).Value = 1;
                command.Parameters.Add("@IssueApproveComments", SqlDbType.VarChar).Value = "Approved By HOD";
                int rowCount = Convert.ToInt32(command.ExecuteNonQuery());
                if (rowCount <= 0)
                {
                    throw new Exception(MessageModel.UpdateFail);
                }
                return new AuditIssue();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IssueRejectComments IssueRejectStatusUpdate(IssueRejectComments model)
        {

            //DataTable commentsDt = new DataTable();
            //string slqCommentsGets = @"select IssueRejectComments FROM A_AuditIssues where 1 =1 and Id = @Id";
            ////sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
            //SqlDataAdapter objComm = CreateAdapter(slqCommentsGets);
            //objComm.SelectCommand = ApplyParameters(objComm.SelectCommand,new string[0],new string[0]);
            //objComm.SelectCommand.Parameters.AddWithValue("@Id",model.AuditIssueId);
            //objComm.Fill(commentsDt);
            //if (commentsDt.Rows.Count > 1)
            //{
            //    foreach (DataRow row in commentsDt.Rows)
            //    {
            //        string issueRejectComments = row["IssueRejectComments"].ToString();
            //        model.IssuesRejectComments = issueRejectComments + "," + model.IssuesRejectComments;
            //    }
            //}
           
            try
            {
                string sql = @"
         --UPDATE A_AuditIssues
         --SET
         --IsIssueReject = @IsIssueReject
         --,IssueRejectComments = @IssueRejectComments   
         --WHERE Id = @Id

insert into IssueRejectComments(

 AuditId
,AuditIssueId
,IsIssueReject
,IssueRejectComments
,CreatedBy
,CreatedOn
)

values( 
 @AuditId
,@AuditIssueId
,@IsIssueReject
,@IssueRejectComments
,@CreatedBy
,@CreatedOn

)SELECT SCOPE_IDENTITY()
";


                var command = CreateCommand(sql);

                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@AuditIssueId", SqlDbType.NVarChar).Value = model.AuditIssueId;
                command.Parameters.Add("@IsIssueReject", SqlDbType.Bit).Value = model.IsIssueReject;
                command.Parameters.Add("@IssueRejectComments", SqlDbType.NVarChar).Value = model.IssuesRejectComments;        
                command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = model.Audit.CreatedBy;
                command.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = model.Audit.CreatedOn;
        
                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;



                //var command = CreateCommand(sql);
                //command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                //command.Parameters.Add("@IsIssueReject", SqlDbType.VarChar).Value = 1;
                //command.Parameters.Add("@IsIssueReject", SqlDbType.Bit).Value = 1;
                //command.Parameters.Add("@IssueRejectComments", SqlDbType.VarChar).Value = Comments;
                //int rowCount = Convert.ToInt32(command.ExecuteNonQuery());
                //if (rowCount <= 0)
                //{
                //    throw new Exception(MessageModel.UpdateFail);
                //}
                //return model;
                // return new AuditIssue();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public AuditIssue MultiplePost(AuditIssue vm)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  ";
                SqlCommand command = CreateCommand(query);

                foreach (string ID in vm.IDs)
                {

                    query = @"  update A_AuditIssues set 
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


        public AuditIssue MultipleUnPost(AuditIssue vm)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;
                string query = @"  ";
                SqlCommand command = CreateCommand(query);



                foreach (string ID in vm.IDs)
                {


                    if (vm.Operation == "unpost")
                    {
                        query = @"   update A_AuditIssues set 

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
                        command.Parameters.Add("@ReasonOfUnPost", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.UnPostReasonOfIssue.ToString()) ? (object)DBNull.Value : vm.UnPostReasonOfIssue.ToString();

                        rowcount = command.ExecuteNonQuery();

                    }







                }
                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.PostFail);
                }

                return vm;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AuditIssue ReportStatusUpdate(AuditIssue model)
        {
            try
            {
                string sql = @"

        UPDATE A_AuditIssues
        SET
        ReportStatus = @ReportStatus
        
        WHERE Id = @Id
        ";


                var command = CreateCommand(sql);


                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@ReportStatus", SqlDbType.VarChar).Value = model.ReportStatus;

                int rowCount = Convert.ToInt32(command.ExecuteNonQuery());


                if (rowCount <= 0)
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

        public AuditIssue Update(AuditIssue model)
        {
            try
            {
                string sql = @"

        UPDATE A_AuditIssues SET
        AuditId = @AuditId,
        IssueName = @IssueName,
        IssueDetails = @IssueDetails,
        DateOfSubmission = @DateOfSubmission,
        IssueOpenDate = @IssueOpenDate,
        ImplementationDate = @ImplementationDate,
        IssueDeadLine = @IssueDeadLine,
        IssuePriority = @IssuePriority,
        InvestigationOrForensis = @InvestigationOrForensis,
        StratigicMeeting = @StratigicMeeting,
        ManagementReviewMeeting = @ManagementReviewMeeting,
        OtherMeeting = @OtherMeeting,
        Training = @Training,
        Operational=@Operational,
        Financial=@Financial,
        Compliance=@Compliance,
        IssueStatus = @IssueStatus,
        Risk = @Risk,
        LastUpdateBy = @LastUpdateBy,
        LastUpdateOn = @LastUpdateOn

        WHERE Id = @Id
        ";

                
                var command = CreateCommand(sql);

                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@IssueName", SqlDbType.NVarChar).Value = model.IssueName;
                command.Parameters.Add("@IssueDetails", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.IssueDetails) ? (object)DBNull.Value : model.IssueDetails;
                command.Parameters.Add("@DateOfSubmission", SqlDbType.DateTime).Value = model.DateOfSubmission;
                command.Parameters.Add("@IssueOpenDate", SqlDbType.DateTime).Value = model.IssueOpenDate;
                command.Parameters.Add("@IssueDeadLine", SqlDbType.DateTime).Value = model.IssueDeadLine;
                command.Parameters.Add("@ImplementationDate", SqlDbType.DateTime).Value = model.ImplementationDate;
                command.Parameters.Add("@IssuePriority", SqlDbType.NVarChar).Value = model.IssuePriority;  
                command.Parameters.Add("@IssueStatus", SqlDbType.NVarChar).Value = model.IssueStatus;
                command.Parameters.Add("@InvestigationOrForensis", SqlDbType.Bit).Value = model.InvestigationOrForensis;
                command.Parameters.Add("@StratigicMeeting", SqlDbType.Bit).Value = model.StratigicMeeting;
                command.Parameters.Add("@ManagementReviewMeeting", SqlDbType.Bit).Value = model.ManagementReviewMeeting;
                command.Parameters.Add("@OtherMeeting", SqlDbType.Bit).Value = model.OtherMeeting;
                command.Parameters.Add("@Training", SqlDbType.Bit).Value = model.Training;
                command.Parameters.Add("@Operational", SqlDbType.Bit).Value = model.Operational;
                command.Parameters.Add("@Financial", SqlDbType.Bit).Value = model.Financial;
                command.Parameters.Add("@Compliance", SqlDbType.Bit).Value = model.Compliance;
                command.Parameters.Add("@Risk", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Risk) ? (object)DBNull.Value : model.Risk;
                command.Parameters.Add("@LastUpdateBy", SqlDbType.NVarChar).Value = model.Audit.LastUpdateBy;
                command.Parameters.Add("@LastUpdateOn", SqlDbType.DateTime).Value = model.Audit.LastUpdateOn;

                int rowCount = Convert.ToInt32(command.ExecuteNonQuery());

                if (rowCount <= 0)
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

