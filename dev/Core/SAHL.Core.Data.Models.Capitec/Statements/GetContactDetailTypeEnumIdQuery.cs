namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetContactDetailTypeEnumIdQuery : ISqlStatement<ContactDetailTypeEnumDataModel>
    {
        public string Name { get; protected set; }

        public GetContactDetailTypeEnumIdQuery(string name)
        {
            this.Name = name;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],
                            [Name],
                            [IsActive]
                    FROM [Capitec].[dbo].[ContactDetailTypeEnum]
                    WHERE [Name] = @Name";
        }
    }
}