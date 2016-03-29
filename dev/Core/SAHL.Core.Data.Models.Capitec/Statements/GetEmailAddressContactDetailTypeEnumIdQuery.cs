namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetEmailAddressContactDetailTypeEnumIdQuery : ISqlStatement<EmailAddressContactDetailTypeEnumDataModel>
    {
        public string Name { get; protected set; }

        public GetEmailAddressContactDetailTypeEnumIdQuery(string name)
        {
            this.Name = name;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],
                            [Name],
                            [IsActive]
                    FROM [Capitec].[dbo].[EmailAddressContactDetailTypeEnum]
                    WHERE [Name] = @Name";
        }
    }
}