use [2am]
go


/* 	********************************************* */
/* 	Temp Procedure to Add/Update Subsidy Provider */
/* 	********************************************* */

if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[dbo].[tmp_GEPF_MaintainSubsidyProvider]') AND type in (N'P', N'PC'))
begin
	drop procedure [dbo].tmp_GEPF_MaintainSubsidyProvider
end
go

create procedure dbo.tmp_GEPF_MaintainSubsidyProvider
(
	@SubsidyProviderName varchar (100),
	@NewSubsidyProviderName varchar (100)
)
as
begin
	declare @SubsidyProviderKey int
	declare @LegalEntityKey int 

	if (@SubsidyProviderName is null and @NewSubsidyProviderName is not null)
	begin
		set @SubsidyProviderName = @NewSubsidyProviderName
		if (not exists(select * from [2am]..LegalEntity where RegisteredName = @NewSubsidyProviderName))
		begin
			print 'Insert new GEPF Affiliated Subsidy Provider : ' + @NewSubsidyProviderName
			insert into [2am].dbo.LegalEntity (LegalEntityTypeKey, IntroductionDate, RegisteredName, CitizenTypeKey, LegalEntityStatusKey, DocumentLanguageKey, UserID, ChangeDate) 
				values (3, getdate(), @NewSubsidyProviderName, 1, 1, 2, 'System', getdate())
			set @LegalEntityKey = scope_identity()
		end
		else
		begin
			select @LegalEntityKey = LegalEntityKey from [2am]..LegalEntity where RegisteredName = @NewSubsidyProviderName
		end

		if (not exists(select * from [2am]..SubsidyProvider where LegalEntityKey = @LegalEntityKey))
		begin
			insert into [2am].[dbo].[SubsidyProvider] ([SubsidyProviderTypeKey],[UserID],[ChangeDate],[LegalEntityKey],[GEPFAffiliate])
				values (1,'System',getdate(),@LegalEntityKey,1)
			set @SubsidyProviderKey = scope_identity()
		end
		else
		begin
			select @SubsidyProviderKey = SubsidyProviderKey from [2am]..SubsidyProvider where LegalEntityKey = @LegalEntityKey
		end
	end
	else if (@SubsidyProviderName is not null) 
	begin
		if (@NewSubsidyProviderName is not null)
		begin
			select @LegalEntityKey = LegalEntityKey from [2am]..LegalEntity where RegisteredName = @SubsidyProviderName
			if (@LegalEntityKey > 0)
			begin
				print 'Update Subsidy Provider Name : ' + @SubsidyProviderName + ' to ' + @NewSubsidyProviderName
				update [2am].dbo.LegalEntity set RegisteredName = @NewSubsidyProviderName where LegalEntityKey = @LegalEntityKey
			end
			set @SubsidyProviderName = @NewSubsidyProviderName
		end
	end

	select 
		@SubsidyProviderKey = SubsidyProviderKey 
	from 
		[2am]..SubsidyProvider sp 
	join
		[2am]..LegalEntity le on le.LegalEntityKey = sp.LegalEntityKey
	where
		le.RegisteredName = @SubsidyProviderName

	if (not exists(select * from [2am]..SubsidyProvider where SubsidyProviderKey = @SubsidyProviderKey and GEPFAffiliate = 1))
	begin
		print 'Update Subsidy Provider GEPF Indicator to TRUE : ' + @SubsidyProviderName
		update [2am].[dbo].SubsidyProvider set GEPFAffiliate = 1 where SubsidyProviderKey = @SubsidyProviderKey
	end

end
go

/* 	***********************************************	*/
/* 	Update the Data                                 */
/* 	***********************************************	*/

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Economic Affairs and Tourism',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Department of Education',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Department of Health',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape Department of Human Settlements','Eastern Cape: Department of Human Settlements'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Local Government Traditional Affairs',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Office of the Premier',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Provincial Planning & Treasury',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Public Works and Roads','Eastern Cape: Roads and Public Works'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Rural Development & Agrarian Reform','Eastern Cape: Rural Development and Agrarian Reform'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Department of Safety and Liaison','Eastern Cape: Safety and Liaison'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Social Development Special Programmes','Eastern Cape: Social Development and Special Programmes'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Sport, Recreation, Arts and Culture',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'Eastern Cape: Department of Transport'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Agriculture and Land Affairs',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Department of Welfare',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Housing and Local Government Affairs',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Eastern Cape: Social Assistance',null

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Department of Agriculture',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'Free State: Cooperative Governance and Traditional Affairs'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Economic Development, Tourism & Enviromental Affairs',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State Department of Education','Free State: Department of Education'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State Department of Health','Free State: Department of Health'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'Free State: Department of Human Settlements'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Office of the Premier',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Police, Roads and Transport',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'Free State: Provincial Treasury'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Public Works, Roads and Transport',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State Department of Social Development','Free State: Department of Social Development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'Free State: Sport, Arts, Culture and Recreation'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Sport, Arts, Culture, Science & Technology',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Department of Finance and Expenditure',null

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Government Motor Transport Trading Account',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Local Development and Housing',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Safety and Security',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Free State: Social Assitance',null

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Agriculture & Rural Development', 'Gauteng: Agriculture and Rural Development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Community Safety',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng Department of Economic Development','Gauteng: Department of Economic development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Education','Gauteng: Department of Education'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Department of Finance',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Department of Health',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Housing ','Gauteng: Department of Housing '
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng Infrastructure Development','Gauteng: Infrastructure Development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Development Planning and Local Government',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Office of the Premier',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Roads & Transport',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Social Development',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Sport, Recreation, Arts & Culture','Gauteng: Sport, Arts, Culture and Recreation'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Treasury',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Cooperative Governance & Traditional Affairs',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Provincial Safety & Liaison',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Social Services and Population Development ',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Department of Finance & Economic Affairs',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Transport & Public Works',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Gauteng: Social Assistance',null

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal:Agriculture & Environment Affairs & Rural Development','KwaZulu-Natal: Agriculture & Environment Affairs & Rural Development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Kwazulu-Natal: Department Arts Culture Tourism','KwaZulu-Natal: Department of Arts and Culture '
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal Co-Operative Governance and Traditional Affairs','KwaZulu-Natal: Cooperative Governance and Traditional Affairs'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal Provincial Government:Safety   ','KwaZulu-Natal: Community Safety and Liaison    '
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'KwaZulu-Natal: Economic Development and Tourism'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal: Department of Education',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal: Department of Health',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal Human Settlements ','KwaZulu-Natal: Department of Human Settlements '
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal Office of the Premier','KwaZulu-Natal: Office of the Premier '
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal Provincial Government: Treasury ','KwaZulu-Natal: Provincial Treasury '
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal Provincial Government: Public Works','KwaZulu-Natal: Public Works'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Kwazulu-Natal: Social Development',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Kwazulu-Natal: Department Sport and Recreation',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal Provincial Government: Royal Household','KwaZulu-Natal: The Royal Household'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal Provincial Government:Transport','KwaZulu-Natal: Department of Transport'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Kwa-Zulu Natal: Provincial Legislature','KwaZulu-Natal: Legislature'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'KwaZulu-Natal Provincial Government: Economic Affairs','KwaZulu-Natal: Department of Economic Affairs'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Kwazulu Natal: Social Assistance','KwaZulu-Natal: Social Assistance'

-- fix some case sensitive data
update [2am]..LegalEntity set RegisteredName = 'KwaZulu-Natal: Department Sport and Recreation' where RegisteredName = 'Kwazulu-Natal: Department Sport and Recreation'
update [2am]..LegalEntity set RegisteredName = 'KwaZulu-Natal: Social Development' where RegisteredName = 'Kwazulu-Natal: Social Development'

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Agriculture','Limpopo Province: Agriculture, Land and Environmental Affairs'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Co-Operative Governance Human Settlement & Traditional Affairs','Limpopo Province: Cooperative Governance, Human Settlement & Traditional Affairs'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Finance and Economic Development','Limpopo Province: Economic Development, Environment and Tourism'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Education','Limpopo Province: Department of Education'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Health ','Limpopo Province: Department of Health '
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Office of the Premier',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Provincial Treasury',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Public Works','Limpopo Province: Department of Public Works'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Transport','Limpopo Province: Roads and Transport'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Safety Security & Liaison','Limpopo Province: Safety, Security and Liaison'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Social Development',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Sport, Arts & Culture','Limpopo Province: Sport, Arts and Culture'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Legislature','Limpopo Province: Legislature'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Local Government & Housing',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Welfare',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Limpopo Province: Social Assistance',null

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga Agriculture Rural Dev and Land Admin','Mpumalanga: Agriculture, Rural Development and Land Administration'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga: Cooperative Government and Traditional Affairs',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'Mpumalanga: Culture, Sport and Recreation'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga Economic Development Environment & Tourism','Mpumalanga: Economic Development, Environment & Tourism'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga: Education','Mpumalanga: Department of Education'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga Department of Finance','Mpumalanga: Department of Finance'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga:Health','Mpumalanga: Department of Health'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'Mpumalanga: Department of Human Settlements'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga : Office of the Premier','Mpumalanga: Office of the Premier'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga: Public works, roads & transport','Mpumalanga: Public Works, Roads & Transport'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga: Community Safety Security & Liason','Mpumalanga: Community Safety, Security & Liaison'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga Social Development','Mpumalanga: Social Development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga Provincial Legislature','Mpumalanga: Legislature'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga: Department of Home Affairs',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga: Housing and Land Administration',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga: Local Government and Traffic',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga: Economic Development and Planning',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga: Social Assistance',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Mpumalanga: Social Services and Population Development',null

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'North West: Agriculture and Rural Development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'North West: Economic Development, Environment, Conservation and Tourism'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North West Education','North West: Department of Education'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'North West: Department of Finance'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North West: Health','North West: Department of Health'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'North West: Local Government and Traditional Affairs'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'North West: Office of the Premier'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'North West: Public Works, Roads and Transport'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'North West: Public Safety and Liaison'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'North West: Social Development, Women, Children and People with Disabilities'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North West Sports, Arts and Culture','North West: Sport, Arts and Culture'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North West: Economic Development and Tourism',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North West Provincial Administration','North West: Provincial Administration'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North West: Department of Community Safety & Transport Management',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North West: Ecomonic Development and Enterprise Development',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North West: Education and Sports Development',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North West Provincial Treasury','North West: Provincial Treasury'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North West: Social Assistance',null

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Agriculture, Land reform & Rural Development','Northern Cape: Agriculture, Land Reform and Rural Development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'Northern Cape: Cooperative Governance, Human Settlement and Traditional Affairs'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North Cape: Economic Development and Tourism','Northern Cape: Economic Development and Tourism'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Department of Education',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North Cape: Environment And Nature Conservation','Northern Cape: Environment and Nature Conservation'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Department of Health ',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Premier','Northern Cape: Office of the Premier'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North Cape Provincial Treasury','Northern Cape: Provincial Treasury'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'North Cape: Department Roads And Public Works','Northern Cape: Roads And Public Works'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Department of Social Development','Northern Cape: Department of Social Development  '
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Sport, Arts and Culture',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape:Department of Transport ','Northern Cape: Transport, Safety and Liaison'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape Provincial Administration','Northern Cape: Provincial Administration'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape Provincial Legislature','Northern Cape: Provincial Legislature'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Dep. Of Housing and Local Government','Northern Cape: Department of Housing and Local Government'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Department of Finance',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Safety and Security',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Social Assistance',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Agriculture, Land reform, Nature Conservation','Northern Cape: Agriculture, Land Reform and Nature Conservation'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Northern Cape: Dep Social Services & Population Development ','Northern Cape: Department of Social Services & Population Development  '

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Agriculture: Western Cape','Western Cape: Agriculture'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape: Community Safety',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Cultural Affairs & Sport: Western Cape','Western Cape: Cultural Affairs and Sport'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape: Economic Development & Tourism',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape Education','Western Cape: Department of Education'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Enviornmental Affairs & Development  Planning: Western Cape','Western Cape: Environmental Affairs and Development Planning'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape department of Health','Western Cape: Department of Health '
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'Western Cape: Department of Human Settlements'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Local Government: Western Cape','Western Cape: Local Government'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape: Department of the Premier','Western Cape: Office of the Premier'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape Provincial Treasury','Western Cape: Provincial Treasury'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape Department of Social Development','Western Cape: Department of Social Development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape: Department of Transport & Public Works',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape: Legislature',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape Provincial Administration','Western Cape: Provincial Administration'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'WEstern Cape Provincial Parlement','Western Cape: Provincial Parlement'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape: Department of Finance',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Western Cape: Social Assistance',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Housing: Western Cape','Western Cape: Department of Housing'

EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Agriculture, Forestry & Fisheries','National Department of Agriculture, Forestry & Fisheries'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Arts & Culture',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Education','National Department of Basic Education'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'National Civilian Secretariat for the Police Service'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department Of Communications','National Department of Communications'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Cooperative Governance and Traditional Affairs ','National Department of Cooperative Governance'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Correctional Services',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'South African National Defence Force',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Economic Development','National Department of Economic Development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'National Department of Energy'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Enviromental Affairs',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Health',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department Of Higher Education and Training','National Department of Higher Education and Training'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Home Affairs',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Human Settlements ','National Department of Human Settlements'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Independent Police Investigation Directorate','Independent Police Investigative Directorate'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of International Relations and Cooperation','National Department of International Relations and Cooperation'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Justice & Constitutional Development',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Labour',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Military Veterans','National Department of Military Veterans'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'National Department of Mineral Resources'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Prosecuting Authority',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'National School of Government'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Treasury',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Public Service Commission','National Office of the Public Service Commission'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Performance Monitoring & Evaluation','National Department of Planning, Monitoring and Evaluation'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Public Enterprises','National Department of Public Enterprises'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Public Service and Administration',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Public Works','National Department of Public Works'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Rural Development & Land Reform','National Department of Rural Development & Land Reform'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department Science & Technology','National Department of Science & Technology'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Social Development','National Department of Social Development'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'South African Police Services',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Sport and Recreation','National Department of Sport and Recreation South Africa'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Statistics South Africa',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'National Department of Telecommunications and Postal Services'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'The Presidency',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Tourism',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Trade & Industry','National Department of Trade and Industry'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Traditional & Local Affairs','National Department of Traditional Affairs'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Transport',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Department of Water & Sanitation','National Department of Water and Sanitation'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Women, Children & People with Disabilities','National Department of Women, Children & People with Disabilities'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Intelligence Agency (NIA)','National Intelligence Agency'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Youth Commission Office of the President',null
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Community and Safety','National Department of Community and Safety'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Finance','National Department of Finance'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Water Affairs and Forestry','National Department of Water Affairs and Forestry'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Nuclear Energy Corporation of South Africa (NECSA)','National Nuclear Energy Corporation of South Africa '
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  null,'National Treasury: Pensions Administration'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'National Health Laboratory Services','National Department of Health Laboratory Services'
EXEC [2am].[dbo].[tmp_GEPF_MaintainSubsidyProvider]  'Department of Housing','National Department of Housing'

/* 	***********************************************	*/
/* 	Procedure - clean up                            */
/* 	***********************************************	*/

if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[dbo].[tmp_GEPF_MaintainSubsidyProvider]') AND type in (N'P', N'PC'))
begin
	drop procedure [dbo].tmp_GEPF_MaintainSubsidyProvider
end
go