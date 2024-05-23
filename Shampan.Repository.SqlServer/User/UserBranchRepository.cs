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

namespace Shampan.Repository.SqlServer.User
{
    public class UserBranchRepository : Repository, IUserBranchRepository
    {

        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;

        

        public UserBranchRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public bool CheckExists(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public int Delete(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public List<UserBranch> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
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

                List<UserBranch> vms = dtResutl.ToList<UserBranch>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            string sqlText = "";
            List<UserBranch> VMs = new List<UserBranch>();
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
    

        public List<UserBranch> GetIndexData(IndexModel Index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            string sqlText = "";
            List<UserBranch> VMs = new List<UserBranch>();
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
                var req = new UserBranch();

                VMs.Add(req);


                VMs = dt
                .ToList<UserBranch>();

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
            List<UserBranch> VMs = new List<UserBranch>();
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


        public string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public UserBranch Insert(UserBranch model)
        {
            try
            {
                // Check if user ID has already been assigned to the branch
                string selectQuery = @"SELECT COUNT(*) FROM UserBranchMap WHERE UserId = @UserId AND BranchId = @BranchId";
                var selectCommand = CreateCommand(selectQuery);
                selectCommand.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
                selectCommand.Parameters.Add("@BranchId", SqlDbType.NChar).Value = model.BranchId;

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
                command.Parameters.Add("@BranchId", SqlDbType.NChar).Value = model.BranchId;



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

        public UserBranch Update(UserBranch model)
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
                command.Parameters.Add("@BranchId", SqlDbType.NChar).Value = model.BranchId;

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
