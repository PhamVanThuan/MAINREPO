USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'test.DeleteValuationRecord') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	drop procedure test.DeleteValuationRecord
	print 'dropped proc test.DeleteValuationRecord'
End

go
create procedure test.DeleteValuationRecord
	   @ValuationUserID varchar(max),
	   @PropertyKey int,
	   @ChangeDate datetime,
	   @IsActive bit
as

begin

	delete from dbo.valuation
	where valuation.ValuationUserID = @ValuationUserID
		and valuation.PropertyKey = @PropertyKey
		and valuation.ChangeDate = @ChangeDate
		and valuation.IsActive = @IsActive
end
