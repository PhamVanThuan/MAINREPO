namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetDeclarationTypeEnumIdQuery : ISqlStatement<DeclarationTypeEnumDataModel>
    {
        public string Name { get; protected set; }

        public GetDeclarationTypeEnumIdQuery(string name)
        {
            this.Name = name;
        }

        public string GetStatement()
        {
            return @"SELECT [Id], [Name], [IsActive]
                     FROM [Capitec].[dbo].[DeclarationTypeEnum]
                     WHERE [Name] = @Name";
        }
    }
}