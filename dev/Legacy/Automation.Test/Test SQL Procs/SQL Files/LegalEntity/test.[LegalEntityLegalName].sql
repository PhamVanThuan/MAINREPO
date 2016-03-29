USE [2AM]
GO

/****** Object:  UserDefinedFunction [dbo].[LegalEntityLegalName]    Script Date: 18/08/2015 03:12:17 PM ******/
IF OBJECT_ID (N'test.[LegalEntityLegalName]') IS NOT NULL
	DROP FUNCTION test.[LegalEntityLegalName]
GO

/****** Object:  UserDefinedFunction [dbo].[LegalEntityLegalName]    Script Date: 18/08/2015 03:12:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO


CREATE   FUNCTION test.[LegalEntityLegalName](
	@LegalEntitykey int,
	@IsInitials int --set as 1 to use initials, set as 0 to use name
)
RETURNS varchar(800)
AS
/**********************************************************
Description: Returns the full legal name for a legal entity
History:
	01/10/2006 Created
	02/10/2008 CraigF	Added (nolock) onto queries
***********************************************************/
begin
	declare @LegalName varchar(500)
	declare @Name Varchar(500)

	declare @Surname varchar(50);
	declare @FirstNames varchar(50);
	declare @Initials varchar(5);
	declare @Salutation varchar(50);
	declare @RegisteredName varchar(50);
	declare @TradingName varchar(50);
	declare @LegalEntitytypekey int;

	select distinct
		@Surname = LE.Surname,
		@FirstNames = LE.FirstNames,
		@Initials = LE.Initials,
		@Salutation = S.Description,
		@RegisteredName = LE.RegisteredName,
		@TradingName = LE.TradingName,
		@LegalEntityTypeKey = LE.LegalEntityTypeKey
	from 
		[2am]..LegalEntity LE (nolock)
	left outer join
		SalutationType S (nolock)
	on
		LE.Salutationkey = S.Salutationkey 
	where
		legalentitykey = @LegalEntitykey;
	
set @LegalName = ''

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
	else if @IsInitials = 2
		set @Name = ISNULL(@Surname, '');
	else
		set @Name = ISNULL(@Salutation + ' ', '') + ISNULL(@Initials + ' ', '') + ISNULL(@Surname, '');
end
else
begin
	set @Name = ISNULL(@RegisteredName + ' ', '');
	
	if @TradingName is not null and @TradingName <> '' and (@TradingName <> @RegisteredName)
	begin
		if @Name <> ''
			set @Name = @Name + ISNULL('trading as ' + @TradingName, '');
		else
			set @Name = @TradingName;
	end
end

set @LegalName = @LegalName + REPLACE(RTRIM(@Name), '  ', ' ');
	
RETURN @LegalName
end

GO


