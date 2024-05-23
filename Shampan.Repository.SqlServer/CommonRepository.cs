using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shampan.Core.Interfaces.Repository;
using Shampan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Repository.SqlServer
{
	public class CommonRepository : Repository, ICommonRepository
	{

		public CommonRepository(SqlConnection context, SqlTransaction transaction)
		{
			this._context = context;
			this._transaction = transaction;
		}


		public List<CommonDropDown>? GetAuditType(bool isPlanned = true)
		{
			try
			{
				var command = CreateAdapter("SELECT  [Id]  Value ,[AuditType] Name FROM [AuditTypes]\r\n\r\n  where IsPlaned = @isPlanned ");

				command.SelectCommand.Parameters.AddWithValue("@isPlanned", isPlanned);

				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public List<CommonDropDown>? GetIssues(string auditId)
		{
			try
			{

				string auditIssues = " select Id [Value],  IssueName [Name] from A_AuditIssues where 1=1 ";

				if (!string.IsNullOrEmpty(auditId))
				{
					auditIssues += " and AuditId = @AuditId";
				}

				SqlDataAdapter command = CreateAdapter(auditIssues);

				if (!string.IsNullOrEmpty(auditId))
				{
					command.SelectCommand.Parameters.AddWithValue("@AuditId", auditId);
				}

				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown>? GetBranchFeedbackIssues(string auditId, string userName,bool team)
		{
			try
			{

				if (team)
				{
                    string auditIssues = " select Id[Value],IssueName[Name]from A_AuditIssues where AuditId = @AuditId ";

                    //if (!string.IsNullOrEmpty(auditId))
                    //{
                    //    auditIssues += " and AI.AuditId = @AuditId AND ss.UserName=@UserName";
                    //}

                    SqlDataAdapter command = CreateAdapter(auditIssues);

                    if (!string.IsNullOrEmpty(auditId))
                    {
                        command.SelectCommand.Parameters.AddWithValue("@AuditId", auditId);
                        //command.SelectCommand.Parameters.AddWithValue("@UserName", userName);
                    }

                    DataTable table = new DataTable();
                    command.Fill(table);
                    return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
                }


				else
				{
                    //string auditIssues = " select Id [Value],  IssueName [Name] from A_AuditIssues where 1=1 ";

                    string auditIssues = " SELECT DISTINCT " +
                        "ai.auditissueid [Value]," +
                        "AIS.IssueName [Name], " +
                        "UserName ,AI.*" +

                        "FROM AuditIssueUsers AI " +

                        "LEFT JOIN GDICAuditAuthDB.dbo.AspNetUsers ss ON ss.Id = AI.UserId " +

                        "left  join A_AuditIssues AIS on AIS.Id = ai.auditissueid where 1=1 ";

                    if (!string.IsNullOrEmpty(auditId))
                    {
                        auditIssues += " and AI.AuditId = @AuditId AND ss.UserName=@UserName";
                    }

                    SqlDataAdapter command = CreateAdapter(auditIssues);

                    if (!string.IsNullOrEmpty(auditId))
                    {
                        command.SelectCommand.Parameters.AddWithValue("@AuditId", auditId);
                        command.SelectCommand.Parameters.AddWithValue("@UserName", userName);
                    }

                    DataTable table = new DataTable();
                    command.Fill(table);

					//Addition

					//string AuditBranch = "select AuditId, AuditIssueId from A_AuditBranchFeedbacks where AuditId = @AuditId";
                    // command = CreateAdapter(AuditBranch);
                    // command.SelectCommand.Parameters.AddWithValue("@AuditId", auditId);
                    // DataTable branch = new DataTable();
                    // command.Fill(branch);
                    // List<string> branchList = new List<string>();
                    // foreach (DataRow row in branch.Rows)
                    // {                     
                    //     string rowData = $"{row["AuditIssueId"]}";
                    //     branchList.Add(rowData);
	                //
                    // }
                    // foreach (string data in branchList)
                    // {                                          
                    //     string auditIssueId = data;                      
                    //     DataRow[] rowsToRemove = table.Select($"AuditIssueId = '{auditIssueId}'");
                    //     foreach (DataRow rowToRemove in rowsToRemove)
                    //     {
                    //         table.Rows.Remove(rowToRemove);
                    //     }
                    // }

                    //End



                    return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
                }

				
			}
			catch (Exception e)
			{
				throw e;
			}
		}



		public List<CommonDropDown>? GetAuditName()
		{
			try
			{
				var command = CreateAdapter("select Id [Value], [Name] from A_Audits");

				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown>? GetIssuePriority()
		{
			try
			{
				var command = CreateAdapter("select ID Value, EnumValue Name  from Enums where EnumType = 'IssuePriority'");

				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown>? GetIssueStatus()
		{
			try
			{
				var command = CreateAdapter("select ID Value, EnumValue Name  from Enums where EnumType = 'IssueStatus'");

				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

        //Status
        public List<CommonDropDown>? GetStatus()
        {
            try
            {
                var command = CreateAdapter("select ID Value, EnumValue Name  from Enums where EnumType = 'IssueStatus'");

                DataTable table = new DataTable();
                command.Fill(table);
                return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<CommonDropDown>? GetAuditTypes()
		{
			try
			{
				var command = CreateAdapter("select id Value, AuditType Name from AuditTypes where IsPlaned = 0");

				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public List<CommonDropDown>? GetTeams()
		{
			try
			{
				var command = CreateAdapter("select id Value, TeamName Name from A_Teams ");


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}



		public List<CommonDropDown>? GetReportStatus()
		{
			try
			{
				var command = CreateAdapter("SELECT EnumValue Value     ,EnumValue Name\r\n      ,[ActiveStatus]\r\n  FROM [Enums]\r\n  where EnumType = 'ReportStatus'\r\n\r\n");


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown>? GetCircularType()
		{
			try
			{
				var command = CreateAdapter("SELECT Id Value ,Name as Name FROM CircularType");


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown>? GetAuditStatus()
		{
			try
			{
				var command = CreateAdapter("SELECT EnumValue Value     ,EnumValue Name\r\n      ,[ActiveStatus]\r\n  FROM [Enums]\r\n  where EnumType = 'AuditStatus'\r\n\r\n");


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}



		public List<CommonDropDown> GetAllProductType()
		{
			try
			{
				var command = CreateAdapter("SELECT ProductTypeId as Name,ProductType as Value FROM CSProductsTypes where IsActive=1 and isnull(IsArchive,0) = 0  ");

				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllStore()
		{
			try
			{
				var command = CreateAdapter("SELECT StoreId as Name,StoreName as Value FROM CSStores where IsActive=1 and isnull(IsArchive,0) = 0  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllUom()
		{
			try
			{
				var command = CreateAdapter("SELECT UomId as Name,UomName as Value FROM CSUoms where IsActive=1 and isnull(IsArchive,0) = 0  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown> GetAllColor()
		{
			try
			{
				var command = CreateAdapter("SELECT ColorId as Name,ColorName as Value FROM CSColors where IsActive=1 and isnull(IsArchive,0) = 0  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllCurrencys()
		{
			try
			{
				var command = CreateAdapter(" SELECT CurrencyId as Name,CurrencyName as Value FROM CSCurrencies where IsActive=1 and isnull(IsArchive,0) = 0  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllDepartment()
		{
			try
			{
				var command = CreateAdapter(" SELECT DepartmentId as Name,DepartmentName as Value FROM CSDepartments where IsActive=1 and isnull(IsArchive,0) = 0  ");
				// command.SelectCommand.Parameters.Add("@SearchBy", SqlDbType.NVarChar).Value =string.IsNullOrEmpty(SearchBy) ? "" : SearchBy;

				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllUserName()
		{
			try
			{
				var command = CreateAdapter(" SELECT Id Value ,UserName as Name FROM GDICAuditAuthDB.dbo.AspNetUsers  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown> GetAllSize()
		{
			try
			{
				var command = CreateAdapter(" SELECT SizeId as Name,SizeName as Value FROM CSSizes where IsActive=1 and ISNULL(IsArchive,0)=0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllProduct()
		{
			try
			{
				var command = CreateAdapter("  SELECT ProductId as Name,ProductName  as Value  FROM CSProducts  where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllVendor()
		{
			try
			{
				var command = CreateAdapter(" SELECT VendorId as Name,VendorName as Value  FROM CSVendors where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public List<CommonDropDown> GetAllCustomers()
		{
			try
			{
				var command = CreateAdapter(" SELECT CustomerId as Name,CustomerName as Value  FROM CSCustomers where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown> GetAllOrderCategories()
		{
			try
			{
				var command = CreateAdapter(" SELECT OrderCategoryId as Name,OrderCategoryName as Value  FROM EXPOrderCategories where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public List<CommonDropDown> GetAllPorts()
		{
			try
			{
				var command = CreateAdapter(" SELECT PortId as Name,PortName as Value  FROM CSPorts where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllPaymentTerms()
		{
			try
			{
				var command = CreateAdapter(" SELECT PaymentTermId as Name,PaymentTerm as Value  FROM CSPaymentTerms where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllBanks()
		{
			try
			{
				var command = CreateAdapter(" SELECT BankId as Name,BankName as Value  FROM CSBanks where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllInsuranceCompanies()
		{
			try
			{
				var command = CreateAdapter(" SELECT InsuranceCompanyId as Name,InsuranceCompanyName as Value  FROM CSInsuranceCompanies where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllDeliveryTerms()
		{
			try
			{
				var command = CreateAdapter(" SELECT DeliveryTermId as Name,DeliveryTerm as Value  FROM CSDeliveryTerms where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllShipmentModes()
		{
			try
			{
				var command = CreateAdapter(" SELECT ShipmentModeId as Name,ShipmentModeName as Value  FROM CSShipmentModes where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllLCCategories()
		{
			try
			{
				var command = CreateAdapter(" SELECT LCCategoryId as Name,LCCategoryName as Value  FROM LCCategories where IsActive=1 and isnull(IsArchive,0) = 0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllMasterLC()
		{
			try
			{
				var command = CreateAdapter(" SELECT LCMasterId as Name,LCNo as Value  FROM LCMasters   where IsBtBLC=0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllPI()
		{
			try
			{
				var command = CreateAdapter(" SELECT PIMasterId as Name,PINo as Value  FROM ExpPIMasters  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllExpOrder()
		{
			try
			{
				var command = CreateAdapter(" SELECT OrderMasterId as Name,OrderNo as Value  FROM ExpOrderMasters  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllBtbLC()
		{
			try
			{
				var command = CreateAdapter("  SELECT LCMasterId as Name,LCNo as Value  FROM LCMasters where IsBtBLC=1 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllExportPIContracts()
		{
			try
			{
				var command = CreateAdapter(" SELECT PIContractMasterId as Name,PIContractNo as Value  FROM  ExpPIContractMasters  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllPackingMode()
		{
			try
			{
				var command = CreateAdapter(" SELECT PackingModeId as Name,PackingModeName as Value  FROM CSPackingModes  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllExpInvoice()
		{
			try
			{
				var command = CreateAdapter(" SELECT InvoiceMasterId as Name,InvoiceNo as Value  FROM ExpInvoiceMasters  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllCountry()
		{
			try
			{
				var command = CreateAdapter(" SELECT CountryId as Name,CountryName as Value  FROM CSCountry  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllCnFAgents()
		{
			try
			{
				var command = CreateAdapter(" SELECT CourierAgentId as Name,CNFAgentName as Value  FROM CSCnFAgents  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllCouriers()
		{
			try
			{
				var command = CreateAdapter(" SELECT CourierId as Name,CourierName as Value  FROM CSCouriers  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllImportTypes()
		{
			try
			{
				var command = CreateAdapter(" SELECT ImportTypeID as Name,ImportType as Value  FROM CSImportTypes  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllEmployees()
		{
			try
			{
				var command = CreateAdapter(" SELECT EmployeeId as Name,Employeename as Value  FROM CSEmployees  ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> GetAllProductCategories()
		{
			try
			{
				var command = CreateAdapter(" SELECT ProductCategoryId as Name,ProductCategoryName as Value  FROM CSProductCategories where IsActive=1 and ISNULL(IsArchive,0)=0 ");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		#region "Äutocomplete"

		public List<CommonDropDown> AutocompleteProduct(string Prefix)
		{
			try
			{
				var command = CreateAdapter("SELECT ProductId as Id,ProductName as Name  FROM CSProducts where IsActive=1 and isnull(IsArchive,0) = 0  and  ProductName like '%" + Prefix + "%' ");

				command.SelectCommand.Parameters.Add("@Prefix", SqlDbType.NVarChar).Value = Prefix;
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> ProductIdByName(string Name)
		{
			try
			{
				var command = CreateAdapter("SELECT ProductId AS Id  FROM CSProducts where IsActive=1 and isnull(IsArchive,0) = 0  and ProductName= '" + Name.Trim() + "'");
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> AutocompleteRequisitionNo(string Prefix)
		{
			try
			{
				var command = CreateAdapter("SELECT RequisitionMasterId as Id,RequisitionNo as Name  FROM ProRequisitionMasters where   RequisitionNo like '%" + Prefix + "%' ");

				command.SelectCommand.Parameters.Add("@Prefix", SqlDbType.NVarChar).Value = Prefix;
				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		//mychange
		public List<CommonDropDown> GetAllHeaders()
		{
			try
			{
				var one = new { Value = 1, Name = "Recept Number" };
				var two = new { Value = 2, Name = "Description" };
				var three = new { Value = 3, Name = "Recept Date" };

				List<object> common = new List<object>();
				common.Add(one);
				common.Add(two);
				common.Add(three);

				//var command = CreateAdapter(" SELECT CurrencyId as Name,CurrencyName as Value FROM CSCurrencies where IsActive=1 and isnull(IsArchive,0) = 0  ");
				DataTable table = new DataTable();
				//common.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(common));
			}
			catch (Exception e)
			{
				throw e;
			}
		}



		public List<CommonDropDown> EntryTypes()
		{
			try
			{
				string sqlText = @"
SELECT EnumValue Name,EnumValue as Value  
FROM Enums
where EnumType = 'ReceiptType'
and ActiveStatus = '1'
order by SLNo";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		//BANK
		//public List<CommonDropDown> DepositType()
		//{
		//    try
		//    {
		//        var one = new { Value = 1, Name = "Savings" };
		//        var two = new { Value = 2, Name = "current" };

		//        List<object> common = new List<object>();
		//        common.Add(one);
		//        common.Add(two);


		//        //var command = CreateAdapter(" SELECT CurrencyId as Name,CurrencyName as Value FROM CSCurrencies where IsActive=1 and isnull(IsArchive,0) = 0  ");
		//        DataTable table = new DataTable();
		//        //common.Fill(table);
		//        return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(common));
		//    }
		//    catch (Exception e)
		//    {
		//        throw e;
		//    }
		//}
		//public List<CommonDropDown> BankEntryType()
		//{
		//    try
		//    {
		//        var one = new { Value = 1, Name = "Transaction entry" };
		//        var two = new { Value = 2, Name = "Closing entry" };

		//        List<object> common = new List<object>();
		//        common.Add(one);
		//        common.Add(two);


		//        //var command = CreateAdapter(" SELECT CurrencyId as Name,CurrencyName as Value FROM CSCurrencies where IsActive=1 and isnull(IsArchive,0) = 0  ");
		//        DataTable table = new DataTable();
		//        //common.Fill(table);
		//        return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(common));
		//    }
		//    catch (Exception e)
		//    {
		//        throw e;
		//    }
		//}
		public List<CommonDropDown> DocumentType()
		{

			try
			{
				string sqlText = @"
SELECT EnumValue Name,EnumValue as Value  
FROM Enums
where EnumType = 'DocumentType'
and ActiveStatus = '1'
order by SLNo";

				SqlDataAdapter command = CreateAdapter(sqlText);

				DataTable table = new DataTable();
				command.Fill(table);

				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));

			}
			catch (Exception e)
			{
				throw e;
			}


			//try
			//{
			//    var one = new { Value = 1, Name = "Transfer" };
			//    var two = new { Value = 2, Name = "Transit Transfer" };
			//    var three = new { Value = 3, Name = "Transit Receipt" };

			//    List<object> common = new List<object>();
			//    common.Add(one);
			//    common.Add(two);
			//    common.Add(three);


			//    //var command = CreateAdapter(" SELECT CurrencyId as Name,CurrencyName as Value FROM CSCurrencies where IsActive=1 and isnull(IsArchive,0) = 0  ");
			//    DataTable table = new DataTable();
			//    //common.Fill(table);
			//    return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(common));
			//}
			//catch (Exception e)
			//{
			//    throw e;
			//}
		}


		public List<CommonDropDown> APDocumentType()
		{
			try
			{
				string sqlText = @"
SELECT EnumValue Name,EnumValue as Value  
FROM Enums
where EnumType = 'APDocumentType'
and ActiveStatus = '1'
order by SLNo";

				SqlDataAdapter command = CreateAdapter(sqlText);

				DataTable table = new DataTable();
				command.Fill(table);

				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> BankEntryType()
		{
			try
			{
				string sqlText = @"
SELECT EnumValue Name,EnumValue as Value  
FROM Enums
where EnumType = 'BankEntryType'
and ActiveStatus = '1'
order by SLNo";

				SqlDataAdapter command = CreateAdapter(sqlText);

				DataTable table = new DataTable();
				command.Fill(table);

				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public List<CommonDropDown> Branchs(string UserId)
		{
			try
			{
				string sqlText = @"

SELECT b.BranchID as Value, b.BranchName as Name

FROM BranchProfiles b
LEFT OUTER JOIN UserBranchMap u ON b.BranchID = u.BranchId
WHERE u.UserId = @UserId
And isnull(b.IsArchive,0)=0

--And isnull(b.ActiveStatus,1) = 1 and isnull(b.IsArchive,0)=0

";

				SqlCommand objComm = CreateCommand(sqlText);
				objComm.Parameters.AddWithValue("@UserId", UserId);

				SqlDataAdapter command = CreateAdapter(sqlText);
				command.SelectCommand.Parameters.AddWithValue("@UserId", UserId);

				DataTable table = new DataTable();
				command.Fill(table);

				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}




		public List<CommonDropDown> DepositType()
		{
			try
			{
				string sqlText = @"
SELECT EnumValue Name,EnumValue as Value  
FROM Enums
where EnumType = 'BankPaymentType'
and ActiveStatus = '1'
order by SLNo";

				SqlDataAdapter command = CreateAdapter(sqlText);

				DataTable table = new DataTable();
				command.Fill(table);

				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		public List<CommonDropDown> ProrationMethod()
		{
			try
			{
				string sqlText = @"
SELECT EnumValue Name,EnumValue as Value  
FROM Enums
where EnumType = 'ProrationMethod'
and ActiveStatus = '1'
order by SLNo";

				SqlDataAdapter command = CreateAdapter(sqlText);

				DataTable table = new DataTable();
				command.Fill(table);

				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));

			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown> POType()
		{

			try
			{
				string sqlText = @"
SELECT EnumValue Name,EnumValue as Value  
FROM Enums
where EnumType = 'POType'
and ActiveStatus = '1'
order by SLNo";

				SqlDataAdapter command = CreateAdapter(sqlText);

				DataTable table = new DataTable();
				command.Fill(table);

				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));

			}
			catch (Exception e)
			{
				throw e;
			}
		}



		public List<CommonDropDown> ApplyMethod()
		{
			try
			{
				string sqlText = @"
SELECT EnumValue Name,EnumValue as Value  
FROM Enums
where EnumType = 'ApplyMethod'
and ActiveStatus = '1'
order by SLNo";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown> TransactionType()
		{
			try
			{
				string sqlText = @"
SELECT EnumValue Name,EnumValue as Value  
FROM Enums
where EnumType = 'TransactionType'
and ActiveStatus = '1'
order by SLNo";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}




		public List<CommonDropDown> OrderBy()
		{
			try
			{
				string sqlText = @"
SELECT EnumValue Name,EnumValue as Value  
FROM Enums
where EnumType = 'OrderBy'
and ActiveStatus = '1'
order by SLNo";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown> UserBranch()
		{
			try
			{
				string sqlText = @"
SELECT BranchId as Value, 
BranchName as Name
FROM BranchProfiles


Where IsArchive = 0


order by BranchId";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}



		public List<CommonDropDown> UserId()
		{
			try
			{
				string sqlText = @"
SELECT Id as Name , UserName as Value
--FROM AuthDB.dbo.AspNetUsers
FROM GDICAuditAuthDB.dbo.AspNetUsers


order by Id";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}
        public List<CommonDropDown> ModulId()
        {
            try
            {
                string sqlText = @"
SELECT
Id as Name ,
Modul as Value

FROM TBLModul

order by Id";

                SqlDataAdapter command = CreateAdapter(sqlText);


                DataTable table = new DataTable();
                command.Fill(table);
                return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<CommonDropDown> NodeName()
        {
            try
            {
                string sqlText = @"
SELECT 

Id as Name ,
--Node as Name ,
Node as Value

FROM TblNode


order by Id";

                SqlDataAdapter command = CreateAdapter(sqlText);


                DataTable table = new DataTable();
                command.Fill(table);
                return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CommonDropDown> GetModuleName()
		{
			try
			{
				string sqlText = @"
SELECT Id as Name , Modul as Value
FROM ModulPermission

order by Id";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		//SSLAudit

		public List<CommonDropDown> TeamName()
		{
			try
			{
				string sqlText = @"
SELECT Id as Value  , TeamName as Name
FROM A_Teams

order by Id";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown> AuditName()
		{
			try
			{
				string sqlText = @"
SELECT Id as Value  , Name as Name
FROM A_Audits

order by Id";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown> ColorName()
		{
			try
			{
				string sqlText = @"
SELECT Color as Name , Color as Value
FROM Colors

order by Id";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public List<CommonDropDown> ReportingManagers()
		{
			try
			{
				string sqlText = @"
SELECT EmployeeId as Value , Name  
FROM EmployeesHierarchy

order by EmployeeId";

				SqlDataAdapter command = CreateAdapter(sqlText);


				DataTable table = new DataTable();
				command.Fill(table);
				return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

        public List<CommonDropDown> BranchName(string UserId)
        {
            try
            {
                string sqlText = @"

SELECT b.BranchName as Value, b.BranchName as Name

FROM BranchProfiles b
LEFT OUTER JOIN UserBranchMap u ON b.BranchID = u.BranchId
WHERE u.UserId = @UserId
And isnull(b.IsArchive,0)=0

--And isnull(b.ActiveStatus,1) = 1 and isnull(b.IsArchive,0)=0

";

                SqlCommand objComm = CreateCommand(sqlText);
                objComm.Parameters.AddWithValue("@UserId", UserId);

                SqlDataAdapter command = CreateAdapter(sqlText);
                command.SelectCommand.Parameters.AddWithValue("@UserId", UserId);

                DataTable table = new DataTable();
                command.Fill(table);

                return JsonConvert.DeserializeObject<List<CommonDropDown>>(JsonConvert.SerializeObject(table));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }


	#endregion "Äutocomplete"
}

