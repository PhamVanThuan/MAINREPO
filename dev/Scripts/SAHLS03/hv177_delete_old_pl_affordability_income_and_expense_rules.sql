use [2am]
go

declare @RuleItemKey int
declare @WorkflowRuleSetKey int

set @WorkflowRuleSetKey = 44 -- 'PersonalLoan - Application in Order'

set @RuleItemKey = 51541 -- AffordabilityIncomeMandatory
if (exists(select * from [2am]..WorkflowRuleSetItem where WorkflowRuleSetKey = @WorkflowRuleSetKey and RuleItemKey = @RuleItemKey))
begin
	delete from [2am]..WorkflowRuleSetItem where WorkflowRuleSetKey = @WorkflowRuleSetKey and RuleItemKey = @RuleItemKey
end
if (exists(select * from [2am]..RuleItem where RuleItemKey = @RuleItemKey))
begin
	delete from [2am]..RuleItem where RuleItemKey = @RuleItemKey
end

set @RuleItemKey = 51542 -- AffordabilityExpenseMandatory
if (exists(select * from [2am]..WorkflowRuleSetItem where WorkflowRuleSetKey = @WorkflowRuleSetKey and RuleItemKey = @RuleItemKey))
begin
	delete from [2am]..WorkflowRuleSetItem where WorkflowRuleSetKey = @WorkflowRuleSetKey and RuleItemKey = @RuleItemKey
end
if (exists(select * from [2am]..RuleItem where RuleItemKey = @RuleItemKey))
begin
	delete from [2am]..RuleItem where RuleItemKey = @RuleItemKey
end


