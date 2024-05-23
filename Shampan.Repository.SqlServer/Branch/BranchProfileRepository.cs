using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Branch;
using Shampan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Repository.SqlServer.Branch
{
    public class BranchProfileRepository : Repository, IBranchProfileRepository
    {
        public BranchProfileRepository(SqlConnection context, SqlTransaction transaction)
        {
            this._context = context;
            this._transaction = transaction;
        }

		public BranchProfile BranchProfileUpdate(BranchProfile model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BranchProfiles set 

  IsArchive=@IsArchive                         
 ,LastUpdateBy=@LastUpdateBy                         
 ,LastUpdateOn=@LastUpdateOn                         
 ,LastUpdateFrom=@LastUpdateFrom

  where  BranchID= @BranchID ";


				SqlCommand command = CreateCommand(query);
				command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
				command.Parameters.Add("@IsArchive", SqlDbType.NChar).Value = 1;

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

		public List<BranchProfile> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            try
            {
                string sqlText = @"select 
BranchID
,BranchCode
,BranchName
,BranchLegalName
,Address
,City
,ZipCode
,TelephoneNo
,FaxNo
,Email
,ContactPerson
,ContactPersonDesignation
,ContactPersonTelephone
,ContactPersonEmail
,Comments
,ActiveStatus

,CogsCode
,TaxCode
,VatCode
,CompanyCode
,CompanyName
,CompanyAddress

,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom
,LastModifiedOn
,LastModifiedBy
,IsCentral
,TINNo
,IsWCF
,IsArchive
,ID






from   BranchProfiles
where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BranchProfile> vms = dtResult.ToList<BranchProfile>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       public List<BranchProfile> GetIndexData(IndexModel Index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {

            string sqlText = "";
            List<BranchProfile> VMs = new List<BranchProfile>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select BranchProfiles.BranchID
                ,BranchProfiles.BranchCode
                ,BranchProfiles.BranchName
                ,BranchProfiles.Address
                ,BranchProfiles.TelephoneNo
               

                from BranchProfiles 
                 where 1=1 And IsArchive = 0";



                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + Index.OrderName + "  " + Index.orderDir;
                sqlText += @" OFFSET  " + Index.startRec + @" ROWS FETCH NEXT " + Index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new BranchProfile();

                VMs.Add(req);


                VMs = dt
                .ToList<BranchProfile>();

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
            List<BranchProfile> VMs = new List<BranchProfile>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(BranchProfiles.BranchID)FilteredCount
                from BranchProfiles  where 1=1 ";


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

        public BranchProfile Insert(BranchProfile model)
        {
            string checkQuery = "SELECT COUNT(*) FROM BranchProfiles WHERE BranchCode = @BranchCode";
            var checkCommand = CreateCommand(checkQuery);
            checkCommand.Parameters.Add("@BranchCode", SqlDbType.NChar).Value = model.BranchCode;
            int count = Convert.ToInt32(checkCommand.ExecuteScalar());
            if (count > 0)
            {
                throw new Exception("Branch Code  already exists");
            }
            else
            {
                try
                {
                    string sqlText = "";
                    
                    //  string[] retResults = { "Fail", "Fail", Id.ToString(), sqlText, "ex", "Insert" };

                    var command = CreateCommand(@" INSERT INTO BranchProfiles(




BranchCode
,BranchName
,BranchLegalName
,Address
,City
,ZipCode
,TelephoneNo
,FaxNo
,Email
,ContactPerson
,ContactPersonDesignation
,ContactPersonTelephone
,ContactPersonEmail
,Comments
,ActiveStatus
,CogsCode
,TaxCode
,VatCode
,CompanyCode
,CompanyName
,CompanyAddress


,CreatedBy
,CreatedOn
,CreatedFrom
,IsArchive

) VALUES (


@BranchCode
,@BranchName
,@BranchLegalName
,@Address
,@City
,@ZipCode
,@TelephoneNo
,@FaxNo
,@Email
,@ContactPerson
,@ContactPersonDesignation
,@ContactPersonTelephone
,@ContactPersonEmail
,@Comments
,@ActiveStatus
,@CogsCode
,@TaxCode
,@VatCode
,@CompanyCode
,@CompanyName
,@CompanyAddress


,@CreatedBy
,@CreatedOn
,@CreatedFrom
,@IsArchive



)SELECT SCOPE_IDENTITY()");
                    command.Parameters.Add("@BranchCode", SqlDbType.NChar).Value = model.BranchCode;
                    command.Parameters.Add("@BranchName", SqlDbType.NChar).Value = model.BranchName;
                    command.Parameters.Add("@BranchLegalName", SqlDbType.NChar).Value = model.BranchLegalName;
                    command.Parameters.Add("@Address", SqlDbType.NChar).Value = model.Address;
                    command.Parameters.Add("@City", SqlDbType.NChar).Value = model.City;

                    command.Parameters.Add("@ZipCode", SqlDbType.NChar).Value = model.ZipCode;
                    command.Parameters.Add("@TelephoneNo", SqlDbType.NChar).Value = model.TelephoneNo;
                    command.Parameters.Add("@FaxNo", SqlDbType.NChar).Value = model.FaxNo;
                    //command.Parameters.Add("@CurrencyCode", SqlDbType.NChar).Value = model.CurrencyCode;
                    command.Parameters.Add("@Email", SqlDbType.NChar).Value = model.Email;
                    command.Parameters.Add("@ContactPerson", SqlDbType.NChar).Value = model.ContactPerson;
                    command.Parameters.Add("@ContactPersonDesignation", SqlDbType.NChar).Value = model.ContactPersonDesignation;
                    command.Parameters.Add("@ContactPersonEmail", SqlDbType.NChar).Value = model.ContactPersonEmail;
                   // command.Parameters.Add("@Comments", SqlDbType.NChar).Value = model.Comments;
					command.Parameters.Add("@Comments", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Comments) ? (object)DBNull.Value : model.Comments;


                    command.Parameters.Add("@ActiveStatus", SqlDbType.NChar).Value = 1;

                    //command.Parameters.Add("@CogsCode", SqlDbType.NChar).Value = model.CogsCode;
                    //command.Parameters.Add("@TaxCode", SqlDbType.NChar).Value = model.TaxCode;
                    //command.Parameters.Add("@VatCode", SqlDbType.NChar).Value = model.VatCode;
                    command.Parameters.Add("@CogsCode", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CogsCode) ? (object)DBNull.Value : model.CogsCode;
                    command.Parameters.Add("@TaxCode", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.TaxCode) ? (object)DBNull.Value : model.TaxCode;
                    command.Parameters.Add("@VatCode", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.VatCode) ? (object)DBNull.Value : model.VatCode;


                    command.Parameters.Add("@CompanyCode", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CompanyCode) ? (object)DBNull.Value : model.CompanyCode;
					command.Parameters.Add("@CompanyName", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CompanyName) ? (object)DBNull.Value : model.CompanyName;
					command.Parameters.Add("@CompanyAddress", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CompanyAddress) ? (object)DBNull.Value : model.CompanyAddress;

					command.Parameters.Add("@ContactPersonTelephone", SqlDbType.NChar).Value = model.ContactPersonTelephone;


                    command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();

                    command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();

                    command.Parameters.Add("@CreatedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : model.Audit.CreatedFrom.ToString();
                    command.Parameters.Add("@IsArchive", SqlDbType.Bit).Value =0;


                    model.BranchID = Convert.ToInt32(command.ExecuteScalar());


                    return model;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public BranchProfile Update(BranchProfile model)
        {



            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"  update BranchProfiles set 

BranchCode               =@BranchCode  
,BranchName              =@BranchName  
,BranchLegalName         =@BranchLegalName 
,Address                 =@Address  
,City                    =@City  
,ZipCode                 =@ZipCode  
,TelephoneNo             =@TelephoneNo  
,FaxNo                   =@FaxNo  
,Email                   =@Email  
,ContactPerson           =@ContactPerson  
,ContactPersonDesignation=@ContactPersonDesignation   
,ContactPersonTelephone  =@ContactPersonTelephone  
,ContactPersonEmail      =@ContactPersonEmail  
,Comments                =@Comments

,CogsCode                =@CogsCode
,TaxCode                 =@TaxCode
,VatCode                 =@VatCode
,CompanyCode             =@CompanyCode
,CompanyName             =@CompanyName
,CompanyAddress             =@CompanyAddress



,ActiveStatus            =@ActiveStatus  
,LastUpdateBy               =@LastUpdateBy  
,LastUpdateOn               =@LastUpdateOn  
,LastUpdateFrom            =@LastUpdateFrom   
                       
where  BranchID= @BranchID ";          
                           
                           
                SqlCommand command = CreateCommand(query);
                command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
                command.Parameters.Add("@BranchCode", SqlDbType.NChar).Value = model.BranchCode;
                command.Parameters.Add("@BranchName", SqlDbType.NChar).Value = model.BranchName;
                command.Parameters.Add("@BranchLegalName", SqlDbType.NChar).Value = model.BranchLegalName;
                command.Parameters.Add("@Address", SqlDbType.NChar).Value = model.Address;
                command.Parameters.Add("@City", SqlDbType.NChar).Value = model.City;

                command.Parameters.Add("@ZipCode", SqlDbType.NChar).Value = model.ZipCode;
                command.Parameters.Add("@TelephoneNo", SqlDbType.NChar).Value = model.TelephoneNo;
                command.Parameters.Add("@FaxNo", SqlDbType.NChar).Value = model.FaxNo;
                //command.Parameters.Add("@CurrencyCode", SqlDbType.NChar).Value = model.CurrencyCode;
                command.Parameters.Add("@Email", SqlDbType.NChar).Value = model.Email;
                command.Parameters.Add("@ContactPerson", SqlDbType.NChar).Value = model.ContactPerson;
                command.Parameters.Add("@ContactPersonDesignation", SqlDbType.NChar).Value = model.ContactPersonDesignation;
                command.Parameters.Add("@ContactPersonEmail", SqlDbType.NChar).Value = model.ContactPersonEmail;
                //command.Parameters.Add("@Comments", SqlDbType.NChar).Value = model.Comments;

				command.Parameters.Add("@Comments", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Comments) ? (object)DBNull.Value : model.Comments;


				command.Parameters.Add("@ActiveStatus", SqlDbType.NChar).Value = model.ActiveStatus;


				command.Parameters.Add("@CogsCode", SqlDbType.NChar).Value = model.CogsCode;
				command.Parameters.Add("@TaxCode", SqlDbType.NChar).Value = model.TaxCode;
				command.Parameters.Add("@VatCode", SqlDbType.NChar).Value = model.VatCode;
				command.Parameters.Add("@CompanyCode", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CompanyCode) ? (object)DBNull.Value : model.CompanyCode;
				command.Parameters.Add("@CompanyName", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CompanyName) ? (object)DBNull.Value : model.CompanyName;

				command.Parameters.Add("@CompanyAddress", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.CompanyAddress) ? (object)DBNull.Value : model.CompanyAddress;

				command.Parameters.Add("@ContactPersonTelephone", SqlDbType.NChar).Value = model.ContactPersonTelephone;


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
