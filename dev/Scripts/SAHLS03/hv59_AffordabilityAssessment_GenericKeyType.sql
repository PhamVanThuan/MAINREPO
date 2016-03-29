use [2am]
go

if (not exists(select * from [2am]..GenericKeyType where GenericKeyTypeKey = 58))
begin
	insert into [2am].[dbo].[GenericKeyType] ([GenericKeyTypeKey],[Description],[TableName],[PrimaryKeyColumn])
    values (58, 'AffordabilityAssessment','[2am].dbo.AffordabilityAssessment','AffordabilityAssessmentKey')
end


