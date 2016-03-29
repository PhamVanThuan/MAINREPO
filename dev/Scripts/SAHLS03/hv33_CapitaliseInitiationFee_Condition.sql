use [2AM]
go

declare   @translatableItemKey int
		, @conditionKey int
		, @conditionSetKey int
		, @conditionConfigurationKey int
		, @phrase_Eng varchar(1000)
		, @phrase_Afr varchar(1000)

set @phrase_Eng = 'Note to Attorney and Borrower:  Initiation fees due by the borrower will be debited to the loan account.'
set @phrase_Afr = 'Nota aan Prokereur & Lener: Aanvangsfooie verskuldig deur die lener sal teen die verband gedebiteer word.'

if not exists(select 1 from [2AM].dbo.Condition where ConditionPhrase = @phrase_Eng)
begin

	INSERT INTO [2AM].[dbo].[TranslatableItem]
		   ([Description])
     VALUES
           (@phrase_Eng)

	set @translatableItemKey = SCOPE_IDENTITY()


	INSERT INTO [2AM].[dbo].[Condition]
			   ([ConditionTypeKey]
			   ,[ConditionPhrase]
			   ,[TokenDescriptions]
			   ,[TranslatableItemKey]
			   ,[ConditionName])
		 VALUES
			   ( 0
			   , @phrase_Eng
			   , null
			   , @translatableItemKey
			   ,'245')

	set @conditionKey = SCOPE_IDENTITY()

end
else
begin
	select @conditionKey = ConditionKey,
		   @translatableItemKey = TranslatableItemKey
	from [2AM].dbo.Condition where ConditionPhrase = @phrase_Eng
end



-- ENGLISH
if not exists(select 1 from [2AM].dbo.TranslatedText where TranslatableItemKey =  @translatableItemKey and LanguageKey = 2) 
begin
	INSERT INTO [2AM].[dbo].[TranslatedText]
           ([TranslatableItemKey]
           ,[LanguageKey]
           ,[TranslatedText])
     VALUES
           (  @translatableItemKey
            , 2
            , @phrase_Eng)
end

-- AFRIKAANS
if not exists(select 1 from [2AM].dbo.TranslatedText where TranslatableItemKey =  @translatableItemKey and LanguageKey = 3) 
begin
	INSERT INTO [2AM].[dbo].[TranslatedText]
           ([TranslatableItemKey]
           ,[LanguageKey]
           ,[TranslatedText])
     VALUES
           (  @translatableItemKey
            , 3
            , @phrase_Afr)
end

------------------------
-- INSERT CONDITION SET
------------------------

set @conditionSetKey = 375

if not exists(select 1 from [2AM].dbo.ConditionSet where ConditionSetKey = @conditionSetKey and Description = 'Capitalised Initiation Fee')
begin
	INSERT INTO [2AM].[dbo].[ConditionSet]
			   ([ConditionSetKey]
			   ,[Description])
		 VALUES
			   ( @conditionSetKey
			   ,'Capitalised Initiation Fee')

end

----------------------------------
-- INSERT CONDITION SET CONDITION
----------------------------------

if not exists(select 1 from [2AM].dbo.ConditionSetCondition where ConditionSetKey = @conditionSetKey and ConditionKey = @conditionKey)
begin
	INSERT INTO [2AM].[dbo].[ConditionSetCondition]
           ([ConditionSetKey]
           ,[ConditionKey]
           ,[RequiredCondition])
     VALUES
           ( @conditionSetKey
           , @conditionKey
           , 1)
end

----------------------------------
-- INSERT CONDITION CONFIGURATION
----------------------------------

if not exists(select 1 from [2AM].[dbo].[ConditionConfiguration] where GenericColumnDefinitionKey = 6 -- OfferAttributeType
																   and GenericColumnDefinitionValue = 35 -- Capitalise Initiation Fee

)
begin
	INSERT INTO [2AM].[dbo].[ConditionConfiguration]
           ([GenericKeyTypeKey]
           ,[GenericColumnDefinitionKey]
           ,[GenericColumnDefinitionValue])
     VALUES
           ( 2  -- offer
           , 6  -- OfferAttributeType
           , 35) --  Capitalise Initiation Fee

	select @conditionConfigurationKey = SCOPE_IDENTITY()
end
else
begin
	select @conditionConfigurationKey = ConditionConfigurationKey 
	from [2AM].[dbo].[ConditionConfiguration] 
	where GenericColumnDefinitionKey = 6
	  and GenericColumnDefinitionValue = 35
end

----------------------------------------------
-- INSERT CONDITIONCONFIGURATION CONDITIONSET
----------------------------------------------

if not exists(select 1 from [2AM].dbo.ConditionConfigurationConditionSet where ConditionSetKey = @conditionSetKey and ConditionConfigurationKey = @conditionConfigurationKey)
begin
	
	INSERT INTO [2AM].[dbo].[ConditionConfigurationConditionSet]
           ([ConditionConfigurationKey]
           ,[ConditionSetKey])
     VALUES
           ( @conditionConfigurationKey
           , @conditionSetKey)
end

