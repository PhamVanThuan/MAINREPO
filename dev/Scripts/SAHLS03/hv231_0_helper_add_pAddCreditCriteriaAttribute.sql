use [2am]
go
------------------------------------------------------------------------------ 
/* create helper proc */
------------------------------------------------------------------------------ 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pAddCreditCriteriaAttribute]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[pAddCreditCriteriaAttribute]
GO

CREATE PROC dbo.pAddCreditCriteriaAttribute (@isNewBusiness bit,@isFurtherLendingAlpha bit,@isGEPF bit, @cckey int)
AS
BEGIN
		if @isNewBusiness='true'
		begin
			--add creditcriteriaattributekey=1
			if not exists (select 1 from [2am].dbo.CreditCriteriaAttribute where CreditCriteriaKey=@cckey and CreditCriteriaAttributeTypeKey=1)
				insert into [2am].dbo.CreditCriteriaAttribute (CreditCriteriaKey, CreditCriteriaAttributeTypeKey) values (@cckey, 1)
		end
		else
		begin
			--remove if exists
			if exists (select 1 from [2am].dbo.CreditCriteriaAttribute where CreditCriteriaKey=@cckey and CreditCriteriaAttributeTypeKey=1)
				begin
					delete from [2am].dbo.CreditCriteriaAttribute where CreditCriteriaKey=@cckey and CreditCriteriaAttributeTypeKey=1
				end
		end
		
		if (@isGEPF = 'false')
		begin	
			if @isFurtherLendingAlpha='true'
			begin
				--add creditcriteriaattributekey=2
				if not exists (select 1 from [2am].dbo.CreditCriteriaAttribute where CreditCriteriaKey=@cckey and CreditCriteriaAttributeTypeKey=2)
					insert into [2am].dbo.CreditCriteriaAttribute (CreditCriteriaKey, CreditCriteriaAttributeTypeKey) values (@cckey, 2)
			end 
			else
			begin
				-- remove if exists
				if exists (select 1 from [2am].dbo.CreditCriteriaAttribute where CreditCriteriaKey=@cckey and CreditCriteriaAttributeTypeKey=2)
					begin
						delete from [2am].dbo.CreditCriteriaAttribute where CreditCriteriaKey=@cckey and CreditCriteriaAttributeTypeKey=2
					end
				--add creditcriteriaattributekey=3
				if not exists (select 1 from [2am].dbo.CreditCriteriaAttribute where CreditCriteriaKey=@cckey and CreditCriteriaAttributeTypeKey=3)
					insert into [2am].dbo.CreditCriteriaAttribute (CreditCriteriaKey, CreditCriteriaAttributeTypeKey) values (@cckey, 3)
			end
		end
			
		if @isGEPF='true'
		begin
			--add creditcriteriaattributekey=4
			if not exists (select 1 from [2am].dbo.CreditCriteriaAttribute where CreditCriteriaKey=@cckey and CreditCriteriaAttributeTypeKey=4)
				insert into [2am].dbo.CreditCriteriaAttribute (CreditCriteriaKey, CreditCriteriaAttributeTypeKey) values (@cckey, 4)
		end
		else
		begin
			--remove if it exists
			if exists (select 1 from [2am].dbo.CreditCriteriaAttribute where CreditCriteriaKey=@cckey and CreditCriteriaAttributeTypeKey=4)
				begin
					delete from [2am].dbo.CreditCriteriaAttribute where CreditCriteriaKey=@cckey and CreditCriteriaAttributeTypeKey=4
				end
		end
END