using System.Data;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Shampan.Repository.SqlServer
{
    public abstract class Repository
    {
        protected SqlConnection _context;
        protected SqlTransaction _transaction;

        protected SqlCommand CreateCommand(string query)
        {
            return new SqlCommand(query, _context, _transaction);
        }

        protected SqlDataAdapter CreateAdapter(string query)
        {
            var cmd =  new SqlCommand(query, _context, _transaction);

            return new SqlDataAdapter(cmd);
        }

        protected SqlDataAdapter CreateAdapter(SqlCommand cmd)
        {
            return new SqlDataAdapter(cmd);
        }

        //change
        public string GetSettingsValue(string[] conditionalFields, string[] conditionalValue)
        {

            try
            {
                string sqlText = @"select SettingValue from Settings where 1=1";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResutl = new DataTable();




                adapter.Fill(dtResutl);

                string settingValue = "2";

                if (dtResutl.Rows.Count > 0)
                {
                    DataRow row = dtResutl.Rows[0];
                    settingValue = row["SettingValue"].ToString();
                }




                return settingValue;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public string StringReplacing(string stringToReplace)
        {
            string newString = stringToReplace;
            if (stringToReplace.Contains("."))
            {
                newString = Regex.Replace(stringToReplace, @"^[^.]*.", "", RegexOptions.IgnorePatternWhitespace);

                //ForDateTimeFormatNewAddition
                //newString = Regex.Replace(newString, @"\s*'[^']*'\s*[\)]", "", RegexOptions.IgnorePatternWhitespace);
                newString = Regex.Replace(newString, @"\s*'[^']*'\s*(?:,|\))", "", RegexOptions.IgnorePatternWhitespace);
                newString = newString.Replace(",", "");
                //newString = ReplaceCharacters(newString);
                //newString = newString + " like";
                //EndOfNewAddition
            }
            newString = newString.Replace(">", "From");
            newString = newString.Replace("<", "To");
            newString = newString.Replace("!", "");
            newString = newString.Replace("[", "");
            newString = newString.Replace("]", "");
          //  newString = newString.Replace("IN", "");
            return newString;
        }

        public  string ApplyConditions(string sqlText, string[] conditionalFields,string[] conditionalValue, bool orOperator = false)
        {
            string cField = "";
            bool conditionFlag = true;
            //bool checkValueExist = false;
            //bool checkConditioanlValue = false;
            var checkValueExist = conditionalValue==null?false: conditionalValue.ToList().Any(x => !string.IsNullOrEmpty(x));
            var checkConditioanlValue = conditionalValue==null?false: conditionalValue.ToList().Any(x => !string.IsNullOrEmpty(x));

            //if (conditionalValue != null)
            //{
            //    checkValueExist = true;
            //}
            //if(conditionalValue !=null )
            //{
            //    checkConditioanlValue=true;
            //}

            if (checkValueExist && orOperator && checkConditioanlValue)
            {
                sqlText += " and (";
            }


            if (conditionalFields != null && conditionalValue != null && conditionalFields.Length == conditionalValue.Length)
            {
                for (int i = 0; i < conditionalFields.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(conditionalFields[i]) || string.IsNullOrWhiteSpace(conditionalValue[i]))
                    {
                        continue;
                    }
                    cField = conditionalFields[i].ToString();
                    cField = StringReplacing(cField);
                    string operand = " AND ";

                    if (orOperator)
                    {
                        operand = " OR ";

                        if (conditionFlag)
                        {
                            operand = "  ";
                            conditionFlag = false;
                        }
                    }

                   
                    if (conditionalFields[i].ToLower().Contains("like"))
                    {
                        sqlText += operand +conditionalFields[i] + " '%'+ " + " @" + cField.Replace("like", "").Trim() + " +'%'";
                    }
                    else if (conditionalFields[i].Contains(">") || conditionalFields[i].Contains("<"))
                    {
                        sqlText += operand + conditionalFields[i] + " @" + cField;

                    }
                    else if (conditionalFields[i].Contains("in",StringComparison.OrdinalIgnoreCase))
                    {
                         
                        //  such as invoice then work it , to avoid this type 

                      var test= conditionalFields[i].Split(" in");

                        if (test.Length > 1)
                        {
                            sqlText += operand + conditionalFields[i] + "(" + conditionalValue[i] + ")";
                        }    else
                        {
                            sqlText += operand + conditionalFields[i] + "= '" + Convert.ToString( conditionalValue[i]) + "'";
                        }

                    }                   
                    else
                    {
                        sqlText += operand + conditionalFields[i] + "= @" + cField;
                    }
                }
            }

            if (checkValueExist && orOperator && checkConditioanlValue)
            {
                sqlText += " )";
            }

            return sqlText;
        }
        
        public  SqlCommand ApplyParameters(SqlCommand objComm, string[] conditionalFields,string[] conditionalValue)
        {
            string cField = "";
            string tst = "";
            if (conditionalFields != null && conditionalValue != null && conditionalFields.Length == conditionalValue.Length)
            {
                for (int j = 0; j < conditionalFields.Length; j++)
                {
                    if (string.IsNullOrWhiteSpace(conditionalFields[j]) || string.IsNullOrWhiteSpace(conditionalValue[j]))
                    {
                        continue;
                    }
                    cField = conditionalFields[j].ToString();
                    cField = StringReplacing(cField);
                    var test = conditionalFields[j].ToLower().Contains("in");


                    if (conditionalFields[j].ToLower().Contains("like"))
                    {
                        objComm.Parameters.AddWithValue("@" + cField.Replace("like", "").Trim(), conditionalValue[j]);
                    }
                    else if (conditionalFields[j].ToLower().Contains("in",StringComparison.OrdinalIgnoreCase))
                    {
                    }
                    else
                    {
                        objComm.Parameters.AddWithValue("@" + cField, conditionalValue[j]);
                    }
                }
            }

            return objComm;
        }

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue)
        {
            try
            {
                // ToDo sql injection
                string sqlText = "select count(" + fieldName + ") TotalRecords from " + tableName + " where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue,true);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                int totalRecords = Convert.ToInt32(command.ExecuteScalar());

                return totalRecords;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Delete(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            try
            {
                // ToDo sql injection
                string sqlText = " delete   " + tableName + "  where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                int totalRecords = Convert.ToInt32(command.ExecuteNonQuery());

                return totalRecords;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int DetailsDelete(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            try
            {
                // ToDo sql injection
                string sqlText = " delete   " + tableName + "  where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                int totalRecords = Convert.ToInt32(command.ExecuteNonQuery());

                return totalRecords;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        // Todo add audit
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            try
            {
                // ToDo sql injection
                string sqlText = "update from " + tableName + " set IsArchive=1 where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                int totalRecords = Convert.ToInt32(command.ExecuteScalar());

                return totalRecords;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CheckExists(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            try
            {
                // ToDo sql injection
                string sqlText = "select count(*)  from " + tableName + " where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                int totalRecords = Convert.ToInt32(command.ExecuteScalar());

                return totalRecords > 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GenerateCode( string CodeGroup, string CodeName, int branchId = 1)
        {
            try
            {
                // ToDo sql injection
                string sqlText = "";

                string NewCode = "";
                string CodePreFix = "";
                string CodeGenerationFormat = "B/N/Y";
                string CodeGenerationMonthYearFormat = "MMYY";
                string BranchCode = "001";
                string CurrentYear = "2020";
                string BranchNumber = "1";
                int CodeLength = 0;
                int nextNumber = 0;
              
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataSet ds = new DataSet();

                DateTime TransactionDate = DateTime.Now;
                string year = Convert.ToDateTime(DateTime.Now).ToString("yyyy");

                int BranchId = branchId;


                sqlText += " SELECT   top 1  SettingName FROM Settings";
                sqlText += " WHERE     (SettingGroup ='" + CodeGenerationFormat + "') and   (SettingValue ='Y')  ";

                sqlText += " SELECT   top 1  SettingName FROM Settings";
                sqlText += " WHERE     (SettingGroup ='"+ CodeGenerationFormat + "') and   (SettingValue ='Y')  ";

                sqlText += " SELECT   top 1  BranchCode FROM BranchProfiles";
                sqlText += " WHERE     (BranchID ='" + BranchId + "')   ";

                sqlText += " SELECT   count(BranchCode) BranchNumber FROM BranchProfiles where IsArchive='0' and ActiveStatus='1'";

                sqlText += "  SELECT   * from  CodeGenerations where CurrentYear<='2020' ";

                sqlText += "  select YEAR from FiscalYears where '" + Convert.ToDateTime(TransactionDate).ToString("dd/MMM/yyyy") + "' between YearStart and YearEnd ";

                SqlCommand command = CreateCommand(sqlText);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(ds);



                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    CodeGenerationFormat = ds.Tables[0].Rows[0][0].ToString();

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    CodeGenerationMonthYearFormat = ds.Tables[1].Rows[0][0].ToString();
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    BranchCode = ds.Tables[2].Rows[0][0].ToString();

                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    BranchNumber = ds.Tables[3].Rows[0][0].ToString();


                sqlText = "  ";
                sqlText += "  update CodeGenerations set CurrentYear ='2020'  where CurrentYear <='2020'";

                 command = CreateCommand(sqlText);               
                command.ExecuteNonQuery();

                if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                    CurrentYear = ds.Tables[5].Rows[0][0].ToString();

                sqlText = "  ";

                sqlText += " SELECT     * FROM Codes";
                sqlText += " WHERE     (CodeGroup =@CodeGroup) AND (CodeName = @CodeName)";

                command.CommandText = sqlText;


                command.Parameters.AddWithValue("@CodeGroup", CodeGroup);
                command.Parameters.AddWithValue("@CodeName", CodeName);

                dataAdapter = new SqlDataAdapter(command);


                dataAdapter.Fill(dt1);
                if (dt1 == null || dt1.Rows.Count <= 0)
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    CodePreFix = dt1.Rows[0]["prefix"].ToString();
                    CodeLength = Convert.ToInt32(dt1.Rows[0]["Lenth"]);
                }

                sqlText = "  ";
                sqlText += @" 
SELECT top 1 
Id
,CurrentYear
,BranchId
,Prefix
,ISNULL(LastId,0) LastId
FROM CodeGenerations 
WHERE CurrentYear=@CurrentYear AND BranchId=@BranchId AND Prefix=@Prefix order by LastId Desc
";


                command.CommandText = sqlText;


                command.Parameters.AddWithValue("@BranchId", BranchId);
                command.Parameters.AddWithValue("@CurrentYear", CurrentYear);
                command.Parameters.AddWithValue("@Prefix", CodePreFix);


                dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dt2);


                if (dt2 == null || dt2.Rows.Count <= 0)
                {
                    sqlText = "  ";
                    sqlText +=
                        " INSERT INTO CodeGenerations(	CurrentYear,BranchId,Prefix,LastId)";
                    sqlText += " VALUES(";
                    sqlText += " @CurrentYear,";
                    sqlText += " @BranchId,";
                    sqlText += " @Prefix,";
                    sqlText += " 1";
                    sqlText += " )";

                    command.CommandText = sqlText;

                   
                   // command.Parameters.AddWithValue("@CurrentYear", CurrentYear);
                  //  command.Parameters.AddWithValue("@BranchId", BranchId);
                  //  command.Parameters.AddWithValue("@Prefix", CodePreFix);

                    object objfoundId1 = command.ExecuteNonQuery();

                    nextNumber = 1;
                }
                else
                {
                    if (nextNumber != 1)
                    {
                        nextNumber = dt2.Rows[0]["LastId"] == null ? 1 : Convert.ToInt32(dt2.Rows[0]["LastId"]) + 1;
                    }

                    sqlText = "  ";
                    sqlText += " update  CodeGenerations set LastId='" + nextNumber + "'";
                    sqlText += " WHERE CurrentYear=@CurrentYear AND BranchId=@BranchId AND Prefix=@Prefix";


                    command.CommandText = sqlText;

                   // command.Parameters.AddWithValue("@Prefix", CodePreFix);
                    command.ExecuteNonQuery();

                }

                //  NewCode = CodeGenerationMonthYearFormat + "~" + BranchNumber + "~" + CodeGenerationFormat + "~" + BranchCode + "~" + CodeLength + "~" + nextNumber + "~" + CodePreFix + "~" + TransactionDate.ToString("yyyymmdd") + "~" + CurrentYear;
             // DateTime TransactionDate= Convert.ToDateTime(DateTime.Now).ToString("dd/MMM/yyyy");

              NewCode=   CodeGeneration1(CodeGenerationMonthYearFormat, BranchNumber, CodeGenerationFormat, BranchCode, CodeLength, nextNumber, CodePreFix, TransactionDate.ToString());
                return NewCode;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public string SageNextId(string ColumnName, string tableName,string ConditionFeild="", string ConditionValue = "")
        {
            #region Initializ
            string sqlText = "";
            string nextId = "000001";
            #endregion

            try
            {
                #region Count
                sqlText = "SELECT " + ColumnName + "  FROM  " + tableName + " where 1=1 and " + ConditionFeild + "=@ConditionValue ";
                SqlCommand command = CreateCommand(sqlText);
                command.Parameters.AddWithValue("@ConditionValue", ConditionValue);

                var exeRes = command.ExecuteScalar();
                nextId = exeRes.ToString();
                #endregion Count
                return nextId;
            }
            catch (Exception e)
            {
                throw e;
            }

        }



        public string CodeGeneration1(string CodeGenerationMonthYearFormat, string BranchNumber, string CodeGenerationFormat, string BranchCode, int CodeLength
                   , int nextNumber, string CodePreFix, string TransactionDate)
        {
            string NewCode = "";

            #region try

            try
            {
                CodeGenerationMonthYearFormat = CodeGenerationMonthYearFormat.Replace("Y", "y");
                if (Convert.ToInt32(BranchNumber) <= 1)
                {
                    CodeGenerationFormat = CodeGenerationFormat.Replace("B/", "");
                }

               // CodeGenerationFormat = CodeGenerationFormat.Substring(0, CodeGenerationFormat.Length - 1);

                var my = Convert.ToDateTime(TransactionDate).ToString(CodeGenerationMonthYearFormat);
                var nextNumb = nextNumber.ToString().PadLeft(CodeLength, '0');
                CodeGenerationFormat = CodeGenerationFormat.Replace("N", nextNumb);
                CodeGenerationFormat = CodeGenerationFormat.Replace("Y", my);
                CodeGenerationFormat = CodeGenerationFormat.Replace("B", BranchCode);

                NewCode = CodePreFix + "-" + CodeGenerationFormat;
            }
            #endregion

            #region catch

            catch (Exception ex)
            {
               // FileLogger.Log("CommonDAL", "CodeGeneration1", ex.ToString());

                throw ex;
            }
            #endregion

            return NewCode;


        }


        public string GetSingleValeByID(string tableName,string ReturnFields, string[] conditionalFields, string[] conditionalValue)
        {
            try
            {
                // ToDo sql injection
                string sqlText = "select "+ ReturnFields + "  from " + tableName + " where 1=1  ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dtResutl = new DataTable();
                adapter.Fill(dtResutl);
                if(dtResutl.Rows.Count>0)
                {
                    return dtResutl.Rows[0][ReturnFields].ToString();
                }
                else
                {
                    return ""; 
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetAccountColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("Account Number".ToLower(), "ACCTFMTTD");
            columns.Add("AccountNumber".ToLower(), "ACCTFMTTD");
           
            columns.Add("Description".ToLower(), "Acctdesc");

            return columns[DisplayName.ToLower()];


        }

        public string GetVendorColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("VendorCode".ToLower(), "VENDORID");
            columns.Add("ShortName".ToLower(), "SHORTNAME");
            columns.Add("VendorName".ToLower(), "VENDNAME");
            columns.Add("GroupCode".ToLower(), "IDGRP");
            //columns.Add("GroupCode".ToLower(), "IDGRP");
            columns.Add("CurrencyCode".ToLower(), "CURNCODE");
            columns.Add("AccountSetDescription".ToLower(), "TEXTDESC");

            return columns[DisplayName.ToLower()];


        }

        public string GetVendorAccountColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("Set".ToLower(), "ACCTSET");
            columns.Add("Description".ToLower(), "TEXTDESC");
            columns.Add("Code".ToLower(), "CURRCODE");
            

            return columns[DisplayName.ToLower()];


        }




        public string GetTestColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("TransactionDate".ToLower(), "TransDate");
      
            return columns[DisplayName.ToLower()];


        }

        public string GetItemColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("ItemNumber".ToLower(), "FMTITEMNO");
            columns.Add("ItemUnformatted".ToLower(), "ITEMNO");
            columns.Add("Description".ToLower(), "DESC");
            columns.Add("StructureCode".ToLower(), "ITEMBRKID");
            columns.Add("Category".ToLower(), "CATEGORY");
            columns.Add("UnitofMeasure".ToLower(), "STOCKUNIT");

            return columns[DisplayName.ToLower()];


        }

        

        public string GetLocationColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("Location".ToLower(), "[LOCATION]");
            columns.Add("Name".ToLower(), "[DESC]");
            columns.Add("QuantityOnHand".ToLower(), "[QTYONHAND]");
            columns.Add("QuantityOnPO".ToLower(), "[QTYONORDER]");
            columns.Add("QuantityOnSO".ToLower(), "[QTYSALORDR]");
            columns.Add("QuantityAvailable".ToLower(), "[QTYOFFSET]");

            return columns[DisplayName.ToLower()];


        }
        public string GetShipViaColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("Code".ToLower(), "[CODE]");
            columns.Add("ShipViaName".ToLower(), "[Name]");
            columns.Add("Address1".ToLower(), "[ADDRESS1]");
            columns.Add("Address2".ToLower(), "[ADDRESS2]");

            return columns[DisplayName.ToLower()];


        }
        public string GetTemplateColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("Code".ToLower(), "[TEMPLATE]");
            columns.Add("Description".ToLower(), "[PLATEDESC]");


            return columns[DisplayName.ToLower()];


        }
        public string GetTermsCodeColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("Code".ToLower(), "[TERMSCODE]");
            columns.Add("Description".ToLower(), "[CODEDESC]");


            return columns[DisplayName.ToLower()];


        }

        public string GetPriceColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("PriceList".ToLower(), "[PRICELIST]");
            columns.Add("Description".ToLower(), "[DESC]");

            return columns[DisplayName.ToLower()];


        }
        public string GetShipToLocationColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("Location".ToLower(), "[Location]");
            columns.Add("Description".ToLower(), "[DESC]");
            columns.Add("Address".ToLower(), "[ADDRESS1]");

            return columns[DisplayName.ToLower()];


        }
        public string GetCustomerColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("CustomerNo".ToLower(), "IDCUST");
            columns.Add("CustomerName".ToLower(), "NAMECUST");
            columns.Add("Status".ToLower(), "SWACTV");
            columns.Add("OnHold".ToLower(), "SWHOLD");
            columns.Add("GroupCode".ToLower(), "IDGRP");
            columns.Add("NationalAccount".ToLower(), "IDNATACCT");
            columns.Add("Contract".ToLower(), "TEXTPHON1");
            columns.Add("CustomerDescription".ToLower(), "NAMECUST");

            return columns[DisplayName.ToLower()];


        }
        public string GetBatchListColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("BATCHID".ToLower(), "BATCHID");
            columns.Add("Description".ToLower(), "BTCHDESC");
            columns.Add("SourceLedger".ToLower(), "SRCELEDGR");
            columns.Add("Type".ToLower(), "BATCHTYPE");
            columns.Add("Status".ToLower(), "BATCHSTAT");
            columns.Add("NoOfEntries".ToLower(), "ENTRYCNT");


            return columns[DisplayName.ToLower()];


        }
        public string GetInvoiceBatchListColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("BatchNumber".ToLower(), "CNTBTCH");
            columns.Add("Description".ToLower(), "BTCHDESC");
            columns.Add("BatchDate".ToLower(), "DATEBTCH");

            columns.Add("Type".ToLower(), "BTCHTYPE");
            columns.Add("BatchStatus".ToLower(), "BTCHSTTS");
            


            return columns[DisplayName.ToLower()];


        }



        public string GetEntryColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();

            columns.Add("BatchNumber".ToLower(), "BATCHID");
            columns.Add("EntryNumber".ToLower(), "BTCHENTRY");
            columns.Add("Description".ToLower(), "JRNLDESC");
            
            columns.Add("SourceLedger".ToLower(), "SRCELEDGER");
            return columns[DisplayName.ToLower()];


        }


        public string GetBatchColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();


            columns.Add("BatchNumber".ToLower(), "BATCHID");
            columns.Add("Description".ToLower(), "BTCHDESC");
            columns.Add("Status".ToLower(), "Status");
            columns.Add("Type".ToLower(), "Type");
            return columns[DisplayName.ToLower()];


        }
        public string GetUserName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();


            columns.Add("userName".ToLower(), "USERID");
            
            return columns[DisplayName.ToLower()];


        }


        public string GetSourceCodeColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();


            columns.Add("SourceLedger".ToLower(), "SRCELEDGER");
            columns.Add("Description".ToLower(), "SRCEDESC");
            columns.Add("SourceType".ToLower(), "SRCETYPE");
            
            return columns[DisplayName.ToLower()];


        }

        public string GetAccountSetColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();


            columns.Add("AccountSetCode".ToLower(), "ACCTSET");
            columns.Add("Description".ToLower(), "TEXTDESC");
            columns.Add("CurrencyCode".ToLower(), "CURRCODE");
            
            return columns[DisplayName.ToLower()];


        }
        
        public string GetRemiToColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("VendorId".ToLower(), "IDVEND");
            columns.Add("RemitToLocation".ToLower(), "IDVENDRMIT");
            columns.Add("AddressLine1".ToLower(), "TEXTSTRE1");
            columns.Add("Description".ToLower(), "RMITNAME");
            columns.Add("AddressLine2".ToLower(), "TEXTSTRE2");
            columns.Add("AddressLine3".ToLower(), "TEXTSTRE3");
            columns.Add("City".ToLower(), "NAMECITY");
            return columns[DisplayName.ToLower()];


        }
        public string GetPaymentCodeColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("PaymentCode".ToLower(), "PAYMCODE");
            columns.Add("Description".ToLower(), "TEXTDESC");
           

            return columns[DisplayName.ToLower()];


        }
        public string GetBatchCodeColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("BatchNumber".ToLower(), "CNTBTCH");
            columns.Add("Description".ToLower(), "BATCHDESC");
            columns.Add("BatchDate".ToLower(), "DATEBTCH");


            return columns[DisplayName.ToLower()];


        }
        public string GetBankColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("BankCode".ToLower(), "BANK");
            columns.Add("Description".ToLower(), "Name");
            columns.Add("BankEntryNo".ToLower(), "BKACCT");
            columns.Add("Currency".ToLower(), "CURNSTMT");


            return columns[DisplayName.ToLower()];


        }
        public string GetReiptColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("Number".ToLower(), "RCPNUMBER");
            columns.Add("Vendor".ToLower(), "VDCODE");
            columns.Add("Name".ToLower(), "VDNAME");


            return columns[DisplayName.ToLower()];


        }
        public string GetBankEntryColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("BankEntryNumber".ToLower(), "ENTRYNBR");
            columns.Add("Reference".ToLower(), "EntryDescription");


            return columns[DisplayName.ToLower()];


        }
        public string GetCurrencyCodeColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("CurrencyCode".ToLower(), "CURID");
            columns.Add("CurrencyDescription".ToLower(), "CURNAME");


            return columns[DisplayName.ToLower()];


        }
        public string GetBatchInvoiceColumnName(string DisplayName)
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("BatchNumber".ToLower(), "CNTBTCH");
            columns.Add("Description".ToLower(), "BATCHDESC");
            


            return columns[DisplayName.ToLower()];


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

		public bool CheckUnPostStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
			try
			{
				bool isUnPost = false;
				bool Post = false;

				DataTable dt = new DataTable();


				string sqlText = "select IsApprovedL1  from " + tableName + " where 1=1 and IsApprovedL1 = 1";

				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

				SqlCommand command = CreateCommand(sqlText);

				command = ApplyParameters(command, conditionalFields, conditionalValue);

				SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
				dataAdapter.Fill(dt);

				if (dt.Rows.Count > 0)
				{

					//return (Post == "Y");
					isUnPost = true;
				}


				return isUnPost;
			}
			catch (Exception e)
			{
				throw e;
			}
		}


		public bool CheckPushStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            try
            {
                bool ÌsPush=false;
                string Push="";

                DataTable dt = new DataTable();

                // ToDo sql injection
                string sqlText = "select IsPush  from " + tableName + " where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);



                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string push = dt.Rows[0]["IsPush"].ToString();
                    return (push == "Y");
                }


                return ÌsPush ;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}


   
