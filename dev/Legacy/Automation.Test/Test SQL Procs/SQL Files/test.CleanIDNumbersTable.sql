USE [2AM]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[CleanIDNumbersTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].[CleanIDNumbersTable]
	Print 'Dropped Proc [test].[CleanIDNumbersTable]'
End
Go

CREATE Procedure [test].[CleanIDNumbersTable]

As
Begin

--delete anything that has been used and wasnt deleted after use.
delete from test.idnumbers
where idnumber in (
select id.idnumber from test.idnumbers id
join legalentity le on id.idnumber=le.idnumber
)

End