USE [2AM]
GO
/****** 
Object:  StoredProcedure [test].[CreateDebtCounsellingCase]    
Script Date: 10/19/2010 16:48:58 
******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[InsertCorrespondence]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].[InsertCorrespondence]

GO


create procedure [test].[InsertCorrespondence]

@genericKey int,
@reportStatementKey int,
@genericKeyTypeKey int

as

begin

	declare @legalEntityKey int
	declare @emailAddress varchar(250)

		if (@ReportStatementKey = 4060 or @ReportStatementKey = 4074)
			begin
				select @legalEntityKey = legalEntityKey from dbo.ExternalRole 
				where genericKey = @genericKey
				and externalRoleTypeKey = 2 
				and generalstatuskey = 1
				
				select @emailAddress = emailAddress from [2am].dbo.legalEntity where legalEntityKey = @legalEntityKey
			end
		
	insert into [2am].dbo.Correspondence
	(GenericKey, ReportStatementKey, CorrespondenceMediumKey, DestinationValue, DueDate, CompletedDate, 
	UserID, ChangeDate,OutputFile, GenericKeyTypeKey, LegalEntityKey)
	values
	(@GenericKey, @ReportStatementKey, 2, @emailAddress, getdate(), getdate(), 
	'SAHL\ClintonS', getdate(), null, @genericKeyTypeKey, @legalEntityKey)

end



