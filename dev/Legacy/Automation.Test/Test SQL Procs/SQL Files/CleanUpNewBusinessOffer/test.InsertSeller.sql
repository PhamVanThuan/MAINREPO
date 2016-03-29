USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertEmploymentRecords******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'test.InsertSeller') AND type in (N'P', N'PC'))
DROP PROCEDURE test.InsertSeller
go

USE [2AM]
GO
/****** Object:  StoredProcedure test.InsertSeller******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE test.InsertSeller

@offerKey int

AS

--add the Seller
if (select offerTypeKey from [2am].dbo.Offer (nolock) where offerKey=@offerKey) = 7
	begin
		insert into [2am].dbo.OfferRole
		select top 1 le.legalentitykey,@OfferKey,9,1,getdate() 
		from [2am].dbo.legalentity le (nolock)
		left join [2am].dbo.role r (nolock) 
		on le.legalentitykey=r.legalentitykey
		left join [2am].dbo.offerRole ofr (nolock) 
		on le.legalentitykey=r.legalentitykey
		where idnumber is not null
		and r.legalentitykey is null
		and ofr.legalentitykey is null
		and le.legalentityTypekey = 2
		and maritalstatuskey is not null
		and genderkey is not null
		and populationgroupkey is not null
		and salutationkey is not null
	end	