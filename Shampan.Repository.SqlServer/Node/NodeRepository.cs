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
using Shampan.Core.Interfaces.Repository.Node;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.Node
{
    public class NodeRepository : Repository, INodeRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;

        public NodeRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

        }

        public List<SubmanuList> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @"select 




--Id, 
--UserId,
--ModulId,
--Node, 
--Url,
--ActionName,
--ControllerName 
--from  NodePermission

np.Id, 
np.ModulId,
np.Node, 
np.Url,
np.ActionName,
np.ControllerName ,

apu.Id UserId,

TblNode.Id NodeId

from  NodePermission np
left outer join [GDICAuditAuthDB].[dbo].[AspNetUsers] apu on apu.UserName = np.UserId
left outer join TblNode on TblNode.Node = np.Node

where 1=1";




                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<SubmanuList> vms = dtResult.ToList<SubmanuList>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SubmanuList> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<SubmanuList> VMs = new List<SubmanuList>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

SELECT

Id,
UserId,
Node,
Url,
ActionName,
ControllerName,
isnull(IsAllowByUser,0)IsAllowByUser

FROM NodePermission
where 1=1 
";



                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                VMs = dt.ToList<SubmanuList>();

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
            List<SubmanuList> VMs = new List<SubmanuList>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                from NodePermission  where 1=1 ";


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

        public SubmanuList GetNodeById(string id)
        {
            string sqlText = "";
            //List<SubmanuList> VMs = new List<SubmanuList>();
            SubmanuList VMs = new SubmanuList();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

SELECT

[Node],
[Url],
[ActionName],
[ControllerName],
isnull([IsActive],0)IsActive


FROM [TblNode]
where 1=1 And Id = @Id
";



                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                sqlText = ApplyConditions(sqlText, null, null, false);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@Id", id);


                objComm.Fill(dt);
                VMs = dt.ToList<SubmanuList>().FirstOrDefault();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public SubmanuList Insert(SubmanuList model)
        {
            try
            {
                string sqlText = "";
                int Id = 0;


                sqlText = @"
insert into NodePermission(


 [UserId]
,[ModulId]
,[Node]
,[Url]
,[ActionName]
,[ControllerName]

)

values(

 @UserId
,@ModulId
,@Node
,@Url
,@ActionName
,@ControllerName

)SELECT SCOPE_IDENTITY() ";



                var command = CreateCommand(sqlText);




                command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = model.UserId;
                command.Parameters.Add("@ModulId", SqlDbType.NVarChar).Value = model.ModulId;
                command.Parameters.Add("@Node", SqlDbType.NVarChar).Value = model.Node;
                command.Parameters.Add("@Url", SqlDbType.NVarChar).Value = model.Url;
                command.Parameters.Add("@ActionName", SqlDbType.NVarChar).Value = model.ActionName;
                command.Parameters.Add("@ControllerName", SqlDbType.NVarChar).Value = model.ControllerName;


                model.Id = Convert.ToInt32(command.ExecuteScalar());


                return model;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SubmanuList Update(SubmanuList model)
        {
            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"  update NodePermission set

  UserId                        =@UserId  
 ,ModulId                       =@ModulId  
 ,Node                          =@Node  
 ,Url                           =@Url  
 ,ActionName                    =@ActionName  
 ,ControllerName                =@ControllerName  

 
                      
where  Id= @Id ";


                SqlCommand command = CreateCommand(query);

                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                command.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
                command.Parameters.Add("@ModulId", SqlDbType.NChar).Value = model.ModulId;
                command.Parameters.Add("@Node", SqlDbType.NChar).Value = model.Node;
                command.Parameters.Add("@Url", SqlDbType.NChar).Value = model.Url;
                command.Parameters.Add("@ActionName", SqlDbType.NChar).Value = model.ActionName;
                command.Parameters.Add("@ControllerName", SqlDbType.NChar).Value = model.ControllerName;


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
