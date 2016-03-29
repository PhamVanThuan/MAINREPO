use [2am]
go


declare @ReportStatementKey int
declare @ReportName varchar(50)

set @ReportStatementKey = 500 --'Letter of Understanding'
set @ReportName = 'Letter of Understanding'
if (not exists(select * from [2am]..ReportStatement where ReportStatementKey = @ReportStatementKey))
begin
	insert into [2am].[dbo].[ReportStatement] ([ReportStatementKey],[OriginationSourceProductKey],[ReportName],[Description],[StatementName],[GroupBy],[OrderBy],[ReportGroupKey],[FeatureKey],[ReportTypeKey],[ReportOutputPath])
		values (@ReportStatementKey, 1, @ReportName, @ReportName, '/Origination/Registration/FL.Letter of Understanding',null,null,null,null,2,null)
end

if (not exists(select * from [2am]..CorrespondenceMediumReportStatement where ReportStatementKey = @ReportStatementKey and CorrespondenceMediumKey = 1))
begin
	insert into [2am].[dbo].[CorrespondenceMediumReportStatement] ([ReportStatementKey],[CorrespondenceMediumKey]) values (@ReportStatementKey, 1)
end
if (not exists(select * from [2am]..CorrespondenceMediumReportStatement where ReportStatementKey = @ReportStatementKey and CorrespondenceMediumKey = 2))
begin
	insert into [2am].[dbo].[CorrespondenceMediumReportStatement] ([ReportStatementKey],[CorrespondenceMediumKey]) values (@ReportStatementKey, 2)
end
if (not exists(select * from [2am]..CorrespondenceMediumReportStatement where ReportStatementKey = @ReportStatementKey and CorrespondenceMediumKey = 3))
begin
	insert into [2am].[dbo].[CorrespondenceMediumReportStatement] ([ReportStatementKey],[CorrespondenceMediumKey]) values (@ReportStatementKey, 3)
end
