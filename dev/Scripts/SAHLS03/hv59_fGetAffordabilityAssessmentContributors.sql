USE [2am]
GO

if not exists (select * from sys.objects so join sys.schemas ss on ss.schema_id = so.schema_id where type in (N'FN', N'IF', N'TF', N'FS', N'FT') and ss.name = 'dbo' and so.name = 'fGetAffordabilityAssessmentContributors')
begin
   exec('create function [dbo].[fGetAffordabilityAssessmentContributors] (@AffordabilityAssessmentKey int) returns varchar(max) as begin return ''x'' end')
end

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER FUNCTION [dbo].fGetAffordabilityAssessmentContributors(
	@AffordabilityAssessmentKey int,
	@ShowOfferRole bit,
	@IsInitials bit --set as 1 to use initials, set as 0 to use name
)
RETURNS varchar(800)
AS
/***********************************************************************
Description: Returns the contributors names and roles in one long string
History:
	14/05/2015	CraigF		Create
************************************************************************/
begin
	declare @ContributorName varchar(500)
	declare @Name Varchar(500)

	declare @Surname varchar(50);
	declare @FirstNames varchar(50);
	declare @Initials varchar(5);
	declare @Salutation varchar(50);
	declare @RegisteredName varchar(50);
	declare @TradingName varchar(50);
	declare @LegalEntitytypekey int;
	declare @OfferRoleTypeDescription varchar(50);
	declare @SortOfferRoleTypeKey int;

	declare LEName cursor local fast_forward --Insensitive Cursor not allowed by Functions
	for
	select distinct
		LE.Surname,
		LE.FirstNames,
		LE.Initials,
		S.Description as Salutation,
		LE.RegisteredName,
		LE.TradingName,
		LE.LegalEntityTypeKey,
		coalesce(ort.Description, ert.Description,'Non-Applicant') as [Description],
		coalesce(ort.OfferRoleTypeKey, ert.ExternalRoleTypeKey,99) as SortOfferRoleTypeKey
	from 
		[2am].dbo.LegalEntity LE  (nolock)
	join 
		[2am].dbo.AffordabilityAssessmentLegalEntity ale (nolock)
	on
		ale.LegalEntityKey = LE.LegalEntityKey
	join 
		[2am].dbo.AffordabilityAssessment ass (nolock)
	on
		ass.AffordabilityAssessmentKey = ale.AffordabilityAssessmentKey
	left join
		SalutationType S  (nolock)
	on
		LE.Salutationkey = S.Salutationkey 
	left join
		[2am].dbo.OfferRole ofr (nolock)
	on
		ofr.OfferKey = ass.GenericKey and ofr.LegalEntityKey = ale.LegalEntityKey and ofr.GeneralStatusKey = 1
	left join 
		[2am].dbo.OfferRoleType ort (nolock) on ort.OfferRoleTypeKey = ofr.OfferRoleTypeKey
	left join
		[2am].dbo.ExternalRole er (nolock)
	on
		er.GenericKey = ass.GenericKey and er.GenericKeyTypeKey = 2 and er.LegalEntityKey = ale.LegalEntityKey and er.GeneralStatusKey = 1
	left join 
		[2am].dbo.ExternalRoleType ert (nolock) on ert.ExternalRoleTypeKey = er.ExternalRoleTypeKey
	where
		ass.AffordabilityAssessmentKey = @AffordabilityAssessmentKey
	order by 
		SortOfferRoleTypeKey, 
		LE.FirstNames, 
		LE.Surname; 
OPEN LEName

FETCH NEXT FROM LEName
INTO @Surname, @FirstNames, @Initials, @Salutation, @RegisteredName, @TradingName, @LegalEntityTypeKey, @OfferRoleTypeDescription, @SortOfferRoleTypeKey;

set @ContributorName = ''

WHILE (@@fetch_status <> -1)
BEGIN
	if @ContributorName <> ''
		set @ContributorName = @ContributorName + ' & ';

	set @Name = '';
	
	--unknown
	if @LegalEntityTypeKey = 1
	begin
		--if surname is populated then default to 'Natural Person', otherwise go the company route.
		if @Surname is not null and @Surname <> ''
			set @LegalEntityTypekey = 2;
	end

	if @LegalEntityTypeKey = 2
	begin
		if @IsInitials = 0
			set @Name = ISNULL(@Salutation + ' ', '') + ISNULL(@FirstNames + ' ', '') + ISNULL(@Surname, '');
		else
			set @Name = ISNULL(@Salutation + ' ', '') + COALESCE(@Initials + ' ', LEFT(@FirstNames, 1) + ' ', '') + ISNULL(@Surname, '');
	end
	else
	begin
		set @Name = ISNULL(@RegisteredName + ' ', '');
		
		if @TradingName is not null and @TradingName <> '' and @TradingName <> @RegisteredName
		begin
			if @Name <> ''
				set @Name = @Name + ISNULL('trading as ' + @TradingName, '');
			else
				set @Name = @TradingName;
		end
	end

	if (@ShowOfferRole = 1)
		set @ContributorName = @ContributorName + REPLACE(RTRIM(@Name), '  ', ' ') +  ' (' + isnull(@OfferRoleTypeDescription,'Non-Applicant') + ')';
	else
		set @ContributorName = @ContributorName + REPLACE(RTRIM(@Name), '  ', ' ');
	
	FETCH NEXT FROM LEName
	INTO @Surname, @FirstNames, @Initials, @Salutation, @RegisteredName, @TradingName, @LegalEntityTypekey, @OfferRoleTypeDescription, @SortOfferRoleTypeKey;
END

CLOSE LEName
Deallocate LEName

-- strip out any 'rogue' spcaces before returning the value
RETURN LTRIM(REPLACE(@ContributorName,'&  ','& '))
end

go

GRANT EXECUTE ON [dbo].[fGetAffordabilityAssessmentContributors] TO [AppRole]
GRANT EXECUTE ON [dbo].[fGetAffordabilityAssessmentContributors] TO [ProcessRole]