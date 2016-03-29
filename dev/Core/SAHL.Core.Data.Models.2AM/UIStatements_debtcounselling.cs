using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models._2AM
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string debtcounsellingdatamodel_selectwhere = "SELECT DebtCounsellingKey, DebtCounsellingGroupKey, AccountKey, DebtCounsellingStatusKey, PaymentReceivedDate, PaymentReceivedAmount, DiaryDate, ReferenceNumber FROM [2am].[debtcounselling].[DebtCounselling] WHERE";
        public const string debtcounsellingdatamodel_selectbykey = "SELECT DebtCounsellingKey, DebtCounsellingGroupKey, AccountKey, DebtCounsellingStatusKey, PaymentReceivedDate, PaymentReceivedAmount, DiaryDate, ReferenceNumber FROM [2am].[debtcounselling].[DebtCounselling] WHERE DebtCounsellingKey = @PrimaryKey";
        public const string debtcounsellingdatamodel_delete = "DELETE FROM [2am].[debtcounselling].[DebtCounselling] WHERE DebtCounsellingKey = @PrimaryKey";
        public const string debtcounsellingdatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[DebtCounselling] WHERE";
        public const string debtcounsellingdatamodel_insert = "INSERT INTO [2am].[debtcounselling].[DebtCounselling] (DebtCounsellingGroupKey, AccountKey, DebtCounsellingStatusKey, PaymentReceivedDate, PaymentReceivedAmount, DiaryDate, ReferenceNumber) VALUES(@DebtCounsellingGroupKey, @AccountKey, @DebtCounsellingStatusKey, @PaymentReceivedDate, @PaymentReceivedAmount, @DiaryDate, @ReferenceNumber); select cast(scope_identity() as int)";
        public const string debtcounsellingdatamodel_update = "UPDATE [2am].[debtcounselling].[DebtCounselling] SET DebtCounsellingGroupKey = @DebtCounsellingGroupKey, AccountKey = @AccountKey, DebtCounsellingStatusKey = @DebtCounsellingStatusKey, PaymentReceivedDate = @PaymentReceivedDate, PaymentReceivedAmount = @PaymentReceivedAmount, DiaryDate = @DiaryDate, ReferenceNumber = @ReferenceNumber WHERE DebtCounsellingKey = @DebtCounsellingKey";



        public const string debtcounsellordetaildatamodel_selectwhere = "SELECT LegalEntityKey, NCRDCRegistrationNumber FROM [2am].[debtcounselling].[DebtCounsellorDetail] WHERE";
        public const string debtcounsellordetaildatamodel_selectbykey = "SELECT LegalEntityKey, NCRDCRegistrationNumber FROM [2am].[debtcounselling].[DebtCounsellorDetail] WHERE LegalEntityKey = @PrimaryKey";
        public const string debtcounsellordetaildatamodel_delete = "DELETE FROM [2am].[debtcounselling].[DebtCounsellorDetail] WHERE LegalEntityKey = @PrimaryKey";
        public const string debtcounsellordetaildatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[DebtCounsellorDetail] WHERE";
        public const string debtcounsellordetaildatamodel_insert = "INSERT INTO [2am].[debtcounselling].[DebtCounsellorDetail] (LegalEntityKey, NCRDCRegistrationNumber) VALUES(@LegalEntityKey, @NCRDCRegistrationNumber); ";
        public const string debtcounsellordetaildatamodel_update = "UPDATE [2am].[debtcounselling].[DebtCounsellorDetail] SET LegalEntityKey = @LegalEntityKey, NCRDCRegistrationNumber = @NCRDCRegistrationNumber WHERE LegalEntityKey = @LegalEntityKey";



        public const string debtcounsellinggroupdatamodel_selectwhere = "SELECT DebtCounsellingGroupKey, CreatedDate FROM [2am].[debtcounselling].[DebtCounsellingGroup] WHERE";
        public const string debtcounsellinggroupdatamodel_selectbykey = "SELECT DebtCounsellingGroupKey, CreatedDate FROM [2am].[debtcounselling].[DebtCounsellingGroup] WHERE DebtCounsellingGroupKey = @PrimaryKey";
        public const string debtcounsellinggroupdatamodel_delete = "DELETE FROM [2am].[debtcounselling].[DebtCounsellingGroup] WHERE DebtCounsellingGroupKey = @PrimaryKey";
        public const string debtcounsellinggroupdatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[DebtCounsellingGroup] WHERE";
        public const string debtcounsellinggroupdatamodel_insert = "INSERT INTO [2am].[debtcounselling].[DebtCounsellingGroup] (CreatedDate) VALUES(@CreatedDate); select cast(scope_identity() as int)";
        public const string debtcounsellinggroupdatamodel_update = "UPDATE [2am].[debtcounselling].[DebtCounsellingGroup] SET CreatedDate = @CreatedDate WHERE DebtCounsellingGroupKey = @DebtCounsellingGroupKey";



        public const string courttypedatamodel_selectwhere = "SELECT CourtTypeKey, Description FROM [2am].[debtcounselling].[CourtType] WHERE";
        public const string courttypedatamodel_selectbykey = "SELECT CourtTypeKey, Description FROM [2am].[debtcounselling].[CourtType] WHERE CourtTypeKey = @PrimaryKey";
        public const string courttypedatamodel_delete = "DELETE FROM [2am].[debtcounselling].[CourtType] WHERE CourtTypeKey = @PrimaryKey";
        public const string courttypedatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[CourtType] WHERE";
        public const string courttypedatamodel_insert = "INSERT INTO [2am].[debtcounselling].[CourtType] (CourtTypeKey, Description) VALUES(@CourtTypeKey, @Description); ";
        public const string courttypedatamodel_update = "UPDATE [2am].[debtcounselling].[CourtType] SET CourtTypeKey = @CourtTypeKey, Description = @Description WHERE CourtTypeKey = @CourtTypeKey";



        public const string courtdatamodel_selectwhere = "SELECT CourtKey, CourtTypeKey, ProvinceKey, Name, GeneralStatusKey FROM [2am].[debtcounselling].[Court] WHERE";
        public const string courtdatamodel_selectbykey = "SELECT CourtKey, CourtTypeKey, ProvinceKey, Name, GeneralStatusKey FROM [2am].[debtcounselling].[Court] WHERE CourtKey = @PrimaryKey";
        public const string courtdatamodel_delete = "DELETE FROM [2am].[debtcounselling].[Court] WHERE CourtKey = @PrimaryKey";
        public const string courtdatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[Court] WHERE";
        public const string courtdatamodel_insert = "INSERT INTO [2am].[debtcounselling].[Court] (CourtKey, CourtTypeKey, ProvinceKey, Name, GeneralStatusKey) VALUES(@CourtKey, @CourtTypeKey, @ProvinceKey, @Name, @GeneralStatusKey); ";
        public const string courtdatamodel_update = "UPDATE [2am].[debtcounselling].[Court] SET CourtKey = @CourtKey, CourtTypeKey = @CourtTypeKey, ProvinceKey = @ProvinceKey, Name = @Name, GeneralStatusKey = @GeneralStatusKey WHERE CourtKey = @CourtKey";



        public const string hearingtypedatamodel_selectwhere = "SELECT HearingTypeKey, Description FROM [2am].[debtcounselling].[HearingType] WHERE";
        public const string hearingtypedatamodel_selectbykey = "SELECT HearingTypeKey, Description FROM [2am].[debtcounselling].[HearingType] WHERE HearingTypeKey = @PrimaryKey";
        public const string hearingtypedatamodel_delete = "DELETE FROM [2am].[debtcounselling].[HearingType] WHERE HearingTypeKey = @PrimaryKey";
        public const string hearingtypedatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[HearingType] WHERE";
        public const string hearingtypedatamodel_insert = "INSERT INTO [2am].[debtcounselling].[HearingType] (HearingTypeKey, Description) VALUES(@HearingTypeKey, @Description); ";
        public const string hearingtypedatamodel_update = "UPDATE [2am].[debtcounselling].[HearingType] SET HearingTypeKey = @HearingTypeKey, Description = @Description WHERE HearingTypeKey = @HearingTypeKey";



        public const string hearingappearancetypedatamodel_selectwhere = "SELECT HearingAppearanceTypeKey, HearingTypeKey, Description FROM [2am].[debtcounselling].[HearingAppearanceType] WHERE";
        public const string hearingappearancetypedatamodel_selectbykey = "SELECT HearingAppearanceTypeKey, HearingTypeKey, Description FROM [2am].[debtcounselling].[HearingAppearanceType] WHERE HearingAppearanceTypeKey = @PrimaryKey";
        public const string hearingappearancetypedatamodel_delete = "DELETE FROM [2am].[debtcounselling].[HearingAppearanceType] WHERE HearingAppearanceTypeKey = @PrimaryKey";
        public const string hearingappearancetypedatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[HearingAppearanceType] WHERE";
        public const string hearingappearancetypedatamodel_insert = "INSERT INTO [2am].[debtcounselling].[HearingAppearanceType] (HearingAppearanceTypeKey, HearingTypeKey, Description) VALUES(@HearingAppearanceTypeKey, @HearingTypeKey, @Description); ";
        public const string hearingappearancetypedatamodel_update = "UPDATE [2am].[debtcounselling].[HearingAppearanceType] SET HearingAppearanceTypeKey = @HearingAppearanceTypeKey, HearingTypeKey = @HearingTypeKey, Description = @Description WHERE HearingAppearanceTypeKey = @HearingAppearanceTypeKey";



        public const string hearingdetaildatamodel_selectwhere = "SELECT HearingDetailKey, DebtCounsellingKey, HearingTypeKey, HearingAppearanceTypeKey, CourtKey, CaseNumber, HearingDate, GeneralStatusKey, Comment FROM [2am].[debtcounselling].[HearingDetail] WHERE";
        public const string hearingdetaildatamodel_selectbykey = "SELECT HearingDetailKey, DebtCounsellingKey, HearingTypeKey, HearingAppearanceTypeKey, CourtKey, CaseNumber, HearingDate, GeneralStatusKey, Comment FROM [2am].[debtcounselling].[HearingDetail] WHERE HearingDetailKey = @PrimaryKey";
        public const string hearingdetaildatamodel_delete = "DELETE FROM [2am].[debtcounselling].[HearingDetail] WHERE HearingDetailKey = @PrimaryKey";
        public const string hearingdetaildatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[HearingDetail] WHERE";
        public const string hearingdetaildatamodel_insert = "INSERT INTO [2am].[debtcounselling].[HearingDetail] (DebtCounsellingKey, HearingTypeKey, HearingAppearanceTypeKey, CourtKey, CaseNumber, HearingDate, GeneralStatusKey, Comment) VALUES(@DebtCounsellingKey, @HearingTypeKey, @HearingAppearanceTypeKey, @CourtKey, @CaseNumber, @HearingDate, @GeneralStatusKey, @Comment); select cast(scope_identity() as int)";
        public const string hearingdetaildatamodel_update = "UPDATE [2am].[debtcounselling].[HearingDetail] SET DebtCounsellingKey = @DebtCounsellingKey, HearingTypeKey = @HearingTypeKey, HearingAppearanceTypeKey = @HearingAppearanceTypeKey, CourtKey = @CourtKey, CaseNumber = @CaseNumber, HearingDate = @HearingDate, GeneralStatusKey = @GeneralStatusKey, Comment = @Comment WHERE HearingDetailKey = @HearingDetailKey";



        public const string snapshotfinancialadjustmentdatamodel_selectwhere = "SELECT SnapShotFinancialAdjustmentKey, SnapShotFinancialServiceKey, AccountKey, FinancialAdjustmentKey, FinancialServiceKey, FinancialAdjustmentSourceKey, FinancialAdjustmentTypeKey, FinancialAdjustmentStatusKey, FromDate, EndDate, CancellationDate, CancellationReasonKey, TransactionTypeKey, FRARate, IRAAdjustment, RPAReversalPercentage, DPADifferentialAdjustment, DPABalanceTypeKey, Amount FROM [2am].[debtcounselling].[SnapShotFinancialAdjustment] WHERE";
        public const string snapshotfinancialadjustmentdatamodel_selectbykey = "SELECT SnapShotFinancialAdjustmentKey, SnapShotFinancialServiceKey, AccountKey, FinancialAdjustmentKey, FinancialServiceKey, FinancialAdjustmentSourceKey, FinancialAdjustmentTypeKey, FinancialAdjustmentStatusKey, FromDate, EndDate, CancellationDate, CancellationReasonKey, TransactionTypeKey, FRARate, IRAAdjustment, RPAReversalPercentage, DPADifferentialAdjustment, DPABalanceTypeKey, Amount FROM [2am].[debtcounselling].[SnapShotFinancialAdjustment] WHERE SnapShotFinancialAdjustmentKey = @PrimaryKey";
        public const string snapshotfinancialadjustmentdatamodel_delete = "DELETE FROM [2am].[debtcounselling].[SnapShotFinancialAdjustment] WHERE SnapShotFinancialAdjustmentKey = @PrimaryKey";
        public const string snapshotfinancialadjustmentdatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[SnapShotFinancialAdjustment] WHERE";
        public const string snapshotfinancialadjustmentdatamodel_insert = "INSERT INTO [2am].[debtcounselling].[SnapShotFinancialAdjustment] (SnapShotFinancialServiceKey, AccountKey, FinancialAdjustmentKey, FinancialServiceKey, FinancialAdjustmentSourceKey, FinancialAdjustmentTypeKey, FinancialAdjustmentStatusKey, FromDate, EndDate, CancellationDate, CancellationReasonKey, TransactionTypeKey, FRARate, IRAAdjustment, RPAReversalPercentage, DPADifferentialAdjustment, DPABalanceTypeKey, Amount) VALUES(@SnapShotFinancialServiceKey, @AccountKey, @FinancialAdjustmentKey, @FinancialServiceKey, @FinancialAdjustmentSourceKey, @FinancialAdjustmentTypeKey, @FinancialAdjustmentStatusKey, @FromDate, @EndDate, @CancellationDate, @CancellationReasonKey, @TransactionTypeKey, @FRARate, @IRAAdjustment, @RPAReversalPercentage, @DPADifferentialAdjustment, @DPABalanceTypeKey, @Amount); select cast(scope_identity() as int)";
        public const string snapshotfinancialadjustmentdatamodel_update = "UPDATE [2am].[debtcounselling].[SnapShotFinancialAdjustment] SET SnapShotFinancialServiceKey = @SnapShotFinancialServiceKey, AccountKey = @AccountKey, FinancialAdjustmentKey = @FinancialAdjustmentKey, FinancialServiceKey = @FinancialServiceKey, FinancialAdjustmentSourceKey = @FinancialAdjustmentSourceKey, FinancialAdjustmentTypeKey = @FinancialAdjustmentTypeKey, FinancialAdjustmentStatusKey = @FinancialAdjustmentStatusKey, FromDate = @FromDate, EndDate = @EndDate, CancellationDate = @CancellationDate, CancellationReasonKey = @CancellationReasonKey, TransactionTypeKey = @TransactionTypeKey, FRARate = @FRARate, IRAAdjustment = @IRAAdjustment, RPAReversalPercentage = @RPAReversalPercentage, DPADifferentialAdjustment = @DPADifferentialAdjustment, DPABalanceTypeKey = @DPABalanceTypeKey, Amount = @Amount WHERE SnapShotFinancialAdjustmentKey = @SnapShotFinancialAdjustmentKey";



        public const string proposalstatusdatamodel_selectwhere = "SELECT ProposalStatusKey, Description FROM [2am].[debtcounselling].[ProposalStatus] WHERE";
        public const string proposalstatusdatamodel_selectbykey = "SELECT ProposalStatusKey, Description FROM [2am].[debtcounselling].[ProposalStatus] WHERE ProposalStatusKey = @PrimaryKey";
        public const string proposalstatusdatamodel_delete = "DELETE FROM [2am].[debtcounselling].[ProposalStatus] WHERE ProposalStatusKey = @PrimaryKey";
        public const string proposalstatusdatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[ProposalStatus] WHERE";
        public const string proposalstatusdatamodel_insert = "INSERT INTO [2am].[debtcounselling].[ProposalStatus] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string proposalstatusdatamodel_update = "UPDATE [2am].[debtcounselling].[ProposalStatus] SET Description = @Description WHERE ProposalStatusKey = @ProposalStatusKey";



        public const string proposaltypedatamodel_selectwhere = "SELECT ProposalTypeKey, Description FROM [2am].[debtcounselling].[ProposalType] WHERE";
        public const string proposaltypedatamodel_selectbykey = "SELECT ProposalTypeKey, Description FROM [2am].[debtcounselling].[ProposalType] WHERE ProposalTypeKey = @PrimaryKey";
        public const string proposaltypedatamodel_delete = "DELETE FROM [2am].[debtcounselling].[ProposalType] WHERE ProposalTypeKey = @PrimaryKey";
        public const string proposaltypedatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[ProposalType] WHERE";
        public const string proposaltypedatamodel_insert = "INSERT INTO [2am].[debtcounselling].[ProposalType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string proposaltypedatamodel_update = "UPDATE [2am].[debtcounselling].[ProposalType] SET Description = @Description WHERE ProposalTypeKey = @ProposalTypeKey";



        public const string debtcounsellingstatusdatamodel_selectwhere = "SELECT DebtCounsellingStatusKey, Description FROM [2am].[debtcounselling].[DebtCounsellingStatus] WHERE";
        public const string debtcounsellingstatusdatamodel_selectbykey = "SELECT DebtCounsellingStatusKey, Description FROM [2am].[debtcounselling].[DebtCounsellingStatus] WHERE DebtCounsellingStatusKey = @PrimaryKey";
        public const string debtcounsellingstatusdatamodel_delete = "DELETE FROM [2am].[debtcounselling].[DebtCounsellingStatus] WHERE DebtCounsellingStatusKey = @PrimaryKey";
        public const string debtcounsellingstatusdatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[DebtCounsellingStatus] WHERE";
        public const string debtcounsellingstatusdatamodel_insert = "INSERT INTO [2am].[debtcounselling].[DebtCounsellingStatus] (DebtCounsellingStatusKey, Description) VALUES(@DebtCounsellingStatusKey, @Description); ";
        public const string debtcounsellingstatusdatamodel_update = "UPDATE [2am].[debtcounselling].[DebtCounsellingStatus] SET DebtCounsellingStatusKey = @DebtCounsellingStatusKey, Description = @Description WHERE DebtCounsellingStatusKey = @DebtCounsellingStatusKey";



        public const string proposaldatamodel_selectwhere = "SELECT ProposalKey, ProposalTypeKey, ProposalStatusKey, DebtCounsellingKey, HOCInclusive, LifeInclusive, ADUserKey, CreateDate, ReviewDate, Accepted, MonthlyServiceFee FROM [2am].[debtcounselling].[Proposal] WHERE";
        public const string proposaldatamodel_selectbykey = "SELECT ProposalKey, ProposalTypeKey, ProposalStatusKey, DebtCounsellingKey, HOCInclusive, LifeInclusive, ADUserKey, CreateDate, ReviewDate, Accepted, MonthlyServiceFee FROM [2am].[debtcounselling].[Proposal] WHERE ProposalKey = @PrimaryKey";
        public const string proposaldatamodel_delete = "DELETE FROM [2am].[debtcounselling].[Proposal] WHERE ProposalKey = @PrimaryKey";
        public const string proposaldatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[Proposal] WHERE";
        public const string proposaldatamodel_insert = "INSERT INTO [2am].[debtcounselling].[Proposal] (ProposalTypeKey, ProposalStatusKey, DebtCounsellingKey, HOCInclusive, LifeInclusive, ADUserKey, CreateDate, ReviewDate, Accepted, MonthlyServiceFee) VALUES(@ProposalTypeKey, @ProposalStatusKey, @DebtCounsellingKey, @HOCInclusive, @LifeInclusive, @ADUserKey, @CreateDate, @ReviewDate, @Accepted, @MonthlyServiceFee); select cast(scope_identity() as int)";
        public const string proposaldatamodel_update = "UPDATE [2am].[debtcounselling].[Proposal] SET ProposalTypeKey = @ProposalTypeKey, ProposalStatusKey = @ProposalStatusKey, DebtCounsellingKey = @DebtCounsellingKey, HOCInclusive = @HOCInclusive, LifeInclusive = @LifeInclusive, ADUserKey = @ADUserKey, CreateDate = @CreateDate, ReviewDate = @ReviewDate, Accepted = @Accepted, MonthlyServiceFee = @MonthlyServiceFee WHERE ProposalKey = @ProposalKey";



        public const string proposalitemdatamodel_selectwhere = "SELECT ProposalItemKey, ProposalKey, StartDate, EndDate, MarketRateKey, InterestRate, Amount, AdditionalAmount, ADUserKey, CreateDate, InstalmentPercent, AnnualEscalation, StartPeriod, EndPeriod FROM [2am].[debtcounselling].[ProposalItem] WHERE";
        public const string proposalitemdatamodel_selectbykey = "SELECT ProposalItemKey, ProposalKey, StartDate, EndDate, MarketRateKey, InterestRate, Amount, AdditionalAmount, ADUserKey, CreateDate, InstalmentPercent, AnnualEscalation, StartPeriod, EndPeriod FROM [2am].[debtcounselling].[ProposalItem] WHERE ProposalItemKey = @PrimaryKey";
        public const string proposalitemdatamodel_delete = "DELETE FROM [2am].[debtcounselling].[ProposalItem] WHERE ProposalItemKey = @PrimaryKey";
        public const string proposalitemdatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[ProposalItem] WHERE";
        public const string proposalitemdatamodel_insert = "INSERT INTO [2am].[debtcounselling].[ProposalItem] (ProposalKey, StartDate, EndDate, MarketRateKey, InterestRate, Amount, AdditionalAmount, ADUserKey, CreateDate, InstalmentPercent, AnnualEscalation, StartPeriod, EndPeriod) VALUES(@ProposalKey, @StartDate, @EndDate, @MarketRateKey, @InterestRate, @Amount, @AdditionalAmount, @ADUserKey, @CreateDate, @InstalmentPercent, @AnnualEscalation, @StartPeriod, @EndPeriod); select cast(scope_identity() as int)";
        public const string proposalitemdatamodel_update = "UPDATE [2am].[debtcounselling].[ProposalItem] SET ProposalKey = @ProposalKey, StartDate = @StartDate, EndDate = @EndDate, MarketRateKey = @MarketRateKey, InterestRate = @InterestRate, Amount = @Amount, AdditionalAmount = @AdditionalAmount, ADUserKey = @ADUserKey, CreateDate = @CreateDate, InstalmentPercent = @InstalmentPercent, AnnualEscalation = @AnnualEscalation, StartPeriod = @StartPeriod, EndPeriod = @EndPeriod WHERE ProposalItemKey = @ProposalItemKey";



        public const string snapshotaccountdatamodel_selectwhere = "SELECT SnapShotAccountKey, AccountKey, RemainingInstallments, ProductKey, ValuationKey, InsertDate, DebtCounsellingKey, HOCPremium, LifePremium, MonthlyServiceFee FROM [2am].[debtcounselling].[SnapShotAccount] WHERE";
        public const string snapshotaccountdatamodel_selectbykey = "SELECT SnapShotAccountKey, AccountKey, RemainingInstallments, ProductKey, ValuationKey, InsertDate, DebtCounsellingKey, HOCPremium, LifePremium, MonthlyServiceFee FROM [2am].[debtcounselling].[SnapShotAccount] WHERE SnapShotAccountKey = @PrimaryKey";
        public const string snapshotaccountdatamodel_delete = "DELETE FROM [2am].[debtcounselling].[SnapShotAccount] WHERE SnapShotAccountKey = @PrimaryKey";
        public const string snapshotaccountdatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[SnapShotAccount] WHERE";
        public const string snapshotaccountdatamodel_insert = "INSERT INTO [2am].[debtcounselling].[SnapShotAccount] (AccountKey, RemainingInstallments, ProductKey, ValuationKey, InsertDate, DebtCounsellingKey, HOCPremium, LifePremium, MonthlyServiceFee) VALUES(@AccountKey, @RemainingInstallments, @ProductKey, @ValuationKey, @InsertDate, @DebtCounsellingKey, @HOCPremium, @LifePremium, @MonthlyServiceFee); select cast(scope_identity() as int)";
        public const string snapshotaccountdatamodel_update = "UPDATE [2am].[debtcounselling].[SnapShotAccount] SET AccountKey = @AccountKey, RemainingInstallments = @RemainingInstallments, ProductKey = @ProductKey, ValuationKey = @ValuationKey, InsertDate = @InsertDate, DebtCounsellingKey = @DebtCounsellingKey, HOCPremium = @HOCPremium, LifePremium = @LifePremium, MonthlyServiceFee = @MonthlyServiceFee WHERE SnapShotAccountKey = @SnapShotAccountKey";



        public const string snapshotfinancialservicedatamodel_selectwhere = "SELECT SnapShotFinancialServiceKey, SnapShotAccountKey, AccountKey, FinancialServiceKey, FinancialServiceTypeKey, ResetConfigurationKey, ActiveMarketRate, MarginKey, Installment, ParentFinancialServiceKey FROM [2am].[debtcounselling].[SnapShotFinancialService] WHERE";
        public const string snapshotfinancialservicedatamodel_selectbykey = "SELECT SnapShotFinancialServiceKey, SnapShotAccountKey, AccountKey, FinancialServiceKey, FinancialServiceTypeKey, ResetConfigurationKey, ActiveMarketRate, MarginKey, Installment, ParentFinancialServiceKey FROM [2am].[debtcounselling].[SnapShotFinancialService] WHERE SnapShotFinancialServiceKey = @PrimaryKey";
        public const string snapshotfinancialservicedatamodel_delete = "DELETE FROM [2am].[debtcounselling].[SnapShotFinancialService] WHERE SnapShotFinancialServiceKey = @PrimaryKey";
        public const string snapshotfinancialservicedatamodel_deletewhere = "DELETE FROM [2am].[debtcounselling].[SnapShotFinancialService] WHERE";
        public const string snapshotfinancialservicedatamodel_insert = "INSERT INTO [2am].[debtcounselling].[SnapShotFinancialService] (SnapShotAccountKey, AccountKey, FinancialServiceKey, FinancialServiceTypeKey, ResetConfigurationKey, ActiveMarketRate, MarginKey, Installment, ParentFinancialServiceKey) VALUES(@SnapShotAccountKey, @AccountKey, @FinancialServiceKey, @FinancialServiceTypeKey, @ResetConfigurationKey, @ActiveMarketRate, @MarginKey, @Installment, @ParentFinancialServiceKey); select cast(scope_identity() as int)";
        public const string snapshotfinancialservicedatamodel_update = "UPDATE [2am].[debtcounselling].[SnapShotFinancialService] SET SnapShotAccountKey = @SnapShotAccountKey, AccountKey = @AccountKey, FinancialServiceKey = @FinancialServiceKey, FinancialServiceTypeKey = @FinancialServiceTypeKey, ResetConfigurationKey = @ResetConfigurationKey, ActiveMarketRate = @ActiveMarketRate, MarginKey = @MarginKey, Installment = @Installment, ParentFinancialServiceKey = @ParentFinancialServiceKey WHERE SnapShotFinancialServiceKey = @SnapShotFinancialServiceKey";



    }
}