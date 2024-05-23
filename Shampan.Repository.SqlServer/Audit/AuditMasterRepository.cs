using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Audit;
using Shampan.Models;
using Shampan.Models.AuditModule;
using SixLabors.ImageSharp.ColorSpaces;

namespace Shampan.Repository.SqlServer.Audit
{
    public class AuditMasterRepository : Repository, IAuditMasterRepository
    {
        private DbConfig _dbConfig;
       
        public AuditMasterRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;
            
        }

        public List<AuditMaster> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT  [Id]
      ,[Code]
      ,[Name]
      ,[AuditTypeId]
      ,[IsPlaned]
      ,[Location]
      ,[TeamId]
      ,[BranchID]
      ,[StartDate]
      ,[EndDate]
      ,[Duratiom]
      ,[BusinessTarget]
      ,[AuditStatus]
      ,[ReportStatus]
      ,[CreatedBy]
      ,[CreatedOn]
      ,[CreatedFrom]
      ,[LastUpdateBy]
      ,[LastUpdateOn]
      ,[LastUpdateFrom]
      ,[IsPosted]
      ,[PostedBy]
      ,[PostedOn]
      ,[PostedFrom]
      ,[ReasonOfUnPost]
      ,[CompanyId]
      ,[IsPost]
      ,[Remarks]

      ,isnull([IsRejected],'0')IsRejected
      ,isnull([IssueIsRejected],'0')IssueIsRejected
      ,isnull([BFIsRejected],'0')BFIsRejected
      ,isnull([IsApprovedL4],'0')IsApprovedL4
      ,isnull([IsCompleteIssue],'0')IsCompleteIssue
      ,isnull([IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
      ,isnull([IsCompleteIssueBranchFeedback],'0')IsCompleteIssueBranchFeedback

      ,(select AuditType FROM AuditTypes WHERE Id = A_Audits.AuditTypeId)AuditTypeName
      ,(select TeamName FROm A_Teams WHERE Id = A_Audits.TeamId)TeamName

       FROM [A_Audits]  

       WHERE 1=1";
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.Fill(dt);

                VMs = dt.ToList<AuditMaster>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditUser> GetAuditUserByTeamId(string TeamId, PeramModel pm=null)
        {
            string sqlText = "";
            List<AuditUser> VMs = new List<AuditUser>();
            DataTable dt = new DataTable();
            try
            {
                sqlText = $@"
 SELECT
 TM.TeamId
 ,TM.UserId
 ,u.Email EmailAddress
 ,u.UserName
 FROM A_TeamMembers TM
 LEFT JOIN {AuthDbConfig.AuthDB}.[dbo].[AspNetUsers] u on u.Id=TM.UserId
 WHERE TeamId=@TeamId
";

                // sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@TeamId", TeamId);
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

        public List<AuditMaster> FeedBackGetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();
            try
            {
                sqlText = $@"

declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);
declare @UserId varchar(max);

--select @UserId=Id  from GDICAuditAuthDB.dbo.AspNetUsers where UserName=@UserName
SELECT @UserId=Id  FROM {AuthDbConfig.AuthDB}.dbo.AspNetUsers where UserName=@UserName

Create table #TempId(Id int)
select @A1=AuditFeedbackApproval1,@A2=AuditFeedbackApproval2,@A3=AuditFeedbackApproval3,@A4=AuditFeedbackApproval4  from UserRolls
where  IsAuditFeedback=1 and UserId=@UserId

if(@A4=1)
begin
insert into  #TempId(Id)
select Id from A_Audits where IssueIsApprovedL4=1 and  isnull(BFIsRejected,0)=0 and BFIsApprovedL1=1and BFIsApprovedL2=1 and BFIsApprovedL3=1and BFIsApprovedL4=0
end
if(@A3=1)
begin
insert into  #TempId(Id)
 
select id from A_Audits where IssueIsApprovedL4=1 and  BFIsRejected=0 and BFIsApprovedL1=1and BFIsApprovedL2=1 and BFIsApprovedL3=0 and BFIsApprovedL4=0
end
if(@A2=1)
begin
insert into  #TempId(Id)
select id from A_Audits where IssueIsApprovedL4=1 and  BFIsRejected=0 and BFIsApprovedL1=1and BFIsApprovedL2=0 and BFIsApprovedL3=0 and BFIsApprovedL4=0
end
if(@A1=1)
begin
insert into  #TempId(Id)
select id from A_Audits where IssueIsApprovedL4=1 and  BFIsRejected=0 and BFIsApprovedL1=0and BFIsApprovedL2=0 and BFIsApprovedL3=0 and BFIsApprovedL4=0
end

 select 
 ad.Id
,ad.[Code]
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,case 
				 when isnull(ad.BFIsRejected,0)=1 then 'Reject'
				 when isnull(ad.BFIsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(ad.BFIsApprovedL3,0)=1 then 'Waiting For Approval 4' 
				 when isnull(ad.BFIsApprovedL2,0)=1 then 'Waiting For Approval 3' 
				 when isnull(ad.BFIsApprovedL1,0)=1 then 'Waiting For Approval 2' 
				 else 'Waiting For Approval 1' 
				 end ApproveStatus
				 ,case when isnull(ad.IsAudited,0)=1 then 'Audited' else 'Not yet Audited' end AuditStatus
,ad.[IsPost]
from A_Audits ad 
where 1=1
and IsApprovedL4=1
and ad.id in( select id from #TempId)

";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  ORDER BY  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                sqlText += @"  drop table #TempId  ";


                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value = index.UserName;
                objComm.Fill(dt);

                VMs = dt.ToList<AuditMaster>();
                return VMs;

            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<AuditMaster> IssueGetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = $@"


declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);
declare @UserId varchar(max);

--select @UserId=Id  from GDICAuditAuthDB.dbo.AspNetUsers where UserName=@UserName
SELECT @UserId=Id  FROM {AuthDbConfig.AuthDB}.dbo.AspNetUsers where UserName=@UserName

Create table #TempId(Id int)
select @A1=AuditIssueApproval1,@A2=AuditIssueApproval2,@A3=AuditIssueApproval3,@A4=AuditIssueApproval4  from UserRolls
where  IsAuditIssue=1 and UserId=@UserId


if(@A4=1)
begin
insert into  #TempId(Id)
select Id from A_Audits where IsApprovedL4=1 and  isnull(IssueIsRejected,0)=0 and IssueIsApprovedL1=1and IssueIsApprovedL2=1 and IssueIsApprovedL3=1and IssueIsApprovedL4=0
end
if(@A3=1)
begin
insert into  #TempId(Id)
 
select id from A_Audits where IsApprovedL4=1 and  IssueIsRejected=0 and IssueIsApprovedL1=1and IssueIsApprovedL2=1 and IssueIsApprovedL3=0 and IssueIsApprovedL4=0
end
if(@A2=1)
begin
insert into  #TempId(Id)
select id from A_Audits where IsApprovedL4=1 and  IssueIsRejected=0 and IssueIsApprovedL1=1and IssueIsApprovedL2=0 and IssueIsApprovedL3=0 and IssueIsApprovedL4=0
end
if(@A1=1)
begin
insert into  #TempId(Id)
select id from A_Audits where IsApprovedL4=1 and  IssueIsRejected=0 and IssueIsApprovedL1=0and IssueIsApprovedL2=0 and IssueIsApprovedL3=0 and IssueIsApprovedL4=0
end

 select 
 ad.Id
,ad.[Code]
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,case 
				 when isnull(ad.IssueIsRejected,0)=1 then 'Reject'
				 when isnull(ad.IssueIsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(ad.IssueIsApprovedL3,0)=1 then 'Waiting For Approval 4' 
				 when isnull(ad.IssueIsApprovedL2,0)=1 then 'Waiting For Approval 3' 
				 when isnull(ad.IssueIsApprovedL1,0)=1 then 'Waiting For Approval 2' 
				 else 'Waiting For Approval 1' 
				 end ApproveStatus
				 ,case when isnull(ad.IsAudited,0)=1 then 'Audited' else 'Not yet Audited' end AuditStatus
,ad.[IsPost]
from A_Audits ad 
  where 1=1
--and IsApprovedL4=1 
and IsApprovedL4=1 and IsCompleteIssueBranchFeedback=1
and ad.id in( select id from #TempId)

";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                sqlText += @"  drop table #TempId";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value = index.UserName;

                //objComm.SelectCommand.Parameters.AddWithValue("@BranchId", index.CurrentBranchid);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditMaster>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<AuditMaster> GetAuditApproveIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();       

            try
            {
                sqlText = $@"

declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);
declare @UserId varchar(max);

--select @UserId=Id  from GDICAuditAuthDB.dbo.AspNetUsers where UserName=@UserName
SELECT @UserId=Id  FROM {AuthDbConfig.AuthDB}.dbo.AspNetUsers where UserName=@UserName

Create table #TempId(Id int)
select @A1=AuditApproval1,@A2=AuditApproval2,@A3=AuditApproval3,@A4=AuditApproval4  from UserRolls
where  IsAudit=1 and UserId=@UserId

if(@A4=1)
begin
insert into  #TempId(Id)
select Id from A_Audits where IsRejected=0 and IsApprovedL1=1 and IsApprovedL2=1 and IsApprovedL3=1and IsApprovedL4=0
end
if(@A3=1)
begin
insert into  #TempId(Id)
 
select id from A_Audits where IsRejected=0 and IsApprovedL1=1 and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A2=1)
begin
insert into  #TempId(Id)
select id from A_Audits where IsRejected=0 and IsApprovedL1=1 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A1=1)
begin
insert into  #TempId(Id)
select id from A_Audits where IsRejected=0 and IsApprovedL1=0 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end

 select 
 ad.Id
,ad.[Code]
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,case 
				 when isnull(ad.IsRejected,0)=1 then 'Reject'
				 when isnull(ad.IsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(ad.IsApprovedL3,0)=1 then 'Waiting For Approval 4' 
				 when isnull(ad.IsApprovedL2,0)=1 then 'Waiting For Approval 3' 
				 when isnull(ad.IsApprovedL1,0)=1 then 'Waiting For Approval 2' 
				 else 'Waiting For Approval 1' 
				 end ApproveStatus
				 ,case when isnull(ad.IsAudited,0)=1 then 'Audited' else 'Not yet Audited' end AuditStatus


,ad.[IsPost]

from A_Audits ad 

where 1=1

and ad.id in( select id from #TempId)

";
                if (index.self)
                {

                    sqlText += @"  and ad.CreatedBy = @UserName";
                }

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                sqlText += @"  drop table #TempId  ";


                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value = index.UserName;
        
                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditMaster>();

                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditMaster> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = $@"

declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);
declare @UserId varchar(max);

--select @UserId=Id  from GDICAuditAuthDB.dbo.AspNetUsers where UserName=@UserName
SELECT @UserId=Id  FROM {AuthDbConfig.AuthDB}.dbo.AspNetUsers where UserName=@UserName

Create table #TempId(Id int)
select @A1=AuditApproval1,@A2=AuditApproval1,@A3=AuditApproval1,@A4=AuditApproval1  from UserRolls
where  IsAudit=1 and UserId=@UserId

if(@A4=1)
begin
insert into  #TempId(Id)
select Id from A_Audits where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1and IsApprovedL4=0
end
if(@A3=1)
begin
insert into  #TempId(Id)
 
select id from A_Audits where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A2=1)
begin
insert into  #TempId(Id)
select id from A_Audits where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A1=1)
begin
insert into  #TempId(Id)
select id from A_Audits where IsRejected=0 and IsApprovedL1=0and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end

 select 
 ad.Id
,ad.[Code]
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,case 
				 when isnull(ad.IsRejected,0)=1 then 'Reject'
				 when isnull(ad.IsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(ad.IsApprovedL3,0)=1 then 'Waiting For Approval 4' 
				 when isnull(ad.IsApprovedL2,0)=1 then 'Waiting For Approval 3' 
				 when isnull(ad.IsApprovedL1,0)=1 then 'Waiting For Approval 2' 
				 else 'Waiting For Approval 1' 
				 end ApproveStatus
				 ,case when isnull(ad.IsAudited,0)=1 then 'Audited' else 'Not yet Audited' end AuditStatus




,ad.[IsPost]


from A_Audits ad 


  where 1=1  and ad.CreatedBy=@UserName

and ad.id in( select id from #TempId)

";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                sqlText += @"  drop table #TempId  ";


                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value = index.UserName;
                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditMaster>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditMaster> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string auth = AuthDbConfig.AuthDB;

            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();
            try
            {
                //For Audit Index
                if (index.Status == "Audit")
                {
                    sqlText = @"

 select 
 ad.Id
,ad.[Code]
,ad.[AuditStatus]

,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
,isnull(ad.[IsCompleteIssueBranchFeedback],'0')IsCompleteIssueBranchFeedback

,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,ad.[IsPlaned]
,bp.BranchName

,At.AuditType AuditTypeId

,case 
	    		 when isnull(ad.IsRejected,0)=1 then 'Reject'
				 when isnull(ad.IsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(ad.IsApprovedL3,0)=1 then 'Waiting For Approval 4' 
				 when isnull(ad.IsApprovedL2,0)=1 then 'Waiting For Approval 3' 
				 when isnull(ad.IsApprovedL1,0)=1 then 'Waiting For Approval 2' 
				 else 'Waiting For Approval 1' 
				 end ApproveStatus

--,case when isnull(ad.IsAudited,0)=1 then 'Audited' else 'Not yet Audited' end AuditStatus

,ad.[IsPost]

from A_Audits ad 

left outer join BranchProfiles bp on bp.BranchID = ad.BranchID
--left outer join A_AuditIssues AI on AI.AuditId = ad.Id
left outer join AuditTypes AT on AT.Id = ad.AuditTypeId

where 1=1
";

                    if (index.AuditPlan == "Plan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 1";
                    }
                    if (index.AuditPlan == "UnPlan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 0";
                    }
                    if (index.AuditApprove == "Approve")
                    {
                        sqlText = sqlText + "and ad.[IsApprovedL4] = 1";
                    }
                    if (index.AuditApprove == "Pending")
                    {
                        sqlText = sqlText + "and ad.[IsApprovedL4] = 0";
                    }

                    if (index.IssueComplete == "Complete")
                    {
                        sqlText = sqlText + "and ad.[IsCompleteIssue] = 1";
                    }
                    if (index.IssueComplete == "Pending")
                    {
                        sqlText = sqlText + "and ad.[IsCompleteIssue] = 0";
                    }
                    if (index.Feedback == "Complete")
                    {
                        sqlText = sqlText + "and ad.[IsCompleteIssueTeamFeedback] = 1";
                    }
                    if (index.Feedback == "Pending")
                    {
                        sqlText = sqlText + "and ad.[IsCompleteIssueTeamFeedback] = 0";
                    }
                    if (index.BranchFeedback == "Complete")
                    {
                        sqlText = sqlText + "and ad.[IsCompleteIssueBranchFeedback] = 1";
                    }
                    if (index.BranchFeedback == "Pending")
                    {
                        sqlText = sqlText + "and ad.[IsCompleteIssueBranchFeedback] = 0";
                    }


                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);


                    // Apply date range condition
                    if(!string.IsNullOrEmpty(index.FromDate) && !string.IsNullOrEmpty(index.ToDate))
                    {
                        DateTime fromDate = DateTime.Parse(index.FromDate);
                        DateTime toDate = DateTime.Parse(index.ToDate);
                        if (fromDate != toDate)
                        {
                            sqlText += " AND ad.StartDate >= @fromDate AND ad.EndDate <= DATEADD(DAY, 1, @toDate)";

                        }
                        else
                        {
                            //sqlText += " AND ad.StartDate >= @fromDate AND ad.EndDate <= DATEADD(DAY, 1, @toDate)";
                            sqlText += " AND ad.StartDate >= DATEADD(MONTH, -6, @fromDate) AND ad.EndDate <= DATEADD(MONTH, 6, @toDate)";

                        }
                    }
                    

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);


                    if (!string.IsNullOrEmpty(index.FromDate) && !string.IsNullOrEmpty(index.ToDate))
                    {
                        objComm.SelectCommand.Parameters.AddWithValue("@fromDate", index.FromDate);
                        objComm.SelectCommand.Parameters.AddWithValue("@toDate", index.ToDate);
                    }


                    objComm.Fill(dt);

                    VMs = dt
                        .ToList<AuditMaster>();
                    return VMs;
                }
           
                else if (index.Status == "IndividualAudit")
                {
 sqlText = @"

 select 
 ad.Id
,ad.[Code]
,ad.[AuditStatus]
,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
,isnull(ad.[IsCompleteIssueBranchFeedback],'0')IsCompleteIssueBranchFeedback
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate
,ad.[IsPlaned]

,bp.BranchName
,At.AuditType AuditTypeId

,case 
	    when isnull(ad.IsRejected,0)=1 then 'Reject'
		when isnull(ad.IsApprovedL4,0)=1 then 'Approveed' 
		when isnull(ad.IsApprovedL3,0)=1 then 'Waiting For Approval 4' 
		when isnull(ad.IsApprovedL2,0)=1 then 'Waiting For Approval 3' 
		when isnull(ad.IsApprovedL1,0)=1 then 'Waiting For Approval 2' 
		else 'Waiting For Approval 1' 
		end ApproveStatus

,ad.[IsPost]

from A_Audits ad 
left outer join BranchProfiles bp on bp.BranchID = ad.BranchID
left outer join AuditTypes AT on AT.Id = ad.AuditTypeId
where 1=1 AND ad.CreatedBy = @UserName AND ad.IsApprovedL4 != 1
";

        if (index.AuditPlan == "Plan")
        {
            sqlText = sqlText + "and ad.[IsPlaned] = 1";
        }
        if (index.AuditPlan == "UnPlan")
        {
            sqlText = sqlText + "and ad.[IsPlaned] = 0";
        }
        if (index.AuditApprove == "Approve")
        {
            sqlText = sqlText + "and ad.[IsApprovedL4] = 1";
        }
        if (index.AuditApprove == "Pending")
        {
            sqlText = sqlText + "and ad.[IsApprovedL4] = 0";
        }

        if (index.IssueComplete == "Complete")
        {
            sqlText = sqlText + "and ad.[IsCompleteIssue] = 1";
        }
        if (index.IssueComplete == "Pending")
        {
            sqlText = sqlText + "and ad.[IsCompleteIssue] = 0";
        }
        if (index.Feedback == "Complete")
        {
            sqlText = sqlText + "and ad.[IsCompleteIssueTeamFeedback] = 1";
        }
        if (index.Feedback == "Pending")
        {
            sqlText = sqlText + "and ad.[IsCompleteIssueTeamFeedback] = 0";
        }
        if (index.BranchFeedback == "Complete")
        {
            sqlText = sqlText + "and ad.[IsCompleteIssueBranchFeedback] = 1";
        }
        if (index.BranchFeedback == "Pending")
        {
            sqlText = sqlText + "and ad.[IsCompleteIssueBranchFeedback] = 0";
        }


        sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

        // ToDo Escape Sql Injection
        sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
        sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
        SqlDataAdapter objComm = CreateAdapter(sqlText);
        objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
        objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.createdBy);

        objComm.Fill(dt);
        VMs = dt.ToList<AuditMaster>();
        return VMs;

}

            //For Deshboard

                else if (index.Status == "AuditApproved")
                {
                    sqlText = @"

 SELECT 
 ad.Id
,ad.[Code]

,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback

,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,ad.[IsPlaned]
,ad.[IsPost]
,bp.BranchName

from A_Audits ad 
left outer join BranchProfiles bp on bp.BranchID = ad.BranchID

where 1=1 and ad.IsApprovedL4=1 and ad.ApprovedByL4=@UserName

";

                    if (index.AuditPlan == "Plan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 1";
                    }
                    if (index.AuditPlan == "UnPlan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 0";
                    }


                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
        
                    objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.createdBy);

                    objComm.Fill(dt);

                    VMs = dt
                        .ToList<AuditMaster>();
                    return VMs;

                }

                else if (index.Status == "AuditRejected")
                {


                    sqlText = @"

 select 
 ad.Id
,ad.[Code]
,ad.[IsPlaned]

,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback

,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,ad.[IsPost]

from A_Audits ad 

where 1=1 and ad.isrejected=1 ";


                    //and ad.RejectedBy = @UserName
                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                    //objComm.SelectCommand.Parameters.AddWithValue("@BranchId", index.CurrentBranchid);
                    //objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.createdBy);

                    objComm.Fill(dt);

                    VMs = dt
                        .ToList<AuditMaster>();
                    return VMs;

                }

                else if (index.Status == "IssueRejected")
                {

                    sqlText = @"

select 
 ad.Id
,ad.[Code]
,ad.[IsPlaned]
,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback

,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate


,ad.[IsPost]

from A_Audits ad 

where 1=1 and ad.IssueIsRejected=1  

                     ";

                    //and ad.IssueRejectedBy=@UserName

                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                    objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.createdBy);

                    objComm.Fill(dt);

                    VMs = dt
                        .ToList<AuditMaster>();
                    return VMs;

                }

                else if (index.Status == "IssueRisk")
                {
                    sqlText = @"


 SELECT  
 ad.Id
,ad.[Code]
,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,ad.[IsPlaned]
,ad.[IsPost]
,bp.BranchName
 
FROM A_AuditIssues ai 

left outer join A_audits ad on ad.Id = ai.AuditId
left outer join BranchProfiles bp on bp.BranchID = ad.BranchID

WHERE risk IS NOT NULL and ai.Createdby=@UserName

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
                        .ToList<AuditMaster>();
                    return VMs;

                }

                else if (index.Status == "Issues")
                {
                    sqlText = @"


 SELECT

 ad.Id
,ad.[Code]
,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,ad.[IsPlaned]
,ad.[IsPost]
,bp.BranchName
 
FROM A_AuditIssues ai 

left outer join A_audits ad on ad.Id = ai.AuditId
left outer join BranchProfiles bp on bp.BranchID = ad.BranchID

WHERE ai.Createdby=@UserName

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
                        .ToList<AuditMaster>();
                    return VMs;

                }

                else if (index.Status == "BeforeDeadLineIssue")
                {

                    sqlText = @"


 SELECT

 ad.Id
,ad.[Code]
,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,ad.[IsPlaned]
,ad.[IsPost]

,bp.BranchName
 
FROM A_AuditIssues ai 

left outer join A_audits ad on ad.Id = ai.AuditId
left outer join BranchProfiles bp on bp.BranchID = ad.BranchID
WHERE GETDATE() <= ai.IssueDeadLine

--WHERE ai.Createdby=@UserName and GETDATE() <= ai.IssueDeadLine

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
                        .ToList<AuditMaster>();
                    return VMs;

                }

                else if (index.Status == "TotalFollowUpAudit")
                {

                    sqlText = @"

                 SELECT 
                
                 A.Id
                ,A.[Code]
                ,A.[IsPlaned]
                ,A.[IsApprovedL4]
                ,isnull(A.[IsCompleteIssue],'0')IsCompleteIssue
                ,isnull(A.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
                ,isnull(A.[Name] ,'')Name
                ,Format(A.[StartDate], 'yyyy-MM-dd') StartDate
                ,Format(A.[EndDate], 'yyyy-MM-dd') EndDate
                ,A.[IsPost]

                 FROM A_Audits A Where Id IN 
			     (			   
			     SELECT              
			     Distinct 

                 AI.AuditId

                 
                 FROM A_AuditIssues AI
                 LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id

                 WHERE 1=1 AND			 
			     CAST(AI.ImplementationDate AS DATE) <= CAST(GETDATE() AS DATE)
			     AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks)			      
			     )






-- select 
-- ad.Id
--,ad.[Code]
--,ad.[IsPlaned]
--,ad.[IsApprovedL4]
--,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
--,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
--,isnull(ad.[Name] ,'')Name
--,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
--,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate
--,ad.[IsPost]
--FROM A_Audits ad Where ad.Id in 
--(SELECT
--AI.AuditId
--From A_AuditIssues  AI
--WHERE AI.CreatedBy = @UserName AND AI.IssueDeadLine <= DateOfSubmission)
 
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
                        .ToList<AuditMaster>();
                    return VMs;

                }

                else if (index.Status == "PendingForAuditFeedback")
                {

                    sqlText = @"
        SELECT 
        ad.Id
       ,ad.Name AuditName
       ,ad.[Code]
       ,ad.[IsApprovedL4]
       ,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
       ,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
       ,isnull(ad.[Name] ,'')Name
       ,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
       ,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate
       
       ,ad.[IsPlaned]
       ,ad.[IsPost]

       ,bp.BranchName

       from A_AuditIssues AI

       left outer join A_Audits ad on AI.AuditId=ad.Id
       left outer join BranchProfiles bp on bp.BranchID = ad.BranchID

       WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)

	   and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks)

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
                        .ToList<AuditMaster>();
                    return VMs;

                }

                else if (index.Status == "PendingAuditResponse")
                {
                    sqlText = @"
        SELECT 
        ad.Id
       ,ad.Name AuditName
       ,ad.[Code]
       ,ad.[IsApprovedL4]
       ,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
       ,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
       ,isnull(ad.[Name] ,'')Name
       ,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
       ,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate
       
       ,ad.[IsPlaned]
       ,ad.[IsPost]

       ,bp.BranchName

       --,AI.IssueName
       --,E.EnumValue IssuePriority
       --,format(AI.DateOfSubmission,'dd/MM/yyyy') DateOfSubmission 

       from A_AuditIssues AI

       left outer join A_Audits ad on AI.AuditId=ad.Id
       left outer join BranchProfiles bp on bp.BranchID = ad.BranchID
       --left outer join Enums E on E.Id=AI.IssuePriority

       WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)

	   and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks)

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
                        .ToList<AuditMaster>();
                    return VMs;

                }


                else if (index.Status == "PendingForApproval")
                {

                sqlText = @"

select 
 ad.Id
,ad.[Code]

,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback

,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,ad.[IsPost]

from A_Audits ad 

where ad.CreatedBy=@UserName and ad.IsApprovedL4 != 1

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
                        .ToList<AuditMaster>();
                    return VMs;

                }

                else if (index.Status == "PendingAuditApproval")
                {
                    sqlText = @"

select 
 ad.Id
,ad.[Code]

,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback

,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,ad.[IsPost]

FROM A_Audits ad 
--where ad.CreatedBy=@UserName and ad.IsApprovedL4 != 1
where ad.IsApprovedL4 != 1

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
                        .ToList<AuditMaster>();
                    return VMs;


                }

                else if (index.Status == "FinalAuidtApproved")
                {
                    sqlText = @"

 select 
 ad.Id
,ad.[Code]

,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback

,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate
,ad.[IsPlaned]
,ad.[IsPost]


,bp.BranchName

FROM A_Audits ad 
LEFT outer join BranchProfiles bp on bp.BranchID = ad.BranchID
where 1=1 and ad.IssueIsApprovedL4=1

--where 1=1 and ad.BFIsApprovedL4=1 and ad.CreatedBy=@UserName



                     ";

                    if (index.AuditPlan == "Plan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 1";
                    }
                    if (index.AuditPlan == "UnPlan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 0";
                    }


                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                    
                    objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.createdBy);
                    objComm.Fill(dt);

                    VMs = dt
                        .ToList<AuditMaster>();
                    return VMs;

                }

                //End Of Deshboard

                //ForAuditIssueIndex

                else if (index.Status == "Issue")
                {
                    sqlText = $@"
 SELECT 
 ad.Id
,ad.[Code]
,ad.[AuditStatus]

,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
,isnull(ad.[IsCompleteIssueBranchFeedback],'0')IsCompleteIssueBranchFeedback

--,ad.[IsCompleteIssue]
--,ad.[IsCompleteIssueTeamFeedback]

,ad.[IsPost]
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate

,ad.[IsPlaned]
,bp.BranchName

--,ai.IssueStatus
--,CASE 
--WHEN ai.IssueStatus = 1045 THEN 'Open'
--ELSE 'Close'
--END AS IssueStatus

from A_Audits ad

left outer join BranchProfiles bp on bp.BranchID = ad.BranchID
--left outer join A_AuditIssues ai on ad.id = ai.auditid






                  where 1=1 and ad.id 
				  in(
                  select distinct AuditId from AuditUsers
                  where UserId in(  select id from {AuthDbConfig.AuthDB}.dbo.AspNetUsers where userName=@userName))

                  ";



                    if (index.AuditPlan == "Plan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 1";
                    }
                    if (index.AuditPlan == "UnPlan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 0";
                    }



                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                    objComm.SelectCommand.Parameters.AddWithValue("@userName", index.createdBy);
                   
                    objComm.Fill(dt);

                    VMs = dt
                        .ToList<AuditMaster>();
                    return VMs;


                }
                //ForAuditFeedckIndex

                else if (index.Status == "Feedback")
                {
                    sqlText = $@"
 SELECT 
 ad.Id
,ad.[Code]

,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
,isnull(ad.[IsCompleteIssueBranchFeedback],'0')IsCompleteIssueBranchFeedback

--,ad.[IsCompleteIssue]
--,ad.[IsCompleteIssueTeamFeedback]

,ad.[IsPost]
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate


,ad.[IsPlaned]
,bp.BranchName


from A_Audits ad


left outer join BranchProfiles bp on bp.BranchID = ad.BranchID


                  --where 1=1 and id 
                  where 1=1 and ad.id 
				  in(
                  select distinct AuditId from AuditUsers
                  where UserId in(  select id from {AuthDbConfig.AuthDB}.dbo.AspNetUsers where userName=@userName))

                  ";

                    if (index.AuditPlan == "Plan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 1";
                    }
                    if (index.AuditPlan == "UnPlan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 0";
                    }

                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                    objComm.SelectCommand.Parameters.AddWithValue("@userName", index.createdBy);

                    objComm.Fill(dt);

                    VMs = dt
                        .ToList<AuditMaster>();
                    return VMs;

                }

                //ForAuditBranchFeedbackIndex
                //index.Status == "BranchFeedback"
                else
                {
                    sqlText = $@"

 SELECT 
 ad.Id
,ad.[Code]

,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
,isnull(ad.[IsCompleteIssueBranchFeedback],'0')IsCompleteIssueBranchFeedback

--,ad.[IsCompleteIssue]
--,ad.[IsCompleteIssueTeamFeedback]

,ad.[IsPost]
,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate


,ad.[IsPlaned]
,bp.BranchName

from A_Audits ad

left outer join BranchProfiles bp on bp.BranchID = ad.BranchID


                 
                  where 1=1 and ad.id 
				  in(
                  select distinct AIU.AuditId from AuditIssueUsers AIU ,AuditUsers AU
                  where AIU.UserId in(  select id from {AuthDbConfig.AuthDB}.dbo.AspNetUsers where userName=@UserName) or
                  AU.UserId in(  select id from {AuthDbConfig.AuthDB}.dbo.AspNetUsers where userName=@UserName))

                  ";


                    if (index.AuditPlan == "Plan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 1";
                    }
                    if (index.AuditPlan == "UnPlan")
                    {
                        sqlText = sqlText + "and ad.[IsPlaned] = 0";
                    }

                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                    SqlDataAdapter objComm = CreateAdapter(sqlText);
                    objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                    //objComm.SelectCommand.Parameters.AddWithValue("@userName", index.createdBy);
                    objComm.SelectCommand.Parameters.AddWithValue("@userName", index.UserName);

                    //objComm.SelectCommand.Parameters.AddWithValue("@BranchId", index.CurrentBranchid);

                    objComm.Fill(dt);

                    VMs = dt
                        .ToList<AuditMaster>();
                    return VMs;

                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string auth = AuthDbConfig.AuthDB;

            string sqlText = "";
            DataTable dt = new DataTable();
            try
            {
                if (index.Status == "Audit")
                {
                    sqlText = @"
              select count(ad.ID) FilteredCount
              from  A_Audits ad 
              where 1=1  
              ";


                    //AddForExcel
                    if (!string.IsNullOrEmpty(index.FromDate) && !string.IsNullOrEmpty(index.ToDate))
                    {
                        DateTime fromDate = DateTime.Parse(index.FromDate);
                        DateTime toDate = DateTime.Parse(index.ToDate);
                        if (fromDate != toDate)
                        {
                            sqlText += " AND ad.StartDate >= @fromDate AND ad.EndDate <= DATEADD(DAY, 1, @toDate)";

                        }
                        else
                        {
                                                  
                        }
                    }


                }

                if (index.Status == "IndividualAudit")
                {
                 sqlText = @"
                 SELECT COUNT(A_Audits.Id)Id
                 FROM A_Audits
                 WHERE A_Audits.CreatedBy = @UserName AND A_Audits.IsApprovedL4 != 1  
                 ";
                }
         
                if (index.Status == "Issue" || index.Status == "Feedback")
                {
                  sqlText = $@"

                  select count(ad.ID) FilteredCount
                  from  A_Audits ad 

                  left outer join BranchProfiles bp on bp.BranchID = ad.BranchID

                  where 1=1 and ad.id 
				  in(
                  select distinct AuditId from AuditUsers                 
                  where UserId in(  select id from {AuthDbConfig.AuthDB}.dbo.AspNetUsers where userName=@userName))

                ";
                }
                if (index.Status == "BranchFeedback")
                {
                    sqlText = @"

                    select count(ad.ID) FilteredCount
                    from  A_Audits ad
                ";

                }




                //For Deshboard

                if (index.Status == "AuditApproved")
                {
                    sqlText = @"SELECT Count(ad.Id)Id FROM A_Audits ad
                --left Join AuditIssueUsers on  AuditIssueUsers.AuditIssueId = A_AuditIssues.Id
                --where AuditIssueUsers.UserId = @UserId

                  left outer join BranchProfiles bp on bp.BranchID = ad.BranchID
                  where ad.CreatedBy = @userName and ad.IsApprovedL4 = 1";

                }
                if (index.Status == "AuditRejected")
                {
                    sqlText = @"SELECT Count(A_Audits.Id)Id 
                FROM A_Audits
                where A_Audits.RejectedBy = @UserName and A_Audits.IsRejected = 1";

                }

                if (index.Status == "IssueRejected")
                {

                    sqlText = @"SELECT Count(ad.Id)Id
                 FROM A_Audits ad
                 where ad.IssueRejectedBy = @UserName and ad.IssueIsRejected = 1";



                }


                if (index.Status == "Issues")
                {
                    sqlText = @"

                 SELECT Count(A_AuditIssues.Id)Id

                 FROM A_AuditIssues

                 where A_AuditIssues.CreatedBy=@UserName";

                }
                if (index.Status == "BeforeDeadLineIssue")
                {
                    sqlText = @"

                 SELECT Count(A_AuditIssues.Id)Id 
                 FROM A_AuditIssues
                 where A_AuditIssues.CreatedBy=@UserName and GETDATE() <= A_AuditIssues.IssueDeadLine";

                }
                if (index.Status == "TotalFollowUpAudit")
                {
                    sqlText = @"
                
                 SELECT 
                 COUNT(Distinct AI.AuditId)Id
                 FROM A_AuditIssues AI
                 LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
                 WHERE 1=1 AND			 
			     CAST(AI.ImplementationDate AS DATE) <= CAST(GETDATE() AS DATE)
			     AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks)


                 --SELECT COUNT(ad.Id) Id
                 --FROM A_Audits ad where ad.Id in
                 --(select AI.AuditId
                 --From A_AuditIssues  AI
                 --WHERE AI.CreatedBy = @UserName AND AI.IssueDeadLine <= DateOfSubmission)";

                }


                if (index.Status == "PendingForAuditFeedback")
                {
                    sqlText = @"

                     select 
                     count(ad.Id)FilteredAmount
	                 
                     from A_AuditIssues AI
	                 
                     left outer join A_Audits ad on AI.AuditId=ad.Id
                     left outer join Enums E on E.Id=AI.IssuePriority
	                 
                     WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)
	                 and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks)

                ";

                }




                if (index.Status == "FinalAuidtApproved")
                {
                    sqlText = @"

                  SELECT Count(ad.Id)Id

                 FROM A_Audits ad

                 where ad.CreatedBy='erp' and ad.BFIsApprovedL4=1

                ";

                }

                if (index.Status == "IssueRisk")
                {
                    sqlText = @"

                   SELECT Count(AI.Id)Id
                   FROM A_AuditIssues AI

                   left outer join A_audits ad on ad.Id = AI.AuditId


                   WHERE risk IS NOT NULL and AI.Createdby = @UserName

                ";

                }

                if (index.Status == "PendingForApproval")
                {
                    sqlText = @"

                    SELECT Count(ad.Id)Id
                    FROM A_Audits ad
                    where ad.CreatedBy = @UserName and ad.IsApprovedL4 != 1

                ";

                }



                if (index.Status == "PendingAuditApproval")
                {
                    sqlText = @"

                  SELECT Count(ad.Id)Id

                 FROM A_Audits ad

                 where ad.CreatedBy='erp' and ad.IsApprovedL4 != 1

                ";

                }
                if (index.Status == "PendingAuditResponse")
                {
                    sqlText = @"

        select 
        count(ad.Id)FilteredAmount

       from A_AuditIssues AI

       left outer join A_Audits ad on AI.AuditId=ad.Id
       left outer join Enums E on E.Id=AI.IssuePriority

       WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)
	   and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks)

                ";

                }


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                //if (index.Status != "Audit" || index.Status == "AuditApproved" || index.Status == "AuditRejected")
                if (index.Status != "Audit")
                {
                    objComm.SelectCommand.Parameters.AddWithValue("@userName", index.UserName);
                    
                }

                //AddForExcel
                if(index.Status == "Audit")
                {
                    if (!string.IsNullOrEmpty(index.FromDate) && !string.IsNullOrEmpty(index.ToDate))
                    {
                        DateTime fromDate = DateTime.Parse(index.FromDate);
                        DateTime toDate = DateTime.Parse(index.ToDate);
                        if (fromDate != toDate)
                        {
                            objComm.SelectCommand.Parameters.AddWithValue("@fromDate", index.FromDate);
                            objComm.SelectCommand.Parameters.AddWithValue("@toDate", index.ToDate);
                        }
                        else
                        {
                            
                        }
                    }                
                }

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0][0]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetAuditApprovedDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";

            DataTable dt = new DataTable();
            try
            {
                sqlText = @"
DECLARE @A1 VARCHAR(1);
DECLARE @A2 VARCHAR(1);
DECLARE @A3 VARCHAR(1);
DECLARE @A4 VARCHAR(1);
DECLARE @UserId VARCHAR(MAX);
DECLARE @Count INT;

-- Set the desired UserName
--DECLARE @UserName VARCHAR(100) = 'erp';

-- Retrieve UserId based on the UserName
SELECT @UserId = Id FROM GDICAuditAuthDB.dbo.AspNetUsers WHERE UserName = @UserName;

-- Create a temporary table to store IDs
CREATE TABLE #TempId (Id INT);

-- Retrieve @A1, @A2, @A3, @A4 based on @UserId and conditions
SELECT @A1 = AuditApproval1, @A2 = AuditApproval2, @A3 = AuditApproval3, @A4 = AuditApproval4
FROM UserRolls
WHERE IsAudit = 1 AND UserId = @UserId;

-- Insert IDs into #TempId based on conditions
IF (@A4 = '1')
BEGIN
    INSERT INTO #TempId (Id)
    SELECT Id
    FROM A_Audits
    WHERE IsRejected = 0 AND IsApprovedL1 = 1 AND IsApprovedL2 = 1 AND IsApprovedL3 = 1 AND IsApprovedL4 = 0;
END

IF (@A3 = '1')
BEGIN
    INSERT INTO #TempId (Id)
    SELECT Id
    FROM A_Audits
    WHERE IsRejected = 0 AND IsApprovedL1 = 1 AND IsApprovedL2 = 1 AND IsApprovedL3 = 0 AND IsApprovedL4 = 0;
END

IF (@A2 = '1')
BEGIN
    INSERT INTO #TempId (Id)
    SELECT Id
    FROM A_Audits
    WHERE IsRejected = 0 AND IsApprovedL1 = 1 AND IsApprovedL2 = 0 AND IsApprovedL3 = 0 AND IsApprovedL4 = 0;
END

IF (@A1 = '1')
BEGIN
    INSERT INTO #TempId (Id)
    SELECT Id
    FROM A_Audits
    WHERE IsRejected = 0 AND IsApprovedL1 = 0 AND IsApprovedL2 = 0 AND IsApprovedL3 = 0 AND IsApprovedL4 = 0;
END




-- Count the IDs based on conditions

SELECT @Count = COUNT(Id)
FROM A_Audits
WHERE IsPost = 'Y' AND EXISTS (SELECT 1 FROM #TempId WHERE Id = A_Audits.Id);

SELECT 
    @Count AS FilteredCount, -- Count of IDs based on conditions
    ad.ID,
    ad.[Code]
FROM 
    A_Audits ad
WHERE 1=1

and ad.id in( select id from #TempId)


";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                sqlText = sqlText + "DROP TABLE #TempId";


                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value = index.UserName;

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0][0]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public int GetAuditIssueDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";

            DataTable dt = new DataTable();
            try
            {
                sqlText = @"


             DECLARE @A1 VARCHAR(1);
DECLARE @A2 VARCHAR(1);
DECLARE @A3 VARCHAR(1);
DECLARE @A4 VARCHAR(1);
DECLARE @UserId VARCHAR(MAX);

-- Set @UserName to the desired value


SELECT @UserId = Id FROM GDICAuditAuthDB.dbo.AspNetUsers WHERE UserName = @UserName;

CREATE TABLE #TempId (Id INT);

SELECT @A1 = AuditIssueApproval1, @A2 = AuditIssueApproval2, @A3 = AuditIssueApproval3, @A4 = AuditIssueApproval4
FROM UserRolls
WHERE IsAuditIssue = 1 AND UserId = @UserId;

IF (@A4 = '1')
BEGIN
    INSERT INTO #TempId (Id)
    SELECT Id 
    FROM A_Audits 
    WHERE IsApprovedL4 = 1 
        AND ISNULL(IssueIsRejected, 0) = 0 
        AND IssueIsApprovedL1 = 1 
        AND IssueIsApprovedL2 = 1 
        AND IssueIsApprovedL3 = 1 
        AND IssueIsApprovedL4 = 0;
END

IF (@A3 = '1')
BEGIN
    INSERT INTO #TempId (Id)
    SELECT Id 
    FROM A_Audits 
    WHERE IsApprovedL4 = 1 
        AND IssueIsRejected = 0 
        AND IssueIsApprovedL1 = 1 
        AND IssueIsApprovedL2 = 1 
        AND IssueIsApprovedL3 = 0 
        AND IssueIsApprovedL4 = 0;
END

IF (@A2 = '1')
BEGIN
    INSERT INTO #TempId (Id)
    SELECT Id 
    FROM A_Audits 
    WHERE IsApprovedL4 = 1 
        AND IssueIsRejected = 0 
        AND IssueIsApprovedL1 = 1 
        AND IssueIsApprovedL2 = 0 
        AND IssueIsApprovedL3 = 0 
        AND IssueIsApprovedL4 = 0;
END

IF (@A1 = '1')
BEGIN
    INSERT INTO #TempId (Id)
    SELECT Id 
    FROM A_Audits 
    WHERE IsApprovedL4 = 1 
        AND IssueIsRejected = 0 
        AND IssueIsApprovedL1 = 0 
        AND IssueIsApprovedL2 = 0 
        AND IssueIsApprovedL3 = 0 
        AND IssueIsApprovedL4 = 0;
END

-- Count the IDs in #TempId
DECLARE @TotalRowCount INT;
SELECT @TotalRowCount = COUNT(Id) FROM #TempId;

-- Display the count and selected columns
SELECT 
    @TotalRowCount AS TotalRowCount, 
    ad.Id,
    ad.[Code],
    ISNULL(ad.[Name], '') AS Name,
    FORMAT(ad.[StartDate], 'yyyy-MM-dd') AS StartDate,
    FORMAT(ad.[EndDate], 'yyyy-MM-dd') AS EndDate,
    CASE 
        WHEN ISNULL(ad.IssueIsRejected, 0) = 1 THEN 'Reject'
        WHEN ISNULL(ad.IssueIsApprovedL4, 0) = 1 THEN 'Approved' 
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
    1 = 1
    AND IsApprovedL4 = 1
    AND ad.Id IN (SELECT Id FROM #TempId)
";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value = index.UserName;

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0][0]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public int GetAuditFeedBackDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";

            DataTable dt = new DataTable();
            try
            {
                sqlText = @"
              declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);
declare @UserId varchar(max);

select @UserId = Id from GDICAuditAuthDB.dbo.AspNetUsers where UserName = 'erp';

Create table #TempId(Id int);

select @A1 = AuditApproval1, @A2 = AuditApproval2, @A3 = AuditApproval3, @A4 = AuditApproval4  
from UserRolls
where IsAudit = 1 and UserId = @UserId;

if (@A4 = 1)
begin
    insert into #TempId(Id)
    select Id from A_Audits where IsRejected = 0 and IsApprovedL1 = 1 and IsApprovedL2 = 1 and IsApprovedL3 = 1 and IsApprovedL4 = 0;
end
if (@A3 = 1)
begin
    insert into #TempId(Id)
    select id from A_Audits where IsRejected = 0 and IsApprovedL1 = 1 and IsApprovedL2 = 1 and IsApprovedL3 = 0 and IsApprovedL4 = 0;
end
if (@A2 = 1)
begin
    insert into #TempId(Id)
    select id from A_Audits where IsRejected = 0 and IsApprovedL1 = 1 and IsApprovedL2 = 0 and IsApprovedL3 = 0 and IsApprovedL4 = 0;
end
if (@A1 = 1)
begin
    insert into #TempId(Id)
    select id from A_Audits where IsRejected = 0 and IsApprovedL1 = 0 and IsApprovedL2 = 0 and IsApprovedL3 = 0 and IsApprovedL4 = 0;
end;

-- Count the number of rows in the temporary table
SELECT Count(*)-1 AS RowCoun FROM #TempId;

-- Drop the temporary table when you're done
DROP TABLE #TempId;
 

";

                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
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


        public AuditMaster Insert(AuditMaster model)
        {
            try
            {
                string sqlText = "";
                int Id = 0;
                sqlText = @"

insert into A_Audits(
[Code]
,[Name]
,[AuditTypeId]
,[IsPlaned]
,[TeamId]
,[BranchID]
,[StartDate]
,[EndDate]
,[Duratiom]
,[BusinessTarget]
,[AuditStatus]
,[ReportStatus]
,[CreatedBy]
,[CreatedOn]
,[CreatedFrom]
,[LastUpdateBy]
,[LastUpdateOn]
,[LastUpdateFrom]
,[CompanyId]
,[Remarks]
,[IsPost]


,IsApprovedL1
,IsApprovedL2
,IsApprovedL3
,IsApprovedL4
,IsAudited
,IsRejected
,IssueIsApprovedL1
,IssueIsApprovedL2
,IssueIsApprovedL3
,IssueIsApprovedL4
,IssueIsAudited
,IssueIsRejected
,BFIsApprovedL1
,BFIsApprovedL2
,BFIsApprovedL3
,BFIsApprovedL4
,BFIsAudited
,BFIsRejected

,IsCompleteIssue
,IsCompleteIssueTeamFeedback
,IsCompleteIssueBranchFeedback
)
values
( 
 @Code
,@Name
,@AuditTypeId
,@IsPlaned
,@TeamId
,@BranchID
,@StartDate
,@EndDate
,@Duratiom
,@BusinessTarget
,@AuditStatus
,@ReportStatus
,@CreatedBy
,@CreatedOn
,@CreatedFrom
,@LastUpdateBy
,@LastUpdateOn
,@LastUpdateFrom
,@CompanyId
,@Remarks
,@IsPost

,@IsApprovedL1
,@IsApprovedL2
,@IsApprovedL3
,@IsApprovedL4
,@IsAudited
,@IsRejected
,@IssueIsApprovedL1
,@IssueIsApprovedL2
,@IssueIsApprovedL3
,@IssueIsApprovedL4
,@IssueIsAudited
,@IssueIsRejected
,@BFIsApprovedL1
,@BFIsApprovedL2
,@BFIsApprovedL3
,@BFIsApprovedL4
,@BFIsAudited
,@BFIsRejected

,@IsCompleteIssue
,@IsCompleteIssueTeamFeedback
,@IsCompleteIssueBranchFeedback   

)SELECT SCOPE_IDENTITY() ";


                var command = CreateCommand(sqlText);
                int value = (Convert.ToDateTime(model.EndDate) - Convert.ToDateTime(model.StartDate)).Days;

                command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = model.Code;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = model.Name;
                command.Parameters.Add("@AuditTypeId", SqlDbType.Int).Value = model.AuditTypeId;
                command.Parameters.Add("@IsPlaned", SqlDbType.Bit).Value = model.IsPlaned;         
                command.Parameters.Add("@TeamId", SqlDbType.Int).Value = model.TeamId;
                command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
                command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = model.StartDate;
                command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = model.EndDate;
                command.Parameters.Add("@Duratiom", SqlDbType.Int).Value = value;              
                command.Parameters.Add("@BusinessTarget", SqlDbType.NVarChar).Value = model.BusinessTarget == null ? DBNull.Value : model.BusinessTarget;
                command.Parameters.Add("@AuditStatus", SqlDbType.NVarChar).Value = model.AuditStatus;
                command.Parameters.Add("@ReportStatus", SqlDbType.NVarChar).Value = model.ReportStatus;
                command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = model.Audit.CreatedBy;
                command.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = model.Audit.CreatedOn;
                command.Parameters.Add("@CreatedFrom", SqlDbType.NVarChar).Value = model.Audit.CreatedFrom;
                command.Parameters.Add("@LastUpdateBy", SqlDbType.NVarChar).Value = model.Audit.LastUpdateBy;
                command.Parameters.Add("@LastUpdateOn", SqlDbType.NVarChar).Value = model.Audit.LastUpdateOn;
                command.Parameters.Add("@LastUpdateFrom", SqlDbType.NVarChar).Value = model.Audit.LastUpdateFrom;
                command.Parameters.Add("@CompanyId", SqlDbType.Int).Value = model.CompanyId;
                command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = model.Remarks is null ? DBNull.Value : model.Remarks;
                command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "N";

                command.Parameters.Add("@IsApprovedL1", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IsApprovedL2", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IsApprovedL3", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IsApprovedL4", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IsAudited", SqlDbType.Bit).Value = 1;
                command.Parameters.Add("@IsRejected", SqlDbType.Bit).Value = 0;


                command.Parameters.Add("@IssueIsApprovedL1", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IssueIsApprovedL2", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IssueIsApprovedL3", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IssueIsApprovedL4", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IssueIsRejected", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IssueIsAudited", SqlDbType.Bit).Value = 0;

                command.Parameters.Add("@BFIsApprovedL1", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@BFIsApprovedL2", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@BFIsApprovedL3", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@BFIsApprovedL4", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@BFIsAudited", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@BFIsRejected", SqlDbType.Bit).Value = 0;

                command.Parameters.Add("@IsCompleteIssue", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IsCompleteIssueTeamFeedback", SqlDbType.Bit).Value = 0;
                command.Parameters.Add("@IsCompleteIssueBranchFeedback", SqlDbType.Bit).Value = 0;

                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }


            catch (Exception ex)
            {
                throw ex;
            }

        }

        public AuditMaster MultiplePost(AuditMaster vm)
        {
            try
            {
                string sqlText = "";
                int rowcount = 0;
                string query = @"  ";
                SqlCommand command = CreateCommand(query);
                foreach (string ID in vm.IDs)
                {

                    query = @"  update A_Audits set 

                     IsPost=@IsPost
                    ,PostedBy=@PostedBy
                    ,PostedOn=@PostedOn
                    ,PostedFrom=@PostedFrom
                    
                    ,IsRejected=@IsRejected 
                    ,IssueIsRejected=@IssueIsRejected 
                    ,BFIsRejected=@BFIsRejected 


                     where  Id= @Id ";


                    command = CreateCommand(query);
                    command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "Y";
                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = ID;
                    command.Parameters.Add("@PostedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedBy.ToString()) ? (object)DBNull.Value : vm.Audit.PostedBy.ToString();
                    command.Parameters.Add("@PostedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedOn.ToString()) ? (object)DBNull.Value : vm.Audit.PostedOn.ToString();
                    command.Parameters.Add("@PostedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedFrom.ToString()) ? (object)DBNull.Value : vm.Audit.PostedFrom.ToString();
                    command.Parameters.Add("@IsRejected", SqlDbType.Bit).Value = 0;
                    command.Parameters.Add("@IssueIsRejected", SqlDbType.Bit).Value = 0;
                    command.Parameters.Add("@BFIsRejected", SqlDbType.Bit).Value = 0;

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

        public AuditMaster MultipleUnPost(AuditMaster vm)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;
                string query = @"  ";
                SqlCommand command = CreateCommand(query);



                foreach (string ID in vm.IDs)
                {

                    //ForUnPost
                    if (vm.Operation == "unpost")
                    {
                        query = @"   update A_Audits set 

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
                        command.Parameters.Add("@ReasonOfUnPost", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.ReasonOfUnPost.ToString()) ? (object)DBNull.Value : vm.ReasonOfUnPost.ToString();
                        rowcount = command.ExecuteNonQuery();

                    }

                    //ForReject
                    else if (vm.Operation == "reject")
                    //else if (vm.ApproveStatus == "auditStatus")
                    {

                        query = @"update A_Audits set 

      IsPost=@IsPost  
     ,IsRejected=@IsRejected 
     ,RejectedBy=@RejectedBy  
     ,RejectedDate=@RejectedDate
     ,RejectedComments=@RejectedComments

     ,IsApprovedL1=@IsApprovedL1
     ,IsApprovedL2=@IsApprovedL2
     ,IsApprovedL3=@IsApprovedL3
     ,IsApprovedL4=@IsApprovedL4


      where  Id= @Id "
                        ;


                        command = CreateCommand(query);

                        command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
                        command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "N";
                        command.Parameters.Add("@IsRejected", SqlDbType.Bit).Value = vm.Approval.IsRejected;


                        command.Parameters.Add("@RejectedComments", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.RejectedComments) ? (object)DBNull.Value : vm.RejectedComments;
                        command.Parameters.Add("@RejectedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Approval.RejectedBy) ? (object)DBNull.Value : vm.Approval.RejectedBy;
                        command.Parameters.Add("@RejectedDate", SqlDbType.DateTime).Value = vm.Approval.RejectedDate;

                        command.Parameters.Add("@IsApprovedL1", SqlDbType.NChar).Value = 0;
                        command.Parameters.Add("@IsApprovedL2", SqlDbType.NChar).Value = 0;
                        command.Parameters.Add("@IsApprovedL3", SqlDbType.NChar).Value = 0;
                        command.Parameters.Add("@IsApprovedL4", SqlDbType.NChar).Value = 0;



                        rowcount = command.ExecuteNonQuery();

                    }

                    else if (vm.Operation == "IssueReject")
                    //else if (vm.ApproveStatus == "auditStatus")
                    {

                        query = @"update A_Audits set 

      IsPost=@IsPost  
     ,IssueIsRejected=@IssueIsRejected 
     ,IssueRejectedBy=@IssueRejectedBy  
     --,RejectedDate=@RejectedDate
     ,IssueRejectedComments=@IssueRejectedComments

     ,IssueIsApprovedL1=@IssueIsApprovedL1
     ,IssueIsApprovedL2=@IssueIsApprovedL2
     ,IssueIsApprovedL3=@IssueIsApprovedL3
     ,IssueIsApprovedL4=@IssueIsApprovedL4

     ,IsCompleteIssueBranchFeedback=@IsCompleteIssueBranchFeedback


      where  Id= @Id "
                        ;


                        command = CreateCommand(query);

                        command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
                        command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "N";
                        command.Parameters.Add("@IssueIsRejected", SqlDbType.Bit).Value = vm.Approval.IsRejected;


                        command.Parameters.Add("@IssueRejectedComments", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.RejectedComments) ? (object)DBNull.Value : vm.RejectedComments;
                        command.Parameters.Add("@IssueRejectedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Approval.RejectedBy) ? (object)DBNull.Value : vm.Approval.RejectedBy;
                        //command.Parameters.Add("@RejectedDate", SqlDbType.DateTime).Value = vm.Approval.RejectedDate;

                        command.Parameters.Add("@IssueIsApprovedL1", SqlDbType.NChar).Value = 0;
                        command.Parameters.Add("@IssueIsApprovedL2", SqlDbType.NChar).Value = 0;
                        command.Parameters.Add("@IssueIsApprovedL3", SqlDbType.NChar).Value = 0;
                        command.Parameters.Add("@IssueIsApprovedL4", SqlDbType.NChar).Value = 0;
                        command.Parameters.Add("@IsCompleteIssueBranchFeedback", SqlDbType.NChar).Value = 0;



                        rowcount = command.ExecuteNonQuery();

                    }


                    else if (vm.Operation == "BranchFeedbackReject")
                    {

                        query = @"update A_Audits set 

      IsPost=@IsPost  
     ,BFIsRejected=@BFIsRejected 
     ,BFRejectedBy=@BFRejectedBy  
     ,BFRejectedComments=@BFRejectedComments

     ,BFIsApprovedL1=@BFIsApprovedL1
     ,BFIsApprovedL2=@BFIsApprovedL2
     ,BFIsApprovedL3=@BFIsApprovedL3
     ,BFIsApprovedL4=@BFIsApprovedL4


      where  Id= @Id "
                        ;


                        command = CreateCommand(query);

                        command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
                        command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "N";
                        command.Parameters.Add("@BFIsRejected", SqlDbType.Bit).Value = vm.Approval.IsRejected;


                        command.Parameters.Add("@BFRejectedComments", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.RejectedComments) ? (object)DBNull.Value : vm.RejectedComments;
                        command.Parameters.Add("@BFRejectedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Approval.RejectedBy) ? (object)DBNull.Value : vm.Approval.RejectedBy;

                        command.Parameters.Add("@BFIsApprovedL1", SqlDbType.NChar).Value = 0;
                        command.Parameters.Add("@BFIsApprovedL2", SqlDbType.NChar).Value = 0;
                        command.Parameters.Add("@BFIsApprovedL3", SqlDbType.NChar).Value = 0;
                        command.Parameters.Add("@BFIsApprovedL4", SqlDbType.NChar).Value = 0;



                        rowcount = command.ExecuteNonQuery();

                    }



                    //else if (vm.Operation == "approved")
                    //ForApprove
                    else if (vm.ApproveStatus == "auditStatus")
                    {

                        query = @"


declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);


declare @AC1 varchar(1);
declare @AC2 varchar(1);
declare @AC3 varchar(1);
declare @AC4 varchar(1);

declare @UserId varchar(max);
Create table #TempId(Id int)

select @UserId=Id  from GDICAuditAuthDB.dbo.AspNetUsers where UserName=@UserName

select @A1=AuditApproval1,@A2=AuditApproval2,@A3=AuditApproval3,@A4=AuditApproval4  from UserRolls

where  IsAudit=1 and UserId=@UserId
select @AC1=IsApprovedL1,@AC2=IsApprovedL2,@AC3=IsApprovedL3,@AC4=IsApprovedL4 from A_Audits   where id=@Id and IsPost='Y' and  IsRejected=0 

if(@A1=1 and @AC1=0)
begin
	update A_Audits set IsApprovedL1=1 ,ApprovedByL1=@UserName, ApprovedDateL1=@date,CommentsL1=@Comments  where id=@Id and IsPost='Y' and  IsRejected=0 and IsApprovedL1=0 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
	if(@A2=1)
	begin
		update A_Audits set IsApprovedL2=1  ,ApprovedByL2=@UserName, ApprovedDateL2=@date,CommentsL2=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
		if(@A3=1)
			begin
			update A_Audits set IsApprovedL3=1  ,ApprovedByL3=@UserName, ApprovedDateL3=@date,CommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
			if(@A4=1)
			begin
			update A_Audits set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
			end
		end 
	end 
end 
else if(@A2=1 and @AC2=0)
begin
	update A_Audits set IsApprovedL2=1  ,ApprovedByL2=@UserName, ApprovedDateL2=@date,CommentsL2=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
	if(@A3=1)
		begin
		update A_Audits set IsApprovedL3=1  ,ApprovedByL3=@UserName, ApprovedDateL3=@date,CommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
		if(@A4=1)
		begin
		update A_Audits set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
		end
	end 
end 
else if(@A3=1 and @AC3=0)
begin
	update A_Audits set IsApprovedL3=1  ,ApprovedByL3=@UserName, ApprovedDateL3=@date,CommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
	if(@A4=1)
	begin
		update A_Audits set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
	end
end 
else if(@A4=1 and @AC4=0)
begin
	update A_Audits set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
end

"
                        ;


                        command = CreateCommand(query);

                        command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
                        //command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
                        command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = vm.Audit.PostedBy;
                        command.Parameters.Add("@date", SqlDbType.VarChar).Value = vm.Audit.PostedOn;
                        //command.Parameters.Add("@IsApprovedL1", SqlDbType.Bit).Value = vm.Approval.IsApprovedL1;
                        command.Parameters.Add("@Comments", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.CommentsL1) ? (object)DBNull.Value : vm.CommentsL1;

                        //if(vm.IsPost == "N")
                        //{
                        //	throw new Exception("Data is not post yet.");
                        //}

                        rowcount = command.ExecuteNonQuery();

                    }
                    else if (vm.ApproveStatus == "issueApprove")
                    {

                        query = @"


declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);


declare @AC1 varchar(1);
declare @AC2 varchar(1);
declare @AC3 varchar(1);
declare @AC4 varchar(1);

declare @UserId varchar(max);
Create table #TempId(Id int)

select @UserId=Id  from GDICAuditAuthDB.dbo.AspNetUsers where UserName=@UserName

select @A1=AuditIssueApproval1,@A2=AuditIssueApproval2,@A3=AuditIssueApproval3,@A4=AuditIssueApproval4  from UserRolls

where  IsAuditIssue=1 and UserId=@UserId
select @AC1=IssueIsApprovedL1,@AC2=IssueIsApprovedL2,@AC3=IssueIsApprovedL3,@AC4=IssueIsApprovedL4 from A_Audits   where id=@Id and IsPost='Y' and  IssueIsRejected=0 

if(@A1=1 and @AC1=0)
begin
	update A_Audits set IssueIsApprovedL1=1 ,IssueApprovedByL1=@UserName, IssueApprovedDateL1=@date,IssueCommentsL1=@Comments  where id=@Id and IsPost='Y' and  IssueIsRejected=0 and IssueIsApprovedL1=0 and IssueIsApprovedL2=0 and IssueIsApprovedL3=0 and IssueIsApprovedL4=0
	if(@A2=1)
	begin
		update A_Audits set IssueIsApprovedL2=1  ,IssueApprovedByL2=@UserName, IssueApprovedDateL2=@date,IssueCommentsL2=@Comments   where id=@Id and   IsPost='Y' and IssueIsRejected=0 and IssueIsApprovedL1=1 and IssueIsApprovedL2=0 and IssueIsApprovedL3=0 and IssueIsApprovedL4=0
		if(@A3=1)
			begin
			update A_Audits set IssueIsApprovedL3=1  ,IssueApprovedByL3=@UserName, IssueApprovedDateL3=@date,IssueCommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IssueIsRejected=0 and IssueIsApprovedL1=1and IssueIsApprovedL2=1 and IssueIsApprovedL3=0 and IssueIsApprovedL4=0
			if(@A4=1)
			begin
			update A_Audits set IssueIsApprovedL4=1 ,IssueApprovedByL4=@UserName, IssueApprovedDateL4=@date,IssueCommentsL4=@Comments   where id=@Id and   IsPost='Y' and IssueIsRejected=0 and IssueIsApprovedL1=1and IssueIsApprovedL2=1 and IssueIsApprovedL3=1 and IssueIsApprovedL4=0
			end
		end 
	end 
end 
else if(@A2=1 and @AC2=0)
begin
	update A_Audits set IssueIsApprovedL2=1  ,IssueApprovedByL2=@UserName, IssueApprovedDateL2=@date,IssueCommentsL2=@Comments   where id=@Id and   IsPost='Y' and IssueIsRejected=0 and IssueIsApprovedL1=1 and IssueIsApprovedL2=0 and IssueIsApprovedL3=0 and IssueIsApprovedL4=0
	if(@A3=1)
		begin
		update A_Audits set IssueIsApprovedL3=1  ,IssueApprovedByL3=@UserName, IssueApprovedDateL3=@date,IssueCommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IssueIsRejected=0 and IssueIsApprovedL1=1and IssueIsApprovedL2=1 and IssueIsApprovedL3=0 and IssueIsApprovedL4=0
		if(@A4=1)
		begin
		update A_Audits set IssueIsApprovedL4=1 ,IssueApprovedByL4=@UserName, IssueApprovedDateL4=@date,IssueCommentsL4=@Comments   where id=@Id and   IsPost='Y' and IssueIsRejected=0 and IssueIsApprovedL1=1and IssueIsApprovedL2=1 and IssueIsApprovedL3=1 and IssueIsApprovedL4=0
		end
	end 
end 
else if(@A3=1 and @AC3=0)
begin
	update A_Audits set IssueIsApprovedL3=1  ,IssueApprovedByL3=@UserName, IssueApprovedDateL3=@date,IssueCommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IssueIsRejected=0 and IssueIsApprovedL1=1and IssueIsApprovedL2=1 and IssueIsApprovedL3=0 and IssueIsApprovedL4=0
	if(@A4=1)
	begin
		update A_Audits set IssueIsApprovedL4=1 ,IssueApprovedByL4=@UserName, IssueApprovedDateL4=@date,IssueCommentsL4=@Comments   where id=@Id and   IsPost='Y' and IssueIsRejected=0 and IssueIsApprovedL1=1and IssueIsApprovedL2=1 and IssueIsApprovedL3=1 and IssueIsApprovedL4=0
	end
end 
else if(@A4=1 and @AC4=0)
begin
	update A_Audits set IssueIsApprovedL4=1 ,IssueApprovedByL4=@UserName, IssueApprovedDateL4=@date,IssueCommentsL4=@Comments   where id=@Id and   IsPost='Y' and IssueIsRejected=0 and IssueIsApprovedL1=1and IssueIsApprovedL2=1 and IssueIsApprovedL3=1 and IssueIsApprovedL4=0
end

"
                        ;


                        command = CreateCommand(query);

                        command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
                        //command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
                        command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = vm.Audit.PostedBy;
                        command.Parameters.Add("@date", SqlDbType.VarChar).Value = vm.Audit.PostedOn;
                        //command.Parameters.Add("@IsApprovedL1", SqlDbType.Bit).Value = vm.Approval.IsApprovedL1;
                        command.Parameters.Add("@Comments", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.CommentsL1) ? (object)DBNull.Value : vm.CommentsL1;



                        rowcount = command.ExecuteNonQuery();

                    }

                    else if (vm.ApproveStatus == "branchFeedbackApprove")
                    {

                        query = @"


declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);


declare @AC1 varchar(1);
declare @AC2 varchar(1);
declare @AC3 varchar(1);
declare @AC4 varchar(1);

declare @UserId varchar(max);
Create table #TempId(Id int)

select @UserId=Id  from GDICAuditAuthDB.dbo.AspNetUsers where UserName=@UserName

select @A1=AuditFeedbackApproval1,@A2=AuditFeedbackApproval2,@A3=AuditFeedbackApproval3,@A4=AuditFeedbackApproval4  from UserRolls

where  IsAuditFeedback=1 and UserId=@UserId
select @AC1=BFIsApprovedL1,@AC2=BFIsApprovedL2,@AC3=BFIsApprovedL3,@AC4=BFIsApprovedL4 from A_Audits   where id=@Id and IsPost='Y' and  BFIsRejected=0 

if(@A1=1 and @AC1=0)
begin
	update A_Audits set BFIsApprovedL1=1 ,BFApprovedByL1=@UserName, BFApprovedDateL1=@date,BFCommentsL1=@Comments  where id=@Id and IsPost='Y' and  BFIsRejected=0 and BFIsApprovedL1=0 and BFIsApprovedL2=0 and BFIsApprovedL3=0 and BFIsApprovedL4=0
	if(@A2=1)
	begin
		update A_Audits set BFIsApprovedL2=1  ,BFApprovedByL2=@UserName, BFApprovedDateL2=@date,BFCommentsL2=@Comments   where id=@Id and   IsPost='Y' and BFIsRejected=0 and BFIsApprovedL1=1 and BFIsApprovedL2=0 and BFIsApprovedL3=0 and BFIsApprovedL4=0
		if(@A3=1)
			begin
			update A_Audits set BFIsApprovedL3=1  ,BFApprovedByL3=@UserName, BFApprovedDateL3=@date,BFCommentsL3=@Comments   where id=@Id and  IsPost='Y' and  BFIsRejected=0 and BFIsApprovedL1=1and BFIsApprovedL2=1 and BFIsApprovedL3=0 and BFIsApprovedL4=0
			if(@A4=1)
			begin
			update A_Audits set BFIsApprovedL4=1 ,BFApprovedByL4=@UserName, BFApprovedDateL4=@date,BFCommentsL4=@Comments   where id=@Id and   IsPost='Y' and BFIsRejected=0 and BFIsApprovedL1=1and BFIsApprovedL2=1 and BFIsApprovedL3=1 and BFIsApprovedL4=0
			end
		end 
	end 
end 
else if(@A2=1 and @AC2=0)
begin
	update A_Audits set BFIsApprovedL2=1  ,BFApprovedByL2=@UserName, BFApprovedDateL2=@date,BFCommentsL2=@Comments   where id=@Id and   IsPost='Y' and BFIsRejected=0 and BFIsApprovedL1=1 and BFIsApprovedL2=0 and BFIsApprovedL3=0 and BFIsApprovedL4=0
	if(@A3=1)
		begin
		update A_Audits set BFIsApprovedL3=1  ,BFApprovedByL3=@UserName, BFApprovedDateL3=@date,BFCommentsL3=@Comments   where id=@Id and  IsPost='Y' and  BFIsRejected=0 and BFIsApprovedL1=1and BFIsApprovedL2=1 and BFIsApprovedL3=0 and BFIsApprovedL4=0
		if(@A4=1)
		begin
		update A_Audits set BFIsApprovedL4=1 ,BFApprovedByL4=@UserName, BFApprovedDateL4=@date,BFCommentsL4=@Comments   where id=@Id and   IsPost='Y' and BFIsRejected=0 and BFIsApprovedL1=1and BFIsApprovedL2=1 and BFIsApprovedL3=1 and BFIsApprovedL4=0
		end
	end 
end 
else if(@A3=1 and @AC3=0)
begin
	update A_Audits set BFIsApprovedL3=1  ,BFApprovedByL3=@UserName, BFApprovedDateL3=@date,BFCommentsL3=@Comments   where id=@Id and  IsPost='Y' and  BFIsRejected=0 and BFIsApprovedL1=1and BFIsApprovedL2=1 and BFIsApprovedL3=0 and BFIsApprovedL4=0
	if(@A4=1)
	begin
		update A_Audits set BFIsApprovedL4=1 ,BFApprovedByL4=@UserName, BFApprovedDateL4=@date,BFCommentsL4=@Comments   where id=@Id and   IsPost='Y' and BFIsRejected=0 and BFIsApprovedL1=1and BFIsApprovedL2=1 and BFIsApprovedL3=1 and BFIsApprovedL4=0
	end
end 
else if(@A4=1 and @AC4=0)
begin
	update A_Audits set BFIsApprovedL4=1 ,BFApprovedByL4=@UserName, BFApprovedDateL4=@date,BFCommentsL4=@Comments   where id=@Id and   IsPost='Y' and BFIsRejected=0 and BFIsApprovedL1=1and BFIsApprovedL2=1 and BFIsApprovedL3=1 and BFIsApprovedL4=0
end

"
                        ;


                        command = CreateCommand(query);

                        command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
                        //command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
                        command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = vm.Audit.PostedBy;
                        command.Parameters.Add("@date", SqlDbType.VarChar).Value = vm.Audit.PostedOn;
                        //command.Parameters.Add("@IsApprovedL1", SqlDbType.Bit).Value = vm.Approval.IsApprovedL1;
                        command.Parameters.Add("@Comments", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.CommentsL1) ? (object)DBNull.Value : vm.CommentsL1;



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

        public AuditMaster Update(AuditMaster model)
        {
            try
            {
                string sql = "";
                sql = @"

Update [A_Audits]
set
 Name=@Name
,AuditTypeId=@AuditTypeId
,IsPlaned=@IsPlaned
,TeamId=@TeamId
,BranchID=@BranchID
,StartDate=@StartDate
,EndDate=@EndDate
,Duratiom=@Duratiom
,BusinessTarget=@BusinessTarget
,AuditStatus=@AuditStatus
,Remarks=@Remarks
,ReportStatus=@ReportStatus
,LastUpdateBy=@LastUpdateBy
,LastUpdateOn=@LastUpdateOn
,LastUpdateFrom=@LastUpdateFrom

where Id=@Id
";


                var command = CreateCommand(sql);

                int value = (Convert.ToDateTime(model.EndDate) - Convert.ToDateTime(model.StartDate)).Days;
                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = model.Code;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = model.Name;
                command.Parameters.Add("@AuditTypeId", SqlDbType.Int).Value = model.AuditTypeId;
                command.Parameters.Add("@IsPlaned", SqlDbType.Bit).Value = model.IsPlaned;         
                command.Parameters.Add("@TeamId", SqlDbType.Int).Value = model.TeamId;
                command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
                command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = model.StartDate;
                command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = model.EndDate;
                command.Parameters.Add("@Duratiom", SqlDbType.Int).Value = value;            
                command.Parameters.Add("@BusinessTarget", SqlDbType.NVarChar).Value = model.BusinessTarget == null ? DBNull.Value : model.BusinessTarget;
                command.Parameters.Add("@AuditStatus", SqlDbType.NVarChar).Value = model.AuditStatus;
                command.Parameters.Add("@ReportStatus", SqlDbType.NVarChar).Value = model.ReportStatus;
                command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = model.Remarks == null ? DBNull.Value : model.Remarks;
                command.Parameters.Add("@LastUpdateBy", SqlDbType.NVarChar).Value = model.Audit.LastUpdateBy;
                command.Parameters.Add("@LastUpdateOn", SqlDbType.NVarChar).Value = model.Audit.LastUpdateOn;
                command.Parameters.Add("@LastUpdateFrom", SqlDbType.NVarChar).Value = model.Audit.LastUpdateFrom;

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

        public AuditMaster AuditStatusUpdate(AuditMaster model)
        {
            try
            {

                string sql = "";

                sql = @"

Update [A_Audits]

 set

AuditStatus=@AuditStatus

where Id=@Id
";


                var command = CreateCommand(sql);


                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                command.Parameters.Add("@AuditStatus", SqlDbType.NVarChar).Value = model.AuditStatus;





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

        public int GetAuditStatusDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";

            DataTable dt = new DataTable();
            try
            {
                sqlText = @"
              select count(A_Audits.IsPlaned) PlanCount
             
             from  A_Audits 

             where 1=1  and A_Audits.IsPlaned = 1

             ";

                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                //objComm.SelectCommand.Parameters.AddWithValue("@BranchId", index.CurrentBranchid);
                objComm.Fill(dt);

                List<AuditMaster> VMs = new List<AuditMaster>();
                VMs = dt.ToList<AuditMaster>();


                return Convert.ToInt32(dt.Rows[0][0]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditResponse> AuditResponseGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditResponse> VMs = new List<AuditResponse>();
            DataTable dt = new DataTable();





            try
            {
                sqlText = @"

        select 
        --AI.Id
        A.Id
       ,A.Name AuditName
       ,AI.IssueName
       ,E.EnumValue IssuePriority
       ,format(AI.DateOfSubmission,'dd/MM/yyyy') DateOfSubmission 
       --,A.Id 
       --,AI.DateOfSubmission

       from A_AuditIssues AI

       left outer join A_Audits A on AI.AuditId=A.Id
       left outer join Enums E on E.Id=AI.IssuePriority

       WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)

	   --and AI.Id not in(select AuditId from A_AuditBranchFeedbacks)

	   and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks)";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                //objComm.SelectCommand.Parameters.AddWithValue("@BranchId", index.CurrentBranchid);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditResponse>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public MailSetting SaveUrl(MailSetting model)
        {
            try
            {
                string sqlText = "";
                int Id = 0;

                sqlText = @"
insert into MailSetting(
 [AutidId]
,[AuditIssueId]
,[BranchFeedbackId]
,[ApprovedUrl]
,[ApproveOparetion]
,[Status]
,[IsMailed]


)
values( 

 @AutidId
,@AuditIssueId
,@BranchFeedbackId
,@ApprovedUrl
,@ApproveOparetion
,@Status
,@IsMailed


     
) SELECT SCOPE_IDENTITY() ";

                var command = CreateCommand(sqlText);


                command.Parameters.Add("@AutidId", SqlDbType.Int).Value = model.AutidId;
                command.Parameters.Add("@ApprovedUrl", SqlDbType.NVarChar).Value = model.ApprovedUrl;
                command.Parameters.Add("@AuditIssueId", SqlDbType.Int).Value = model.AuditIssueId == 0 ? DBNull.Value : model.AuditIssueId;
                command.Parameters.Add("@BranchFeedbackId", SqlDbType.Int).Value = model.BranchFeedbackId == 0 ? DBNull.Value : model.BranchFeedbackId;
                command.Parameters.Add("@ApproveOparetion", SqlDbType.NVarChar).Value = model.ApproveOparetion is null ? DBNull.Value : model.ApproveOparetion;
                command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = model.Status;
                command.Parameters.Add("@IsMailed", SqlDbType.Bit).Value = model.IsMailed;


                model.Id = Convert.ToInt32(command.ExecuteScalar());




                return model;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<UserProfile> GetEamil(UserProfile Email)
        {
            string sqlText = "";
            List<UserProfile> VMs = new List<UserProfile>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

declare @AL1 varchar(1);
declare @AL2 varchar(1);
declare @AL3 varchar(1);
declare @AL4 varchar(1);


select @AL1=  t.IsApprovedL1, @AL2=  t.IsApprovedL2, @AL3=  t.IsApprovedL3, @AL4=  t.IsApprovedL4 from A_Audits  t where   t.Id=@AuditId 


if (@AL1=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id 
where up.AuditApproval1 =1 and au.UserName = @userName
end
else  if (@AL2=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id 
where up.AuditApproval2 =1 and au.UserName = @userName
end
else if (@AL3=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id 
where up.AuditApproval3 =1 and au.UserName = @userName
end
else if (@AL4=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id 
where up.AuditApproval4 =1 and au.UserName = @userName
end

 ";
                sqlText = ApplyConditions(sqlText, null, null);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", Email.Id);
                objComm.SelectCommand.Parameters.AddWithValue("@userName", Email.UserName);


                objComm.Fill(dt);

                VMs = dt.ToList<UserProfile>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MailSetting> GetUrl(MailSetting model)
        {
            string sqlText = "";
            List<MailSetting> VMs = new List<MailSetting>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
       SELECT [Id]
      ,[ApprovedUrl]
     
       FROM [MailSetting]  

       where 1=1 and AutidId = @AutidId and Status=@status";

                sqlText = ApplyConditions(sqlText, null, null);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);


                objComm.SelectCommand.Parameters.AddWithValue("@AutidId", model.Id);
                //objComm.SelectCommand.Parameters.AddWithValue("@status", "audit");
                objComm.SelectCommand.Parameters.AddWithValue("@status", model.Status);

                objComm.Fill(dt);

                VMs = dt.ToList<MailSetting>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditUser> GetAuditUserByAuditId(string AuditId)
        {
            string sqlText = "";
            List<AuditUser> VMs = new List<AuditUser>();
            DataTable dt = new DataTable();

            //Format(ICReceipts.ReceiptDate, 'yyyy-MM-dd') ReceiptDate,


            try
            {
                sqlText = @"
select TM.UserId,u.Email EmailAddress from AuditUsers TM
left Join [GDICAuditAuthDB].[dbo].[AspNetUsers] u on u.Id=TM.UserId
where AuditId=@AuditId
";

                // sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", AuditId);

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

        public List<MailSetting> GetUrlForIssue(MailSetting Email)
        {
            throw new NotImplementedException();
        }

        public List<UserProfile> GetEamilForIssue(UserProfile Email)
        {
            string sqlText = "";
            List<UserProfile> VMs = new List<UserProfile>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"



declare @AL1 varchar(1);
declare @AL2 varchar(1);
declare @AL3 varchar(1);
declare @AL4 varchar(1);


select @AL1=  t.IssueIsApprovedL1, @AL2=  t.IssueIsApprovedL2, @AL3=  t.IssueIsApprovedL3, @AL4=  t.IssueIsApprovedL4 from A_Audits  t  where   t.Id=@AuditId 



if (@AL1=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id 
where up.AuditIssueApproval1 =1 and au.UserName = @userName
end
else  if (@AL2=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id 
where up.AuditIssueApproval2 =1 and au.UserName = @userName
end
else if (@AL3=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id 
where up.AuditIssueApproval3 =1 and au.UserName = @userName
end
else if (@AL4=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id
where up.AuditIssueApproval4 =1 and au.UserName = @userName
end

 ";
                sqlText = ApplyConditions(sqlText, null, null);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", Email.Id);
                objComm.SelectCommand.Parameters.AddWithValue("@userName", Email.UserName);


                objComm.Fill(dt);

                VMs = dt.ToList<UserProfile>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<UserProfile> GetEamilForBranchFeedback(UserProfile Email)
        {
            string sqlText = "";
            List<UserProfile> VMs = new List<UserProfile>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"



declare @AL1 varchar(1);
declare @AL2 varchar(1);
declare @AL3 varchar(1);
declare @AL4 varchar(1);


select @AL1=  t.BFIsApprovedL1, @AL2=  t.BFIsApprovedL2, @AL3=  t.BFIsApprovedL3, @AL4=  t.BFIsApprovedL4 from A_Audits  t  where   t.Id=@AuditId 



if (@AL1=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id 
where up.AuditFeedbackApproval1 =1 and au.UserName = @userName
end
else  if (@AL2=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id 
where up.AuditFeedbackApproval2 =1 and au.UserName = @userName
end
else if (@AL3=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id 
where up.AuditFeedbackApproval3 =1 and au.UserName = @userName
end
else if (@AL4=0)
begin
select UserName,Email from GDICAuditAuthDB.dbo.AspNetUsers au
left join UserRolls up on up.UserId=au.Id
where up.AuditFeedbackApproval4 =1 and au.UserName = @userName
end

 ";
                sqlText = ApplyConditions(sqlText, null, null);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", Email.Id);
                objComm.SelectCommand.Parameters.AddWithValue("@userName", Email.UserName);


                objComm.Fill(dt);

                VMs = dt.ToList<UserProfile>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AuditMaster IssueCompleted(AuditMaster model)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  ";
                SqlCommand command = CreateCommand(query);


                query = @"  update A_Audits set 

     IsCompleteIssue=@IsCompleteIssue

     where  Id= @Id ";

                command = CreateCommand(query);

                command.Parameters.Add("@IsCompleteIssue", SqlDbType.Bit).Value = model.IssueCompleted;
                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;


                rowcount = command.ExecuteNonQuery();

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

        public AuditMaster BranchFeedbackCompleteCompleted(AuditMaster model)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  ";
                SqlCommand command = CreateCommand(query);


                query = @"  update A_Audits set 

     IsCompleteIssueBranchFeedback=@IsCompleteIssueBranchFeedback

     where  Id= @Id ";

                command = CreateCommand(query);

                command.Parameters.Add("@IsCompleteIssueBranchFeedback", SqlDbType.Bit).Value = model.BranchFeedbackCompleted;
                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;


                rowcount = command.ExecuteNonQuery();

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

        public AuditMaster FeedbackComplete(AuditMaster model)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  ";
                SqlCommand command = CreateCommand(query);


                query = @"  update A_Audits set 

     IsCompleteIssueTeamFeedback=@IsCompleteIssueTeamFeedback

     where  Id= @Id ";

                command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
				command.Parameters.Add("@IsCompleteIssueTeamFeedback", SqlDbType.Bit).Value = model.IsCompleteIssueTeamFeedback;

				command.Parameters.Add("@LastUpdateBy", SqlDbType.NVarChar).Value = model.Audit.LastUpdateBy;
				command.Parameters.Add("@LastUpdateOn", SqlDbType.NVarChar).Value = model.Audit.LastUpdateOn;
				command.Parameters.Add("@LastUpdateFrom", SqlDbType.NVarChar).Value = model.Audit.LastUpdateFrom;


				rowcount = command.ExecuteNonQuery();

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

        public List<AuditUser> GetAuditUsers(int Id)
        {
            string sqlText = "";
            List<AuditUser> VMs = new List<AuditUser>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"


select
au.userid
,au.emailaddress
,*

from A_Audits

left outer join AuditUsers au on au.auditid = A_Audits.id

where A_Audits.id = @id

";

                sqlText = ApplyConditions(sqlText, null, null);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);


                objComm.SelectCommand.Parameters.AddWithValue("@id", Id);
                //objComm.SelectCommand.Parameters.AddWithValue("@status", "audit");
                //objComm.SelectCommand.Parameters.AddWithValue("@status", model.Status);

                objComm.Fill(dt);

                VMs = dt.ToList<AuditUser>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditUser> GetAuditIssueUsers(int Id)
        {
            string sqlText = "";
            List<AuditUser> VMs = new List<AuditUser>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"


select * 
from AuditIssueUsers
where AuditIssueId = @id

";

                sqlText = ApplyConditions(sqlText, null, null);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);


                objComm.SelectCommand.Parameters.AddWithValue("@id", Id);
                //objComm.SelectCommand.Parameters.AddWithValue("@status", "audit");
                //objComm.SelectCommand.Parameters.AddWithValue("@status", model.Status);

                objComm.Fill(dt);

                VMs = dt.ToList<AuditUser>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AuditMaster UpdateHOD(AuditMaster model)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  ";
                SqlCommand command = CreateCommand(query);


                query = @"  update A_Audits set 

     IsHOD=@IsHOD

     where  Id= @Id ";

                command = CreateCommand(query);

                command.Parameters.Add("@IsHOD", SqlDbType.Bit).Value = model.IsHOD;
                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;


                rowcount = command.ExecuteNonQuery();

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

        public int AuditResponseGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";

            DataTable dt = new DataTable();
            try
            {

                sqlText = @"

        select 
        count(AI.ID) FilteredCount

        from A_AuditIssues AI

        left outer join A_Audits A on AI.AuditId=A.Id
        left outer join Enums E on E.Id=AI.IssuePriority

        WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)
	    and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks)

        ";




                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                //objComm.SelectCommand.Parameters.AddWithValue("@BranchId", index.CurrentBranchid);

                //if (index.Status != "Audit")
                //{
                //    objComm.SelectCommand.Parameters.AddWithValue("@userName", index.createdBy);

                //}

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0][0]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditMaster> GetAuditStatusData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {

            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            //Format(ICReceipts.ReceiptDate, 'yyyy-MM-dd') ReceiptDate,

            try
            {
                sqlText = @"
select 
 ad.Id
,ad.[Code]
,ad.[AuditStatus]

,isnull(ad.[Name] ,'')Name
,Format(ad.[StartDate], 'yyyy-MM-dd') StartDate
,Format(ad.[EndDate], 'yyyy-MM-dd') EndDate
,ad.IsRejected
,ad.IssueIsRejected
,ad.BFIsApprovedL4
,ad.BFIsApprovedL4


,ad.[IsApprovedL4]
,isnull(ad.[IsCompleteIssue],'0')IsCompleteIssue
,isnull(ad.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
,isnull(ad.[IsCompleteIssueBranchFeedback],'0')IsCompleteIssueBranchFeedback

,ad.[IsPlaned]
,bp.BranchName



,case 


				 when isnull(ad.IsRejected,0)=1 then 'Audit Reject'		
				 when isnull(ad.IssueIsRejected,0)=1 then 'Issue Reject'
				 when isnull(ad.BFIsRejected,0)=1 then 'BranchFeedback Reject'
				 --when isnull(ad.BFIsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(ad.IssueIsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(ad.BFIsApprovedL4,0)=0 then 'On Going' 
						
				 end ApprovalStatus

,ad.[IsPost]


from A_Audits ad 

left outer join BranchProfiles bp on bp.BranchID = ad.BranchID

where 1=1





--and BFIsApprovedL4 is not null or IsRejected is not null or IssueIsRejected is not null or BFIsRejected is not null
--and BFIsApprovedL4=1
 
";


                if (index.AuditStatus == "Reject")
                {
                    sqlText = sqlText + "and IsRejected = 1 OR IssueIsRejected = 1 OR BFIsRejected = 1";

                }
                if (index.AuditStatus == "Approved")
                {
                    sqlText = sqlText + "and BFIsApprovedL4 = 1";

                }

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                //sqlText += @"  drop table #TempId  ";


                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value = index.UserName;

                //objComm.SelectCommand.Parameters.AddWithValue("@BranchId", index.CurrentBranchid);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditMaster>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetStatusDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";

            DataTable dt = new DataTable();
            try
            {

                sqlText = @"
              select count(ad.ID) FilteredCount
             --select count(ad.IsApprovedL4) FilteredCount
            from  A_Audits ad 

             where 1=1 

             ";



                if (index.AuditStatus == "Reject")
                {
                    sqlText = sqlText + "and IsRejected = 1 OR IssueIsRejected = 1 OR BFIsRejected = 1";

                }
                if (index.AuditStatus == "Approved")
                {
                    sqlText = sqlText + "and BFIsApprovedL4 = 1";

                }


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                //objComm.SelectCommand.Parameters.AddWithValue("@BranchId", index.CurrentBranchid);


                //objComm.SelectCommand.Parameters.AddWithValue("@userName", index.createdBy);



                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0][0]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditIssue> GetReportData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditIssue> VMs = new List<AuditIssue>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT  
        ai.[Id]
       ,ai.[Risk]
       ,ad.[Name]
       ,ad.[TeamId]
      

	  ,CASE
        WHEN ai.IssuePriority = 1029 THEN 'High'
        WHEN ai.IssuePriority = 1031 THEN 'Low'
        ELSE 'Medium'

        END AS IssuePriority

        --,CASE
		--WHEN ai.Operational = 1 THEN 'Operational'
		--WHEN ai.Financial = 1 THEN 'Financial'
		--WHEN ai.Compliance = 1 THEN 'Compliance'
		--ELSE 'Not Selected'
		--END AS IssueProcess

        ,CASE WHEN ai.Operational = 1 THEN 'Operational' ELSE '' END as OperationalText			
		,CASE when ai.Financial=1 then 'Financial' ELSE '' END as ComplianceText
		,CASE when ai.Compliance=1 then 'Compliance' ELSE '' END as FinancialText

	    ,ai.IssueName
        ,ai.IssueDetails
        ,FORMAT(ai.ImplementationDate,'dd-MM-yyyy')ImplementationDate



       FROM A_AuditIssues ai	   
	   left outer join  A_Audits  ad on ad.Id = ai.AuditId 
	   

       where 1=1 

";

                //--and ai.IsIssueApprove=1

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

        public List<AuditBranchFeedback> GetReportBranchFeedbackData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditBranchFeedback> VMs = new List<AuditBranchFeedback>();
            DataTable dt = new DataTable();
            string date = DateTime.Now.ToString();

            try
            {
                sqlText = @"

        --select top(1)  	 
		--ABF.Id
        --,ABF.AuditId
        --,ABF.AuditIssueId
        --,ABF.IssueDetails
        --,Format(ABF.ImplementationDate,'dd-MM-yyyy')ImplementationDate
        --,Format(ABF.CreatedOn,'dd-MM-yyyy')CreatedOn
		--,ABF.CreatedBy
		--from A_AuditBranchFeedbacks  ABF
		--where  ABF.CreatedOn <= ABF.ImplementationDate and CreatedBy not in   (  
		--select aspU.UserName from A_Audits A
		--left join  A_TeamMembers as Tm On A.TeamId=Tm.TeamId
		--left join GDICAuditAuthDB..AspNetUsers aspU On aspU.Id=tm.UserId
		--where A.Id=@AuditId )

select 	 
ABF.Id
,ABF.AuditId
,ABF.AuditIssueId
,ABF.IssueDetails
,Format(ABF.ImplementationDate,'dd-MM-yyyy')ImplementationDate
,Format(ABF.CreatedOn,'dd-MM-yyyy')CreatedOn
,ABF.CreatedBy
from A_AuditBranchFeedbacks ABF
where AuditIssueId=@IssueId and IsReport=1 and ABF.CreatedOn <= ABF.ImplementationDate
and CreatedBy not in(
select aspuser.UserName from A_Audits A
left join  A_TeamMembers as teamMbr On A.TeamId=teamMbr.TeamId
left join GDICAuditAuthDB..AspNetUsers aspuser On aspuser.Id=teamMbr.UserId where A.Id=@AuditId)






          ";
                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                sqlText = sqlText + " order by ABF.ID desc";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", vm.AuditId);
                //objComm.SelectCommand.Parameters.AddWithValue("@IssueId", conditionalValue[0]);
                objComm.SelectCommand.Parameters.AddWithValue("@IssueId", conditionalValue[0]);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserName", vm.UserName);

                objComm.Fill(dt);

                VMs = dt.ToList<AuditBranchFeedback>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditFeedback> GetReportAuditFeedbackData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditFeedback> VMs = new List<AuditFeedback>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"       
        -- select top(1)  	 
		-- ABF.Id
        --,ABF.AuditId
        --,ABF.AuditIssueId
        --,ABF.IssueDetails
        --,Format(ABF.ImplementationDate,'dd-MM-yyyy')ImplementationDate
        --,Format(ABF.CreatedOn,'dd-MM-yyyy')CreatedOn
		--,ABF.CreatedBy
		--from A_AuditBranchFeedbacks  ABF
		--where  ABF.CreatedOn <= ABF.ImplementationDate and CreatedBy in   (  
		--select aspU.UserName from A_Audits A
		--left join  A_TeamMembers as Tm On A.TeamId=Tm.TeamId
		--left join GDICAuditAuthDB..AspNetUsers aspU On aspU.Id=tm.UserId
		--where A.Id=@AuditId )



select 	 
 ABF.Id
,ABF.AuditId
,ABF.AuditIssueId
,ABF.IssueDetails
,Format(ABF.ImplementationDate,'dd-MM-yyyy')ImplementationDate
,Format(ABF.CreatedOn,'dd-MM-yyyy')CreatedOn
,ABF.CreatedBy

from A_AuditBranchFeedbacks ABF
where AuditIssueId=@IssueId and IsReport=1 and ABF.CreatedOn <= ABF.ImplementationDate
and CreatedBy in(
         select aspuser.UserName from A_Audits A
		 left join  A_TeamMembers as teamMbr On A.TeamId=teamMbr.TeamId
		 left join GDICAuditAuthDB..AspNetUsers aspuser On aspuser.Id=teamMbr.UserId where A.Id=@AuditId)
          

          ";
                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                //sqlText = sqlText + " order by AF.ID desc";
                sqlText = sqlText + " order by ABF.ID desc";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);


                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", vm.AuditId);
                objComm.SelectCommand.Parameters.AddWithValue("@IssueId", conditionalValue[0]);


                objComm.Fill(dt);

                VMs = dt.ToList<AuditFeedback>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AuditReportHeading AuditReportHeadingInsert(AuditReportHeading model)
        {
            try
            {
                string sqlText = "";


                if (model.Check == "")
                {
                    sqlText = @"

insert into ReportHeading(

 [AuditId]
,[ReportDetails]


)
values( 

  @AuditId
 ,@ReportDetails

     
)     SELECT SCOPE_IDENTITY() ";


                    var command = CreateCommand(sqlText);

                    command.Parameters.Add("@AuditId", SqlDbType.NVarChar).Value = model.AuditId;
                    command.Parameters.Add("@ReportDetails", SqlDbType.NVarChar).Value = model.AuditReportDetails == null ? DBNull.Value : model.AuditReportDetails;
                    //command.Parameters.Add("@SecondReportDetails", SqlDbType.NVarChar).Value = model.AuditSecondReportDetails == null ? DBNull.Value : model.AuditSecondReportDetails;

                    model.Id = Convert.ToInt32(command.ExecuteScalar());




                    return model;
                }

                else if (model.Check == "AnnexureReport")
                {
                    sqlText = @"

insert into ReportHeading(

 [AuditId]
,[AnnexureDetails]

)
values( 

  @AuditId
 ,@AnnexureDetails
  
)     SELECT SCOPE_IDENTITY() ";


                    var command = CreateCommand(sqlText);

                    command.Parameters.Add("@AuditId", SqlDbType.NVarChar).Value = model.AuditId;
                    command.Parameters.Add("@AnnexureDetails", SqlDbType.NVarChar).Value = model.AuditAnnexureDetails == null ? DBNull.Value : model.AuditAnnexureDetails;
                    //command.Parameters.Add("@SecondReportDetails", SqlDbType.NVarChar).Value = model.AuditSecondReportDetails == null ? DBNull.Value : model.AuditSecondReportDetails;

                    model.Id = Convert.ToInt32(command.ExecuteScalar());




                    return model;
                }

                else if (model.Check == "SecondReportHeading")
                {

                    sqlText = @"

insert into ReportHeading(

 [AuditId]
,[SecondReportDetails]


)
values( 

  @AuditId
 ,@SecondReportDetails

     
)     SELECT SCOPE_IDENTITY() ";


                    var command = CreateCommand(sqlText);

                    command.Parameters.Add("@AuditId", SqlDbType.NVarChar).Value = model.AuditId;
                    //command.Parameters.Add("@ReportDetails", SqlDbType.NVarChar).Value = model.AuditReportDetails == null ? DBNull.Value : model.AuditReportDetails;
                    command.Parameters.Add("@SecondReportDetails", SqlDbType.NVarChar).Value = model.AuditSecondReportDetails == null ? DBNull.Value : model.AuditSecondReportDetails;

                    model.Id = Convert.ToInt32(command.ExecuteScalar());




                    return model;
                }


                return model;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public AuditReportHeading AuditReportHeadingUpdate(AuditReportHeading model)
        {
            try
            {

                string sql = "";


                if (model.Check == "")
                {
                    sql = @"


Update [ReportHeading]
set

 AuditId=@AuditId
,ReportDetails=@ReportDetails

where Id=@Id
";


                    var command = CreateCommand(sql);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                    command.Parameters.Add("@AuditId", SqlDbType.NVarChar).Value = model.AuditId;
                    command.Parameters.Add("@ReportDetails", SqlDbType.NVarChar).Value = model.AuditReportDetails == null ? DBNull.Value : model.AuditReportDetails;



                    int rowcount = Convert.ToInt32(command.ExecuteNonQuery());

                    if (rowcount <= 0)
                    {
                        throw new Exception(MessageModel.UpdateFail);
                    }

                    return model;




                }



                else if (model.Check == "AnnexureReport")
                {
                    sql = @"



Update [ReportHeading]
set

 AuditId=@AuditId
,AnnexureDetails=@AnnexureDetails

 where Id=@Id ";


                    var command = CreateCommand(sql);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                    command.Parameters.Add("@AuditId", SqlDbType.NVarChar).Value = model.AuditId;
                    command.Parameters.Add("@AnnexureDetails", SqlDbType.NVarChar).Value = model.AuditAnnexureDetails == null ? DBNull.Value : model.AuditAnnexureDetails;
                    //command.Parameters.Add("@SecondReportDetails", SqlDbType.NVarChar).Value = model.AuditSecondReportDetails == null ? DBNull.Value : model.AuditSecondReportDetails;

                    model.Id = Convert.ToInt32(command.ExecuteScalar());




                    return model;
                }


                else if (model.Check == "SecondReportHeading")
                {



                    sql = @"


Update [ReportHeading]
set

 AuditId=@AuditId
,SecondReportDetails=@SecondReportDetails

 where Id=@Id
";


                    var command = CreateCommand(sql);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                    command.Parameters.Add("@AuditId", SqlDbType.NVarChar).Value = model.AuditId;
                    command.Parameters.Add("@SecondReportDetails", SqlDbType.NVarChar).Value = model.AuditSecondReportDetails == null ? DBNull.Value : model.AuditSecondReportDetails;



                    int rowcount = Convert.ToInt32(command.ExecuteNonQuery());

                    if (rowcount <= 0)
                    {
                        throw new Exception(MessageModel.UpdateFail);
                    }

                    return model;




                }



                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public List<AuditReportHeading> GetReportHeadingData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditReportHeading> VMs = new List<AuditReportHeading>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT top(1)

 Id
,AuditId
,ReportDetails AuditReportDetails
,SecondReportDetails AuditSecondReportDetails
,AnnexureDetails AuditAnnexureDetails

from ReportHeading where 1=1 

";
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);

                VMs = dt.ToList<AuditReportHeading>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditReportHeading> GetReportHeadingById(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditReportHeading> VMs = new List<AuditReportHeading>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT [Id]
      ,[AuditId]
      ,[ReportDetails] AuditReportDetails
      ,[SecondReportDetails] AuditSecondReportDetails
      ,[AnnexureDetails] AuditAnnexureDetails
      
  FROM [ReportHeading]  

  where 1=1  
";
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);

                VMs = dt.ToList<AuditReportHeading>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditBranchFeedback> GetBranchFeedbackDeprtemnetFollowUpData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditBranchFeedback> VMs = new List<AuditBranchFeedback>();
            DataTable dt = new DataTable();
            string date = DateTime.Now.ToString();

            try
            {
                sqlText = @"

        --Select Top(1)  
        --abf.CreatedBy,
        --abf.CreatedOn,
        --format(abf.ImplementationDate,'dd-MM-yyyy')ImplementationDate,
        --abf.IssueDetails        
        --from A_AuditBranchFeedbacks  abf
        --where cast(abf.ImplementationDate as date) <= cast(abf.CreatedOn as date)and abf.CreatedBy  In
        --(select authU.UserName from AuditIssueUsers  ai
        --left Join GDICAuditAuthDB..AspNetUsers authU On ai.UserId=authU.Id)


select 
ABF.CreatedBy,
ABF.CreatedOn,
format(ABF.ImplementationDate,'dd-MM-yyyy')ImplementationDate,
ABF.IssueDetails

from A_AuditBranchFeedbacks ABF
where AuditIssueId=@IssueId and IsReport=1 and ABF.ImplementationDate <= ABF.CreatedOn
and CreatedBy not in(
         select aspuser.UserName from A_Audits A
		 left join  A_TeamMembers as teamMbr On A.TeamId=teamMbr.TeamId
		 left join GDICAuditAuthDB..AspNetUsers aspuser On aspuser.Id=teamMbr.UserId where A.Id=@AuditId)


          ";


                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                sqlText = sqlText + " order by ABF.ID desc";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                //objComm.SelectCommand.Parameters.AddWithValue("@CurrenData", date);
                objComm.SelectCommand.Parameters.AddWithValue("@IssueId", conditionalValue[0]);
                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", vm.AuditId);


                objComm.Fill(dt);

                VMs = dt.ToList<AuditBranchFeedback>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditBranchFeedback> GetBranchFeedbackAuditResponseFollowUpData(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditBranchFeedback> VMs = new List<AuditBranchFeedback>();
            DataTable dt = new DataTable();
            string date = DateTime.Now.ToString();

            try
            {
                sqlText = @"

--Select Top(1) 
--abf.CreatedBy,
--abf.CreatedOn,
--format(abf.ImplementationDate,'dd-MM-yyyy')ImplementationDate,
--abf.IssueDetails
--from A_AuditBranchFeedbacks  abf
--where cast(abf.ImplementationDate as date) < cast(abf.CreatedOn as date) 
--and abf.CreatedBy  In 
--(select authU.UserName from AuditUsers  au
--left Join GDICAuditAuthDB..AspNetUsers authU On au.UserId=authU.Id )


select 
ABF.CreatedBy,
ABF.CreatedOn,
format(ABF.ImplementationDate,'dd-MM-yyyy')ImplementationDate,
ABF.IssueDetails

from A_AuditBranchFeedbacks ABF
where AuditIssueId=@IssueId and IsReport=1 and ABF.ImplementationDate <= ABF.CreatedOn
and CreatedBy  in(
         select aspuser.UserName from A_Audits A
		 left join  A_TeamMembers as teamMbr On A.TeamId=teamMbr.TeamId
		 left join GDICAuditAuthDB..AspNetUsers aspuser On aspuser.Id=teamMbr.UserId where A.Id=@AuditId)





        ";
                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                sqlText = sqlText + " order by ABF.ID desc";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                //objComm.SelectCommand.Parameters.AddWithValue("@CurrenData", date);
                objComm.SelectCommand.Parameters.AddWithValue("@IssueId", conditionalValue[0]);
                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", vm.AuditId);


                objComm.Fill(dt);

                VMs = dt.ToList<AuditBranchFeedback>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditApprove> GetAuditById(int id, string UserName)
        {

            string sqlText = "";
            List<AuditApprove> VMs = new List<AuditApprove>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                      select 
                      isnull([IsApprovedL1],'0')IsApprovedL1             
                     ,isnull([IsApprovedL2],'0')IsApprovedL2             
                     ,isnull([IsApprovedL3],'0')IsApprovedL3             
                     ,isnull([IsApprovedL4],'0')IsApprovedL4 


                     ,isnull([IsAudited],'0')IsAudited 

                     ,isnull([IssueIsApprovedL1],'0')IssueIsApprovedL1
                     ,isnull([IssueIsApprovedL2],'0')IssueIsApprovedL2
                     ,isnull([IssueIsApprovedL3],'0')IssueIsApprovedL3
                     ,isnull([IssueIsApprovedL4],'0')IssueIsApprovedL4
                     ,isnull([IssueIsAudited],'0')IssueIsAudited

                     ,isnull([BFIsApprovedL1],'0')BFIsApprovedL1
                     ,isnull([BFIsApprovedL2],'0')BFIsApprovedL2
                     ,isnull([BFIsApprovedL3],'0')BFIsApprovedL3
                     ,isnull([BFIsApprovedL4],'0')BFIsApprovedL4



                     ,ApprovedByL1
                     ,ApprovedByL2
                     ,ApprovedByL3
                     ,ApprovedByL4

                     ,IssueApprovedByL1
                     ,IssueApprovedByL2
                     ,IssueApprovedByL3
                     ,IssueApprovedByL4

                     ,BFApprovedByL1
                     ,BFApprovedByL2
                     ,BFApprovedByL3
                     ,BFApprovedByL4

                     from A_Audits 
                    
                     where Id=@Id ";


                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@Id", id);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditApprove>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<UserRolls> GetUserRoles(string UserName)
        {
            string sqlText1 = "";
            DataTable authdt = new DataTable();
            DataTable dt = new DataTable();
            List<UserRolls> VMs = new List<UserRolls>();



            string query = " select Id,UserName from [GDICAuditAuthDB].[dbo].[AspNetUsers] authuser where authuser.UserName=@userName ";
            sqlText1 = ApplyConditions(sqlText1, null, null);
            SqlDataAdapter obj = CreateAdapter(query);
            obj.SelectCommand = ApplyParameters(obj.SelectCommand, null, null);
            obj.SelectCommand.Parameters.AddWithValue("@userName", UserName);

            obj.Fill(authdt);
            string id1 = null;
            foreach (DataRow row in authdt.Rows)
            {
                id1 = row["Id"].ToString();

            }




            try
            {
                string sqlText = @"select 
 Id

 ,UserId
,IsAudit
,AuditApproval1
,AuditApproval2
,AuditApproval3
,AuditApproval4

,IsTour
,TourApproval1
,TourApproval2
,TourApproval3
,TourApproval4

,IsAdvance
,AdvanceApproval1
,AdvanceApproval2
,AdvanceApproval3
,AdvanceApproval4

,IsTa
,IsTaApproval1
,IsTaApproval2
,IsTaApproval3
,IsTaApproval4

,IsTourCompletionReport
,TourCompletionReportApproval1
,TourCompletionReportApproval2
,TourCompletionReportApproval3
,TourCompletionReportApproval4

,IsAuditIssue
,AuditIssueApproval1
,AuditIssueApproval2
,AuditIssueApproval3
,AuditIssueApproval4

,IsAuditFeedback
,AuditFeedbackApproval1
,AuditFeedbackApproval2
,AuditFeedbackApproval3
,AuditFeedbackApproval4


,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom


from UserRolls
where 1=1 and UserId=@Id";


                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@Id", id1);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<UserRolls>();
                return VMs;



                //sqlText = ApplyConditions(sqlText, null, null);
                //SqlCommand objComm = CreateCommand(sqlText);             
                //objComm = ApplyParameters(objComm, null, null);
                //SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                //DataTable dtResult = new DataTable();
                //adapter.Fill(dtResult);
                //List<UserRolls> vms = dtResult.ToList<UserRolls>();
                //return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<AuditIssueUser> GetAuditIssueUserById(int AuditId)
        {
            string sqlText = "";
            List<AuditIssueUser> VMs = new List<AuditIssueUser>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
select 
TM.UserId,
u.Email EmailAddress,
u.UserName

from AuditIssueUsers TM

left Join [GDICAuditAuthDB].[dbo].[AspNetUsers] u on u.Id=TM.UserId

where AuditId=@AuditId
";



                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", AuditId);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditIssueUser>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetTotoalIssuesById(int id, string UserName)
        {
            string sqlText = "";
            List<AuditIssueUser> VMs = new List<AuditIssueUser>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

select
count(auditid)TotalIssues
from 
A_AuditIssues where auditid = @AuditId";



                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", id);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0][0]);

                //VMs = dt.ToList<AuditIssueUser>();
                //return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AuditMaster MultipleAuditApproval(AuditMaster model)
        {
            try
            {

                string sqlText = "";
                int rowcount = 0;
                string query = @"  ";
                SqlCommand command = CreateCommand(query);

                query = @"


declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);


declare @AC1 varchar(1);
declare @AC2 varchar(1);
declare @AC3 varchar(1);
declare @AC4 varchar(1);

declare @UserId varchar(max);
Create table #TempId(Id int)

select @UserId=Id  from GDICAuditAuthDB.dbo.AspNetUsers where UserName=@UserName

select @A1=AuditApproval1,@A2=AuditApproval2,@A3=AuditApproval3,@A4=AuditApproval4  from UserRolls

where  IsAudit=1 and UserId=@UserId
select @AC1=IsApprovedL1,@AC2=IsApprovedL2,@AC3=IsApprovedL3,@AC4=IsApprovedL4 from A_Audits   where id=@Id and IsPost='Y' and  IsRejected=0 

if(@A1=1 and @AC1=0)
begin
	update A_Audits set IsApprovedL1=1 ,ApprovedByL1=@UserName, ApprovedDateL1=@date,CommentsL1=@Comments  where id=@Id and IsPost='Y' and  IsRejected=0 and IsApprovedL1=0 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
	if(@A2=1)
	begin
		update A_Audits set IsApprovedL2=1  ,ApprovedByL2=@UserName, ApprovedDateL2=@date,CommentsL2=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
		if(@A3=1)
			begin
			update A_Audits set IsApprovedL3=1  ,ApprovedByL3=@UserName, ApprovedDateL3=@date,CommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
			if(@A4=1)
			begin
			update A_Audits set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
			end
		end 
	end 
end 
else if(@A2=1 and @AC2=0)
begin
	update A_Audits set IsApprovedL2=1  ,ApprovedByL2=@UserName, ApprovedDateL2=@date,CommentsL2=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
	if(@A3=1)
		begin
		update A_Audits set IsApprovedL3=1  ,ApprovedByL3=@UserName, ApprovedDateL3=@date,CommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
		if(@A4=1)
		begin
		update A_Audits set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
		end
	end 
end 
else if(@A3=1 and @AC3=0)
begin
	update A_Audits set IsApprovedL3=1  ,ApprovedByL3=@UserName, ApprovedDateL3=@date,CommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
	if(@A4=1)
	begin
		update A_Audits set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
	end
end 
else if(@A4=1 and @AC4=0)
begin
	update A_Audits set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
end

"
;


                command = CreateCommand(query);

                //int AuditId = Convert.ToInt32(ID);
                command.Parameters.Add("@Id", SqlDbType.BigInt).Value = model.Id;
                command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = model.Audit.PostedBy;
                command.Parameters.Add("@date", SqlDbType.VarChar).Value = model.Audit.PostedOn;
                command.Parameters.Add("@Comments", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CommentsL1) ? (object)DBNull.Value : model.CommentsL1;

                rowcount = command.ExecuteNonQuery();


                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.PostFail);
                }

                return model;

            }



            catch (Exception ex)
            {
                throw ex;
            }


        }

        public AuditMaster ReportDataUpdate(AuditMaster model)
        {
            try
            {
                //DeleteIsReportDataFirst
                string sqlUpdateAll = @" Update [A_AuditBranchFeedbacks] set IsReport=0";
                var commandUpdateAll = CreateCommand(sqlUpdateAll);
                int rowcountForUpdateAll = Convert.ToInt32(commandUpdateAll.ExecuteNonQuery());
                //EndOfUpdate

                foreach (string ID in model.IDs)
                {


                    string sql = "";
                    sql = @"

                Update [A_AuditBranchFeedbacks] set IsReport=1

                where Id=@Id
                ";


                    var command = CreateCommand(sql);

                    command.Parameters.Add("@Id", SqlDbType.Int).Value = ID;

                    int rowcount = Convert.ToInt32(command.ExecuteNonQuery());


                    if (rowcount <= 0)
                    {
                        throw new Exception(MessageModel.UpdateFail);
                    }

                }

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserBranch> GetUserIdbyUserName(string UserName)
        {
            string sqlText1 = "";
            DataTable authdt = new DataTable();
            DataTable dt = new DataTable();
            List<UserBranch> VMs = new List<UserBranch>();



            string query = " select Id as UserId,UserName from [GDICAuditAuthDB].[dbo].[AspNetUsers] authuser where authuser.UserName=@userName ";
            sqlText1 = ApplyConditions(sqlText1, null, null);
            SqlDataAdapter obj = CreateAdapter(query);
            obj.SelectCommand = ApplyParameters(obj.SelectCommand, null, null);
            obj.SelectCommand.Parameters.AddWithValue("@userName", UserName);

            obj.Fill(authdt);
            VMs = authdt.ToList<UserBranch>();

            return VMs;
        }

        public UserBranch UpdateBranchName(UserBranch user)
        {
            try
            {
                string sql = "";
                sql = @"


UPDATE [GDICAuditAuthDB].[dbo].[AspNetUsers]
SET
BranchName = @BranchName
WHERE Id   = @Id

";


                var command = CreateCommand(sql);

                
                command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = user.UserId;
                command.Parameters.Add("@BranchName", SqlDbType.NVarChar).Value = user.BranchName;
                
                int rowcount = Convert.ToInt32(command.ExecuteNonQuery());

                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.UpdateFail);
                }

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HODdetails GetHODdetails()
        {
            try
            {

                DataTable dt = new DataTable();

                string sql = "";
                string sqlText = "";

                sql = @"


SELECT 

Id,
Name,
email

FROM HODdetails

WHERE 1=1

";




                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter obj = CreateAdapter(sql);
                obj.SelectCommand = ApplyParameters(obj.SelectCommand, null, null);
              
                obj.Fill(dt);

                HODdetails hod = dt.ToList<HODdetails>().FirstOrDefault();

                return hod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DetailsInformation(ReportModel vm, string[] conditionFields = null, string[] conditionValues = null)
        {
            
            string sqlText = "";


            try
            {

                sqlText = @"

 SELECT 

 AD.Id
,AD.[Code]
,isnull(AD.[Name] ,'')Name
,bp.BranchName
,At.AuditType AuditTypeId
,AD.[AuditStatus]
,AD.[IsPlaned]
,Format(AD.[StartDate], 'yyyy-MM-dd') StartDate
,Format(AD.[EndDate], 'yyyy-MM-dd') EndDate
--,isnull(AD.[IsCompleteIssue],'0')IsCompleteIssue
,AD.[IsApprovedL4]
,ISNULL(AD.[IsCompleteIssueTeamFeedback],'0')IsCompleteIssueTeamFeedback
,ISNULL(AD.[IsCompleteIssueBranchFeedback],'0')IsCompleteIssueBranchFeedback


,CASE 
	    		 WHEN isnull(AD.IsRejected,0)=1 THEN 'Reject'
				 WHEN isnull(AD.IsApprovedL4,0)=1 THEN 'Approveed' 
				 WHEN isnull(AD.IsApprovedL3,0)=1 THEN 'Waiting For Approval 4' 
				 WHEN isnull(AD.IsApprovedL2,0)=1 THEN 'Waiting For Approval 3' 
				 WHEN isnull(AD.IsApprovedL1,0)=1 THEN 'Waiting For Approval 2' 
				 ELSE 'Waiting For Approval 1' 
				 END ApproveStatus

,AD.[IsPost]

FROM A_Audits AD 

LEFT OUTER JOIN BranchProfiles bp on bp.BranchID = ad.BranchID

LEFT OUTER JOIN AuditTypes AT on AT.Id = ad.AuditTypeId

WHERE 1=1

";



                if (!string.IsNullOrWhiteSpace(vm.FromDate))
                {
                    sqlText += @" AND AD.StartDate >= @FromDate";
                }

                if (!string.IsNullOrWhiteSpace(vm.ToDate))
                {
                    sqlText += @" AND AD.EndDate <= @ToDate";
                }


                sqlText = ApplyConditions(sqlText, conditionFields, conditionValues, true);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionFields, conditionValues);




                if (!string.IsNullOrWhiteSpace(vm.FromDate))
                {
                    objComm.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(vm.FromDate)); 
                }

                if (!string.IsNullOrWhiteSpace(vm.ToDate))
                {
                    objComm.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(vm.ToDate)); 
                }



                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                return dtResult;


            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
