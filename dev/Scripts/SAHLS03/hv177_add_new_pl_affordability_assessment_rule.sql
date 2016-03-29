use [2am]
go

declare @RuleItemKey int
declare @WorkflowRuleSetKey int

set @WorkflowRuleSetKey = 44 -- 'PersonalLoan - Application in Order'

set @RuleItemKey = 515571 -- AffordabilityAssessmentMandatory
if (not exists(select * from [2am]..RuleItem where RuleItemKey = @RuleItemKey))
begin
	insert into [2am]..RuleItem	(RuleItemKey, Name, [Description], AssemblyName, TypeName, EnforceRule, GeneralStatusKey)
	values (@RuleItemKey, 'AffordabilityAssessmentMandatory', 'Check that an Active Affordability Assessment exists', 'SAHL.Rules.DLL', 'SAHL.Common.BusinessModel.Rules.AffordabilityAssessment.AffordabilityAssessmentMandatory', 1, 1)
end


if (not exists(select * from [2am]..WorkflowRuleSetItem where WorkflowRuleSetKey = @WorkflowRuleSetKey and RuleItemKey = @RuleItemKey))
begin
	insert into [2am]..WorkflowRuleSetItem ([WorkflowRuleSetKey],[RuleItemKey])
    values (@WorkflowRuleSetKey, @RuleItemKey)
end


