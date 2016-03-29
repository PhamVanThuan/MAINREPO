using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.EventProjection
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string currentstateforinstancedatamodel_selectwhere = "SELECT Id, StateChangeDate, InstanceId, StateName, WorkflowName, GenericKeyTypeKey, GenericKey, DaysInState FROM [EventProjection].[projection].[CurrentStateForInstance] WHERE";
        public const string currentstateforinstancedatamodel_selectbykey = "SELECT Id, StateChangeDate, InstanceId, StateName, WorkflowName, GenericKeyTypeKey, GenericKey, DaysInState FROM [EventProjection].[projection].[CurrentStateForInstance] WHERE Id = @PrimaryKey";
        public const string currentstateforinstancedatamodel_delete = "DELETE FROM [EventProjection].[projection].[CurrentStateForInstance] WHERE Id = @PrimaryKey";
        public const string currentstateforinstancedatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[CurrentStateForInstance] WHERE";
        public const string currentstateforinstancedatamodel_insert = "INSERT INTO [EventProjection].[projection].[CurrentStateForInstance] (StateChangeDate, InstanceId, StateName, WorkflowName, GenericKeyTypeKey, GenericKey) VALUES(@StateChangeDate, @InstanceId, @StateName, @WorkflowName, @GenericKeyTypeKey, @GenericKey); select cast(scope_identity() as int)";
        public const string currentstateforinstancedatamodel_update = "UPDATE [EventProjection].[projection].[CurrentStateForInstance] SET StateChangeDate = @StateChangeDate, InstanceId = @InstanceId, StateName = @StateName, WorkflowName = @WorkflowName, GenericKeyTypeKey = @GenericKeyTypeKey, GenericKey = @GenericKey WHERE Id = @Id";



        public const string hasbeenincompany2datamodel_selectwhere = "SELECT AccountKey FROM [EventProjection].[projection].[HasBeenInCompany2] WHERE";
        public const string hasbeenincompany2datamodel_selectbykey = "SELECT AccountKey FROM [EventProjection].[projection].[HasBeenInCompany2] WHERE AccountKey = @PrimaryKey";
        public const string hasbeenincompany2datamodel_delete = "DELETE FROM [EventProjection].[projection].[HasBeenInCompany2] WHERE AccountKey = @PrimaryKey";
        public const string hasbeenincompany2datamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[HasBeenInCompany2] WHERE";
        public const string hasbeenincompany2datamodel_insert = "INSERT INTO [EventProjection].[projection].[HasBeenInCompany2] (AccountKey) VALUES(@AccountKey); ";
        public const string hasbeenincompany2datamodel_update = "UPDATE [EventProjection].[projection].[HasBeenInCompany2] SET AccountKey = @AccountKey WHERE AccountKey = @AccountKey";



        public const string dcinstalmentupdatedatamodel_selectwhere = "SELECT PK, AccountKey FROM [EventProjection].[projection].[DCInstalmentUpdate] WHERE";
        public const string dcinstalmentupdatedatamodel_selectbykey = "SELECT PK, AccountKey FROM [EventProjection].[projection].[DCInstalmentUpdate] WHERE PK = @PrimaryKey";
        public const string dcinstalmentupdatedatamodel_delete = "DELETE FROM [EventProjection].[projection].[DCInstalmentUpdate] WHERE PK = @PrimaryKey";
        public const string dcinstalmentupdatedatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[DCInstalmentUpdate] WHERE";
        public const string dcinstalmentupdatedatamodel_insert = "INSERT INTO [EventProjection].[projection].[DCInstalmentUpdate] (AccountKey) VALUES(@AccountKey); select cast(scope_identity() as int)";
        public const string dcinstalmentupdatedatamodel_update = "UPDATE [EventProjection].[projection].[DCInstalmentUpdate] SET AccountKey = @AccountKey WHERE PK = @PK";



        public const string capitalizeinterestdatamodel_selectwhere = "SELECT PK, FinancialServiceKey, Amount, EventEffectiveDate FROM [EventProjection].[projection].[CapitalizeInterest] WHERE";
        public const string capitalizeinterestdatamodel_selectbykey = "SELECT PK, FinancialServiceKey, Amount, EventEffectiveDate FROM [EventProjection].[projection].[CapitalizeInterest] WHERE PK = @PrimaryKey";
        public const string capitalizeinterestdatamodel_delete = "DELETE FROM [EventProjection].[projection].[CapitalizeInterest] WHERE PK = @PrimaryKey";
        public const string capitalizeinterestdatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[CapitalizeInterest] WHERE";
        public const string capitalizeinterestdatamodel_insert = "INSERT INTO [EventProjection].[projection].[CapitalizeInterest] (FinancialServiceKey, Amount, EventEffectiveDate) VALUES(@FinancialServiceKey, @Amount, @EventEffectiveDate); select cast(scope_identity() as int)";
        public const string capitalizeinterestdatamodel_update = "UPDATE [EventProjection].[projection].[CapitalizeInterest] SET FinancialServiceKey = @FinancialServiceKey, Amount = @Amount, EventEffectiveDate = @EventEffectiveDate WHERE PK = @PK";



        public const string correspondencedatamodel_selectwhere = "SELECT Id, CorrespondenceType, CorrespondenceReason, CorrespondenceMedium, Date, UserName, MemoText, GenericKey, GenericKeyTypeKey FROM [EventProjection].[projection].[Correspondence] WHERE";
        public const string correspondencedatamodel_selectbykey = "SELECT Id, CorrespondenceType, CorrespondenceReason, CorrespondenceMedium, Date, UserName, MemoText, GenericKey, GenericKeyTypeKey FROM [EventProjection].[projection].[Correspondence] WHERE Id = @PrimaryKey";
        public const string correspondencedatamodel_delete = "DELETE FROM [EventProjection].[projection].[Correspondence] WHERE Id = @PrimaryKey";
        public const string correspondencedatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[Correspondence] WHERE";
        public const string correspondencedatamodel_insert = "INSERT INTO [EventProjection].[projection].[Correspondence] (CorrespondenceType, CorrespondenceReason, CorrespondenceMedium, Date, UserName, MemoText, GenericKey, GenericKeyTypeKey) VALUES(@CorrespondenceType, @CorrespondenceReason, @CorrespondenceMedium, @Date, @UserName, @MemoText, @GenericKey, @GenericKeyTypeKey); select cast(scope_identity() as int)";
        public const string correspondencedatamodel_update = "UPDATE [EventProjection].[projection].[Correspondence] SET CorrespondenceType = @CorrespondenceType, CorrespondenceReason = @CorrespondenceReason, CorrespondenceMedium = @CorrespondenceMedium, Date = @Date, UserName = @UserName, MemoText = @MemoText, GenericKey = @GenericKey, GenericKeyTypeKey = @GenericKeyTypeKey WHERE Id = @Id";



        public const string attorneyinvoicemonthlybreakdowndatamodel_selectwhere = "SELECT AttorneyId, AttorneyName, Capitalised, PaidBySPV, DebtReview, Total, AvgRValuePerInvoice, AvgRValuePerAccount, Paid, Rejected, Unprocessed, Processed, AccountsPaid FROM [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown] WHERE";
        public const string attorneyinvoicemonthlybreakdowndatamodel_selectbykey = "SELECT AttorneyId, AttorneyName, Capitalised, PaidBySPV, DebtReview, Total, AvgRValuePerInvoice, AvgRValuePerAccount, Paid, Rejected, Unprocessed, Processed, AccountsPaid FROM [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown] WHERE  = @PrimaryKey";
        public const string attorneyinvoicemonthlybreakdowndatamodel_delete = "DELETE FROM [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown] WHERE  = @PrimaryKey";
        public const string attorneyinvoicemonthlybreakdowndatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown] WHERE";
        public const string attorneyinvoicemonthlybreakdowndatamodel_insert = "INSERT INTO [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown] (AttorneyId, AttorneyName, Capitalised, PaidBySPV, DebtReview, Paid, Rejected, Unprocessed, Processed, AccountsPaid) VALUES(@AttorneyId, @AttorneyName, @Capitalised, @PaidBySPV, @DebtReview, @Paid, @Rejected, @Unprocessed, @Processed, @AccountsPaid); ";
        public const string attorneyinvoicemonthlybreakdowndatamodel_update = "UPDATE [EventProjection].[projection].[AttorneyInvoiceMonthlyBreakdown] SET AttorneyId = @AttorneyId, AttorneyName = @AttorneyName, Capitalised = @Capitalised, PaidBySPV = @PaidBySPV, DebtReview = @DebtReview, Paid = @Paid, Rejected = @Rejected, Unprocessed = @Unprocessed, Processed = @Processed, AccountsPaid = @AccountsPaid WHERE  = @";



        public const string calibreempiricadatamodel_selectwhere = "SELECT PK, AccountKey, Empirica FROM [EventProjection].[projection].[CalibreEmpirica] WHERE";
        public const string calibreempiricadatamodel_selectbykey = "SELECT PK, AccountKey, Empirica FROM [EventProjection].[projection].[CalibreEmpirica] WHERE PK = @PrimaryKey";
        public const string calibreempiricadatamodel_delete = "DELETE FROM [EventProjection].[projection].[CalibreEmpirica] WHERE PK = @PrimaryKey";
        public const string calibreempiricadatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[CalibreEmpirica] WHERE";
        public const string calibreempiricadatamodel_insert = "INSERT INTO [EventProjection].[projection].[CalibreEmpirica] (AccountKey, Empirica) VALUES(@AccountKey, @Empirica); select cast(scope_identity() as int)";
        public const string calibreempiricadatamodel_update = "UPDATE [EventProjection].[projection].[CalibreEmpirica] SET AccountKey = @AccountKey, Empirica = @Empirica WHERE PK = @PK";



        public const string accountspaidforattorneyinvoicesmonthlydatamodel_selectwhere = "SELECT AttorneyId, ThirdPartyInvoiceKey, AccountKey FROM [EventProjection].[projection].[AccountsPaidForAttorneyInvoicesMonthly] WHERE";
        public const string accountspaidforattorneyinvoicesmonthlydatamodel_selectbykey = "SELECT AttorneyId, ThirdPartyInvoiceKey, AccountKey FROM [EventProjection].[projection].[AccountsPaidForAttorneyInvoicesMonthly] WHERE  = @PrimaryKey";
        public const string accountspaidforattorneyinvoicesmonthlydatamodel_delete = "DELETE FROM [EventProjection].[projection].[AccountsPaidForAttorneyInvoicesMonthly] WHERE  = @PrimaryKey";
        public const string accountspaidforattorneyinvoicesmonthlydatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[AccountsPaidForAttorneyInvoicesMonthly] WHERE";
        public const string accountspaidforattorneyinvoicesmonthlydatamodel_insert = "INSERT INTO [EventProjection].[projection].[AccountsPaidForAttorneyInvoicesMonthly] (AttorneyId, ThirdPartyInvoiceKey, AccountKey) VALUES(@AttorneyId, @ThirdPartyInvoiceKey, @AccountKey); ";
        public const string accountspaidforattorneyinvoicesmonthlydatamodel_update = "UPDATE [EventProjection].[projection].[AccountsPaidForAttorneyInvoicesMonthly] SET AttorneyId = @AttorneyId, ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey, AccountKey = @AccountKey WHERE  = @";



        public const string attorneyinvoicespaidthismonthdatamodel_selectwhere = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth] WHERE";
        public const string attorneyinvoicespaidthismonthdatamodel_selectbykey = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth] WHERE  = @PrimaryKey";
        public const string attorneyinvoicespaidthismonthdatamodel_delete = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth] WHERE  = @PrimaryKey";
        public const string attorneyinvoicespaidthismonthdatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth] WHERE";
        public const string attorneyinvoicespaidthismonthdatamodel_insert = "INSERT INTO [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth] (Count, Value) VALUES(@Count, @Value); ";
        public const string attorneyinvoicespaidthismonthdatamodel_update = "UPDATE [EventProjection].[projection].[AttorneyInvoicesPaidThisMonth] SET Count = @Count, Value = @Value WHERE  = @";



        public const string attorneyinvoicespaidthisyeardatamodel_selectwhere = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesPaidThisYear] WHERE";
        public const string attorneyinvoicespaidthisyeardatamodel_selectbykey = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesPaidThisYear] WHERE  = @PrimaryKey";
        public const string attorneyinvoicespaidthisyeardatamodel_delete = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesPaidThisYear] WHERE  = @PrimaryKey";
        public const string attorneyinvoicespaidthisyeardatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesPaidThisYear] WHERE";
        public const string attorneyinvoicespaidthisyeardatamodel_insert = "INSERT INTO [EventProjection].[projection].[AttorneyInvoicesPaidThisYear] (Count, Value) VALUES(@Count, @Value); ";
        public const string attorneyinvoicespaidthisyeardatamodel_update = "UPDATE [EventProjection].[projection].[AttorneyInvoicesPaidThisYear] SET Count = @Count, Value = @Value WHERE  = @";



        public const string attorneyinvoicespaidlastmonthdatamodel_selectwhere = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesPaidLastMonth] WHERE";
        public const string attorneyinvoicespaidlastmonthdatamodel_selectbykey = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesPaidLastMonth] WHERE  = @PrimaryKey";
        public const string attorneyinvoicespaidlastmonthdatamodel_delete = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesPaidLastMonth] WHERE  = @PrimaryKey";
        public const string attorneyinvoicespaidlastmonthdatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesPaidLastMonth] WHERE";
        public const string attorneyinvoicespaidlastmonthdatamodel_insert = "INSERT INTO [EventProjection].[projection].[AttorneyInvoicesPaidLastMonth] (Count, Value) VALUES(@Count, @Value); ";
        public const string attorneyinvoicespaidlastmonthdatamodel_update = "UPDATE [EventProjection].[projection].[AttorneyInvoicesPaidLastMonth] SET Count = @Count, Value = @Value WHERE  = @";



        public const string attorneyinvoicesnotprocessedthismonthdatamodel_selectwhere = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisMonth] WHERE";
        public const string attorneyinvoicesnotprocessedthismonthdatamodel_selectbykey = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisMonth] WHERE  = @PrimaryKey";
        public const string attorneyinvoicesnotprocessedthismonthdatamodel_delete = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisMonth] WHERE  = @PrimaryKey";
        public const string attorneyinvoicesnotprocessedthismonthdatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisMonth] WHERE";
        public const string attorneyinvoicesnotprocessedthismonthdatamodel_insert = "INSERT INTO [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisMonth] (Count, Value) VALUES(@Count, @Value); ";
        public const string attorneyinvoicesnotprocessedthismonthdatamodel_update = "UPDATE [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisMonth] SET Count = @Count, Value = @Value WHERE  = @";



        public const string attorneyinvoicesnotprocessedlastmonthdatamodel_selectwhere = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedLastMonth] WHERE";
        public const string attorneyinvoicesnotprocessedlastmonthdatamodel_selectbykey = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedLastMonth] WHERE  = @PrimaryKey";
        public const string attorneyinvoicesnotprocessedlastmonthdatamodel_delete = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedLastMonth] WHERE  = @PrimaryKey";
        public const string attorneyinvoicesnotprocessedlastmonthdatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedLastMonth] WHERE";
        public const string attorneyinvoicesnotprocessedlastmonthdatamodel_insert = "INSERT INTO [EventProjection].[projection].[AttorneyInvoicesNotProcessedLastMonth] (Count, Value) VALUES(@Count, @Value); ";
        public const string attorneyinvoicesnotprocessedlastmonthdatamodel_update = "UPDATE [EventProjection].[projection].[AttorneyInvoicesNotProcessedLastMonth] SET Count = @Count, Value = @Value WHERE  = @";



        public const string attorneyinvoicesnotprocessedthisyeardatamodel_selectwhere = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisYear] WHERE";
        public const string attorneyinvoicesnotprocessedthisyeardatamodel_selectbykey = "SELECT Count, Value FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisYear] WHERE  = @PrimaryKey";
        public const string attorneyinvoicesnotprocessedthisyeardatamodel_delete = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisYear] WHERE  = @PrimaryKey";
        public const string attorneyinvoicesnotprocessedthisyeardatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisYear] WHERE";
        public const string attorneyinvoicesnotprocessedthisyeardatamodel_insert = "INSERT INTO [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisYear] (Count, Value) VALUES(@Count, @Value); ";
        public const string attorneyinvoicesnotprocessedthisyeardatamodel_update = "UPDATE [EventProjection].[projection].[AttorneyInvoicesNotProcessedThisYear] SET Count = @Count, Value = @Value WHERE  = @";



        public const string currentlyassigneduserforinstancedatamodel_selectwhere = "SELECT Id, LastUpdated, InstanceId, CapabilityKey, UserOrganisationStructureKey, GenericKeyTypeKey, GenericKey, UserName FROM [EventProjection].[projection].[CurrentlyAssignedUserForInstance] WHERE";
        public const string currentlyassigneduserforinstancedatamodel_selectbykey = "SELECT Id, LastUpdated, InstanceId, CapabilityKey, UserOrganisationStructureKey, GenericKeyTypeKey, GenericKey, UserName FROM [EventProjection].[projection].[CurrentlyAssignedUserForInstance] WHERE Id = @PrimaryKey";
        public const string currentlyassigneduserforinstancedatamodel_delete = "DELETE FROM [EventProjection].[projection].[CurrentlyAssignedUserForInstance] WHERE Id = @PrimaryKey";
        public const string currentlyassigneduserforinstancedatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[CurrentlyAssignedUserForInstance] WHERE";
        public const string currentlyassigneduserforinstancedatamodel_insert = "INSERT INTO [EventProjection].[projection].[CurrentlyAssignedUserForInstance] (LastUpdated, InstanceId, CapabilityKey, UserOrganisationStructureKey, GenericKeyTypeKey, GenericKey, UserName) VALUES(@LastUpdated, @InstanceId, @CapabilityKey, @UserOrganisationStructureKey, @GenericKeyTypeKey, @GenericKey, @UserName); select cast(scope_identity() as int)";
        public const string currentlyassigneduserforinstancedatamodel_update = "UPDATE [EventProjection].[projection].[CurrentlyAssignedUserForInstance] SET LastUpdated = @LastUpdated, InstanceId = @InstanceId, CapabilityKey = @CapabilityKey, UserOrganisationStructureKey = @UserOrganisationStructureKey, GenericKeyTypeKey = @GenericKeyTypeKey, GenericKey = @GenericKey, UserName = @UserName WHERE Id = @Id";



        public const string lastassigneduserbycapabilityforinstancedatamodel_selectwhere = "SELECT Id, LastUpdated, InstanceId, CapabilityKey, UserOrganisationStructureKey, GenericKeyTypeKey, GenericKey, UserName FROM [EventProjection].[projection].[LastAssignedUserByCapabilityForInstance] WHERE";
        public const string lastassigneduserbycapabilityforinstancedatamodel_selectbykey = "SELECT Id, LastUpdated, InstanceId, CapabilityKey, UserOrganisationStructureKey, GenericKeyTypeKey, GenericKey, UserName FROM [EventProjection].[projection].[LastAssignedUserByCapabilityForInstance] WHERE Id = @PrimaryKey";
        public const string lastassigneduserbycapabilityforinstancedatamodel_delete = "DELETE FROM [EventProjection].[projection].[LastAssignedUserByCapabilityForInstance] WHERE Id = @PrimaryKey";
        public const string lastassigneduserbycapabilityforinstancedatamodel_deletewhere = "DELETE FROM [EventProjection].[projection].[LastAssignedUserByCapabilityForInstance] WHERE";
        public const string lastassigneduserbycapabilityforinstancedatamodel_insert = "INSERT INTO [EventProjection].[projection].[LastAssignedUserByCapabilityForInstance] (LastUpdated, InstanceId, CapabilityKey, UserOrganisationStructureKey, GenericKeyTypeKey, GenericKey, UserName) VALUES(@LastUpdated, @InstanceId, @CapabilityKey, @UserOrganisationStructureKey, @GenericKeyTypeKey, @GenericKey, @UserName); select cast(scope_identity() as int)";
        public const string lastassigneduserbycapabilityforinstancedatamodel_update = "UPDATE [EventProjection].[projection].[LastAssignedUserByCapabilityForInstance] SET LastUpdated = @LastUpdated, InstanceId = @InstanceId, CapabilityKey = @CapabilityKey, UserOrganisationStructureKey = @UserOrganisationStructureKey, GenericKeyTypeKey = @GenericKeyTypeKey, GenericKey = @GenericKey, UserName = @UserName WHERE Id = @Id";



    }
}