namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetPhoneNumberContactDetailTypeEnumIdQuery : ISqlStatement<PhoneNumberContactDetailTypeEnumDataModel>
    {
        public string Name { get; protected set; }

        public GetPhoneNumberContactDetailTypeEnumIdQuery(string name)
        {
            this.Name = name;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],
                            [Name],
                            [IsActive]
                    FROM [Capitec].[dbo].[PhoneNumberContactDetailTypeEnum]
                    WHERE [Name] = @Name";
        }
    }
}