
using Shampan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Core.Interfaces.Repository
{
    public interface IcommonBaseRepository<TModel> where TModel : class, new()
    {

        //SSlAudit
        

        List<TModel> TeamName();
        List<TModel> AuditName();
        List<TModel> ColorName();
        List<TModel> ReportingManagers();

        //EndAudit


        //bank

        List<TModel> DepositType();
        List<TModel> BankEntryType();
        //mychange

        //user_branch
        List<TModel> UserBranch();
        List<TModel> Branchs(string UserId);
        List<TModel> BranchName(string UserId);
        List<TModel> UserId();
        List<TModel> ModulId();
        List<TModel> NodeName();
        List<TModel> GetModuleName();
        List<TModel> GetAllHeaders();
        List<TModel> EntryTypes();
        List<TModel> DocumentType();
        List<TModel> APDocumentType();
        List<TModel> ProrationMethod();
        List<TModel> POType();


        List<TModel> ApplyMethod();
        List<TModel> OrderBy();
        List<TModel> TransactionType();
        List<TModel> GetAllProductType();
        List<TModel> GetAllStore();
        List<TModel> GetAllUom();
        List<TModel> GetAllColor();
        List<TModel> GetAllCurrencys();

        List<TModel> GetAllDepartment();
        List<TModel> GetAllUserName();
        List<TModel> GetAllSize();
        List<TModel> GetAllProduct();
        List<TModel> GetAllVendor();
        List<TModel> GetAllOrderCategories();

        List<TModel> GetAllCustomers();

        List<TModel> GetAllPorts();
        List<TModel> GetAllPaymentTerms();
        List<TModel> GetAllBanks();

        List<TModel> GetAllInsuranceCompanies();
        List<TModel> GetAllDeliveryTerms();
        List<TModel> GetAllShipmentModes();
        List<TModel> GetAllLCCategories();
        List<TModel> GetAllMasterLC();
        List<TModel> GetAllPI();
        List<TModel> GetAllExpOrder();
        List<TModel> GetAllBtbLC();
        List<TModel> GetAllExportPIContracts();
        List<TModel> GetAllPackingMode();
        List<TModel> GetAllExpInvoice();
        List<TModel> GetAllCountry();
        List<TModel> GetAllCnFAgents();
        List<TModel> GetAllCouriers();
        List<TModel> GetAllImportTypes();

        List<TModel> GetAllEmployees();

        //List<CommonDropDown> POType();
        //List<CommonDropDown> ApplyMethod();
        //List<CommonDropDown> TransactionType();

        List<TModel> GetAllProductCategories();
        #region "Autocomplete"

        List<TModel> AutocompleteProduct(string Prefix); 
        List<TModel> ProductIdByName(string Name);

        List<TModel> AutocompleteRequisitionNo(string Prefix);
        #endregion "Autocomplete"
    }
}
