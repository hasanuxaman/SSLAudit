using Shampan.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Core.Interfaces.Services
{
    public interface ICommonService
    {
        //IList<UserBranch> GetBranch();
        List<CommonDropDown>? GetIssues(string auditId);
        List<CommonDropDown>? GetBranchFeedbackIssues(string auditId,string userName,bool team);
        List<CommonDropDown>? GetAuditName();

        List<CommonDropDown>? GetIssuePriority();
        List<CommonDropDown>? GetIssueStatus();
        List<CommonDropDown>? GetStatus();
        List<CommonDropDown>? GetAuditTypes();
        //SSLAudit
        IList <CommonDropDown> TeamName();
        IList <CommonDropDown> AuditName();
        IList <CommonDropDown> ColorName();
        IList <CommonDropDown> ReportingManagers();


        //EndAudit
        IList <CommonDropDown> DepositType();
        IList<CommonDropDown> BankEntryType();
        //mychange
        IList<CommonDropDown> GetAllHeaders();
        IList<CommonDropDown> EntryTypes();
        IList<CommonDropDown> DocumentType();
        IList<CommonDropDown> APDocumentType();
        IList<CommonDropDown> ProrationMethod();
        IList<CommonDropDown> POType();
        IList<CommonDropDown> UserBranch();
        IList<CommonDropDown> UserId();
        IList<CommonDropDown> ModulId();
        IList<CommonDropDown> NodeName();
        IList<CommonDropDown> GetModuleName();
        List<CommonDropDown> GetAllProductType();
        List<CommonDropDown> GetAllStore();
        List<CommonDropDown> GetAllUom();
        List<CommonDropDown> GetAllColor();
        List<CommonDropDown> GetAllCurrencys();
        List<CommonDropDown> GetAllDepartment();
        List<CommonDropDown> GetAllUserName();
        List<CommonDropDown> GetAllSize();
        List<CommonDropDown> GetAllProduct();
        List<CommonDropDown> GetAllVendor();
        List<CommonDropDown> GetAllOrderCategories();

        List<CommonDropDown> GetAllCustomers();
        List<CommonDropDown> GetAllPorts();

        List<CommonDropDown> GetAllBanks();

        List<CommonDropDown> GetAllPaymentTerms();

        List<CommonDropDown> GetAllDeliveryTerms();
        List<CommonDropDown> GetAllInsuranceCompanies();
        List<CommonDropDown> GetAllShipmentModes();
        List<CommonDropDown> GetAllLCCategories();
        List<CommonDropDown> GetAllMasterLC();
        List<CommonDropDown> GetAllPI();
        List<CommonDropDown> GetAllExpOrder();
        List<CommonDropDown> GetAllBtbLC();
        List<CommonDropDown> GetAllExportPIContracts();
        List<CommonDropDown> GetAllPackingMode();
        List<CommonDropDown> GetAllExpInvoice();
        List<CommonDropDown> GetAllCountry();   
        List<CommonDropDown> GetAllCnFAgents();
        List<CommonDropDown> GetAllCouriers();
        List<CommonDropDown> GetAllImportTypes();

        List<CommonDropDown> GetAllEmployees();
        List<CommonDropDown> GetAllProductCategories();
        List<CommonDropDown> ApplyMethod();
        List<CommonDropDown> TransactionType();
    
        List<CommonDropDown> OrderBy();
        List<CommonDropDown> Branchs(string UserId);
        List<CommonDropDown> BranchName(string UserId);





        List<CommonDropDown>? GetAuditType(bool isPlanned = true);
        List<CommonDropDown>? GetTeams();
        List<CommonDropDown>? GetReportStatus();
        List<CommonDropDown>? GetCircularType();
        List<CommonDropDown>? GetAuditStatus();

        #region "Autocomplete"
        List<CommonDropDown> AutocompleteProduct(string Prefix);
        List<CommonDropDown> ProductIdByName(string Name);
        List<CommonDropDown> AutocompleteRequisitionNo(string Prefix);
        

        #endregion "Autocomplete"

    }
}
