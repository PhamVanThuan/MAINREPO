namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetAddressTypeEnumIdQuery : ISqlStatement<AddressTypeEnumDataModel>
    {
        public string Name { get; protected set; }

        public GetAddressTypeEnumIdQuery(string name)
        {
            this.Name = name;
        }

        public string GetStatement()
        {
            return @"SELECT [Id], [Name], [IsActive], [SAHLAddressTypeKey]
                     FROM [Capitec].[dbo].[AddressTypeEnum]
                     WHERE [Name] = @Name";
        }
    }
}