using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.TeamMember;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.TeamMember
{
    public class TeamMembersRepository : Repository, ITeamMembersRepository
    {
        private DbConfig _dbConfig;
        public TeamMembersRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            _dbConfig = dbConfig;
        }
        public List<TeamMembers> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @"select 

U.Id,
U.UserId,
U.TeamId,

A.UserName,

B.TeamName


from  [GDICAuditAuthDB].[dbo]. AspNetUsers A

left outer join A_TeamMembers U on U.UserId=A.Id

left outer join A_Teams B on B.Id=U.TeamId

WHERE U.UserId IS NOT NULL";

                //SRCELEDGER + '-' + SRCETYPE
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResutl = new DataTable();
                adapter.Fill(dtResutl);

                List<TeamMembers> vms = dtResutl.ToList<TeamMembers>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<TeamMembers> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            string AuthDbName = _dbConfig.AuthDB;
            string DbName = _dbConfig.DbName;
            string SageDbName = _dbConfig.SageDbName;

            List<TeamMembers> VMs = new List<TeamMembers>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = $@"Select
U.Id,
U.UserId,
U.TeamId,

A.UserName,

B.TeamName


from  [GDICAuditAuthDB].[dbo].AspNetUsers A
left outer join A_TeamMembers U on U.UserId=A.Id
left outer join A_Teams B on B.Id=U.TeamId


--from  {AuthDbName}.[dbo].AspNetUsers A
--left outer join {DbName}.[dbo].A_TeamMembers U on U.UserId=A.Id
--left outer join {DbName}.[dbo].A_Teams B on B.Id=U.TeamId



WHERE U.TeamId = @TeamIdDtata";

 //change
//WHERE U.TeamId IS NOT NULL ";



                //cange
                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

				objComm.SelectCommand.Parameters.AddWithValue("@TeamIdDtata", index.TeamId);


                objComm.Fill(dt);
                var req = new TeamMembers();

                VMs.Add(req);


                VMs = dt
                .ToList<TeamMembers>();

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
            List<TeamMembers> VMs = new List<TeamMembers>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                from A_TeamMembers  where 1=1 ";


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

        public List<UserProfileAttachments> GetUserProfileName(int TeamId)
        {
            string sqlText = "";
            string AuthDbName = _dbConfig.AuthDB;
            string DbName = _dbConfig.DbName;
            string SageDbName = _dbConfig.SageDbName;

            List<UserProfileAttachments> VMs = new List<UserProfileAttachments>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = $@"
SELECT

ISNULl(U.ProfileName,'NONAME')ProfileName,
ISNULl(U.Designation,'-')Designation

FROM 
A_TeamMembers

LEFT JOIN GDICAuditAuthDB.[dbo].[AspNetUsers] U on A_TeamMembers.UserId = U.Id
WHERE TeamId=@TeamId

";


               
                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@TeamId", TeamId);


                objComm.Fill(dt);
                var req = new UserProfileAttachments();

                VMs.Add(req);


                VMs = dt.ToList<UserProfileAttachments>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }



        //public ResultModel<string> GetUserId(string name)
        //{
        //    string sqlText = "";
        //    List<TeamMembers> VMs = new List<TeamMembers>();
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        sqlText = @"select UserId from A_TeamMembers  where TeamId = @UserId ";




        //        var selectCommand = CreateCommand(sqlText);

        //        selectCommand.Parameters.Add("@UserId", SqlDbType.NChar).Value = name;

        //        using (SqlDataReader reader = selectCommand.ExecuteReader())
        //        {

        //            while (reader.Read())
        //            {

        //                int userId = reader.GetInt32(0); 

        //            }
        //        }








        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public TeamMembers Insert(TeamMembers model)
        {
            try
            {
                // Check if user ID has already been assigned to the branch


                string selectQuery = @"SELECT COUNT(*) FROM A_TeamMembers WHERE UserId = @UserId AND TeamId = @TeamId";
                var selectCommand = CreateCommand(selectQuery);
                selectCommand.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
                selectCommand.Parameters.Add("@TeamId", SqlDbType.NChar).Value = model.TeamId;
                int count = Convert.ToInt32(selectCommand.ExecuteScalar());
                if (count > 0)
                {
                    throw new Exception("User ID has already been assigned to the Same Team");
                }




                //  string[] retResults = { "Fail", "Fail", Id.ToString(), sqlText, "ex", "Insert" };

                var command = CreateCommand(@" INSERT INTO A_TeamMembers(




 TeamId
,UserId
,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @TeamId
,@UserId


,@CreatedBy
,@CreatedOn
,@CreatedFrom




)SELECT SCOPE_IDENTITY()");


                command.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
                command.Parameters.Add("@TeamId", SqlDbType.NChar).Value = model.TeamId;


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

        public TeamMembers Update(TeamMembers model)
        {

            // Check if user ID has already been assigned to the branch


            //string selectQuery = @"SELECT COUNT(*) FROM A_TeamMembers WHERE UserId = @UserId AND TeamId = @TeamId";
            //var selectCommand = CreateCommand(selectQuery);
            //selectCommand.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
            //selectCommand.Parameters.Add("@TeamId", SqlDbType.NChar).Value = model.TeamId;
            //int countdata = Convert.ToInt32(selectCommand.ExecuteScalar());
            //if (countdata > 0)
            //{
            //    throw new Exception("User ID has already been assigned to the Same Team");
            //}




            //,UserId = @UserId


            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"  update A_TeamMembers set 

 TeamId               =@TeamId  
 
 
,LastUpdateBy               =@LastUpdateBy  
,LastUpdateOn               =@LastUpdateOn  
,LastUpdateFrom            =@LastUpdateFrom   
                       
where  Id= @Id ";


                SqlCommand command = CreateCommand(query);

                //command.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
                command.Parameters.Add("@Id", SqlDbType.NChar).Value = model.Id;
                command.Parameters.Add("@TeamId", SqlDbType.NChar).Value = model.TeamId;

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
