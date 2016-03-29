namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetEmploymentTypeEnumIdQuery : ISqlStatement<EmploymentTypeEnumDataModel>
    {
        public string Name { get; protected set; }

        public GetEmploymentTypeEnumIdQuery()
        {
        }

        public GetEmploymentTypeEnumIdQuery(string name)
        {
            this.Name = name;
        }

        public string GetStatement()
        {
            return @"SELECT  [Id]
                            ,[Name]
                            ,[IsActive]
                            ,[SAHLEmploymentTypeKey]
                    FROM [Capitec].[dbo].[EmploymentTypeEnum]
                    WHERE [Name] = @Name";
        }
    }
}