use [2am]
go

declare @ruleItemKey int
set @ruleItemKey = 51584

if not exists (select 1 from RuleItem where RuleItemKey = @ruleItemKey and Name = 'NaedoDebitOrderPending')
begin
	insert into RuleItem
	(RuleItemKey, Name, [Description], AssemblyName, TypeName, EnforceRule, GeneralStatusKey)
	VALUES (@ruleItemKey, 'NaedoDebitOrderPending', 'Alert the business user if a Naedo debit order is in the tracking period / pending', 'SAHL.Rules.DLL', 'SAHL.Common.BusinessModel.Rules.Account.NaedoDebitOrderPending', 1, 1)
end

use [2am]
go

declare @ruleItemKey int
set @ruleItemKey = 51586

if not exists (select 1 from RuleItem where RuleItemKey = @ruleItemKey and Name = 'NaedoDebitOrderPendingWarning')
begin
	insert into RuleItem
	(RuleItemKey, Name, [Description], AssemblyName, TypeName, EnforceRule, GeneralStatusKey)
	VALUES (@ruleItemKey, 'NaedoDebitOrderPendingWarning', 'Alert the business user if a Naedo debit order is in the tracking period / pending', 'SAHL.Rules.DLL', 'SAHL.Common.BusinessModel.Rules.Account.NaedoDebitOrderPending', 0, 1)
end

