use [2am]
go

declare @ruleItemKey int
set @ruleItemKey = 51583

if not exists (select 1 from RuleItem where RuleItemKey = @ruleItemKey and Name = 'CapitaliseInitiationFeeLTV')
begin
	insert into RuleItem
	(RuleItemKey, Name, [Description], AssemblyName, TypeName, EnforceRule, GeneralStatusKey)
	VALUES (@ruleItemKey, 'CapitaliseInitiationFeeLTV', 'LTV max when capitalising initiation fees is 100%', 'SAHL.Rules.DLL', 'SAHL.Common.BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV', 1, 1)
end

if not exists (select 1 from RuleParameter where RuleItemKey = @ruleItemKey and Name = '@MaxLTV')
begin
	insert into RuleParameter
	(RuleItemKey, Name, ParameterTypeKey, Value)
	Values (@ruleItemKey, '@MaxLTV', 7, '1.0')
end