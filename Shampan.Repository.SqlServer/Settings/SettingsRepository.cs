using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Settings;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.Settings
{
    public class SettingsRepository : Repository , ISettingsRepository
    {

        private DbConfig _dbConfig;

        public SettingsRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
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

        public bool CheckPostStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public bool CheckPushStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public DbUpdateModel DBTableFieldAdd(string TableName, string FieldName, string DataType, bool NullType)
        {
            try
            {
                string sqlText = "";
                sqlText = "";
                sqlText += " if not exists(select * from sys.columns ";
                sqlText += " where Name = N'" + FieldName + "' and Object_ID = Object_ID(N'" + TableName + "'))   ";
                sqlText += " begin";
                if (NullType == true)
                {
                    sqlText += " ALTER TABLE " + TableName + " ADD " + FieldName + " " + DataType + " NULL DEFAULT 0 ;";
                }
                else
                {
                    sqlText += " ALTER TABLE " + TableName + " ADD " + FieldName + " " + DataType + " NOT NULL DEFAULT 0 ;";
                }
                sqlText += " END";

                SqlCommand command = CreateCommand(sqlText);

                command.ExecuteNonQuery();
                return new DbUpdateModel();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbUpdateModel DBTableFieldAlter(string TableName, string FieldName, string DataType)
        {
            try
            {
                string sqlText = "";
                int count = 0;
                sqlText = "";
                sqlText += " ALTER TABLE " + TableName + " ALTER COLUMN " + FieldName + "   " + DataType + "";

                SqlCommand command = CreateCommand(sqlText);


                command.ExecuteNonQuery();


                return new DbUpdateModel();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbUpdateModel DBTableFieldRemove(string TableName, string FieldName)
        {
            try
            {
                string sqlText = "";
                sqlText = "";
                sqlText += " if exists(select * from sys.columns ";
                sqlText += " where Name = N'" + FieldName + "' and Object_ID = Object_ID(N'" + TableName + "'))   ";
                sqlText += " begin";
                sqlText += " ALTER TABLE " + TableName + " DROP COLUMN " + FieldName;
                sqlText += " END";

                SqlCommand command = CreateCommand(sqlText);


                command.ExecuteNonQuery();


                return new DbUpdateModel();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public List<SettingsModel> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {

            try
            {
                string sqlText = @"Select
                 Id
                ,SettingGroup
                ,SettingName
                ,SettingValue
                ,SettingType
                ,Remarks
                ,IsActive
                ,IsArchive
                           

                from Settings 
                       
                 where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<SettingsModel> vms = dtResult.ToList<SettingsModel>();
                return vms;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public List<SettingsModel> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public int GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public SettingsModel Insert(SettingsModel model)
        {


            try
            {
                string sqlText = "";
                int count = 0;
                var command = CreateCommand(@" INSERT INTO Settings(

                 SettingGroup
                ,SettingName
                ,SettingValue
                ,SettingType
                ,Remarks
                ,IsActive
                ,IsArchive
                ,CreatedBy
                ,CreatedAt
                ,CreatedFrom


                ) VALUES (


                 @SettingGroup
                ,@SettingName
                ,@SettingValue
                ,@SettingType
                ,@Remarks
                ,@IsActive
                ,@IsArchive
                ,@CreatedBy
                ,@CreatedAt
                ,@CreatedFrom


                )SELECT SCOPE_IDENTITY()");



                command.Parameters.Add("@SettingGroup", SqlDbType.VarChar).Value = model.SettingGroup.ToString();
                command.Parameters.Add("@SettingName", SqlDbType.VarChar).Value = model.SettingName.ToString();
                command.Parameters.Add("@SettingValue", SqlDbType.VarChar).Value = model.SettingValue.ToString();
                command.Parameters.Add("@SettingType", SqlDbType.VarChar).Value = model.SettingType.ToString();
                command.Parameters.Add("@Remarks", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Remarks) ? (object)DBNull.Value : model.Remarks;

                command.Parameters.AddWithValue("@IsActive", true);
                command.Parameters.AddWithValue("@IsArchive", false);

                command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy) ? (object)DBNull.Value : model.Audit.CreatedBy;
                command.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = model.Audit.CreatedAt;
                command.Parameters.Add("@CreatedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedFrom) ? (object)DBNull.Value : model.Audit.CreatedFrom;




                model.Id = Convert.ToInt32(command.ExecuteScalar());


                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbUpdateModel NewTableAdd(string TableName, string createQuery)
        {
            try
            {
                string sqlText = "";
                int count = 0;
                sqlText = "";

                sqlText += " IF  NOT EXISTS (SELECT * FROM sys.objects ";
                sqlText += " WHERE object_id = OBJECT_ID(N'" + TableName + "') AND type in (N'U'))";

                sqlText += " BEGIN";
                sqlText += " " + createQuery;
                sqlText += " END";

                SqlCommand command = CreateCommand(sqlText);

                command.ExecuteNonQuery();


                return new DbUpdateModel();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SettingsModel Update(SettingsModel model)
        {
            try
            {
                string query = @"  update Settings set 
 SettingGroup = @SettingGroup
,SettingName=@SettingName 
,SettingValue=@SettingValue 
,SettingType=@SettingType
,Remarks=@Remarks
,IsActive=@IsActive
,IsArchive=@IsArchive


,LastUpdateBy = @LastUpdateBy 
,LastUpdateAt=@LastUpdateAt 
,LastUpdateFrom=@LastUpdateFrom 



where  Id=@Id";


                SqlCommand command = CreateCommand(query);

                command.Parameters.Add("@SettingGroup", SqlDbType.VarChar).Value = model.SettingGroup.ToString();
                command.Parameters.Add("@SettingName", SqlDbType.NChar).Value = model.SettingName.ToString();
                command.Parameters.Add("@SettingValue", SqlDbType.NChar).Value = model.SettingValue.ToString();
                command.Parameters.Add("@SettingType", SqlDbType.NChar).Value = model.SettingType.ToString();
                command.Parameters.Add("@Remarks", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Remarks) ? (object)DBNull.Value : model.Remarks;

                command.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = model.IsActive;
                command.Parameters.Add("@IsArchive", SqlDbType.VarChar).Value = model.IsArchive;

                command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy) ? (object)DBNull.Value : model.Audit.LastUpdateBy;
                command.Parameters.Add("@LastUpdateAt", SqlDbType.DateTime).Value = model.Audit.LastUpdateAt;
                command.Parameters.Add("@LastUpdateFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateFrom) ? (object)DBNull.Value : model.Audit.LastUpdateFrom;



                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

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
