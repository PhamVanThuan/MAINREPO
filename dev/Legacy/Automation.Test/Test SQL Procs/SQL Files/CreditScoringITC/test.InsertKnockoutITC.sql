USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.InsertKnockoutITC') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure test.InsertKnockoutITC
	Print 'Dropped Proc test.[InsertKnockoutITC]'
End
Go

CREATE PROCEDURE test.InsertKnockoutITC 

@idNumber varchar(50),
@offerKey int,
@knockOutType varchar(255)


AS
/*
		@offerKey int,
		@idNumber varchar(13),
		@knockOut varchar(15)='None',
		@G012 varchar(8)='-2',
		@G032 varchar(8)='-2',
		@G044 varchar(8)='0',
		@WEPPstatus varchar(8)='0',
		@EmpiricaScore varchar(5)='00720',
		@AT094 varchar(8)='7',
		@AT107 varchar(8)='0',
		@AT164 varchar(8)='40',
		@IN106 varchar(8)='0',
		@IN141 varchar(8)='35',
		@OT007 varchar(8)='1',
		@OT041 varchar(8)='0',
		@RE105 varchar(8)='25',
		@RE140 varchar(8)='25',
		@EmpiricaExclusionCode varchar(8)='N/A'
		*/
BEGIN

	if (@knockOutType = 'ITCValidSBC')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, '-5'
		end
		
	if (@knockOutType = 'ITCValidEmpericaScore')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, ''
		end
	
	if (@knockOutType = 'ITCValidEmpericaScoreCodeD')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT,
				 DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'D'
		end
	
	if (@knockOutType = 'ITCValidEmpericaScoreCodeN')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT,
				 DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'N'
		end
	
	if (@knockOutType = 'ITCValidEmpericaScoreCodeL')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT,
				 DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'L'
		end
	
	if (@knockOutType = 'ITCValidEmpericaScoreCodeZ')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT,
				 DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'Z'
		end				
		
	if (@knockOutType = 'ITCDebtReviewIndicated')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, 'DebtReview'
		end	
	
	if (@knockOutType = 'ITCDisputeIndicated')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, 'Disputes'
		end
		
	if (@knockOutType = 'ITCAccountDefaultsIndicated')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, '4'
		end
	
	if (@knockOutType = 'ITCAccountJudgementsIndicated')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, '8'
		end
		
	if (@knockOutType = 'ITCAccountLegalNoticesIndicated')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, '2'
		end
	
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusDecline')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, '4'
		end
		
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusRefer')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, '-2'
		end
	
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusAlpha')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'AC'
		end
	
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusCodeE')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'E'
		end
		
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusCodeH')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'H'
		end
		
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusCodeI')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'I'
		end
		
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusCodeJ')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'J'
		end
		
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusCodeL')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'L'
		end
		
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusCodeW')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'W'
		end
		
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusCodeZ')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'Z'
		end
		
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusCodeAA')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'AA'
		end
		
	if (@knockOutType = 'ITCAccountCustomerWEPPStatusCodeAC')
		begin
			exec [test].[InsertCSknockOut] @offerKey, @idNumber, DEFAULT, DEFAULT, DEFAULT, DEFAULT, 'AC'
		end															
END
		
