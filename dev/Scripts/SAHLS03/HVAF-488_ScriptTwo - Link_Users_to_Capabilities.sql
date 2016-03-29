use [2am]


go

/*
1	Invoice Processor
2	Loss Control Fee Invoice Approver Under R15000
3	Loss Control Fee Invoice Approver Under R30000
4	Loss Control Fee Invoice Approver Up to R60000
5	Invoice Approver Over R60000
6	Invoice Payment Processor
7	Invoice Approver
8	Loss Control Fee Consultant
*/

declare @OrganisationStructureKey int
declare @uosKey int
declare @adUserName varchar(50)

--'SAHL\DavidLa' - Payment Processor, Invoice Approver < R 60000, Invoice Approver
set @adUserName = 'SAHL\DavidLa'
set @OrganisationStructureKey = (select OrganisationStructureKey from vOrganisationStructure where
pathstr = 'Company: South African Home Loans (Pty) Ltd - Loss Control - Operations - Manager')

select @uosKey = UserOrganisationStructureKey from [2am].dbo.UserOrganisationStructure uos
join [2am].dbo.ADUser a on uos.ADUserKey=a.ADUserKey
where ADUserName = @adUserName 
and OrganisationStructureKey = @OrganisationStructureKey

exec [2AM].dbo.InsertCapability 4, @uosKey

exec [2AM].dbo.InsertCapability 6, @uosKey

exec [2AM].dbo.InsertCapability 7, @uosKey

--SAHL\NivenG Processor, Fee Consultant , Invoice Approver
set @adUserName = 'SAHL\NivenG'
set @OrganisationStructureKey = (select OrganisationStructureKey from vOrganisationStructure where
pathstr = 'Company: South African Home Loans (Pty) Ltd - Loss Control - Operations - Consultant') 

select @uosKey = UserOrganisationStructureKey from [2am].dbo.UserOrganisationStructure uos
join [2am].dbo.ADUser a on uos.ADUserKey=a.ADUserKey
where ADUserName = @adUserName 
and OrganisationStructureKey = @OrganisationStructureKey

exec [2AM].dbo.InsertCapability 1, @uosKey

exec [2AM].dbo.InsertCapability 2, @uosKey

exec [2AM].dbo.InsertCapability 7, @uosKey

exec [2AM].dbo.InsertCapability 8, @uosKey

--SAHL\ZandileMa Processor, Fee Consultant , Invoice Approver
set @adUserName = 'SAHL\ZandileMa'
set @OrganisationStructureKey = (select OrganisationStructureKey from vOrganisationStructure where
pathstr = 'Company: South African Home Loans (Pty) Ltd - Loss Control - Operations - Consultant') 

select @uosKey = UserOrganisationStructureKey from [2am].dbo.UserOrganisationStructure uos
join [2am].dbo.ADUser a on uos.ADUserKey=a.ADUserKey
where ADUserName = @adUserName 
and OrganisationStructureKey = @OrganisationStructureKey

exec [2AM].dbo.InsertCapability 1, @uosKey

exec [2AM].dbo.InsertCapability 2, @uosKey

exec [2AM].dbo.InsertCapability 7, @uosKey

exec [2AM].dbo.InsertCapability 8, @uosKey

--SAHL\LeonaN Processor, Fee Consultant , Invoice Approver
set @adUserName = 'SAHL\LeonaN'
set @OrganisationStructureKey = (select OrganisationStructureKey from vOrganisationStructure where
pathstr = 'Company: South African Home Loans (Pty) Ltd - Loss Control - Operations - Consultant') 

select @uosKey = UserOrganisationStructureKey from [2am].dbo.UserOrganisationStructure uos
join [2am].dbo.ADUser a on uos.ADUserKey=a.ADUserKey
where ADUserName = @adUserName 
and OrganisationStructureKey = @OrganisationStructureKey

exec [2AM].dbo.InsertCapability 1, @uosKey

exec [2AM].dbo.InsertCapability 2, @uosKey

exec [2AM].dbo.InsertCapability 7, @uosKey

exec [2AM].dbo.InsertCapability 8, @uosKey


--SAHL\ShrivashniN Invoice Approver, Loss Control Fee Invoice Approver Under R30000
set @adUserName = 'SAHL\ShrivashniN'
set @OrganisationStructureKey = (select OrganisationStructureKey from vOrganisationStructure where
pathstr = 'Company: South African Home Loans (Pty) Ltd - Loss Control - Operations - Consultant') 

select @uosKey = UserOrganisationStructureKey from [2am].dbo.UserOrganisationStructure uos
join [2am].dbo.ADUser a on uos.ADUserKey=a.ADUserKey
where ADUserName = @adUserName 
and OrganisationStructureKey = @OrganisationStructureKey

exec [2AM].dbo.InsertCapability 3, @uosKey

exec [2AM].dbo.InsertCapability 7, @uosKey

--'SAHL\CliffV', Invoice Approver, Loss Control Fee Invoice Approver Under R30000
set @adUserName = 'SAHL\CliffV'
set @OrganisationStructureKey = (select OrganisationStructureKey from vOrganisationStructure where
pathstr = 'Company: South African Home Loans (Pty) Ltd - Loss Control - Operations - Consultant') 

select @uosKey = UserOrganisationStructureKey from [2am].dbo.UserOrganisationStructure uos
join [2am].dbo.ADUser a on uos.ADUserKey=a.ADUserKey
where ADUserName = @adUserName 
and OrganisationStructureKey = @OrganisationStructureKey

exec [2AM].dbo.InsertCapability 3, @uosKey

exec [2AM].dbo.InsertCapability 7, @uosKey

--SAHL\ColinY - Invoice Approver, Invoice Approver Over R60000
set @adUserName = 'SAHL\ColinY'
set @OrganisationStructureKey = (select OrganisationStructureKey from vOrganisationStructure where
pathstr = 'Company: South African Home Loans (Pty) Ltd - Loss Control - EXCO Management') 

select @uosKey = UserOrganisationStructureKey from [2am].dbo.UserOrganisationStructure uos
join [2am].dbo.ADUser a on uos.ADUserKey=a.ADUserKey
where ADUserName = @adUserName 
and OrganisationStructureKey = @OrganisationStructureKey

exec [2AM].dbo.InsertCapability 5, @uosKey

exec [2AM].dbo.InsertCapability 7, @uosKey


