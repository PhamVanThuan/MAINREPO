namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetApplicationPurposeEnumIdFromNameQuery : ISqlStatement<ApplicationPurposeEnumDataModel>
    {
        public GetApplicationPurposeEnumIdFromNameQuery(string name)
        {
            this.Name = name;
        }

        public string Name { get; protected set; }

        public string GetStatement()
        {
            return @"SELECT
                        [Id]
                        ,[Name]
                        ,[IsActive]
                        ,[SAHLMortgageLoanPurposeKey]
                    FROM
                        [Capitec].[dbo].[ApplicationPurposeEnum]
                    WHERE
                        [IsActive] = 1 and [Name] = @Name";
        }
    }
}