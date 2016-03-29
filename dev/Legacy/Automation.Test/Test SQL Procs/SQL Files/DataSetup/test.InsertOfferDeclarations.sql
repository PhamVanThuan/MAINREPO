USE [2AM]
GO
/****** Object:  StoredProcedure [test].[InsertOfferDeclarations]    Script Date: 10/06/2010 12:51:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[test].[InsertOfferDeclarations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [test].[InsertOfferDeclarations]
go

USE [2AM]
GO
/****** Object:  StoredProcedure [test].[InsertOfferDeclarations]    Script Date: 10/07/2010 11:55:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [test].[InsertOfferDeclarations]
		@offerKey int

AS
--we need to give legal entities without ID numbers a valid ID number

if not exists(  
select 1 from [2am].dbo.OfferRole ofr   
join [2am].dbo.legalentity le   
on ofr.legalentitykey=le.legalentitykey and legalentitytypekey=2  
join OfferDeclaration od  
on ofr.offerrolekey=od.offerrolekey where ofr.offerkey=@OfferKey  
and ofr.offerroletypekey IN (8,10,12,11)   
)  
  
begin   
  
print 'Inserting Offer Declaration Records for: ' + cast(@OfferKey as varchar(10))  
  
declare @key int  
set @key=1;  
  
with declarations (key1,key2,key3) as   
(  
select 1, 1,2  
union  
select 1, 2,3  
union  
select 1, 3,2  
union  
select 1, 4,3  
union  
select 1, 5,2  
union  
select 1, 6,2  
union  
select 1, 7,1  
)  
  
insert into [2am].dbo.OfferDeclaration  
select offerrolekey, d.key2,d.key3,NULL   
from [2am].dbo.OfferRole ofr  
join declarations d on @key=d.key1  
join [2am].dbo.legalentity le on ofr.legalentitykey=le.legalentitykey and legalentitytypekey=2  
where ofr.offerkey=@OfferKey  
and offerroletypekey in (8,10,12,11)   
order by 1  
  
end  