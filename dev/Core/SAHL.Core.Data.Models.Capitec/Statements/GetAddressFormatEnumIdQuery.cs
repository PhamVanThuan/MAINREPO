namespace SAHL.Core.Data.Models.Capitec.Statements
{
    public class GetAddressFormatEnumIdQuery : ISqlStatement<AddressFormatEnumDataModel>
    {
        public string Name { get; protected set; }

        public GetAddressFormatEnumIdQuery(string name)
        {
            this.Name = name;
        }

        public string GetStatement()
        {
            return @"SELECT [Id],[Name],[IsActive],[SAHLAddressFormatKey]
                    FROM [Capitec].[dbo].[AddressFormatEnum]
                    WHERE [Name] = @Name";
        }
    }
}