USE [2AM]
GO
/****** Object:  StoredProcedure [test].[UpdateDeedsOfficeDetails]    Script Date: 12/11/2012 15:45:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[test].[UpdateDeedsOfficeDetails]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
Begin
	Drop Procedure [test].UpdateDeedsOfficeDetails
	Print 'Dropped procedure [test].[UpdateDeedsOfficeDetails]'
End

go

create procedure [test].UpdateDeedsOfficeDetails
	@offerkey int
as

declare @dok int
	
update p 
set ErfNumber = isnull(p.ErfNumber, 'Test'), 
	ErfPortionNumber = isnull(p.ErfPortionNumber, 'Test'),
	ErfSuburbDescription = isnull(p.ErfSuburbDescription, 'Test'),
	ErfMetroDescription = isnull(p.ErfMetroDescription, 'Test'),
	SectionalSchemeName = isnull(p.SectionalSchemeName, 'Test'),
	SectionalUnitNumber = isnull(p.SectionalUnitNumber, 'Test')
from offer o 
join offermortgageloan oml on o.offerkey = oml.offerkey
join property p on oml.propertykey = p.propertykey
where o.offerkey = @offerkey

if(not exists(select * from offer o 
join offermortgageloan oml on o.offerkey = oml.offerkey
join property p on oml.propertykey = p.propertykey
join propertytitledeed ptd on p.propertykey = ptd.propertykey
where o.offerkey = @offerkey))
begin
	select top 1 @dok = deedsofficekey from deedsoffice order by newid()
	
	insert into propertytitledeed (PropertyKey, TitleDeedNumber, DeedsOfficeKey)
	select propertykey, 'Test', @dok from offer o 
	join offermortgageloan oml on o.offerkey = oml.offerkey
	where o.offerkey = @offerkey
end

if not exists(select *
	from offer o
		join offermortgageloan oml on o.offerkey = oml.offerkey
		join property p on oml.propertykey = p.propertykey
		join propertyaccessdetails pad on p.propertykey = pad.propertykey
	where o.offerkey = @offerkey)
begin
	insert into propertyaccessdetails (PropertyKey, Contact1, Contact1Phone)
	select p.propertykey,
		'Test',
		'Test'
	from offer o
		join offermortgageloan oml on o.offerkey = oml.offerkey
		join property p on oml.propertykey = p.propertykey
	where o.offerkey = @offerkey
end