USE [2AM]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[CreateSAHLHOC]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [test].[CreateSAHLHOC] 
GO

create proc [test].[CreateSAHLHOC]
	@mortgageLoanOfferKey int,
	@UserID varchar(max)
as
begin

declare @accountkey int
declare @financialservicekey int
declare @msg varchar(max)
declare @HOCHistoryKey int
declare @HOCInsurerKey int
set @HOCInsurerKey = 2

print 'EXECUTING pCreateHOC'
begin tran
EXEC [Process].[halo].[pCreateHOC] 3,117,'SAHL\NBPUser7',@accountkey OUTPUT,@financialservicekey OUTPUT,@msg OUTPUT
commit tran
print 'EXECUTED pCreateHOC, Message:' + ISNULL(convert(Varchar(max),@msg),'None')

print 'Creating OfferAccountRelationship'
INSERT INTO [dbo].[OfferAccountRelationship] ([AccountKey],[OfferKey]) VALUES(@accountkey,@mortgageLoanOfferKey)
print 'OfferAccountRelationship Created'


print 'Premium Calculation Started'
EXEC [Process].[halo].[pUpdateHOCPremium] @financialservicekey, @msg OUTPUT
print 'Premium Calculation Message:' + isnull(convert(varchar(Max),@msg),'None')

print 'Creating HOC'
INSERT INTO [dbo].[HOC] ([FinancialServiceKey],[HOCInsurerKey],[HOCPolicyNumber],[HOCProrataPremium],[HOCMonthlyPremium],[HOCThatchAmount],[HOCConventionalAmount],[HOCShingleAmount] ,[HOCTotalSumInsured]
            ,[HOCSubsidenceKey] ,[HOCConstructionKey],[HOCRoofKey],[HOCStatusID],[HOCSBICFlag],[CapitalizedMonthlyBalance],[CommencementDate],[AnniversaryDate],[UserID],[ChangeDate],[HOCStatusKey],[Ceded]            
			,[SAHLPolicyNumber],[CancellationDate],[HOCHistoryKey],[HOCAdministrationFee],[HOCBasePremium],[SASRIAAmount] ,[HOCRatesKey])
        VALUES
            (@financialservicekey,2,@accountkey,0,200,0,0,0,0,1,1,2,NULL,1,0,NULL,NULL,@UserID,getdate(),1,0,NULL,NULL,@HOCHistoryKey,-15,963.996,15.77448,5)
print 'HOC Created'

print 'Creating HOCHistory'
insert into [dbo].[HOCHistory] ([FinancialServiceKey],[HOCInsurerKey],[CommencementDate],[CancellationDate],[ChangeDate],[UserID])
values (@financialservicekey,@HOCInsurerKey,getdate(),NULL,getdate(),@UserID)
print 'HOCHistory Created'

print 'Updating HOCHistory'
set @HOCHistoryKey = (select top 01 HOCHistoryKey from dbo.HOCHistory  where FinancialServiceKey = @financialservicekey)
update [dbo].[HOC] set hochistorykey =@HOCHistoryKey where financialservicekey = @financialservicekey
print 'HOCHistory Updated'

end
go