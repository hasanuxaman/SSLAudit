using Microsoft.Data.SqlClient;
using Shampan.Core.Interfaces.Repository;
using Shampan.Models;
using Shampan.Models.AuditModule;
using System.Data;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.AuditIssues;
using Shampan.Core.Interfaces.Repository.Circular;
using Shampan.Core.Interfaces.Repository.Oragnogram;

namespace Shampan.Repository.SqlServer.Oragnogram;

public class EmployeesHierarchyAttachmentsRepository : Repository, IEmployeesHiAttachmentsRepository
{
    private DbConfig _dbConfig;
    public EmployeesHierarchyAttachmentsRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
    {
        this._context = context;
        this._transaction = transaction;
        this._dbConfig = dbConfig;
    }


    public List<EmployeesHierarchyAttachments> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
    {
        string sqlText = "";
        List<EmployeesHierarchyAttachments> VMs = new List<EmployeesHierarchyAttachments>();
        DataTable dt = new DataTable();

        try
        {
            sqlText = @"

       SELECT  [Id]
      ,[EmployeeId]
      ,[FileName]
       FROM EmployeesHierarchyAttachments  

      where 1=1  

";
            sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

            SqlDataAdapter objComm = CreateAdapter(sqlText);

            objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

            objComm.Fill(dt);

            VMs = dt.ToList<EmployeesHierarchyAttachments>();

            return VMs;


        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public List<EmployeesHierarchyAttachments> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
    {
        string sqlText = "";
        List<EmployeesHierarchyAttachments> VMs = new List<EmployeesHierarchyAttachments>();
        DataTable dt = new DataTable();

        try
        {
            sqlText = @"
SELECT  [Id]
      ,[AuditId]
      ,[AuditIssueId]
      ,[FileName]
  FROM EmployeesHierarchyAttachments  

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
                .ToList<EmployeesHierarchyAttachments>();
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

    from FROM EmployeesHierarchyAttachments

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

    public EmployeesHierarchyAttachments Insert(EmployeesHierarchyAttachments model)
    {
        try
        {
            string sqlText = "";
            int Id = 0;


            sqlText = @"
insert into EmployeesHierarchyAttachments(
[EmployeeId]
,[FileName]
)
values( 
 @EmployeeId
,@FileName
  
)     SELECT SCOPE_IDENTITY() ";

            var command = CreateCommand(sqlText);


            //command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
            command.Parameters.Add("@EmployeeId", SqlDbType.NVarChar).Value = model.EmployeeId;
            command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = model.FileName;

            model.Id = Convert.ToInt32(command.ExecuteScalar());

            return model;

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public EmployeesHierarchyAttachments Update(EmployeesHierarchyAttachments model)
    {
        try
        {

            string sql = "";

            sql = @"

Update EmployeesHierarchyAttachments
set
,[CircularId]=@CircularId
,[FileName]=@FileName

where Id=@Id
";


            var command = CreateCommand(sql);
            command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
            //command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
            //command.Parameters.Add("@CircularId", SqlDbType.NVarChar).Value = model.CircularId;
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
