use [2am]
go

/**************************************************
 Statement  : Application\GetOfferAttributeTypes
 Change Date: 2015/09/18 10:54:07 AM - Version 7
 Change User: SAHL\ClintS
 **************************************************/
if( not exists(select StatementName from [2am].dbo.uiStatement where StatementName = 'GetOfferAttributeTypes' and ApplicationName = 'Application' and Version = 7) ) 
begin
   declare @higherversion int;
   -- check version numbers to determine if we can do insert
   select @higherversion = count(*) from [2am].dbo.uiStatement where StatementName = 'GetOfferAttributeTypes' and ApplicationName = 'Application' and Version > 7
   -- if there are no higher versions on file then we can go ahead and insert
   if (@higherversion = 0)
   begin
       -- if we are not on dev environment then delete all versions
       if (cast(serverproperty('MachineName') as varchar(128)) not like '%DEV%')
       begin
           if( exists(select statementname from [2am].dbo.uiStatement where statementname = 'GetOfferAttributeTypes' and ApplicationName = 'Application' ) )
	            delete from [2am].dbo.uiStatement where statementname = 'GetOfferAttributeTypes' and ApplicationName = 'Application'; 
       end

       -- insert new version of uistatement
       insert into [2am].dbo.uiStatement 
           (ApplicationName, StatementName, ModifyDate, Version, ModifyUser, Statement, Type, LastAccessedDate)
       values ('Application', 'GetOfferAttributeTypes', GetDate(), 7, 'SAHL\ClintS',
                'declare @maxOfferInformationKey int
		select 
			@maxOfferInformationKey = max(OfferInformationKey) 
		from 
			[2am].dbo.OfferInformation (nolock)
		where 
			OfferKey = @OfferKey

			declare @ltv float
			declare @employmentTypeKey int
			declare @houseHoldIncome float
			declare @isStaffLoan bit
			declare @isGEPF bit
			
			select 
					@ltv = oivl.LTV,
					@employmentTypeKey = oivl.employmentTypeKey,
					@householdIncome = oivl.HouseholdIncome,
					@isStaffLoan = (case when attr1.OfferKey is not null then 1
									else 0
									end),
					@isGEPF = (case when attr2.OfferKey is not null then 1
									else 0
									end)
			from
					[2am].dbo.Offer offer (nolock)
			join	[2am].dbo.OfferInformation oi (nolock) on oi.OfferKey = offer.OfferKey
			join	[2am].dbo.OfferInformationVariableLoan oivl (nolock) on oivl.OfferInformationKey = oi.OfferInformationKey
			left join [2am].dbo.OfferAttribute attr1 (nolock) on offer.OfferKey = attr1.OfferKey 
				and attr1.OfferAttributeTypeKey = 7 --Staff Loan
			left join [2am].dbo.OfferAttribute attr2 (nolock) on offer.OfferKey = attr2.OfferKey 
				and attr2.OfferAttributeTypeKey = 36 --Government Employee Pension Fund
			where
				oivl.OfferInformationKey = @maxOfferInformationKey

			select 
				OfferAttributeTypeKey,
				Remove
			from 
				GetOfferAttributes (@ltv, @employmentTypeKey, @houseHoldIncome, @isStaffLoan, @isGEPF, @OfferKey)',
                1, GetDate());
   end
end
