use [2am]
go

declare @legalEntityKey int
declare @OrganisationStructureKey int
declare @VendorCode varchar(15)
declare @RegisteredName varchar(15)

set @VendorCode = 'SAHLTest01'
set @RegisteredName = 'TestVendor'
set @OrganisationStructureKey = 60


if(not exists(select 1 from dbo.Vendor where VendorCode = @VendorCode))

set @legalEntityKey = (select LegalEntityKey from [2am].dbo.LegalEntity where RegisteredName = @RegisteredName)

begin

     if(@legalEntityKey > 0)

     begin 
           insert into dbo.Vendor
           (VendorCode,ParentKey, OrganisationStructureKey,LegalEntityKey, GeneralStatusKey)
           values
           (@VendorCode, null, @OrganisationStructureKey, @legalEntityKey, 1)
     end

     else

     begin
           exec dbo.AddVendor @RegisteredName, @RegisteredName, @VendorCode, @OrganisationStructureKey, null
     end

end



