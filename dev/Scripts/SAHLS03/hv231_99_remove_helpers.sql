use [2am]
go

------------------------------------------------------------------------------ 
/* clean up */
------------------------------------------------------------------------------ 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pAddCreditCriteriaAttribute]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].pAddCreditCriteriaAttribute
GO

