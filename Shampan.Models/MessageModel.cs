namespace Shampan.Models;

public class MessageModel
{
    public static string InsertSuccess { get; set; } = "Saved Successfully";
    public static string DbUpdateSuccess { get; set; } = "DbUpdated Successfully";
    public static string DbUpdateFail { get; set; } = "DbUpdate Fail";
    public static string UpdateSuccess { get; set; } = "Updated Successfully";
    public static string DeleteSuccess { get; set; } = "Data Deleted Successfully";
    public static string DeActivateSuccess { get; set; } = "Data DeActivate Successfully";
    public static string PushSuccess { get; set; } = "Data Pushed Successfully";
    public static string PostSuccess { get; set; } = "Data Posted Successfully";
    public static string UnPostSuccess { get; set; } = "Data UnPosted Successfully";


    public static string ExcelSuccess { get; set; } = "Excel file generated successfully.";
    public static string ExcelFail { get; set; } = "Failed to generate Excel file.";


    public static string InsertFail { get; set; } = "Insert Failed";
    public static string UpdateFail { get; set; } = "Update Failed";
    public static string DeleteFail { get; set; } = "Delete Failed";
    public static string PostFail { get; set; } = "Data Posted Failed";
    public static string UnPostFail { get; set; } = "Data UnPosted Failed";
    public static string PushFail { get; set; } = "Data Pushed Failed";



    public static string MasterInsertFailed { get; set; } = "Master Insert Failed";
    public static string DetailInsertFailed { get; set; } = "Details Insert Failed";
    public static string NotFoundForSave { get; set; } = "Data Not Found for Save";
    public static string NotFoundForUpdate { get; set; } = "Data Not Found for Update";
    public static string DataLoaded { get; set; } = "Data Loaded Successfully";
    public static string DataLoadedFailed { get; set; } = "Data Load Failed";
    public static string DetailsNotFoundForSave { get; set; } = " Details Data Not Found for Save";

    public static string AlreadyExists { get; set; } = "Already Exists";


    public static string PostAlready { get; set; } = "Data has Already been Posted";
    public static string UserSavedAlready { get; set; } = "User was saved Already";
    public static string UpPostAlready { get; set; } = "Data is Already In Approve Process.You Do Not Have Premission to Unpost Data";
    public static string PushAlready { get; set; } = "Data Already Pushed";
    public static string NotPost { get; set; } = "Please Data Post First!";
    public static string Permission { get; set; } = "You Do Not Have Permission to change data";



}


public class TableName
{
    #region Old

    public static string ExpIssueDetails { get; set; } = "ExpIssueDetails";
    public static string POInvoiceDetails { get; set; } = "POInvoiceDetails";

    public static string GLJournalDetails { get; set; } = "GLJournalDetails";
    public static string GLBATCH { get; set; } = "GLJournalBatchs";
    public static string APBATCH { get; set; } = "APPaymentBatchs";
    public static string APPBatchs { get; set; } = "APPaymentBatchs";

    public static string BankEntryDetails { get; set; } = "BkEntryDetails";
    public static string CSColors { get; set; } = "CSColors";
    public static string AccountCodes { get; set; } = "GLAMF";
    public static string RemitLoLocation { get; set; } = "APVNR";
    public static string UserName{ get; set; } = "CSAUTH";
    public static string Bank { get; set; } = "BKACCT";
    public static string Entry { get; set; } = "BKENTH";
    public static string ReiptNo { get; set; } = "PORCPH1";
    public static string CurrencyCode { get; set; } = "BKCUR";
    public static string VendorName { get; set; } = "APVEN";
    public static string ItemName { get; set; } = "ICITEM";
    public static string Location { get; set; } = "ICILOC";
    public static string Customer { get; set; } = "ARCUS";
    public static string Price { get; set; } = "ICPCOD";
    public static string ShipToLocation { get; set; } = "ICLOC";
    public static string ShipVia { get; set; } = "POVIA";
    public static string TermsCode { get; set; } = "APRTA";
    public static string VendorAccountSet { get; set; } = "APRAS";
    public static string GLBatch { get; set; } = "GLBCTL";


    //public static string GLJournal{ get; set; } = "GLJournal";
    //public static string BankEntry { get; set; } = "BankEntrys";

    public static string GLJournal { get; set; } = "GLJournals";
    public static string BankEntry { get; set; } = "BkEntries";

    public static string BranchProfile { get; set; } = "BranchProfiles";
    public static string POInvoice { get; set; } = "POInvoices";
    public static string Template { get; set; } = "POPLAT";
    public static string CompanyInfo { get; set; } = "CompanyInfo";

    public static string APPayments { get; set; } = "APPayments";

    public static string BkTransfers { get; set; } = "BkTransfers";

    public static string SourceLedger { get; set; } = "GLSRCE";
    public static string TestHeaders { get; set; } = "TestHeader";

    //public static string InvoiceBatch { get; set; } = "CNTBTCH";
    public static string InvoiceBatch { get; set; } = "APIBC";



    public static string ICReceiptDetails { get; set; } = "ICReceiptDetails";
    public static string APPaymentDetails { get; set; } = "APPaymentDetails";
    public static string APMiscPaymentDetails { get; set; } = "APMiscPaymentDetails";
    public static string APMiscPayments { get; set; } = "APMiscPayments";
    public static string ICReceipts { get; set; } = "ICReceipts";
    public static string PORequesitions { get; set; } = "PORequesitions";
    public static string ICShipments { get; set; } = "ICShipments";
    public static string ICTransfers { get; set; } = "ICTransfers";
    public static string POPurchaseOrders { get; set; } = "POPurchaseOrders";
    public static string POReceipts { get; set; } = "POReceipts";
    public static string ICTransferDetails { get; set; } = "ICTransferDetails";
    public static string ICShipmentDetails { get; set; } = "ICShipmentDetails";
    public static string POPurchaseOrderDetails { get; set; } = "POPurchaseOrderDetails";
    public static string POReceiptDetails { get; set; } = "POReceiptDetails";
    public static string PORequesitionDetails { get; set; } = "PORequesitionDetails";

    public static string APInvoiceDetails { get; set; } = "APInvoiceDetails";
    public static string APInvoice { get; set; } = "APInvoices";
    public static string APInvoiceBatch { get; set; } = "CNTBTCH";

    public static string PaymentBatch { get; set; } = "APBTA";

    public static string UserBranch { get; set; } = "UserBranchMap";
    public static string TeamMember { get; set; } = "A_TeamMembers";
    public static string Teams { get; set; } = "A_Teams";
    public static string Advances { get; set; } = "Advances";
    public static string NodePermission { get; set; } = "NodePermission";
    public static string Users { get; set; } = "UsersPermission";
    public static string MRWiseChangeLog { get; set; } = "MRWiseChangeLog";
    public static string TADABill { get; set; } = "TADABill";
    public static string TransportAllownaceDetails { get; set; } = "TransportAllownaceDetails";
    public static string TransportAllownaceOthers { get; set; } = "TransportAllownaceOthers";
    public static string TransportAllownaceLessAdvance { get; set; } = "TransportAllownaceLessAdvance";
    public static string DateWisePolicyEditLog { get; set; } = "DateWisePolicyEditLog";
    public static string DocumentWiseEditLogDetails { get; set; } = "DocumentWiseEditLogDetails";
    public static string CollectionEditLog { get; set; } = "CollectionEditLog";
    public static string DateWisePolicyEditLogWithDetails { get; set; } = "DateWisePolicyEditLogWithDetails";
    public static string Financial { get; set; } = "Financial";
    public static string NonFinancial { get; set; } = "NonFinancial";
    public static string Tours { get; set; } = "Tours";
    public static string Module { get; set; } = "TBLModul";
    public static string ModulPermission { get; set; } = "ModulPermission";
    public static string TransportAllownaces { get; set; } = "TransportAllownaces";
    public static string UserRolls { get; set; } = "UserRolls";
    public static string Circulars { get; set; } = "Circulars";
    public static string Calenders { get; set; } = "Calenders";
    public static string EmployeesHierarchy { get; set; } = "EmployeesHierarchy";

    #endregion


    public static string A_Audits { get; set; } = "A_Audits";
    public static string A_AuditAreas { get; set; } = "A_AuditAreas";
    public static string AuditUsers { get; set; } = "AuditUsers";
    public static string A_AuditIssueAttachments { get; set; } = "A_AuditIssueAttachments";
    public static string CircularAttachments { get; set; } = "CircularAttachments";
    public static string EmployeesHierarchyAttachments { get; set; } = "EmployeesHierarchyAttachments";
    public static string A_AuditFeedbackAttachments { get; set; } = "A_AuditFeedbackAttachments";
    public static string A_AuditBranchFeedbackAttachments { get; set; } = "A_AuditBranchFeedbackAttachments";
    public static string A_AuditIssues { get; set; } = "A_AuditIssues";
    public static string A_AuditBranchFeedbacks { get; set; } = "A_AuditBranchFeedbacks";
    public static string A_AuditFeedbacks { get; set; } = "A_AuditFeedbacks";



}

public  class ClaimNames
{
    public static string Database { get; set; } = "Database";
    public static string SageDatabase { get; set; } = "SageDatabase";
    public static string CurrentBranch { get; set; } = "CurrentBranch";
    public static string CurrentBranchName { get; set; } = "CurrentBranchName";

    //add
    public static string AuthDatabase { get; set; } = "AuthDatabase";
}

