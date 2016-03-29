USE [2AM]
GO
/****** Object:  StoredProcedure [test].[UpdateLegalEntityIDNumber]    Script Date: 10/06/2010 12:51:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[UpdateLegalEntityIDNumber]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].[UpdateLegalEntityIDNumber]
go

USE [2AM]
GO
/****** Object:  StoredProcedure [test].[CleanupNewBusinessOffer]    Script Date: 10/07/2010 11:55:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [test].[UpdateLegalEntityIDNumber]
		@offerKey int

AS
--we need to give legal entities without ID numbers a valid ID number

declare @legalentity_cursor int
declare leCursor cursor local
for 
select le.legalEntityKey
FROM   [2AM].dbo.offer o   
       JOIN [2AM].dbo.offerrole ofr   
         ON o.offerkey = ofr.offerkey   
       JOIN [2AM].dbo.legalentity le   
         ON ofr.legalentitykey = le.legalentitykey   
WHERE   
o.offerkey = @OfferKey   
AND offerroletypekey IN (8,10,12,11)  
AND legalentitytypekey in (2)  
AND (IDNumber IS NULL OR DateOfBirth IS NULL)

open leCursor;
fetch next from leCursor into
@legalentity_cursor

while (@@fetch_status = 0)
declare @IDNumber varchar(20)
--we need an idnumber
begin

select top 1 @IDNumber = id.IDNumber from test.IDNumbers id
left join [2AM].dbo.legalEntity le on id.idNumber=le.idNumber
where le.legalEntityKey is null

update [2AM].dbo.legalEntity
set idNumber = @IDNumber,
dateOfBirth = '19'+substring(@IDNumber,0,3)+'-'+substring(@IDNumber,3,2)+'-'+substring(@IDNumber,5,2)+' 00:00:00.000'
where legalEntityKey = @legalentity_cursor

fetch next from leCursor into
@legalentity_cursor

end

close leCursor;
deallocate leCursor;