using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Advance;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Core.Interfaces.Repository.UsersPermission;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.UsersPermission
{
	public class UsersPermissionRepository : Repository, IUsersPermissionRepository
	{

		private DbConfig _dbConfig;
		private SqlConnection context;
		private SqlTransaction transaction;

		public UsersPermissionRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
		{
			this._context = context;
			this._transaction = transaction;
			this._dbConfig = dbConfig;

		}

        public bool CheckUserStatus(Users model)
        {
            try
            {
                string sqlText = @"
    SELECT 
    Id,
    Code,
    UserName,
    RoleId,
    UserId,
    ModuleId,
    IsModuleActive

    FROM UsersPermission
    WHERE 1=1 and UserName = @userName 

";


                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                //sqlText = ApplyConditions(sqlText, null, null);
                //sqlText = sqlText + ")";

                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, new string[0], new string[0]);
                //objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);

                objComm.Parameters.AddWithValue("@userName", model.UserName);

                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);
                List<Users> vms = dtResult.ToList<Users>();
                

                if(dtResult.Rows.Count == 9)
                {
                    return true;
                }
                else
                {
                    return false;
                }

                

                //return true;
                //return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
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

        public List<Users> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			try
			{
				string sqlText = @"

    --SELECT 
    --Id,
    --Code,
    --UserName,
    --RoleId,
    --UserId,
    --ModuleId,
    --IsModuleActive,
    --CreatedBy,
    --CreatedOn,
    --CreatedFrom,
    --LastUpdateBy,
    --LastUpdateOn,
    --LastUpdateFrom
    --FROM UsersPermission


    SELECT 
    UsersPermission.Id,
    Code,
    UserName,
    RoleId,
    UserId,
    ModuleId,
    IsModuleActive,

    UsersPermission.CreatedBy,
    UsersPermission.CreatedOn,
    UsersPermission.CreatedFrom,
    UsersPermission.LastUpdateBy,
    UsersPermission.LastUpdateOn,
    UsersPermission.LastUpdateFrom,

	tbl.Modul

FROM UsersPermission

left outer join TBLModul tbl on tbl.id = UsersPermission.ModuleId

WHERE UserName = (SELECT UserName FROM UsersPermission Where 1=1

";


				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
				//sqlText = ApplyConditions(sqlText, null, null);

				sqlText = sqlText + ")";

				SqlCommand objComm = CreateCommand(sqlText);

				objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);
				//objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

				SqlDataAdapter adapter = new SqlDataAdapter(objComm);
				//string id = conditionalValue[0].ToString();

				//objComm.Parameters.AddWithValue("@id", 7);
				//objComm.SelectCommand.Parameters.AddWithValue("@userName", index.createdBy);


				DataTable dtResult = new DataTable();
				adapter.Fill(dtResult);

				List<Users> vms = dtResult.ToList<Users>();
				return vms;


			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

        public List<UserProfileAttachments> GetAllImage(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<UserProfileAttachments> VMs = new List<UserProfileAttachments>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
       SELECT  
       [Id]
      ,[UserId]
      ,[FileName]
       FROM AspNetUsersAttachments  
       where 1=1  

";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<UserProfileAttachments>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<UserProfileAttachments> GetImageByUserName(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<UserProfileAttachments> VMs = new List<UserProfileAttachments>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
       SELECT  
       AUA.[Id]
      ,[UserId]
      ,[FileName]
      ,ASN.ProfileName

       FROM AspNetUsersAttachments AUA
       left outer join [GDICAuditAuthDB].[dbo].[AspNetUsers] ASN on asn.Id = AUA.UserId
       where 1=1  

";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.Fill(dt);
                VMs = dt.ToList<UserProfileAttachments>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Users> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			string sqlText = "";
			List<Users> VMs = new List<Users>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"

SELECT
UserName,
MIN(ID) AS Id

FROM UsersPermission
GROUP BY UserName

Having 1=1 

";



				//sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

				// ToDo Escape Sql Injection
				sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
				sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

				SqlDataAdapter objComm = CreateAdapter(sqlText);

				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

				objComm.Fill(dt);
				var req = new Users();

				VMs.Add(req);


				VMs = dt.ToList<Users>();

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
			List<Users> VMs = new List<Users>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = @"
                --select count(UserName)FilteredCount
                --from UsersPermission
                --GROUP BY UserName
                --Having 1=1

                SELECT COUNT(DISTINCT UserName) AS FilteredCount
                FROM UsersPermission;


                 ";


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

        public List<SubmanuList> GetNodesIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<SubmanuList> VMs = new List<SubmanuList>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

SELECT

Id,
Node,
isnull(IsAllowByUser,0) IsAllowByUser

FROM NodePermission

where 1=1 And  ModulId = @ModulId And UserId = @UserName 

";



                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.AddWithValue("@ModulId", index.ModuleId);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.UserName);



                objComm.Fill(dt);

                VMs = dt.ToList<SubmanuList>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetNodesIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<Users> VMs = new List<Users>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                 from NodePermission                
                 where 1=1 And  ModulId = @ModulId And UserId = @UserName

                 ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.AddWithValue("@ModulId", index.ModuleId);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", index.UserName);

                objComm.Fill(dt);


                return Convert.ToInt32(dt.Rows[0][0]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<UserProfileAttachments> GetUserIdByName(string Name)
        {
            try
            {


                string sqlText1 = "";
                DataTable authdt = new DataTable();
                DataTable dt = new DataTable();
                List<UserProfileAttachments> VMs = new List<UserProfileAttachments>();

                string sqlText = @"

select 
Id UserId
,UserName 
from [GDICAuditAuthDB].[dbo].[AspNetUsers]
authuser where authuser.UserName=@userName
";


                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);       
                //sqlText = sqlText + ")";
                SqlCommand objComm = CreateCommand(sqlText);
                //objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);
                objComm.Parameters.AddWithValue("@userName", Name);
                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
               
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<UserProfileAttachments> vms = dtResult.ToList<UserProfileAttachments>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public UserProfileAttachments ImageInsert(UserProfileAttachments model)
        {
            try
            {
                string sqlText = "";
                int Id = 0;


                sqlText = @"
insert into AspNetUsersAttachments(
[UserId]
,[UserName]
,[FileName]
)
values( 
 @UserId
,@UserName
,@FileName
  
)     SELECT SCOPE_IDENTITY() ";

                var command = CreateCommand(sqlText);


                //command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = model.UserId;
                command.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = model.UserName;
                command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = model.FileName;
                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public UserProfileAttachments ImageUpdate(UserProfileAttachments model)
        {
            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"update AspNetUsersAttachments set

  
 FileName=@FileName  
                       
 where  UserName= @UserName ";


                SqlCommand command = CreateCommand(query);

                command.Parameters.Add("@UserName", SqlDbType.NChar).Value = model.UserName;
                
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

        public Users Insert(Users model)
		{
            try
            {
                string sqlText = "";

                var command = CreateCommand(@" INSERT INTO UsersPermission(


 Code
,UserName
,UserId
,ModuleID
,IsModuleActive

,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@UserName
,@UserId
,@ModuleID
,@IsModuleActive

,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");



                command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;
                command.Parameters.Add("@UserName", SqlDbType.NChar).Value = model.UserName;
                command.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
                //command.Parameters.Add("@RoleId", SqlDbType.NChar).Value = model.RoleValue;
                command.Parameters.Add("@ModuleID", SqlDbType.NChar).Value = model.Module.Id;
                command.Parameters.Add("@IsModuleActive", SqlDbType.NChar).Value = model.Module.IsActive;
                
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

		public Users Update(Users model)
		{
            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"update UsersPermission set

  
 UserName                     =@UserName  
,IsModuleActive               =@IsModuleActive  

,LastUpdateBy                 =@LastUpdateBy  
,LastUpdateOn                 =@LastUpdateOn  
,LastUpdateFrom               =@LastUpdateFrom 
                       
where  Id= @Id ";


                SqlCommand command = CreateCommand(query);

                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@UserName", SqlDbType.NChar).Value = model.UserName;
                command.Parameters.Add("@IsModuleActive", SqlDbType.NChar).Value = model.IsModuleActive;

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

        public SubmanuList UpdateNoes(SubmanuList model)
        {
            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"

update NodePermission set

 IsAllowByUser                =@IsAllowByUser 

,LastUpdateBy                 =@LastUpdateBy  
,LastUpdateOn                 =@LastUpdateOn  
,LastUpdateFrom               =@LastUpdateFrom 
 

where  Id= @Id 

";

                SqlCommand command = CreateCommand(query);

                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@IsAllowByUser", SqlDbType.Bit).Value = model.IsAllowByUser;

                command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
                command.Parameters.Add("@LastUpdateOn", SqlDbType.DateTime).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn;
                command.Parameters.Add("@LastUpdateFrom ", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateFrom) ? (object)DBNull.Value : model.Audit.LastUpdateFrom;


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

        public UserProfile UserProfileDeActivate(UserProfile model)
        {
            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"  UPDATE GDICAuditAuthDB.[dbo].[AspNetUsers] SET 

  IsArchive=@IsArchive   

  WHERE  Id= @Id ";


                SqlCommand command = CreateCommand(query);
                command.Parameters.Add("@Id", SqlDbType.VarChar).Value = model.UserId;
                command.Parameters.Add("@IsArchive", SqlDbType.NChar).Value = 1;

                //command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
                //command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn.ToString();
                //command.Parameters.Add("@LastUpdateFrom ", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateFrom.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateFrom.ToString();

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
