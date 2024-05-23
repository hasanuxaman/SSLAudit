using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Advance;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Core.Interfaces.Repository.Tour;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.Tour
{
	public class ToursRepository : Repository, IToursRepository
    {
		public ToursRepository(SqlConnection context, SqlTransaction transaction)
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
            try
            {
                bool ÌsPost = false;
                string Post = "";

                DataTable dt = new DataTable();

                // ToDo sql injection
                string sqlText = "select IsPost  from " + tableName + " where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Post = dt.Rows[0]["IsPost"].ToString();
                    return (Post == "Y");
                }


                return ÌsPost;
            }
            catch (Exception e)
            {
                throw e;
            }
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

        public List<Tours> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,Code
,Name
,Designation
,AuditId
,TeamId
,FromDate
,ToDate
,VisitPurpose
,Transport
,Amount
,Description



,IsPost
,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom


from Tours
where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<Tours> vms = dtResult.ToList<Tours>();
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
            List<Tours> VMs = new List<Tours>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Tours.Id)FilteredCount
                from Tours  where 1=1 ";


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

        public List<Tours> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<Tours> VMs = new List<Tours>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"



declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);
declare @UserId varchar(max);

select @UserId=Id  from GDICAuditAuthDB.dbo.AspNetUsers where UserName=@UserName

Create table #TempId(Id int)
select @A1=TourApproval1,@A2=TourApproval2,@A3=TourApproval3,@A4=TourApproval4  from UserRolls
where  IsTour=1 and UserId=@UserId


if(@A4=1)
begin
insert into  #TempId(Id)
select Id from Tours where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1and IsApprovedL4=0
end
if(@A3=1)
begin
insert into  #TempId(Id)
 
select id from Tours where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A2=1)
begin
insert into  #TempId(Id)
select id from Tours where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A1=1)
begin
insert into  #TempId(Id)
select id from Tours where IsRejected=0 and IsApprovedL1=0and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end

select 
                 Tours.Id
                 ,Tours.Code
                 ,Tours.Description
                 ,Tours.AuditId
                 ,au.Code AuditCode
                 ,Tours.TeamId
                 ,Tours.TourDate
                 ,Tours.IsPost
                 ,tm.TeamName

				 ,case 
				 when isnull(Tours.IsRejected,0)=1 then 'Reject'
				 when isnull(Tours.IsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(Tours.IsApprovedL3,0)=1 then 'Waiting For Approval 4' 
				 when isnull(Tours.IsApprovedL2,0)=1 then 'Waiting For Approval 3' 
				 when isnull(Tours.IsApprovedL1,0)=1 then 'Waiting For Approval 2' 
				 else 'Waiting For Approval 1' 
				 end ApproveStatus
				  ,case when isnull(Tours.IsAudited,0)=1 then 'Audited' else 'Not yet Audited' end AuditStatus


                 from Tours 
                 left outer join  A_Teams  tm on Tours.TeamId=tm.Id
                 left outer join  A_Audits  au on Tours.AuditId=au.Id
                 where 1=1 
                and Tours.id in( select id from #TempId)



"; 



                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                sqlText += @"  drop table #TempId  ";

                //SqlCommand objSQLComm = new SqlCommand();
                //objSQLComm.CommandText = sqlText;

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
				objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value =index.UserName;

                objComm.Fill(dt);
                var req = new Tours();

                VMs.Add(req);


                VMs = dt.ToList<Tours>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<Tours> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<Tours> VMs = new List<Tours>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"



declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);
declare @UserId varchar(max);

select @UserId=Id  from GDICAuditAuthDB.dbo.AspNetUsers where UserName=@UserName

Create table #TempId(Id int)
select @A1=TourApproval1,@A2=TourApproval1,@A3=TourApproval1,@A4=TourApproval1  from UserRolls
where  IsTour=1 and UserId=@UserId


if(@A4=1)
begin
insert into  #TempId(Id)
select Id from Tours where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1and IsApprovedL4=0
end
if(@A3=1)
begin
insert into  #TempId(Id)
 
select id from Tours where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A2=1)
begin
insert into  #TempId(Id)
select id from Tours where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A1=1)
begin
insert into  #TempId(Id)
select id from Tours where IsRejected=0 and IsApprovedL1=0and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end

select 
                 Tours.Id
                 ,Tours.Code
                 ,Tours.Description
                 ,Tours.AuditId
                 ,au.Code AuditCode
                 ,Tours.TeamId
                 ,Tours.TourDate
                 ,Tours.IsPost
                 ,tm.TeamName

				 ,case 
				 when isnull(Tours.IsRejected,0)=1 then 'Reject'
				 when isnull(Tours.IsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(Tours.IsApprovedL3,0)=1 then 'Waiting For Approval 4' 
				 when isnull(Tours.IsApprovedL2,0)=1 then 'Waiting For Approval 3' 
				 when isnull(Tours.IsApprovedL1,0)=1 then 'Waiting For Approval 2' 
				 else 'Waiting For Approval 1' 
				 end ApproveStatus
				  ,case when isnull(Tours.IsAudited,0)=1 then 'Audited' else 'Not yet Audited' end AuditStatus


                 from Tours 
                 left outer join  A_Teams  tm on Tours.TeamId=tm.Id
                 left outer join  A_Audits  au on Tours.AuditId=au.Id
                 where 1=1 and Tours.CreatedBy=@UserName
and Tours.id in( select id from #TempId)



";



                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                sqlText += @"  drop table #TempId  ";

                //SqlCommand objSQLComm = new SqlCommand();
                //objSQLComm.CommandText = sqlText;

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value = index.UserName;

                objComm.Fill(dt);
                var req = new Tours();

                VMs.Add(req);


                VMs = dt.ToList<Tours>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Tours> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<Tours> VMs = new List<Tours>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"


 
                 SELECT 
                  Tours.Id
                 ,Tours.Code
                 ,Tours.Description
                 ,Tours.AuditId
                 ,au.Code AuditCode
                 ,Tours.TeamId
                 ,Tours.ToDate
                 ,Tours.IsPost
                 ,tm.TeamName

				 ,case 
				 when isnull(Tours.IsRejected,0)=1 then 'Reject'
				 when isnull(Tours.IsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(Tours.IsApprovedL3,0)=1 then 'Waiting For Approval 4' 
				 when isnull(Tours.IsApprovedL2,0)=1 then 'Waiting For Approval 3' 
				 when isnull(Tours.IsApprovedL1,0)=1 then 'Waiting For Approval 2' 
				 else 'Waiting For Approval 1' 
				 end ApproveStatus
				  ,case when isnull(Tours.IsAudited,0)=1 then 'Audited' else 'Not yet Audited' end AuditStatus

                 from Tours 
                 left outer join  A_Teams  tm on Tours.TeamId=tm.Id
                 left outer join  A_Audits  au on Tours.AuditId=au.Id
                 WHERE 1=1 

";

                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new Tours();

                VMs.Add(req);


                VMs = dt.ToList<Tours>();

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
            List<Tours> VMs = new List<Tours>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                from Tours  where 1=1 ";


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

		public Tours Insert(Tours model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO Tours(


 Code
,Name
,Designation
,AuditId
,TeamId
,FromDate
,ToDate
,VisitPurpose
,Transport
,Amount
,Description

,IsPost
,IsApprovedL1
,IsApprovedL2
,IsApprovedL3
,IsApprovedL4
,IsAudited
,IsRejected


,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@Name
,@Designation
,@AuditId
,@TeamId
,@FromDate
,@ToDate
,@VisitPurpose
,@Transport
,@Amount
,@Description


,@IsPost
,@IsApprovedL1
,@IsApprovedL2
,@IsApprovedL3
,@IsApprovedL4
,@IsAudited
,@IsRejected

,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");




				command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;
				command.Parameters.Add("@Name", SqlDbType.NChar).Value = model.Name;
				command.Parameters.Add("@Designation", SqlDbType.NChar).Value = model.Designation;
				command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
				command.Parameters.Add("@TeamId", SqlDbType.Int).Value = model.TeamId;
				command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = model.FromDate;
				command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = model.ToDate;
				command.Parameters.Add("@VisitPurpose", SqlDbType.NVarChar).Value = model.VisitPurpose;
				command.Parameters.Add("@Transport", SqlDbType.NVarChar).Value = model.Transport;
				command.Parameters.Add("@Amount", SqlDbType.NVarChar).Value = model.Amount;
				command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = model.Description;



				command.Parameters.Add("@IsApprovedL1", SqlDbType.Bit).Value = 0;
				command.Parameters.Add("@IsApprovedL2", SqlDbType.Bit).Value = 0;
				command.Parameters.Add("@IsApprovedL3", SqlDbType.Bit).Value = 0;
				command.Parameters.Add("@IsApprovedL4", SqlDbType.Bit).Value = 0;
				command.Parameters.Add("@IsAudited", SqlDbType.Bit).Value = 0;
				command.Parameters.Add("@IsRejected", SqlDbType.Bit).Value = 0;
				

				
				command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
				command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
				command.Parameters.Add("@CreatedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : model.Audit.CreatedFrom.ToString();

				command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "N";


				model.Id = Convert.ToInt32(command.ExecuteScalar());


				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Tours MultiplePost(Tours vm)
		{
			try
			{
				string sqlText = "";

				int rowcount = 0;

				string query = @"  ";
                SqlCommand command = CreateCommand(query);

                foreach (string ID in vm.IDs)
				{
					 
                          query = @"  update Tours set 
     IsPost=@IsPost
,PostedBy=@PostedBy
,PostedOn=@PostedOn
,PostedFrom=@PostedFrom

,IsRejected=@IsRejected 


  where  Id= @Id ";
                        command = CreateCommand(query);

                        command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "Y";
						command.Parameters.Add("@Id", SqlDbType.BigInt).Value = ID;
						command.Parameters.Add("@PostedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedBy.ToString()) ? (object)DBNull.Value : vm.Audit.PostedBy.ToString();
						command.Parameters.Add("@PostedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedOn.ToString()) ? (object)DBNull.Value : vm.Audit.PostedOn.ToString();
						command.Parameters.Add("@PostedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedFrom.ToString()) ? (object)DBNull.Value : vm.Audit.PostedFrom.ToString();


					    command.Parameters.Add("@IsRejected", SqlDbType.Bit).Value = 0;



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

		public Tours MultipleUnPost(Tours vm)
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
                        query = @"   update Tours set 

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

                  else if(vm.Operation == "reject")
                    {

                        query = @"update Tours set 

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

					else if (vm.Operation == "approved")
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

select @A1=TourApproval1,@A2=TourApproval2,@A3=TourApproval3,@A4=TourApproval4  from UserRolls

where  IsTour=1 and UserId=@UserId
select @AC1=IsApprovedL1,@AC2=IsApprovedL2,@AC3=IsApprovedL3,@AC4=IsApprovedL4 from Tours   where id=@Id and IsPost='Y' and  IsRejected=0 

if(@A1=1 and @AC1=0)
begin
	update Tours set IsApprovedL1=1 ,ApprovedByL1=@UserName, ApprovedDateL1=@date,CommentsL1=@Comments  where id=@Id and IsPost='Y' and  IsRejected=0 and IsApprovedL1=0 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
	if(@A2=1)
	begin
		update Tours set IsApprovedL2=1  ,ApprovedByL2=@UserName, ApprovedDateL2=@date,CommentsL2=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
		if(@A3=1)
			begin
			update Tours set IsApprovedL3=1  ,ApprovedByL3=@UserName, ApprovedDateL3=@date,CommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
			if(@A4=1)
			begin
			update Tours set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
			end
		end 
	end 
end 
else if(@A2=1 and @AC2=0)
begin
	update Tours set IsApprovedL2=1  ,ApprovedByL2=@UserName, ApprovedDateL2=@date,CommentsL2=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1 and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
	if(@A3=1)
		begin
		update Tours set IsApprovedL3=1  ,ApprovedByL3=@UserName, ApprovedDateL3=@date,CommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
		if(@A4=1)
		begin
		update Tours set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
		end
	end 
end 
else if(@A3=1 and @AC3=0)
begin
	update Tours set IsApprovedL3=1  ,ApprovedByL3=@UserName, ApprovedDateL3=@date,CommentsL3=@Comments   where id=@Id and  IsPost='Y' and  IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
	if(@A4=1)
	begin
		update Tours set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
	end
end 
else if(@A4=1 and @AC4=0)
begin
	update Tours set IsApprovedL4=1 ,ApprovedByL4=@UserName, ApprovedDateL4=@date,CommentsL4=@Comments   where id=@Id and   IsPost='Y' and IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1 and IsApprovedL4=0
end

"
                        ;


						command = CreateCommand(query);

						command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
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

		public Tours Update(Tours model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update Tours set

 Code                         =@Code  
,Name                         =@Name  
,Designation                  =@Designation  
,AuditId                      =@AuditId  
,TeamId                       =@TeamId  
,FromDate                     =@FromDate  
,ToDate                       =@ToDate  
,VisitPurpose                 =@VisitPurpose  
,Transport                    =@Transport  
,Amount                       =@Amount  
,Description                  =@Description  
  

,LastUpdateBy              =@LastUpdateBy  
,LastUpdateOn              =@LastUpdateOn  
,LastUpdateFrom            =@LastUpdateFrom 
                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;


				command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;
				command.Parameters.Add("@Name", SqlDbType.NChar).Value = model.Name;
				command.Parameters.Add("@Designation", SqlDbType.NChar).Value = model.Designation;
				command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
				command.Parameters.Add("@TeamId", SqlDbType.Int).Value = model.TeamId;
				command.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = model.FromDate;
				command.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = model.ToDate;
				command.Parameters.Add("@VisitPurpose", SqlDbType.NVarChar).Value = model.VisitPurpose;
				command.Parameters.Add("@Transport", SqlDbType.NVarChar).Value = model.Transport;
				command.Parameters.Add("@Amount", SqlDbType.NVarChar).Value = model.Amount;
				command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = model.Description;
				



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

        public List<Tours> GetAllForReports(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            {
                try
                {
                    string sqlText = @" 

 select 
 Tours.Id
,Tours.Code
,AuditId
,Tours.TeamId
,FORMAT(FromDate,'dd-MM-yyyy')FromDate
--,format(FromDate,'yyyy-MM-dd')FromDate
,FORMAT(ToDate,'dd-MM-yyyy')ToDate
--,format(ToDate,'yyyy-MM-dd')ToDate
,Transport
,Amount
,Description
,Tours.IsPost

,u.ProfileName Name
,u.designation Designation
,FORMAT(a.StartDate,'dd-MM-yyyy') Date
--,format(a.StartDate,'yyyy-MM-dd') Date
,bp.BranchName 
,at.AuditType VisitPurpose

from Tours

left join A_TeamMembers tm on tm.TeamId = Tours.TeamId
left join GDICAuditAuthDB.[dbo].[AspNetUsers] u on tm.UserId = u.Id
left join A_Audits a on a.id = tours.AuditId
left join BranchProfiles bp on bp.BranchID=a.BranchID
left join AuditTypes at on at.Id = a.AuditTypeId

where 1=1";


                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                    SqlCommand objComm = CreateCommand(sqlText);

                    objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                    SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                    DataTable dtResult = new DataTable();
                    adapter.Fill(dtResult);

                    List<Tours> vms = dtResult.ToList<Tours>();

                    return vms;

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
